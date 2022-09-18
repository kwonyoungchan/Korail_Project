using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 플레이어가 material을 판별
#region material 판별 로직 
// 플레이어가 Ray를 쏘고 다닌다
// 만약 플레이어가 손에 branch나 steel을 들고 있다면 
// branch나 steel이 위로 쌓인다
// 아닐 경우
// 점프키를 누르면 
// 스테이트가 변경된다
#endregion

#region 추가 사항 및 문제 사항
// + 09.15 문제사항
// : 플레이어가 손에 나무를 들고 있을 때, 바닥 상태도 Branch라면 바닥 상태의 개수에 따라 손에 들고 있는 개수 변경됨
#endregion
public class PlayerMaterial : MonoBehaviourPun
{
    // 리스트
    // 나뭇가지
    public List<GameObject> branchArray = new List<GameObject>();
    // 철
    public List<GameObject> steelArray = new List<GameObject>();
    // 래일
    public List<GameObject> railArray = new List<GameObject>();
    // 아이템 위치
    public Transform itemPos;
    // 레이 위치
    public Transform rayPos;

    // MaterialGod 컴포넌트
    MaterialGOD matGod;
    // ToolGod 컴포넌트
    ToolGOD toolGOD;
    // ItemGod 컴포넌트
    ItemGOD itemGOD;

    // 플레이어 손 상태 컴포넌트
    PlayerItemDown playerItem;
    // 기차 위치
    Transform matTrain;
    MixedItem rail;

    int n;

    // Start is called before the first frame update
    void Start()
    {
        // 움직임 연동 : 내것이 아니면 반환
        if (!photonView.IsMine)
        {
            return;
        }
        playerItem = GetComponent<PlayerItemDown>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddRail();
        }
        // 움직임 연동 : 내것이 아니면 반환
        if (!photonView.IsMine)
        {
            return;

        }
        // RailTrain과의 거리가 가까우면 
        GameObject railtrain = GameObject.Find("train_laugage2");
        float dis = Vector3.Distance(railtrain.transform.position, transform.position);

        // 기차 위치
        matTrain = GameObject.Find("train_laugage1").transform;
        rail = matTrain.GetComponent<MixedItem>();

        // 플레이어가 레이를 발사한다
        Ray pRay = new Ray(rayPos.position + new Vector3(0.2f, 0, 0), -transform.up);
        RaycastHit rayInfo;
        if (Physics.Raycast(pRay, out rayInfo))
        {
            matGod = rayInfo.transform.gameObject.GetComponent<MaterialGOD>();
            toolGOD = rayInfo.transform.gameObject.GetComponent<ToolGOD>();
            itemGOD = rayInfo.transform.gameObject.GetComponent<ItemGOD>();

            // 손에 무언갈 들고 있다면

            #region branch가 손에 있는 경우
            if (branchArray.Count > 0)
            {
                // 기차와의 거리 판별
                float dis1 = Vector3.Distance(transform.position, matTrain.position);
                // 기차와의 거리가 가까우면
                if (dis1 < 1.5f)
                {
                    if (Input.GetButtonDown("Jump"))
                    {
                        matTrain.GetComponent<MixedItem>().branchCount = branchArray.Count;
                        // 기차 위에 branch 쌓기
                        DeleteMat(branchArray);
                        playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                    }
                }
                // 손에 무언갈 들고 있고 기차와의 거리와 멀면
                else
                {
                    if (matGod.matState == MaterialGOD.Materials.Branch)
                    {
                        // Array에 추가하기
                        MakeMat("MK_Prefab/Branch", branchArray);
                        // 손 위치 위로 아이템 쌓게 만들기
                        for (int i = 0; i < branchArray.Count; i++)
                        {
                            branchArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                            branchArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                        }
                        matGod.matState = MaterialGOD.Materials.None;

                    }
                    // 바닥 상태가 Steel일때
                    if(matGod.matState == MaterialGOD.Materials.Steel)
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            matGod.branchCount = branchArray.Count;
                            if (matGod.steelCount > 0)
                            {
                                for (int i = 0; i < matGod.mat.Count; i++)
                                {
                                    Destroy(matGod.mat[i]);
                                }
                                matGod.mat.Clear();
                                for (int i = 0; i < matGod.steelCount; i++)
                                {
                                    MakeMat("MK_Prefab/Steel", steelArray);
                                    steelArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                    steelArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                                }

                                
                                matGod.matState = MaterialGOD.Materials.Branch;
                                DeleteMat(branchArray);
                            }
                        }
                    }
                    // 바닥 상태가 Steel일때
                    if (matGod.matState == MaterialGOD.Materials.Rail)
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            if (matGod.railCount > 0)
                            {
                                for (int i = 0; i < matGod.railCount; i++)
                                {
                                    MakeMat("CHAN_Prefab/Rail", railArray);
                                    railArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                    railArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                                }
                                
                            }
                            matGod.branchCount = branchArray.Count;
                            matGod.matState = MaterialGOD.Materials.Branch;
                            DeleteMat(branchArray);
                        }
                    }

                    // 바닥 상태가 idle이라면
                    if (toolGOD.toolsState == ToolGOD.Tools.Idle)
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            matGod.branchCount = branchArray.Count;
                            matGod.matState = MaterialGOD.Materials.Branch;
                            DeleteMat(branchArray);
                        }
                    }
                    // 나무를 들고있을때
                    // 바닥 상태가 Ax라면
                    if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            ChangeState(PlayerItemDown.Hold.Ax, ToolGOD.Tools.Idle, branchArray, MaterialGOD.Materials.Branch, matGod.branchCount);
                            DeleteMat(branchArray);
                        }
                    }
                    // 바닥 상태가 Pick라면
                    if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, branchArray, MaterialGOD.Materials.Branch, matGod.branchCount);
                            DeleteMat(branchArray);
                        }
                    }
                    // 바닥 상태가 Pail라면
                    if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            ChangeState(PlayerItemDown.Hold.Pail, ToolGOD.Tools.Idle, branchArray, MaterialGOD.Materials.Branch, matGod.branchCount);
                            DeleteMat(branchArray);
                        }
                    }
                    
                }
            }
            #endregion
            #region Steel가 손에 있는 경우
            else if (steelArray.Count > 0)
            {
                // 기차와의 거리 판별
                float dis1 = Vector3.Distance(transform.position, matTrain.position);
                // 기차와의 거리가 가까우면
                if (dis < 1.5f)
                {
                    if (Input.GetButtonDown("Jump"))
                    {
                        matTrain.GetComponent<MixedItem>().steelCount = steelArray.Count;
                        // 기차 위에 branch 쌓기
                        DeleteMat(steelArray);
                    }
                }
                else
                {
                    // 바닥 상태가 Steel 상태라면
                    if (matGod.matState == MaterialGOD.Materials.Steel)
                    {
                        // Array에 추가하기
                        MakeMat("MK_Prefab/Steel", steelArray);
                        // 손 위치 위로 아이템 쌓게 만들기
                        for (int i = 0; i < steelArray.Count; i++)
                        {
                            steelArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                            steelArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                        }
                        matGod.matState = MaterialGOD.Materials.None;

                    }
                    // 바닥 상태가 branch
                    if (matGod.matState == MaterialGOD.Materials.Branch)
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            if (matGod.branchCount > 0)
                            {
                                for (int i = 0; i < matGod.branchCount; i++)
                                {
                                    MakeMat("MK_Prefab/Branch", branchArray);
                                    branchArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                    branchArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                                }

                            }
                            matGod.steelCount = steelArray.Count;
                            matGod.matState = MaterialGOD.Materials.Steel;
                            DeleteMat(steelArray);
                        }
                    }
                    if (matGod.matState == MaterialGOD.Materials.Rail)
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            if (matGod.railCount > 0)
                            {
                                for (int i = 0; i < matGod.railCount; i++)
                                {
                                    MakeMat("CHAN_Prefab/Rail", railArray);
                                    railArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                    railArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                                }

                            }
                            matGod.steelCount = steelArray.Count;
                            matGod.matState = MaterialGOD.Materials.Steel;
                            DeleteMat(steelArray);
                        }
                    }
                    // 점프키를 눌렀을 때,
                    if (toolGOD.toolsState == ToolGOD.Tools.Idle)
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            matGod.steelCount = steelArray.Count;
                            matGod.matState = MaterialGOD.Materials.Steel;
                            DeleteMat(steelArray);
                        }
                    }
                    // 점프키를 눌렀을 때,
                    if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            ChangeState(PlayerItemDown.Hold.Ax, ToolGOD.Tools.Idle, steelArray, MaterialGOD.Materials.Steel, matGod.steelCount);
                            DeleteMat(steelArray);
                        }
                    }
                    if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, steelArray, MaterialGOD.Materials.Steel, matGod.steelCount);
                            DeleteMat(steelArray);
                        }
                    }
                    // 점프키를 눌렀을 때,
                    if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            ChangeState(PlayerItemDown.Hold.Pail, ToolGOD.Tools.Idle, steelArray, MaterialGOD.Materials.Steel, matGod.steelCount);
                            DeleteMat(steelArray);
                        }
                    }
                }
            }
            #endregion
            #region rail이 손에 있는 경우
            else if (railArray.Count > 0)
            {
                GameObject train_main = GameObject.Find("train_main");
                float mainDis = Vector3.Distance(train_main.transform.position, transform.position);


                float railDis = Vector3.Distance(connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count -1].transform.position, transform.position);
                // 레일이 마지막이 아닌 경우
                if (itemGOD.items == ItemGOD.Items.Rail || itemGOD.items == ItemGOD.Items.CornerRail)
                {
                    // 키를 누르면
                    if (Input.GetButtonDown("Jump"))
                    {
                        // 제거
                        itemGOD.ChangeState(ItemGOD.Items.Idle);
                        connectRail.instance.connectedRails.RemoveAt(connectRail.instance.connectedRails.Count - 1);
                        AddRail();
                    }
                    
                }
                else
                {
                    if (railDis < 2 && railDis > 0.5f && rayInfo.transform.gameObject != connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1].gameObject)
                    {

                        // 키를 누르면
                        if (Input.GetButtonDown("Jump"))
                        {
                            // 손에서 제거
                            itemGOD.ChangeState(ItemGOD.Items.Rail);
                            RemoveRail();
                        }

                    }
                    if (rayInfo.transform.gameObject == connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1].gameObject)
                    {
                        // 키를 누르면
                        if (Input.GetButtonDown("Jump"))
                        {
                            // 제거
                            itemGOD.ChangeState(ItemGOD.Items.Idle);
                            connectRail.instance.connectedRails.RemoveAt(connectRail.instance.connectedRails.Count - 1);
                            AddRail();
                        }
                    }
                    if (mainDis < 1.6 && n <= 0)
                    {
                        // 키를 누르면
                        if (Input.GetButtonDown("Jump"))
                        {
                            // 손에서 제거
                            itemGOD.ChangeState(ItemGOD.Items.Rail);
                            RemoveRail();
                        }
                        n = 1;

                    }
                    else if (itemGOD.items == ItemGOD.Items.Idle)
                    {
                        // 바닥 상태가 branch
                        if (matGod.matState == MaterialGOD.Materials.Branch)
                        {
                            if (Input.GetButtonDown("Jump"))
                            {
                                if (matGod.branchCount > 0)
                                {
                                    for (int i = 0; i < matGod.branchCount; i++)
                                    {
                                        MakeMat("MK_Prefab/Branch", branchArray);
                                        branchArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                        branchArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                                    }

                                }
                                matGod.railCount = railArray.Count;
                                matGod.matState = MaterialGOD.Materials.Rail;
                                DeleteMat(railArray);
                            }
                        }
                        if (matGod.matState == MaterialGOD.Materials.Steel)
                        {
                            if (Input.GetButtonDown("Jump"))
                            {
                                if (matGod.steelCount > 0)
                                {
                                    for (int i = 0; i < matGod.steelCount; i++)
                                    {
                                        MakeMat("MK_Prefab/Branch", steelArray);
                                        steelArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                        steelArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                                    }

                                }
                                matGod.railCount = railArray.Count;
                                matGod.matState = MaterialGOD.Materials.Rail;
                                DeleteMat(railArray);
                            }
                        }
                        // 바닥 상태가 idle이라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Idle)
                        {
                            if (Input.GetButtonDown("Jump"))
                            {
                                matGod.railCount = railArray.Count;
                                matGod.matState = MaterialGOD.Materials.Rail;
                                DeleteMat(railArray);
                            }

                        }
                        // 바닥 상태가 Ax라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                        {
                            if (Input.GetButtonDown("Jump"))
                            {
                                ChangeState(PlayerItemDown.Hold.Ax, ToolGOD.Tools.Idle, railArray, MaterialGOD.Materials.Rail, matGod.railCount);
                                DeleteMat(railArray);
                            }
                        }
                        // 바닥 상태가 Pick라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                        {
                            if (Input.GetButtonDown("Jump"))
                            {
                                ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, railArray, MaterialGOD.Materials.Rail, matGod.railCount);
                                DeleteMat(railArray);
                            }
                        }
                        // 바닥 상태가 Pail라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                        {
                            if (Input.GetButtonDown("Jump"))
                            {
                                ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, railArray, MaterialGOD.Materials.Rail, matGod.railCount);
                                DeleteMat(railArray);
                            }
                        }

                    }


                }
                
            }
            #endregion

            // 손에 없고
            else
            {

                if (dis < 1.5f && rail.railCount > 0)
                {
                    if (Input.GetButtonDown("Jump"))
                    {
                        for (int i = 0; i < rail.railArray.Count; i++)
                        {
                            MakeMat("CHAN_Prefab/Rail", railArray);
                            railArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                            railArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                        }
                        // 플레이어 손상태 변환
                        playerItem.holdState = PlayerItemDown.Hold.Mat;
                        rail.railCount = 0;

                    }
                }

                // 점프키를 눌렀을 때,
                if (Input.GetButtonDown("Jump"))
                {
                    #region Branch 
                    // 바닥 상태가 Branch라면
                    if (matGod.matState == MaterialGOD.Materials.Branch)
                    {
                        ChangeToolGod();
                        // 바닥에 branch가 여러개인 경우,
                        // 만약 바닥에 branch가 3개 이하인 경우
                        if (matGod.branchCount > 0)
                        {
                            for (int i = 0; i < matGod.branchCount; i++)
                            {
                                MakeMat("MK_Prefab/Branch", branchArray);
                                branchArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                branchArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                            }

                        }
                        // 플레이어 손상태 변환
                        playerItem.holdState = PlayerItemDown.Hold.Mat;
                        // 바닥상태 변환
                        matGod.matState = MaterialGOD.Materials.None;
                    }
                    #endregion
                    #region Steel
                    // 점프키를 눌렀을 때,
                    else if (matGod.matState == MaterialGOD.Materials.Steel)
                    {
                        ChangeToolGod();
                        // Steel이라면
                        // 바닥에 steel이 여러개인 경우
                        if (matGod.steelCount > 0)
                        {
                            for (int i = 0; i < matGod.steelCount; i++)
                            {
                                MakeMat("MK_Prefab/Steel", steelArray);
                                steelArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                steelArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                            }

                        }
                        // 플레이어 손상태 변환
                        playerItem.holdState = PlayerItemDown.Hold.Mat;
                        // 바닥상태 변환
                        matGod.matState = MaterialGOD.Materials.None;

                    }
                    #endregion
                    #region Rail
                    // 점프키를 눌렀을 때,
                    else if (matGod.matState == MaterialGOD.Materials.Rail)
                    {
                        //ChangeToolGod();
                        // rail
                        // 바닥에 rail이 여러개인 경우
                        if (matGod.railCount > 0)
                        {
                            for (int i = 0; i < matGod.railCount; i++)
                            {
                                MakeMat("CHAN_Prefab/Rail", railArray);
                                railArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                railArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                            }

                        }
                        // 플레이어 손상태 변환
                        playerItem.holdState = PlayerItemDown.Hold.Mat;
                        // 바닥상태 변환
                        matGod.matState = MaterialGOD.Materials.None;
                    }
                    if(itemGOD.items ==ItemGOD.Items.Rail || itemGOD.items == ItemGOD.Items.CornerRail)
                    {
                        // 키를 누르면
                        if (Input.GetButtonDown("Jump"))
                        {
                            // 제거
                            itemGOD.ChangeState(ItemGOD.Items.Idle);
                            connectRail.instance.connectedRails.RemoveAt(connectRail.instance.connectedRails.Count - 1);
                            AddRail();
                        }
                    }
                    #endregion
                }
            }
        }
        
    }

    void ChangeToolGod()
    {
        if (playerItem.holdState != PlayerItemDown.Hold.Idle)
        {
            if (playerItem.holdState == PlayerItemDown.Hold.Ax)
            {
                toolGOD.toolsState = ToolGOD.Tools.Ax;
            }
            if (playerItem.holdState == PlayerItemDown.Hold.Pick)
            {
                toolGOD.toolsState = ToolGOD.Tools.Pick;
            }
            if (playerItem.holdState == PlayerItemDown.Hold.Pail)
            {
                toolGOD.toolsState = ToolGOD.Tools.Pail;
            }
        }
    }

    void MakeMat(string s, List<GameObject> matArray)
    {
        // Array에 추가하기
        GameObject mat = Instantiate(Resources.Load<GameObject>(s));
        mat.transform.parent = itemPos;
        matArray.Add(mat);
    }

    void DeleteMat(List<GameObject> matArray)
    {
        // 바닥에 손에 있는 개수만큼 쌓임
        // 손에 있는 모든 것들이 제거
        for (int i = 0; i < matArray.Count; i++)
        {
            Destroy(matArray[i].gameObject);
        }
        playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
        matArray.Clear();
    }
    // 상태 변화
    void ChangeState(PlayerItemDown.Hold player, ToolGOD.Tools tool, List<GameObject> mat, MaterialGOD.Materials mats, int count)
    {
        playerItem.holdState = player;
        toolGOD.toolsState = tool;
        count = mat.Count;
        matGod.matState = mats;
    }

    public void AddRail()
    {
        GameObject rail = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail"));
        rail.gameObject.transform.parent = itemPos;
        railArray.Add(rail);
        for(int i = 0; i < railArray.Count; i++)
        {
            railArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
        }
        playerItem.holdState = PlayerItemDown.Hold.Mat;
    }
    public void RemoveRail()
    {
        if (railArray.Count > 0)
        {
            GameObject rail = railArray[railArray.Count - 1];
            railArray.RemoveAt(railArray.Count - 1);
            Destroy(rail);
            if(railArray.Count > 0)
            {
                playerItem.holdState = PlayerItemDown.Hold.Mat;
            }
        }
        
    }


}
