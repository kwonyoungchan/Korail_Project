using Photon.Pun;
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
public class PlayerMat : MonoBehaviourPun
{
    // 리스트
    // 나뭇가지
    public List<GameObject> branchArray;
    private void Awake()
    {
        branchArray = new List<GameObject>();
    }
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
    // 강
    RiverGOD riverGOD;

    // 플레이어 레이
    PlayerForwardRay playerRay;

    // 플레이어 손 상태 컴포넌트
    PlayerItemDown playerItem;
    // 기차 위치
    Transform matTrain;
    Maker rail;

    GameObject railtrain;
    GameObject train_main;
    GameObject branch;

    // bool isBranch = true;
    int checkNum;
    int n;
    float mainDis;
    int layer;
    // 기차와의 거리
    float dis;
    float dis1;
    // 레일 
    int countRail;
    int countRailArray;

    // Start is called before the first frame update
    void Start()
    {
        // 움직임 연동 : 내것이 아니면 반환
        playerItem = GetComponent<PlayerItemDown>();
        playerRay = GetComponent<PlayerForwardRay>();
        matTrain = GameObject.Find("train_laugage1").transform;
        railtrain = GameObject.Find("train_laugage2");
        train_main = GameObject.Find("train_main");
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                AddRail();
            }
            if (!matTrain || !railtrain)
            {
                return;
            }
            // RailTrain과의 거리가 가까우면 
            dis = Vector3.Distance(railtrain.transform.position, transform.position);
            // 기차와의 거리 판별
            dis1 = Vector3.Distance(transform.position, matTrain.position);
            rail = matTrain.GetComponent<Maker>();
            // 기차 위치
            //rail = matTrain.GetComponent<MixedItem>();
            layer = 1 << 10;
            // 플레이어가 레이를 발사한다
            Ray pRay = new Ray(rayPos.position + new Vector3(0.2f, 0, 0), -transform.up);
            RaycastHit rayInfo;
            if (Physics.Raycast(pRay, out rayInfo, 10, ~layer))
            {
                matGod = rayInfo.transform.gameObject.GetComponent<MaterialGOD>();
                toolGOD = rayInfo.transform.gameObject.GetComponent<ToolGOD>();
                itemGOD = rayInfo.transform.gameObject.GetComponent<ItemGOD>();
                riverGOD = rayInfo.transform.gameObject.GetComponent<RiverGOD>();

                float railDis = Vector3.Distance(connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1].transform.position, transform.position);

                // 손에 무언갈 들고 있다면
                // 강일 때
                if (riverGOD)
                {
                    // 레일이 손에 있고
                    if (riverGOD.riverState == RiverGOD.River.Bridge)
                    {

                        // 키를 누르면
                        if (Input.GetButtonDown("Jump"))
                        {
                            // 바닥 상태가 bridge일때
                            if (railArray.Count > 0)
                            {
                                if (railDis < 2 && railDis > 0.5f && rayInfo.transform.gameObject != connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1].gameObject)
                                {
                                    // 손에서 제거
                                    itemGOD.ChangeState(ItemGOD.Items.Rail, default, 0.75f);
                                    RemoveRail();
                                }
                                if (rayInfo.transform.gameObject == connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1].gameObject)
                                {
                                    // 제거
                                    itemGOD.ChangeState(ItemGOD.Items.Idle, default, 0.75f);
                                    connectRail.instance.connectedRails.RemoveAt(connectRail.instance.connectedRails.Count - 1);
                                    AddRail();

                                }
                            }
                        }
                    }
                }
                else
                {

                    #region branch가 손에 있는 경우
                    if (branchArray.Count > 0)
                    {
                        // 기차와의 거리가 가까우면
                        if (dis1 < 2)
                        {
                            if (Input.GetButtonDown("Jump"))
                            {
                                rail.branchCount = branchArray.Count;
                                // 기차 위에 branch 쌓기
                                DeleteMat(branchArray);
                                playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                            }
                        }
                        // 손에 무언갈 들고 있고 기차와의 거리와 멀면
                        else
                        {
                            if (rayInfo.transform.name == "Bridge") return;
                            if (matGod.matState == MaterialGOD.Materials.Branch)
                            {
                                MakeMat("MK_Prefab/Branch", branchArray);

                            }
                            // 점프키를 눌렀을 때,
                            if (Input.GetButtonDown("Jump"))
                            {
                                if (matGod.matCount > 0)
                                {
                                    // 바닥 상태가 Steel일때
                                    if (matGod.matState == MaterialGOD.Materials.Steel)
                                    {
                                        ChangeMats(matGod, "MK_Prefab/Steel", steelArray, MaterialGOD.Materials.Branch, branchArray);
                                        DeleteMat(branchArray);
                                    }
                                    // 바닥 상태가 Rail 일때
                                    if (matGod.matState == MaterialGOD.Materials.Rail)
                                    {
                                        ChangeMats(matGod, "CHAN_Prefab/Rail", railArray, MaterialGOD.Materials.Branch, branchArray);
                                        DeleteMat(branchArray);
                                    }
                                }
                                // ToolGOD에 따라 변경
                                if (toolGOD.toolsState == ToolGOD.Tools.Idle && matGod.matState == MaterialGOD.Materials.Idle)
                                {
                                    matGod.ChangeMaterial(MaterialGOD.Materials.Branch, branchArray.Count);
                                    DeleteMat(branchArray);
                                }
                                if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                                {
                                    ChangeState(PlayerItemDown.Hold.Ax, ToolGOD.Tools.Idle, MaterialGOD.Materials.Branch, branchArray.Count);
                                    DeleteMat(branchArray);
                                }
                                if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                                {
                                    ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, MaterialGOD.Materials.Branch, branchArray.Count);
                                    DeleteMat(branchArray);
                                }
                                // 점프키를 눌렀을 때,
                                if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                                {
                                    ChangeState(PlayerItemDown.Hold.Pail, ToolGOD.Tools.Idle, MaterialGOD.Materials.Branch, branchArray.Count);
                                    DeleteMat(branchArray);
                                }
                            }

                        }
                    }
                    #endregion
                    #region Steel가 손에 있는 경우
                    else if (steelArray.Count > 0)
                    {
                        if (rayInfo.transform.name == "Bridge") return;

                        // 기차와의 거리가 가까우면
                        if (dis1 < 2)
                        {
                            if (Input.GetButtonDown("Jump"))
                            {
                                rail.steelCount = steelArray.Count;
                                // 기차 위에 branch 쌓기
                                DeleteMat(steelArray);
                                playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                            }
                        }
                        else
                        {
                            if (riverGOD)
                            {
                                return;
                            }
                            // 바닥 상태가 Steel 상태라면
                            if (matGod.matState == MaterialGOD.Materials.Steel)
                            {
                                MakeMat("MK_Prefab/Steel", steelArray);
                            }
                            if (Input.GetButtonDown("Jump"))
                            {
                                if (matGod.matCount > 0)
                                {
                                    // 바닥 상태가 branch
                                    if (matGod.matState == MaterialGOD.Materials.Branch)
                                    {
                                        ChangeMats(matGod, "MK_Prefab/Steel", branchArray, MaterialGOD.Materials.Steel, steelArray);
                                        DeleteMat(steelArray);
                                    }

                                    if (matGod.matState == MaterialGOD.Materials.Rail)
                                    {
                                        ChangeMats(matGod, "CHAN_Prefab/Rail", railArray, MaterialGOD.Materials.Steel, steelArray);
                                        DeleteMat(steelArray);
                                    }

                                }

                                if (toolGOD.toolsState == ToolGOD.Tools.Idle && matGod.matState == MaterialGOD.Materials.Idle)
                                {
                                    matGod.ChangeMaterial(MaterialGOD.Materials.Steel, steelArray.Count);
                                    DeleteMat(steelArray);
                                }
                                if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                                {
                                    ChangeState(PlayerItemDown.Hold.Ax, ToolGOD.Tools.Idle, MaterialGOD.Materials.Steel, steelArray.Count);
                                    DeleteMat(steelArray);
                                }
                                if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                                {
                                    ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, MaterialGOD.Materials.Steel, steelArray.Count);
                                    DeleteMat(steelArray);
                                }
                                if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                                {
                                    ChangeState(PlayerItemDown.Hold.Pail, ToolGOD.Tools.Idle, MaterialGOD.Materials.Steel, steelArray.Count);
                                    DeleteMat(steelArray);
                                }
                            }

                        }

                    }
                    #endregion
                    #region rail이 손에 있는 경우
                    else if (railArray.Count > 0)
                    {
                        if (rayInfo.transform.name == "Bridge") return;

                        if (train_main)
                        {
                            mainDis = Vector3.Distance(train_main.transform.position, transform.position);
                        }

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
                            if (mainDis < 2 && n <= 0)
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
                                if (Input.GetButtonDown("Jump"))
                                {
                                    if (matGod.matCount > 0)
                                    {
                                        // 바닥 상태가 branch
                                        if (matGod.matState == MaterialGOD.Materials.Branch)
                                        {
                                            ChangeMats(matGod, "MK_Prefab/Branch", branchArray, MaterialGOD.Materials.Rail, railArray);
                                            DeleteMat(railArray);

                                        }
                                        if (matGod.matState == MaterialGOD.Materials.Steel)
                                        {
                                            ChangeMats(matGod, "MK_Prefab/Steel", steelArray, MaterialGOD.Materials.Rail, railArray);
                                            DeleteMat(railArray);
                                        }
                                        playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                                    }
                                    // 바닥 상태가 idle이라면
                                    if (toolGOD.toolsState == ToolGOD.Tools.Idle && matGod.matState == MaterialGOD.Materials.Idle)
                                    {
                                        matGod.ChangeMaterial(MaterialGOD.Materials.Rail, railArray.Count);
                                        DeleteMat(railArray);
                                    }
                                    // 바닥 상태가 Ax라면
                                    if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                                    {
                                        ChangeState(PlayerItemDown.Hold.Ax, ToolGOD.Tools.Idle, MaterialGOD.Materials.Rail, railArray.Count);
                                        DeleteMat(railArray);
                                    }
                                    // 바닥 상태가 Pick라면
                                    if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                                    {
                                        ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, MaterialGOD.Materials.Rail, railArray.Count);
                                        DeleteMat(railArray);
                                    }
                                    // 바닥 상태가 Pail라면
                                    if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                                    {
                                        ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, MaterialGOD.Materials.Rail, railArray.Count);
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
                        // 기차와의 거리를 따져 레일을 들기
                        if (dis < 1.5f)
                        {
                            countRail = rail.railCount;
                            countRailArray = rail.railArray.Count;
                            if (Input.GetButtonDown("Jump") && countRail > 0)
                            {
                                for (int i = 0; i < countRailArray; i++)
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
                                // 바닥 상태 변화
                                ChangeToolGod();
                                // 바닥에 branch가 여러개인 경우,
                                // 만약 바닥에 branch가 3개 이하인 경우
                                if (matGod.matCount > 0)
                                {
                                    for (int i = 0; i < matGod.matCount; i++)
                                    {
                                        // Array에 추가하기
                                        GameObject mat = Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"), itemPos);
                                        branchArray.Add(mat);
                                        branchArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                        branchArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                                    }

                                }
                                // 플레이어 손상태 변환
                                playerItem.holdState = PlayerItemDown.Hold.Mat;

                                // 바닥상태 변환
                                matGod.ChangeMaterial(MaterialGOD.Materials.None, 1);
                            }
                            #endregion
                            #region Steel
                            // 점프키를 눌렀을 때,
                            else if (matGod.matState == MaterialGOD.Materials.Steel)
                            {
                                ChangeToolGod();
                                // Steel이라면
                                // 바닥에 steel이 여러개인 경우
                                if (matGod.matCount > 0)
                                {
                                    for (int i = 0; i < matGod.matCount; i++)
                                    {
                                        GameObject mat = Instantiate(Resources.Load<GameObject>("MK_Prefab/Steel"), itemPos);
                                        steelArray.Add(mat);
                                        steelArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                        steelArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                                    }

                                }
                                // 플레이어 손상태 변환
                                playerItem.holdState = PlayerItemDown.Hold.Mat;
                                // 바닥상태 변환
                                matGod.ChangeMaterial(MaterialGOD.Materials.None, steelArray.Count);

                            }
                            #endregion
                            #region Rail
                            // 점프키를 눌렀을 때,
                            else if (matGod.matState == MaterialGOD.Materials.Rail)
                            {
                                ChangeToolGod();
                                // rail
                                // 바닥에 rail이 여러개인 경우
                                if (matGod.matCount > 0)
                                {
                                    for (int i = 0; i < matGod.matCount; i++)
                                    {
                                        GameObject mat = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail"));
                                        mat.transform.parent = itemPos;
                                        railArray.Add(mat);
                                        railArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                        railArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                                    }

                                }
                                // 플레이어 손상태 변환
                                playerItem.holdState = PlayerItemDown.Hold.Mat;
                                // 바닥상태 변환
                                matGod.ChangeMaterial(MaterialGOD.Materials.None, 1);
                            }
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
                            #endregion
                        }
                    }
                }
            }
        }
        // 동기화
        else
        {
            // 손 위에 올릴 아이템 위치
            /*itemPos.transform.position = Vector3.Lerp(itemPos.transform.position, iPos, 5 * Time.deltaTime);
            itemPos.transform.rotation = Quaternion.Lerp(itemPos.transform.rotation, iPosRot, 5 * Time.deltaTime);*/
            // 손에 무언가가 있을때
            if (playerItem.holdState == PlayerItemDown.Hold.Mat)
            {
                // 나무가 있다면
                if (brnCount > 0)
                {
                    if (brnCount == branchArray.Count) return;
                    MakeMat("MK_Prefab/Branch", branchArray);
                    checkNum = 1;
                }

                if (steelCount > 0)
                {
                    if (steelCount == steelArray.Count) return;
                    MakeMat("MK_Prefab/Steel", steelArray);
                    checkNum = 2;
                }

                if (railCount > 0)
                {
                    if (railCount == railArray.Count) return;
                    MakeMat("CHAN_Prefab/Rail", railArray);
                    checkNum = 3;
                }


            }
            if (brnCount <= 0 && checkNum == 1)
            {
                DeleteMat(branchArray);
                checkNum = 0;
            }
            else if (steelCount <= 0 && checkNum == 2)
            {
                DeleteMat(steelArray);
                checkNum = 0;
            }
            else if (railCount <= 0 && checkNum == 3)
            {
                DeleteMat(railArray);
                checkNum = 0;
            }
        }

    }

    // 손에 무언가 들고 있을 때, 부르는 함수
    void ChangeToolGod()
    {
        if (playerItem.holdState != PlayerItemDown.Hold.Idle)
        {
            if (playerItem.holdState == PlayerItemDown.Hold.Ax)
            {
                toolGOD.ChangeState(ToolGOD.Tools.Ax);
            }
            if (playerItem.holdState == PlayerItemDown.Hold.Pick)
            {
                toolGOD.ChangeState(ToolGOD.Tools.Pick);
            }
            if (playerItem.holdState == PlayerItemDown.Hold.Pail)
            {
                toolGOD.ChangeState(ToolGOD.Tools.Pail);
            }
        }
    }

    int matCount;
    // 바닥이 같은 경우 생성
    void MakeMat(string s, List<GameObject> matArray)
    {
        // 계속 생성되지 않게 만들
        if (matArray.Count == matCount) return;
        // Array에 추가하기
        GameObject mat = Instantiate(Resources.Load<GameObject>(s));
        mat.transform.parent = itemPos;
        matArray.Add(mat);
        // 손 위치 위로 아이템 쌓게 만들기
        for (int i = 0; i < matArray.Count; i++)
        {
            matArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
            matArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
            matCount++;
        }
        matGod.ChangeMaterial(MaterialGOD.Materials.None, 1);
    }

    // 바닥이 다른 경우 생성
    void ChangeMats(MaterialGOD god, string s, List<GameObject> matArray, MaterialGOD.Materials state, List<GameObject> changeMatArray)
    {
        for (int i = 0; i < god.matCount; i++)
        {
            GameObject mat = Instantiate(Resources.Load<GameObject>(s));
            mat.transform.parent = itemPos;
            matArray.Add(mat);
            matArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
            matArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
        }
        for (int i = 0; i < god.mat.Count; i++)
        {
            Destroy(god.mat[i]);
        }
        god.mat.Clear();
        // god.matCount = changeMatArray.Count;
        god.ChangeMaterial(state, changeMatArray.Count);

    }


    void DeleteMat(List<GameObject> matArray)
    {
        // 바닥에 손에 있는 개수만큼 쌓임
        // 손에 있는 모든 것들이 제거
        for (int i = 0; i < matArray.Count; i++)
        {
            Destroy(matArray[i].gameObject);
        }

        if (matArray.Count <= 0)
        {
            playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
        }
        matArray.Clear();
    }
    // 상태 변화
    void ChangeState(PlayerItemDown.Hold player, ToolGOD.Tools tool, MaterialGOD.Materials mats, int count)
    {
        playerItem.holdState = player;
        toolGOD.ChangeState(tool);
        matGod.ChangeMaterial(mats, count);
    }
    public void AddRail()
    {
        photonView.RPC("PunAddRail", RpcTarget.All);
    }
    [PunRPC]
    void PunAddRail()
    {
        GameObject rail = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail"));
        rail.gameObject.transform.parent = itemPos;
        railArray.Add(rail);
        for (int i = 0; i < railArray.Count; i++)
        {
            railArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
        }
        if (railArray.Count > 0)
        {
            playerItem.holdState = PlayerItemDown.Hold.Mat;
        }
        else
        {
            playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
        }
    }
    public void RemoveRail()
    {
        photonView.RPC("PunRemoveRail", RpcTarget.All);
    }

    [PunRPC]
    void PunRemoveRail()
    {
        if (railArray.Count > 0)
        {
            GameObject rail = railArray[railArray.Count - 1];
            railArray.RemoveAt(railArray.Count - 1);
            Destroy(rail);
            if (railArray.Count > 0)
            {
                playerItem.holdState = PlayerItemDown.Hold.Mat;
            }
            else
            {
                playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
            }
        }
    }
    public void RemoveBranch()
    {
        if (branchArray.Count > 0)
        {
            GameObject branch = branchArray[branchArray.Count - 1];
            branchArray.RemoveAt(branchArray.Count - 1);
            Destroy(branch);
            if (branchArray.Count > 0)
            {
                playerItem.holdState = PlayerItemDown.Hold.Mat;
            }
            else
            {
                playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
            }
        }
    }

    Vector3 iPos;
    Quaternion iPosRot;
    int brnCount;
    int steelCount;
    int railCount;
}
