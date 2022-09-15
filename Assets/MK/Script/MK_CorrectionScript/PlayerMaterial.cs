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

    // 기차 위 아이템 위치
    public Transform bPos;
    public Transform sPos;
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
    // connectRail 
    connectRail connectRail;

    // Start is called before the first frame update
    void Start()
    {
        playerItem = GetComponent<PlayerItemDown>();
        connectRail = GetComponent<connectRail>();
    }

    // Update is called once per frame
    void Update()
    {
        // 기차 위치
        matTrain = GameObject.Find("Train").transform;
        rail = GameObject.Find("Train").GetComponent<MixedItem>();

        // 플레이어가 레이를 발사한다
        Ray pRay = new Ray(rayPos.position, -transform.up);
        RaycastHit rayInfo;
        if (Physics.Raycast(pRay, out rayInfo))
        {
            matGod = rayInfo.transform.gameObject.GetComponent<MaterialGOD>();
            toolGOD = rayInfo.transform.gameObject.GetComponent<ToolGOD>();
            itemGOD = rayInfo.transform.gameObject.GetComponent<ItemGOD>();
            // 손에 무언갈 들고 있다면
            if (branchArray.Count > 0 || steelArray.Count > 0 || railArray.Count > 0)
            {
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
                            for (int i = 0; i < branchArray.Count; i++)
                            {
                                Destroy(branchArray[i].gameObject);
                            }
                            branchArray.Clear();
                        }
                    }
                    else
                    {

                        // 바닥 상태가 Branch라면
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
                        // 바닥 상태가 idle이라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Idle)
                        {
                            // 점프키를 눌렀을 때,
                            if (Input.GetButtonDown("Jump"))
                            {
                                DeleteBranch();
                            }
                        }
                        // 바닥 상태가 Ax라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                        {
                            // 점프키를 눌렀을 때,
                            if (Input.GetButtonDown("Jump"))
                            {
                                playerItem.holdState = PlayerItemDown.Hold.Ax;
                                toolGOD.toolsState = ToolGOD.Tools.Idle;
                                DeleteBranch();
                            }
                        }
                        // 바닥 상태가 Pick라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                        {
                            // 점프키를 눌렀을 때,
                            if (Input.GetButtonDown("Jump"))
                            {
                                playerItem.holdState = PlayerItemDown.Hold.Pick;
                                toolGOD.toolsState = ToolGOD.Tools.Idle;
                                DeleteBranch();
                            }
                        }
                        // 바닥 상태가 Pail라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                        {
                            // 점프키를 눌렀을 때,
                            if (Input.GetButtonDown("Jump"))
                            {
                                playerItem.holdState = PlayerItemDown.Hold.Pail;
                                toolGOD.toolsState = ToolGOD.Tools.Idle;
                                DeleteBranch();
                            }
                        }
                    }
                }
                #endregion
                #region Steel가 손에 있는 경우
                if (steelArray.Count > 0)
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
                            for (int i = 0; i < steelArray.Count; i++)
                            {
                                Destroy(steelArray[i].gameObject);
                            }
                            steelArray.Clear();
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
                        if (toolGOD.toolsState == ToolGOD.Tools.Idle)
                        {
                            // 점프키를 눌렀을 때,
                            if (Input.GetButtonDown("Jump"))
                            {
                                DeleteSteel();
                            }
                        }
                        // 바닥 상태가 Ax라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                        {
                            // 점프키를 눌렀을 때,
                            if (Input.GetButtonDown("Jump"))
                            {
                                playerItem.holdState = PlayerItemDown.Hold.Ax;
                                toolGOD.toolsState = ToolGOD.Tools.Idle;
                                DeleteSteel();
                            }
                        }
                        // 바닥 상태가 Pick라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                        {
                            // 점프키를 눌렀을 때,
                            if (Input.GetButtonDown("Jump"))
                            {
                                playerItem.holdState = PlayerItemDown.Hold.Pick;
                                toolGOD.toolsState = ToolGOD.Tools.Idle;
                                DeleteSteel();
                            }
                        }
                        // 바닥 상태가 Pail라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                        {
                            // 점프키를 눌렀을 때,
                            if (Input.GetButtonDown("Jump"))
                            {
                                playerItem.holdState = PlayerItemDown.Hold.Pail;
                                toolGOD.toolsState = ToolGOD.Tools.Idle;
                                DeleteSteel();
                            }
                        }

                    }
                }
                #endregion
                #region rail이 손에 있는 경우
                if (railArray.Count > 0)
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
                    // 바닥 상태가 idle이라면
                    if (toolGOD.toolsState == ToolGOD.Tools.Idle)
                    {
                        // 점프키를 눌렀을 때,
                        if (Input.GetButtonDown("Jump"))
                        {
                            DeleteRail();
                        }
                    }
                    // 바닥 상태가 Ax라면
                    if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                    {
                        // 점프키를 눌렀을 때,
                        if (Input.GetButtonDown("Jump"))
                        {
                            playerItem.holdState = PlayerItemDown.Hold.Ax;
                            toolGOD.toolsState = ToolGOD.Tools.Idle;
                            DeleteRail();
                        }
                    }
                    // 바닥 상태가 Pick라면
                    if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                    {
                        // 점프키를 눌렀을 때,
                        if (Input.GetButtonDown("Jump"))
                        {
                            playerItem.holdState = PlayerItemDown.Hold.Pick;
                            toolGOD.toolsState = ToolGOD.Tools.Idle;
                            DeleteRail();
                        }
                    }
                    // 바닥 상태가 Pail라면
                    if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                    {
                        // 점프키를 눌렀을 때,
                        if (Input.GetButtonDown("Jump"))
                        {
                            playerItem.holdState = PlayerItemDown.Hold.Pail;
                            toolGOD.toolsState = ToolGOD.Tools.Idle;
                            DeleteRail();
                        }
                    }
                    #endregion
                }
                #endregion
            }
            // 손에 없고
            else
            {
                // RailTrain과의 거리가 가까우면 
                GameObject railtrain = GameObject.Find("RailTrain");
                float dis = Vector3.Distance(railtrain.transform.position, transform.position);
                if (dis < 1.5f && rail.railCount > 0)
                {
                    if (Input.GetButtonDown("Jump"))
                    {
                        if (rail.railArray.Count > 0)
                        {
                            for (int i = 0; i < rail.railArray.Count; i++)
                            {
                                MakeMat("CHAN_Prefab/Rail", railArray);
                                railArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                railArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                            }
                            // 플레이어 손상태 변환
                            playerItem.holdState = PlayerItemDown.Hold.Rail;
                            rail.railCount = 0;
                        }
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
                        playerItem.holdState = PlayerItemDown.Hold.Branch;
                        // 바닥상태 변환
                        matGod.matState = MaterialGOD.Materials.None;
                    }
                    #endregion
                    #region Steel
                    // 점프키를 눌렀을 때,
                    if (matGod.matState == MaterialGOD.Materials.Steel)
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
                            playerItem.holdState = PlayerItemDown.Hold.Steel;
                            // 바닥상태 변환
                            matGod.matState = MaterialGOD.Materials.None;
                        
                    }
                    #endregion
                    #region Rail
                    // 점프키를 눌렀을 때,
                    if (matGod.matState == MaterialGOD.Materials.Rail)
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
                        playerItem.holdState = PlayerItemDown.Hold.Rail;
                        // 바닥상태 변환
                        matGod.matState = MaterialGOD.Materials.None;
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

    void DeleteBranch()
    {
        matGod.branchCount = branchArray.Count;
        matGod.matState = MaterialGOD.Materials.Branch;
        // 바닥에 손에 있는 개수만큼 쌓임
        // 손에 있는 모든 것들이 제거
        for (int i = 0; i < branchArray.Count; i++)
        {
            Destroy(branchArray[i].gameObject);
        }
        branchArray.Clear();
    }
    void DeleteSteel()
    {
        matGod.steelCount = steelArray.Count;
        matGod.matState = MaterialGOD.Materials.Steel;
        // 바닥에 손에 있는 개수만큼 쌓임
        // 손에 있는 모든 것들이 제거
        for (int i = 0; i < steelArray.Count; i++)
        {
            Destroy(steelArray[i].gameObject);
        }
        steelArray.Clear();
    }
    void DeleteRail()
    {
        matGod.railCount = railArray.Count;
        matGod.matState = MaterialGOD.Materials.Rail;
        // 바닥에 손에 있는 개수만큼 쌓임
        // 손에 있는 모든 것들이 제거
        for (int i = 0; i < railArray.Count; i++)
        {
            Destroy(railArray[i].gameObject);
        }
        railArray.Clear();
    }

    public void AddRail()
    {
        GameObject rail = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail"));
        railArray.Add(rail);
    }
    public void RemoveRail()
    {
        if (railArray.Count > 0)
        {
            GameObject rail = railArray[railArray.Count - 1];
            railArray.RemoveAt(railArray.Count - 1);
            Destroy(rail);
        }
    }
}
