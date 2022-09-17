using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public class PlayerMaterial : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        playerItem = GetComponent<PlayerItemDown>();
    }

    // Update is called once per frame
    void Update()
    {
        // 기차 위치
        matTrain = GameObject.Find("train_laugage1").transform;
        rail = matTrain.GetComponent<MixedItem>();

        // 플레이어가 레이를 발사한다
        Ray pRay = new Ray(rayPos.position, -transform.up);
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
                float dis = Vector3.Distance(transform.position, matTrain.position);
                // 기차와의 거리가 가까우면
                if (dis < 1.5f)
                {
                    if (Input.GetButtonDown("Jump"))
                    {
                        matTrain.GetComponent<MixedItem>().branchCount = branchArray.Count;
                        // 기차 위에 branch 쌓기
                        DeleteMat(branchArray);
                    }
                }
                // 손에 무언갈 들고 있고 기차와의 거리와 멀면
                else
                {
                    // 바닥 상태가 Branch일때
                    // 바닥의 branch의 수가 1보다 크면
                    // 손에 있는 branch 수 만큼 쌓임
                    // 아니라면 흡수됨
                    if (matGod.matState == MaterialGOD.Materials.Branch)
                    {
                        if (matGod.branchCount <= 1)
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
                        else
                        {
                            if (Input.GetButtonDown("Jump"))
                            {
                                if (branchArray.Count + matGod.branchCount == matGod.mat.Count) return;
                                for(int i = 0; i < branchArray.Count + matGod.branchCount; i++)
                                {
                                    matGod.CreateMat("MK_Prefab/Branch", i);
                                }
                                
                            }
                        }

                    }
                    if (Input.GetButtonDown("Jump"))
                    {
                        // 바닥 상태가 idle이라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Idle)
                        {
                            matGod.branchCount = branchArray.Count;
                            matGod.matState = MaterialGOD.Materials.Branch;
                            DeleteMat(branchArray);
                        }
                        // 바닥 상태가 Ax라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                        {
                            ChangeState(PlayerItemDown.Hold.Ax, ToolGOD.Tools.Idle, branchArray, MaterialGOD.Materials.Branch);
                            DeleteMat(branchArray);
                        }
                        // 바닥 상태가 Pick라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                        {
                            ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, branchArray, MaterialGOD.Materials.Branch);
                            DeleteMat(branchArray);
                        }
                        // 바닥 상태가 Pail라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                        {
                            ChangeState(PlayerItemDown.Hold.Pail, ToolGOD.Tools.Idle, branchArray, MaterialGOD.Materials.Branch);
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
                float dis = Vector3.Distance(transform.position, matTrain.position);
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
                    // 바닥 상태가 idle이라면
                    if (Input.GetButtonDown("Jump"))
                    {
                        // 점프키를 눌렀을 때,
                        if (toolGOD.toolsState == ToolGOD.Tools.Idle)
                        {
                            matGod.branchCount = steelArray.Count;
                            matGod.matState = MaterialGOD.Materials.Steel;
                            DeleteMat(steelArray);
                        }
                        // 점프키를 눌렀을 때,
                        else if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                        {
                            ChangeState(PlayerItemDown.Hold.Ax, ToolGOD.Tools.Idle, steelArray, MaterialGOD.Materials.Steel);
                            DeleteMat(steelArray);
                        }
                        else if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                        {
                            ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, steelArray, MaterialGOD.Materials.Steel);
                            DeleteMat(steelArray);
                        }
                        // 점프키를 눌렀을 때,
                        if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                        {
                            ChangeState(PlayerItemDown.Hold.Pail, ToolGOD.Tools.Idle, steelArray, MaterialGOD.Materials.Steel);
                            DeleteMat(steelArray);
                        }
                    }

                }
            }
            #endregion
            #region rail이 손에 있는 경우
            else if (railArray.Count > 0)
            {
                // 레일의 마지막인 경우
                if (connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1])
                {
                    if (itemGOD.items != ItemGOD.Items.Rail)
                    {
                        // 키를 누르면
                        if (Input.GetButtonDown("Jump"))
                        {
                            // 제거
                            itemGOD.ChangeState(ItemGOD.Items.Rail);
                            RemoveRail();
                        }
                    }
                    else
                    {
                        // 키를 누르면
                        if (Input.GetButtonDown("Jump"))
                        {
                            // 제거
                            itemGOD.ChangeState(ItemGOD.Items.Idle);
                            AddRail();
                        }
                    }
                }
                else
                {
                    #region 레일 설치 x
                    // 바닥 상태가 Rail라면 => 레일 설치 x
                    if (matGod.matState == MaterialGOD.Materials.Rail)
                    {
                        // Array에 추가하기
                        MakeMat("CHAN_Prefab/Rail", railArray);
                        // 손 위치 위로 아이템 쌓게 만들기
                        for (int i = 0; i < railArray.Count; i++)
                        {
                            railArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                            railArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                        }
                        matGod.matState = MaterialGOD.Materials.None;

                    }
                    if (Input.GetButtonDown("Jump"))
                    {
                        // 바닥 상태가 idle이라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Idle)
                        {
                            matGod.branchCount = railArray.Count;
                            matGod.matState = MaterialGOD.Materials.Rail;
                            DeleteMat(railArray);
                        }
                        // 바닥 상태가 Ax라면
                        else if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                        {
                            ChangeState(PlayerItemDown.Hold.Ax, ToolGOD.Tools.Idle, railArray, MaterialGOD.Materials.Rail);
                            DeleteMat(railArray);
                        }
                        // 바닥 상태가 Pick라면
                        else if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                        {
                            ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, railArray, MaterialGOD.Materials.Rail);
                            DeleteMat(railArray);
                        }
                        // 바닥 상태가 Pail라면
                        else if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                        {
                            ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, railArray, MaterialGOD.Materials.Rail);
                            DeleteMat(railArray);
                        }
                    }
                    #endregion
                }
            }
            #endregion
            // 손에 없고
            else
            {
                // RailTrain과의 거리가 가까우면 
                GameObject railtrain = GameObject.Find("train_laugage2");
                float dis = Vector3.Distance(railtrain.transform.position, transform.position);
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
                    if(itemGOD.items == ItemGOD.Items.Rail)
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            // 제거
                            itemGOD.ChangeState(ItemGOD.Items.Idle);
                            AddRail();
                        }
                    }
                    #endregion
                }
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
    void ChangeState(PlayerItemDown.Hold player, ToolGOD.Tools tool, List<GameObject> mat, MaterialGOD.Materials mats)
    {
        playerItem.holdState = player;
        toolGOD.toolsState = tool;
        matGod.branchCount = mat.Count;
        matGod.matState = mats;
    }

    public void AddRail()
    {
        GameObject rail = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail"));
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
            if(railArray.Count <= 0)
            {
                playerItem.holdState = PlayerItemDown.Hold.Idle;
            }
        }
        
    }

}
