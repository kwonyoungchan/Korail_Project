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
    // 오디오 클립
    public AudioClip[] audioClip;

    AudioSource audioSource;

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
    PlayerAnim anim;

    // 플레이어 손 상태 컴포넌트
    PlayerItemDown playerItem;
    // 기차 위치
    Transform matTrain;
    Maker rail;
    PailItem pail;

    GameObject railtrain;
    GameObject train_main; 
    GameObject branch;

    // bool isBranch = true;
    int checkCount;
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

    int bCount;
    int sCount;

    // Start is called before the first frame update
    void Start()
    {
        // 움직임 연동 : 내것이 아니면 반환
        playerItem = GetComponent<PlayerItemDown>();
        playerRay = GetComponent<PlayerForwardRay>();
        matTrain = GameObject.Find("train_laugage1").transform;        
        railtrain = GameObject.Find("train_laugage2");
        train_main = GameObject.Find("train_main");
        anim = GetComponent<PlayerAnim>();
        audioSource = GetComponent<AudioSource>();
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
                pail = rayInfo.transform.gameObject.GetComponent<PailItem>();
                
                if (matGod)
                    checkCount = matGod.matCount;
                float railDis = Vector3.Distance(connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1].transform.position, transform.position);
                if (Input.GetButtonDown("Jump"))
                {
                   
                    if (riverGOD)
                    {
                        matGod.y = 0.55f + 0.2f;
                        toolGOD.y = 0.55f + 0.2f;
                    }
                    else
                    {
                        matGod.y = 0.55f;
                        toolGOD.y = 0.55f;
                    }
                    // 강일 때

                    #region branch가 손에 있는 경우
                    if (branchArray.Count > 0)
                    {

                        // 기차와의 거리가 가까우면
                        if (dis1 < 2)
                        {
                            // 기차 위에 나무 개수 증가   
                            if (branchArray.Count > 0 && bCount <= 0)
                            {
                                rail.CreateMat(0, branchArray.Count, bCount, "MK_Prefab/Branch", 0);
                            }
                            // rail.branchCount = branchArray.Count;
                            // 기차 위에 branch 쌓기
                            DeleteMat(0, 0);
                            playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                        }
                        // 손에 무언갈 들고 있고 기차와의 거리와 멀면
                        else
                        {

                            if (checkCount > 0)
                            {
                                // 바닥 상태가 Steel일때
                                if (matGod.matState == MaterialGOD.Materials.Steel)
                                {
                                    ChangeMats("MK_Prefab/Steel", checkCount, 1);
                                    matGod.DeletMaterial();
                                    matGod.ChangeMaterial(MaterialGOD.Materials.Branch, branchArray.Count);
                                    DeleteMat(0, 0);
                                    playerItem.holdState = PlayerItemDown.Hold.Mat;
                                }
                                // 바닥 상태가 Rail 일때
                                if (matGod.matState == MaterialGOD.Materials.Rail)
                                {
                                    ChangeMats("CHAN_Prefab/Rail", checkCount, 2);
                                    /* for (int i = 0; i < matGod.mat.Count; i++)
                                     {
                                         Destroy(matGod.mat[i]);
                                     }
                                     matGod.mat.Clear();*/
                                    matGod.DeletMaterial();
                                    matGod.ChangeMaterial(MaterialGOD.Materials.Branch, branchArray.Count);
                                    DeleteMat(0, 0);
                                    playerItem.holdState = PlayerItemDown.Hold.Mat;
                                }
                            }
                            // ToolGOD에 따라 변경
                            if (toolGOD.toolsState == ToolGOD.Tools.Idle && matGod.matState == MaterialGOD.Materials.Idle && playerRay.isBranch == false)
                            {
                                matGod.ChangeMaterial(MaterialGOD.Materials.Branch, branchArray.Count);
                                DeleteMat(0, 0);
                                playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                            }
                            if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                            {
                                ChangeState(PlayerItemDown.Hold.Ax, ToolGOD.Tools.Idle, MaterialGOD.Materials.Branch, branchArray.Count);
                                DeleteMat(0, 0);
                                playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                            }
                            if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                            {
                                ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, MaterialGOD.Materials.Branch, branchArray.Count);
                                DeleteMat(0, 0);
                                playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                            }
                            // 점프키를 눌렀을 때,
                            if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                            {
                                ChangeState(PlayerItemDown.Hold.Pail, ToolGOD.Tools.Idle, MaterialGOD.Materials.Branch, branchArray.Count);
                                DeleteMat(0, 0);
                                playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                            }


                        }
                    }
                    #endregion
                    #region Steel가 손에 있는 경우
                    else if (steelArray.Count > 0)
                    {
                        // 기차와의 거리가 가까우면
                        if (dis1 < 2)
                        {
                            // rail.steelCount = steelArray.Count;
                            if (steelArray.Count > 0 && sCount <= 0)
                            {
                                rail.CreateMat(1, steelArray.Count, sCount, "MK_Prefab/Steel", 1);
                            }
                            // 기차 위에 branch 쌓기
                            DeleteMat(1, 0);
                            playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                        }
                        else
                        {

                            if (checkCount > 0)
                            {
                                // 바닥 상태가 branch
                                if (matGod.matState == MaterialGOD.Materials.Branch)
                                {
                                    ChangeMats("MK_Prefab/Branch", checkCount, 0);
                                    matGod.DeletMaterial();
                                    matGod.ChangeMaterial(MaterialGOD.Materials.Steel, steelArray.Count);
                                    DeleteMat(1, 0);
                                    playerItem.holdState = PlayerItemDown.Hold.Mat;
                                }

                                if (matGod.matState == MaterialGOD.Materials.Rail)
                                {
                                    ChangeMats("CHAN_Prefab/Rail", checkCount, 2);
                                    matGod.DeletMaterial();
                                    matGod.ChangeMaterial(MaterialGOD.Materials.Steel, steelArray.Count);
                                    DeleteMat(1, 0);
                                    playerItem.holdState = PlayerItemDown.Hold.Mat;
                                }

                            }

                            if (toolGOD.toolsState == ToolGOD.Tools.Idle && matGod.matState == MaterialGOD.Materials.Idle)
                            {
                                matGod.ChangeMaterial(MaterialGOD.Materials.Steel, steelArray.Count);
                                DeleteMat(1, 0);
                                playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                            }
                            if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                            {
                                ChangeState(PlayerItemDown.Hold.Ax, ToolGOD.Tools.Idle, MaterialGOD.Materials.Steel, steelArray.Count);
                                DeleteMat(1, 0);
                                playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                            }
                            if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                            {
                                ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, MaterialGOD.Materials.Steel, steelArray.Count);
                                DeleteMat(1, 0);
                                playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                            }
                            if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                            {
                                ChangeState(PlayerItemDown.Hold.Pail, ToolGOD.Tools.Idle, MaterialGOD.Materials.Steel, steelArray.Count);
                                DeleteMat(1, 0);
                                playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                            }


                        }

                    }
                    #endregion
                    #region rail이 손에 있는 경우
                    else if (railArray.Count > 0)
                    {
                        if (train_main)
                        {
                            mainDis = Vector3.Distance(train_main.transform.position, transform.position);
                        }

                        // 레일이 마지막이 아닌 경우
                        if (itemGOD.items == ItemGOD.Items.Rail || itemGOD.items == ItemGOD.Items.CornerRail)
                        {
                            // 제거
                            itemGOD.ChangeState(ItemGOD.Items.Idle);
                            connectRail.instance.connectedRails.RemoveAt(connectRail.instance.connectedRails.Count - 1);
                            AddRail();
                        }
                        else
                        {
                            if (railDis < 2 && railDis > 0.5f && rayInfo.transform.gameObject != connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1].gameObject)
                            {
                                if (riverGOD)
                                {
                                    if (riverGOD.riverState == RiverGOD.River.Bridge)
                                    {
                                        itemGOD.ChangeState(ItemGOD.Items.Rail, default, 0.75f);
                                        // 손에서 제거
                                        RemoveRail();
                                    }
                                }
                                else
                                {
                                    // 손에서 제거
                                    itemGOD.ChangeState(ItemGOD.Items.Rail);
                                    RemoveRail();
                                }
                            }
                            if (rayInfo.transform.gameObject == connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1].gameObject)
                            {
                                // 제거
                                itemGOD.ChangeState(ItemGOD.Items.Idle);
                                connectRail.instance.connectedRails.RemoveAt(connectRail.instance.connectedRails.Count - 1);
                                AddRail();
                            }
                            if (mainDis < 2 && n <= 0)
                            {
                                // 손에서 제거
                                itemGOD.ChangeState(ItemGOD.Items.Rail);
                                RemoveRail();
                                n = 1;
                            }
                            else if (itemGOD.items == ItemGOD.Items.Idle)
                            {
                                if (checkCount > 0)
                                {
                                    // 바닥 상태가 branch
                                    if (matGod.matState == MaterialGOD.Materials.Branch)
                                    {
                                        ChangeMats("MK_Prefab/Branch", checkCount, 0);
                                        matGod.DeletMaterial();
                                        matGod.ChangeMaterial(MaterialGOD.Materials.Rail, railArray.Count);
                                        DeleteMat(2, 0);
                                        playerItem.holdState = PlayerItemDown.Hold.Mat;
                                    }
                                    if (matGod.matState == MaterialGOD.Materials.Steel)
                                    {
                                        ChangeMats("MK_Prefab/Steel", checkCount, 1);
                                        matGod.DeletMaterial();
                                        matGod.ChangeMaterial(MaterialGOD.Materials.Rail, railArray.Count);
                                        DeleteMat(2, 0);
                                        playerItem.holdState = PlayerItemDown.Hold.Mat;
                                    }
                                }
                                // 바닥 상태가 idle이라면
                                if (toolGOD.toolsState == ToolGOD.Tools.Idle && matGod.matState == MaterialGOD.Materials.Idle)
                                {
                                    matGod.ChangeMaterial(MaterialGOD.Materials.Rail, railArray.Count);
                                    DeleteMat(2, 0);
                                    if (railArray.Count <= 0) playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                                }
                                // 바닥 상태가 Ax라면
                                if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                                {
                                    ChangeState(PlayerItemDown.Hold.Ax, ToolGOD.Tools.Idle, MaterialGOD.Materials.Rail, railArray.Count);
                                    DeleteMat(2, 0);
                                    playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                                }
                                // 바닥 상태가 Pick라면
                                if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                                {
                                    ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, MaterialGOD.Materials.Rail, railArray.Count);
                                    DeleteMat(2, 0);
                                    playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                                }
                                // 바닥 상태가 Pail라면
                                if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                                {
                                    ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, MaterialGOD.Materials.Rail, railArray.Count);
                                    DeleteMat(2, 0);
                                    playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
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
                            ChangeMats("CHAN_Prefab/Rail", checkCount, 2);
                            // 플레이어 손상태 변환
                            playerItem.holdState = PlayerItemDown.Hold.Mat;
                            rail.railCount = 0;

                        }


                        #region Branch 
                        // 바닥 상태가 Branch라면
                        if (matGod.matState == MaterialGOD.Materials.Branch)
                        {
                            // 바닥 상태 변화
                            ChangeToolGod();
                            // 바닥에 branch가 여러개인 경우,
                            // 만약 바닥에 branch가 3개 이하인 경우
                            if (checkCount > 0)
                            {
                                ChangeMats("MK_Prefab/Branch", checkCount, 0);
                                matGod.DeletMaterial();
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
                            if (checkCount > 0)
                            {
                                ChangeMats("MK_Prefab/Steel", checkCount, 1);
                                matGod.DeletMaterial();
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
                            if (checkCount > 0)
                            {
                                ChangeMats("CHAN_Prefab/Rail", checkCount, 2);
                                matGod.DeletMaterial();

                            }
                            // 플레이어 손상태 변환
                            playerItem.holdState = PlayerItemDown.Hold.Mat;
                            // 바닥상태 변환
                            matGod.ChangeMaterial(MaterialGOD.Materials.None, 1);
                        }
                        if (itemGOD.items == ItemGOD.Items.Rail || itemGOD.items == ItemGOD.Items.CornerRail)
                        {
                            // 키를 누르면

                            // 제거
                            itemGOD.ChangeState(ItemGOD.Items.Idle);
                            connectRail.instance.connectedRails.RemoveAt(connectRail.instance.connectedRails.Count - 1);
                            AddRail();

                        }
                        #endregion

                    }
                }

                // 점프키를 누르지 않은 경우
                else
                {
                    // 나무를 들고 있다면
                    if (branchArray.Count > 0)
                    {
                        if (matGod.matState == MaterialGOD.Materials.Branch)
                        {
                            if (matGod.matCount <= 1)
                            {
                                MakeMat("MK_Prefab/Branch", 0);
                                matGod.ChangeMaterial(MaterialGOD.Materials.None, 1);
                            }
                            else
                            {
                                DeleteMat(0, 1);
                                ChangeMats("MK_Prefab/Branch", (matGod.matCount + branchArray.Count), 0);
                                matGod.ChangeMaterial(MaterialGOD.Materials.None, 1);
                            }
                        }
                    }
                    // 철을 들고 있다면
                    if (steelArray.Count > 0)
                    {
                        if (matGod.matState == MaterialGOD.Materials.Steel)
                        {
                            if (matGod.matCount <= 1)
                            {
                                MakeMat("MK_Prefab/Steel", 1);
                                matGod.ChangeMaterial(MaterialGOD.Materials.None, 1);
                            }
                            else
                            {
                                DeleteMat(1, 1);
                                ChangeMats("MK_Prefab/Steel", (matGod.matCount + steelArray.Count), 1);
                                matGod.ChangeMaterial(MaterialGOD.Materials.None, 1);
                            }
                        }
                    }
                    // 레일을 들고 있다면
                    if (railArray.Count > 0)
                    {
                        if (matGod.matState == MaterialGOD.Materials.Rail)
                        {
                            if (matGod.matCount <= 1)
                            {
                                MakeMat("CHAN_Prefab/Rail", 2);
                                matGod.ChangeMaterial(MaterialGOD.Materials.None, 1);
                            }
                            else
                            {
                                DeleteMat(2, 1);
                                ChangeMats("CHAN_Prefab/Rail", (matGod.matCount + railArray.Count), 2);
                                matGod.ChangeMaterial(MaterialGOD.Materials.None, 1);
                            }
                        }
                    }
                }
            }
            //}
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

    List<GameObject> matArray = new List<GameObject>();
    int matCount;
    // 바닥이 같은 경우 생성
    void MakeMat(string s, int index)
    {
        photonView.RPC("PunMakeMat", RpcTarget.All, s, index);
    }
    int matCnt;
    [PunRPC]
    void PunMakeMat(string s, int index)
    {

        if (index == 0)
        {
            matArray = branchArray;
        }
        else if(index == 1)
        {
            matArray = steelArray;
        }
        else
        {
            matArray = railArray;
        }

        // Array에 추가하기
        GameObject mat = Instantiate(Resources.Load<GameObject>(s));
        mat.gameObject.transform.parent = itemPos;
        matArray.Add(mat);
        // 계속 생성되지 않게 만들
        if (matArray.Count == matCount) return;
        audioSource.clip = audioClip[0];
        audioSource.Stop();
        audioSource.Play();
        // 손 위치 위로 아이템 쌓게 만들기
        for (int i = 0; i < matArray.Count; i++)
        {
            matArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
            matArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
            matCount++;
        }
    }

    // 바닥이 다른 경우 생성
    void ChangeMats( string s, int count, int index)
    {
        photonView.RPC("PunChangeMats", RpcTarget.All, s, count, index);
    }
    [PunRPC]
    void PunChangeMats( string s, int count, int index)
    {
        if (index == 0)
        {
            matArray = branchArray;
            matArray.Clear();
        }
        else if (index == 1)
        {
            matArray = steelArray;
            matArray.Clear();
        }
        else
        {
            matArray = railArray;
            matArray.Clear();
        }
        for (int i = 0; i < count; i++)
        {
            GameObject mat = Instantiate(Resources.Load<GameObject>(s));
            mat.transform.parent = itemPos;
            matArray.Add(mat);
            matArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
            matArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
            audioSource.clip = audioClip[0];
            audioSource.Stop();
            audioSource.Play();
        }

    }

    void DeleteMat(int index, int idx)
    {
        photonView.RPC("PunDeleteMat", RpcTarget.All, index, idx);
    }
    [PunRPC]
    void PunDeleteMat(int index, int idx)
    {
        if (index == 0)
        {
            matArray = branchArray;
        }
        else if (index == 1)
        {
            matArray = steelArray;
        }
        else
        {
            matArray = railArray;
        }
        audioSource.clip = audioClip[1];
        audioSource.Stop();
        audioSource.Play();
        // 바닥에 손에 있는 개수만큼 쌓임
        // 손에 있는 모든 것들이 제거
        for (int i = 0; i < matArray.Count; i++)
        {
            Destroy(matArray[i].gameObject);
        }
        if(idx == 0)
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
            audioSource.clip = audioClip[0];
            audioSource.Stop();
            audioSource.Play();
        }
        playerItem.holdState = PlayerItemDown.Hold.Mat;
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
            audioSource.clip = audioClip[1];
            audioSource.Stop();
            audioSource.Play();
        }
        if(railArray.Count <= 0)
        {
            playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
        }
        else
        {
            playerItem.holdState = PlayerItemDown.Hold.Mat;
        }
    }

    public void RemoveBranch()
    {
        photonView.RPC("RpcRemoveBranch", RpcTarget.All);
    }

    [PunRPC]
    void RpcRemoveBranch()
    {
        if (branchArray.Count > 0)
        {
            GameObject branch = branchArray[branchArray.Count - 1];
            branchArray.RemoveAt(branchArray.Count - 1);
            Destroy(branch);
            audioSource.clip = audioClip[0];
            audioSource.Stop();
            audioSource.Play();
        }
        if (branchArray.Count <= 0)
        {
            playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
        }
        else
        {
            playerItem.holdState = PlayerItemDown.Hold.Mat;
        }
    }

}
