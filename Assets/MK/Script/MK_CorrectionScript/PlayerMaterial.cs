using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ material�� �Ǻ�
#region material �Ǻ� ���� 
// �÷��̾ Ray�� ��� �ٴѴ�
// ���� �÷��̾ �տ� branch�� steel�� ��� �ִٸ� 
// branch�� steel�� ���� ���δ�
// �ƴ� ���
// ����Ű�� ������ 
// ������Ʈ�� ����ȴ�
#endregion

#region �߰� ���� �� ���� ����
// + 09.15 ��������
// : �÷��̾ �տ� ������ ��� ���� ��, �ٴ� ���µ� Branch��� �ٴ� ������ ������ ���� �տ� ��� �ִ� ���� �����
#endregion
public class PlayerMaterial : MonoBehaviourPun
{
    // ����Ʈ
    // ��������
    public List<GameObject> branchArray = new List<GameObject>();
    // ö
    public List<GameObject> steelArray = new List<GameObject>();
    // ����
    public List<GameObject> railArray = new List<GameObject>();
    // ������ ��ġ
    public Transform itemPos;
    // ���� ��ġ
    public Transform rayPos;
    // ����� Ŭ��
    public AudioClip[] audioClip;

    AudioSource audioSource;

    // MaterialGod ������Ʈ
    MaterialGOD matGod;
    // ToolGod ������Ʈ
    ToolGOD toolGOD;
    // ItemGod ������Ʈ
    ItemGOD itemGOD;
    // ��
    RiverGOD riverGOD;

    // �÷��̾� ����
    PlayerForwardRay playerRay;
    PlayerAnim anim;

    // �÷��̾� �� ���� ������Ʈ
    PlayerItemDown playerItem;
    // ���� ��ġ
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
    // �������� �Ÿ�
    float dis;
    float dis1;
    // ���� 
    int countRail;
    int countRailArray;

    int bCount;
    int sCount;

    // Start is called before the first frame update
    void Start()
    {
        // ������ ���� : ������ �ƴϸ� ��ȯ
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
            // RailTrain���� �Ÿ��� ������ 
            dis = Vector3.Distance(railtrain.transform.position, transform.position);
            // �������� �Ÿ� �Ǻ�
            dis1 = Vector3.Distance(transform.position, matTrain.position);
            rail = matTrain.GetComponent<Maker>();
            // ���� ��ġ
            //rail = matTrain.GetComponent<MixedItem>();
            layer = 1 << 10;
            // �÷��̾ ���̸� �߻��Ѵ�
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
                    // ���� ��

                    #region branch�� �տ� �ִ� ���
                    if (branchArray.Count > 0)
                    {

                        // �������� �Ÿ��� ������
                        if (dis1 < 2)
                        {
                            // ���� ���� ���� ���� ����   
                            if (branchArray.Count > 0 && bCount <= 0)
                            {
                                rail.CreateMat(0, branchArray.Count, bCount, "MK_Prefab/Branch", 0);
                            }
                            // rail.branchCount = branchArray.Count;
                            // ���� ���� branch �ױ�
                            DeleteMat(0, 0);
                            playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                        }
                        // �տ� ���� ��� �ְ� �������� �Ÿ��� �ָ�
                        else
                        {

                            if (checkCount > 0)
                            {
                                // �ٴ� ���°� Steel�϶�
                                if (matGod.matState == MaterialGOD.Materials.Steel)
                                {
                                    ChangeMats("MK_Prefab/Steel", checkCount, 1);
                                    matGod.DeletMaterial();
                                    matGod.ChangeMaterial(MaterialGOD.Materials.Branch, branchArray.Count);
                                    DeleteMat(0, 0);
                                    playerItem.holdState = PlayerItemDown.Hold.Mat;
                                }
                                // �ٴ� ���°� Rail �϶�
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
                            // ToolGOD�� ���� ����
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
                            // ����Ű�� ������ ��,
                            if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                            {
                                ChangeState(PlayerItemDown.Hold.Pail, ToolGOD.Tools.Idle, MaterialGOD.Materials.Branch, branchArray.Count);
                                DeleteMat(0, 0);
                                playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                            }


                        }
                    }
                    #endregion
                    #region Steel�� �տ� �ִ� ���
                    else if (steelArray.Count > 0)
                    {
                        // �������� �Ÿ��� ������
                        if (dis1 < 2)
                        {
                            // rail.steelCount = steelArray.Count;
                            if (steelArray.Count > 0 && sCount <= 0)
                            {
                                rail.CreateMat(1, steelArray.Count, sCount, "MK_Prefab/Steel", 1);
                            }
                            // ���� ���� branch �ױ�
                            DeleteMat(1, 0);
                            playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                        }
                        else
                        {

                            if (checkCount > 0)
                            {
                                // �ٴ� ���°� branch
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
                    #region rail�� �տ� �ִ� ���
                    else if (railArray.Count > 0)
                    {
                        if (train_main)
                        {
                            mainDis = Vector3.Distance(train_main.transform.position, transform.position);
                        }

                        // ������ �������� �ƴ� ���
                        if (itemGOD.items == ItemGOD.Items.Rail || itemGOD.items == ItemGOD.Items.CornerRail)
                        {
                            // ����
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
                                        // �տ��� ����
                                        RemoveRail();
                                    }
                                }
                                else
                                {
                                    // �տ��� ����
                                    itemGOD.ChangeState(ItemGOD.Items.Rail);
                                    RemoveRail();
                                }
                            }
                            if (rayInfo.transform.gameObject == connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1].gameObject)
                            {
                                // ����
                                itemGOD.ChangeState(ItemGOD.Items.Idle);
                                connectRail.instance.connectedRails.RemoveAt(connectRail.instance.connectedRails.Count - 1);
                                AddRail();
                            }
                            if (mainDis < 2 && n <= 0)
                            {
                                // �տ��� ����
                                itemGOD.ChangeState(ItemGOD.Items.Rail);
                                RemoveRail();
                                n = 1;
                            }
                            else if (itemGOD.items == ItemGOD.Items.Idle)
                            {
                                if (checkCount > 0)
                                {
                                    // �ٴ� ���°� branch
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
                                // �ٴ� ���°� idle�̶��
                                if (toolGOD.toolsState == ToolGOD.Tools.Idle && matGod.matState == MaterialGOD.Materials.Idle)
                                {
                                    matGod.ChangeMaterial(MaterialGOD.Materials.Rail, railArray.Count);
                                    DeleteMat(2, 0);
                                    if (railArray.Count <= 0) playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                                }
                                // �ٴ� ���°� Ax���
                                if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                                {
                                    ChangeState(PlayerItemDown.Hold.Ax, ToolGOD.Tools.Idle, MaterialGOD.Materials.Rail, railArray.Count);
                                    DeleteMat(2, 0);
                                    playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                                }
                                // �ٴ� ���°� Pick���
                                if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                                {
                                    ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, MaterialGOD.Materials.Rail, railArray.Count);
                                    DeleteMat(2, 0);
                                    playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                                }
                                // �ٴ� ���°� Pail���
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

                    // �տ� ����
                    else
                    {
                        // �������� �Ÿ��� ���� ������ ���
                        if (dis < 1.5f)
                        {
                            countRail = rail.railCount;
                            countRailArray = rail.railArray.Count;
                            ChangeMats("CHAN_Prefab/Rail", checkCount, 2);
                            // �÷��̾� �ջ��� ��ȯ
                            playerItem.holdState = PlayerItemDown.Hold.Mat;
                            rail.railCount = 0;

                        }


                        #region Branch 
                        // �ٴ� ���°� Branch���
                        if (matGod.matState == MaterialGOD.Materials.Branch)
                        {
                            // �ٴ� ���� ��ȭ
                            ChangeToolGod();
                            // �ٴڿ� branch�� �������� ���,
                            // ���� �ٴڿ� branch�� 3�� ������ ���
                            if (checkCount > 0)
                            {
                                ChangeMats("MK_Prefab/Branch", checkCount, 0);
                                matGod.DeletMaterial();
                            }
                            // �÷��̾� �ջ��� ��ȯ
                            playerItem.holdState = PlayerItemDown.Hold.Mat;

                            // �ٴڻ��� ��ȯ
                            matGod.ChangeMaterial(MaterialGOD.Materials.None, 1);
                        }
                        #endregion
                        #region Steel
                        // ����Ű�� ������ ��,
                        else if (matGod.matState == MaterialGOD.Materials.Steel)
                        {
                            ChangeToolGod();
                            // Steel�̶��
                            // �ٴڿ� steel�� �������� ���
                            if (checkCount > 0)
                            {
                                ChangeMats("MK_Prefab/Steel", checkCount, 1);
                                matGod.DeletMaterial();
                            }
                            // �÷��̾� �ջ��� ��ȯ
                            playerItem.holdState = PlayerItemDown.Hold.Mat;
                            // �ٴڻ��� ��ȯ
                            matGod.ChangeMaterial(MaterialGOD.Materials.None, steelArray.Count);

                        }
                        #endregion
                        #region Rail
                        // ����Ű�� ������ ��,
                        else if (matGod.matState == MaterialGOD.Materials.Rail)
                        {
                            ChangeToolGod();
                            // rail
                            // �ٴڿ� rail�� �������� ���
                            if (checkCount > 0)
                            {
                                ChangeMats("CHAN_Prefab/Rail", checkCount, 2);
                                matGod.DeletMaterial();

                            }
                            // �÷��̾� �ջ��� ��ȯ
                            playerItem.holdState = PlayerItemDown.Hold.Mat;
                            // �ٴڻ��� ��ȯ
                            matGod.ChangeMaterial(MaterialGOD.Materials.None, 1);
                        }
                        if (itemGOD.items == ItemGOD.Items.Rail || itemGOD.items == ItemGOD.Items.CornerRail)
                        {
                            // Ű�� ������

                            // ����
                            itemGOD.ChangeState(ItemGOD.Items.Idle);
                            connectRail.instance.connectedRails.RemoveAt(connectRail.instance.connectedRails.Count - 1);
                            AddRail();

                        }
                        #endregion

                    }
                }

                // ����Ű�� ������ ���� ���
                else
                {
                    // ������ ��� �ִٸ�
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
                    // ö�� ��� �ִٸ�
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
                    // ������ ��� �ִٸ�
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
    // �տ� ���� ��� ���� ��, �θ��� �Լ�
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
    // �ٴ��� ���� ��� ����
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

        // Array�� �߰��ϱ�
        GameObject mat = Instantiate(Resources.Load<GameObject>(s));
        mat.gameObject.transform.parent = itemPos;
        matArray.Add(mat);
        // ��� �������� �ʰ� ����
        if (matArray.Count == matCount) return;
        audioSource.clip = audioClip[0];
        audioSource.Stop();
        audioSource.Play();
        // �� ��ġ ���� ������ �װ� �����
        for (int i = 0; i < matArray.Count; i++)
        {
            matArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
            matArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
            matCount++;
        }
    }

    // �ٴ��� �ٸ� ��� ����
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
        // �ٴڿ� �տ� �ִ� ������ŭ ����
        // �տ� �ִ� ��� �͵��� ����
        for (int i = 0; i < matArray.Count; i++)
        {
            Destroy(matArray[i].gameObject);
        }
        if(idx == 0)
            matArray.Clear();
    }
    // ���� ��ȭ
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
