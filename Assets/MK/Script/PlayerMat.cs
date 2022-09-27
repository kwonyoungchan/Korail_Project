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
public class PlayerMat : MonoBehaviourPun
{
    // ����Ʈ
    // ��������
    public List<GameObject> branchArray;
    private void Awake()
    {
        branchArray = new List<GameObject>();
    }
    // ö
    public List<GameObject> steelArray = new List<GameObject>();
    // ����
    public List<GameObject> railArray = new List<GameObject>();
    // ������ ��ġ
    public Transform itemPos;
    // ���� ��ġ
    public Transform rayPos;

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

    // �÷��̾� �� ���� ������Ʈ
    PlayerItemDown playerItem;
    // ���� ��ġ
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
    // �������� �Ÿ�
    float dis;
    float dis1;
    // ���� 
    int countRail;
    int countRailArray;

    // Start is called before the first frame update
    void Start()
    {
        // ������ ���� : ������ �ƴϸ� ��ȯ
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

                float railDis = Vector3.Distance(connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1].transform.position, transform.position);

                // �տ� ���� ��� �ִٸ�
                // ���� ��
                if (riverGOD)
                {
                    // ������ �տ� �ְ�
                    if (riverGOD.riverState == RiverGOD.River.Bridge)
                    {

                        // Ű�� ������
                        if (Input.GetButtonDown("Jump"))
                        {
                            // �ٴ� ���°� bridge�϶�
                            if (railArray.Count > 0)
                            {
                                if (railDis < 2 && railDis > 0.5f && rayInfo.transform.gameObject != connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1].gameObject)
                                {
                                    // �տ��� ����
                                    itemGOD.ChangeState(ItemGOD.Items.Rail, default, 0.75f);
                                    RemoveRail();
                                }
                                if (rayInfo.transform.gameObject == connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1].gameObject)
                                {
                                    // ����
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

                    #region branch�� �տ� �ִ� ���
                    if (branchArray.Count > 0)
                    {
                        // �������� �Ÿ��� ������
                        if (dis1 < 2)
                        {
                            if (Input.GetButtonDown("Jump"))
                            {
                                rail.branchCount = branchArray.Count;
                                // ���� ���� branch �ױ�
                                DeleteMat(branchArray);
                                playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
                            }
                        }
                        // �տ� ���� ��� �ְ� �������� �Ÿ��� �ָ�
                        else
                        {
                            if (rayInfo.transform.name == "Bridge") return;
                            if (matGod.matState == MaterialGOD.Materials.Branch)
                            {
                                MakeMat("MK_Prefab/Branch", branchArray);

                            }
                            // ����Ű�� ������ ��,
                            if (Input.GetButtonDown("Jump"))
                            {
                                if (matGod.matCount > 0)
                                {
                                    // �ٴ� ���°� Steel�϶�
                                    if (matGod.matState == MaterialGOD.Materials.Steel)
                                    {
                                        ChangeMats(matGod, "MK_Prefab/Steel", steelArray, MaterialGOD.Materials.Branch, branchArray);
                                        DeleteMat(branchArray);
                                    }
                                    // �ٴ� ���°� Rail �϶�
                                    if (matGod.matState == MaterialGOD.Materials.Rail)
                                    {
                                        ChangeMats(matGod, "CHAN_Prefab/Rail", railArray, MaterialGOD.Materials.Branch, branchArray);
                                        DeleteMat(branchArray);
                                    }
                                }
                                // ToolGOD�� ���� ����
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
                                // ����Ű�� ������ ��,
                                if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                                {
                                    ChangeState(PlayerItemDown.Hold.Pail, ToolGOD.Tools.Idle, MaterialGOD.Materials.Branch, branchArray.Count);
                                    DeleteMat(branchArray);
                                }
                            }

                        }
                    }
                    #endregion
                    #region Steel�� �տ� �ִ� ���
                    else if (steelArray.Count > 0)
                    {
                        if (rayInfo.transform.name == "Bridge") return;

                        // �������� �Ÿ��� ������
                        if (dis1 < 2)
                        {
                            if (Input.GetButtonDown("Jump"))
                            {
                                rail.steelCount = steelArray.Count;
                                // ���� ���� branch �ױ�
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
                            // �ٴ� ���°� Steel ���¶��
                            if (matGod.matState == MaterialGOD.Materials.Steel)
                            {
                                MakeMat("MK_Prefab/Steel", steelArray);
                            }
                            if (Input.GetButtonDown("Jump"))
                            {
                                if (matGod.matCount > 0)
                                {
                                    // �ٴ� ���°� branch
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
                    #region rail�� �տ� �ִ� ���
                    else if (railArray.Count > 0)
                    {
                        if (rayInfo.transform.name == "Bridge") return;

                        if (train_main)
                        {
                            mainDis = Vector3.Distance(train_main.transform.position, transform.position);
                        }

                        // ������ �������� �ƴ� ���
                        if (itemGOD.items == ItemGOD.Items.Rail || itemGOD.items == ItemGOD.Items.CornerRail)
                        {
                            // Ű�� ������
                            if (Input.GetButtonDown("Jump"))
                            {
                                // ����
                                itemGOD.ChangeState(ItemGOD.Items.Idle);
                                connectRail.instance.connectedRails.RemoveAt(connectRail.instance.connectedRails.Count - 1);
                                AddRail();
                            }

                        }
                        else
                        {
                            if (railDis < 2 && railDis > 0.5f && rayInfo.transform.gameObject != connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1].gameObject)
                            {

                                // Ű�� ������
                                if (Input.GetButtonDown("Jump"))
                                {
                                    // �տ��� ����
                                    itemGOD.ChangeState(ItemGOD.Items.Rail);
                                    RemoveRail();
                                }

                            }
                            if (rayInfo.transform.gameObject == connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1].gameObject)
                            {
                                // Ű�� ������
                                if (Input.GetButtonDown("Jump"))
                                {
                                    // ����
                                    itemGOD.ChangeState(ItemGOD.Items.Idle);
                                    connectRail.instance.connectedRails.RemoveAt(connectRail.instance.connectedRails.Count - 1);
                                    AddRail();
                                }
                            }
                            if (mainDis < 2 && n <= 0)
                            {
                                // Ű�� ������
                                if (Input.GetButtonDown("Jump"))
                                {
                                    // �տ��� ����
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
                                        // �ٴ� ���°� branch
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
                                    // �ٴ� ���°� idle�̶��
                                    if (toolGOD.toolsState == ToolGOD.Tools.Idle && matGod.matState == MaterialGOD.Materials.Idle)
                                    {
                                        matGod.ChangeMaterial(MaterialGOD.Materials.Rail, railArray.Count);
                                        DeleteMat(railArray);
                                    }
                                    // �ٴ� ���°� Ax���
                                    if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                                    {
                                        ChangeState(PlayerItemDown.Hold.Ax, ToolGOD.Tools.Idle, MaterialGOD.Materials.Rail, railArray.Count);
                                        DeleteMat(railArray);
                                    }
                                    // �ٴ� ���°� Pick���
                                    if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                                    {
                                        ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, MaterialGOD.Materials.Rail, railArray.Count);
                                        DeleteMat(railArray);
                                    }
                                    // �ٴ� ���°� Pail���
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

                    // �տ� ����
                    else
                    {
                        // �������� �Ÿ��� ���� ������ ���
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
                                // �÷��̾� �ջ��� ��ȯ
                                playerItem.holdState = PlayerItemDown.Hold.Mat;
                                rail.railCount = 0;
                            }
                        }

                        // ����Ű�� ������ ��,
                        if (Input.GetButtonDown("Jump"))
                        {

                            #region Branch 
                            // �ٴ� ���°� Branch���
                            if (matGod.matState == MaterialGOD.Materials.Branch)
                            {
                                // �ٴ� ���� ��ȭ
                                ChangeToolGod();
                                // �ٴڿ� branch�� �������� ���,
                                // ���� �ٴڿ� branch�� 3�� ������ ���
                                if (matGod.matCount > 0)
                                {
                                    for (int i = 0; i < matGod.matCount; i++)
                                    {
                                        // Array�� �߰��ϱ�
                                        GameObject mat = Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"), itemPos);
                                        branchArray.Add(mat);
                                        branchArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                        branchArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                                    }

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
                                // �÷��̾� �ջ��� ��ȯ
                                playerItem.holdState = PlayerItemDown.Hold.Mat;
                                // �ٴڻ��� ��ȯ
                                matGod.ChangeMaterial(MaterialGOD.Materials.None, 1);
                            }
                            if (itemGOD.items == ItemGOD.Items.Rail || itemGOD.items == ItemGOD.Items.CornerRail)
                            {
                                // Ű�� ������
                                if (Input.GetButtonDown("Jump"))
                                {
                                    // ����
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
        // ����ȭ
        else
        {
            // �� ���� �ø� ������ ��ġ
            /*itemPos.transform.position = Vector3.Lerp(itemPos.transform.position, iPos, 5 * Time.deltaTime);
            itemPos.transform.rotation = Quaternion.Lerp(itemPos.transform.rotation, iPosRot, 5 * Time.deltaTime);*/
            // �տ� ���𰡰� ������
            if (playerItem.holdState == PlayerItemDown.Hold.Mat)
            {
                // ������ �ִٸ�
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

    int matCount;
    // �ٴ��� ���� ��� ����
    void MakeMat(string s, List<GameObject> matArray)
    {
        // ��� �������� �ʰ� ����
        if (matArray.Count == matCount) return;
        // Array�� �߰��ϱ�
        GameObject mat = Instantiate(Resources.Load<GameObject>(s));
        mat.transform.parent = itemPos;
        matArray.Add(mat);
        // �� ��ġ ���� ������ �װ� �����
        for (int i = 0; i < matArray.Count; i++)
        {
            matArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
            matArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
            matCount++;
        }
        matGod.ChangeMaterial(MaterialGOD.Materials.None, 1);
    }

    // �ٴ��� �ٸ� ��� ����
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
        // �ٴڿ� �տ� �ִ� ������ŭ ����
        // �տ� �ִ� ��� �͵��� ����
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
