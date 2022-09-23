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
public class PlayerMaterial : MonoBehaviourPun, IPunObservable
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
    // MixedItem rail;

    GameObject railtrain;

    GameObject branch;

    // bool isBranch = true;

    int n;

    int matCount = 0;

    int layer;

    int r;

    // Start is called before the first frame update
    void Start()
    {
        // ������ ���� : ������ �ƴϸ� ��ȯ
        if (!photonView.IsMine)
        {
            return;
        }
        playerItem = GetComponent<PlayerItemDown>();
        playerRay = GetComponent<PlayerForwardRay>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            r = branchArray.Count;
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                AddRail();
            }

            // RailTrain���� �Ÿ��� ������ 
            railtrain = GameObject.Find("train_laugage2");
            float dis = Vector3.Distance(railtrain.transform.position, transform.position);

            // ���� ��ġ
            matTrain = GameObject.Find("train_laugage1").transform;
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

                // isBranch = playerRay.isBranch;
                // �տ� ���� ��� �ִٸ�
                // ���� ��
                if (riverGOD)
                {
                    // ������ �տ� �ְ�
                    if (riverGOD.riverState == RiverGOD.River.Bridge)
                    {
                        // �ٴ� ���°� bridge�϶�
                        if (railArray.Count > 0)
                        {
                            if (railDis < 2 && railDis > 0.5f && rayInfo.transform.gameObject != connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1].gameObject)
                            {

                                // Ű�� ������
                                if (Input.GetButtonDown("Jump"))
                                {
                                    // �տ��� ����
                                    itemGOD.ChangeState(ItemGOD.Items.Rail, default, 0.75f);
                                    RemoveRail();
                                }

                            }
                            if (rayInfo.transform.gameObject == connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1].gameObject)
                            {
                                // Ű�� ������
                                if (Input.GetButtonDown("Jump"))
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
                        // �������� �Ÿ� �Ǻ�
                        float dis1 = Vector3.Distance(transform.position, matTrain.position);
                        // �������� �Ÿ��� ������
                        if (dis1 < 2)
                        {
                            if (Input.GetButtonDown("Jump"))
                            {
                                // matTrain.GetComponent<MixedItem>().branchCount = branchArray.Count;
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
                                // Array�� �߰��ϱ�
                                GameObject mat = PhotonNetwork.Instantiate("MK_Prefab/Branch", itemPos.position, default);
                                mat.transform.parent = itemPos;
                                branchArray.Add(mat);
                                // �� ��ġ ���� ������ �װ� �����
                                for (int i = 0; i < branchArray.Count; i++)
                                {
                                    //    Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"));

                                    branchArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                    branchArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                                }
                                matGod.ChangeMaterial(MaterialGOD.Materials.None, branchArray.Count);

                            }
                            // �ٴ� ���°� Steel�϶�
                            if (matGod.matState == MaterialGOD.Materials.Steel)
                            {
                                if (Input.GetButtonDown("Jump"))
                                {

                                    if (matGod.steelCount > 0)
                                    {

                                        for (int i = 0; i < matGod.steelCount; i++)
                                        {
                                            MakeMat("MK_Prefab/Steel", steelArray);
                                            steelArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                            steelArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                                        }
                                        for (int i = 0; i < matGod.mat.Count; i++)
                                        {
                                            Destroy(matGod.mat[i]);
                                        }
                                        matGod.mat.Clear();
                                        matGod.branchCount = branchArray.Count;
                                        matGod.ChangeMaterial(MaterialGOD.Materials.Branch, branchArray.Count);
                                        DeleteMat(branchArray);
                                    }
                                }
                            }
                            // �ٴ� ���°� Rail �϶�
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
                                        for (int i = 0; i < matGod.mat.Count; i++)
                                        {
                                            Destroy(matGod.mat[i]);
                                        }
                                        matGod.mat.Clear();
                                        matGod.branchCount = branchArray.Count;
                                        matGod.ChangeMaterial(MaterialGOD.Materials.Branch, branchArray.Count);
                                        DeleteMat(branchArray);

                                    }
                                }
                            }

                            // �ٴ� ���°� idle�̶��
                            if (toolGOD.toolsState == ToolGOD.Tools.Idle && matGod.matState == MaterialGOD.Materials.Idle)
                            {
                                if (Input.GetButtonDown("Jump"))
                                {
                                    //matGod.branchCount = branchArray.Count;
                                    matGod.ChangeMaterial(MaterialGOD.Materials.Branch, branchArray.Count);
                                    DeleteMat(branchArray);
                                }
                            }
                            // ������ ���������
                            // �ٴ� ���°� Ax���
                            if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                            {
                                if (Input.GetButtonDown("Jump"))
                                {
                                    playerItem.holdState = PlayerItemDown.Hold.Ax;
                                    toolGOD.toolsState = ToolGOD.Tools.Idle;
                                    matGod.branchCount = branchArray.Count;
                                    matGod.ChangeMaterial(MaterialGOD.Materials.Branch, branchArray.Count);
                                    // ChangeState(PlayerItemDown.Hold.Ax, ToolGOD.Tools.Idle, branchArray, MaterialGOD.Materials.Branch, matGod.branchCount);
                                    DeleteMat(branchArray);
                                }
                            }
                            // �ٴ� ���°� Pick���
                            if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                            {
                                if (Input.GetButtonDown("Jump"))
                                {
                                    ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, branchArray, MaterialGOD.Materials.Branch, matGod.branchCount);
                                    DeleteMat(branchArray);
                                }
                            }
                            // �ٴ� ���°� Pail���
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
                    #region Steel�� �տ� �ִ� ���
                    else if (steelArray.Count > 0)
                    {
                        if (rayInfo.transform.name == "Bridge") return;
                        // �������� �Ÿ� �Ǻ�
                        float dis1 = Vector3.Distance(transform.position, matTrain.position);
                        // �������� �Ÿ��� ������
                        if (dis < 2)
                        {
                            if (Input.GetButtonDown("Jump"))
                            {
                                //matTrain.GetComponent<MixedItem>().steelCount = steelArray.Count;
                                // ���� ���� branch �ױ�
                                DeleteMat(steelArray);
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
                                // Array�� �߰��ϱ�
                                MakeMat("MK_Prefab/Steel", steelArray);
                                // �� ��ġ ���� ������ �װ� �����
                                for (int i = 0; i < steelArray.Count; i++)
                                {
                                    steelArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                    steelArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                                }
                                matGod.ChangeMaterial(MaterialGOD.Materials.None, 0);

                            }
                            // �ٴ� ���°� branch
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
                                        for (int i = 0; i < matGod.mat.Count; i++)
                                        {
                                            Destroy(matGod.mat[i]);
                                        }
                                        matGod.mat.Clear();
                                        matGod.steelCount = steelArray.Count;
                                        matGod.ChangeMaterial(MaterialGOD.Materials.Steel, steelArray.Count);
                                        DeleteMat(steelArray);
                                    }

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
                                        for (int i = 0; i < matGod.mat.Count; i++)
                                        {
                                            Destroy(matGod.mat[i]);
                                        }
                                        matGod.mat.Clear();
                                        matGod.steelCount = steelArray.Count;
                                        matGod.ChangeMaterial(MaterialGOD.Materials.Steel, steelArray.Count);
                                        DeleteMat(steelArray);
                                    }

                                }
                            }
                            // ����Ű�� ������ ��,
                            if (toolGOD.toolsState == ToolGOD.Tools.Idle && matGod.matState == MaterialGOD.Materials.Idle)
                            {
                                if (Input.GetButtonDown("Jump"))
                                {
                                    matGod.steelCount = steelArray.Count;
                                    matGod.ChangeMaterial(MaterialGOD.Materials.Steel, steelArray.Count);
                                    DeleteMat(steelArray);
                                }
                            }
                            // ����Ű�� ������ ��,
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
                            // ����Ű�� ������ ��,
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
                    #region rail�� �տ� �ִ� ���
                    else if (railArray.Count > 0)
                    {
                        if (rayInfo.transform.name == "Bridge") return;
                        GameObject train_main = GameObject.Find("train_main");
                        float mainDis = Vector3.Distance(train_main.transform.position, transform.position);

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
                                // �ٴ� ���°� branch
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
                                            for (int i = 0; i < matGod.mat.Count; i++)
                                            {
                                                Destroy(matGod.mat[i]);
                                            }
                                            matGod.mat.Clear();
                                            matGod.railCount = railArray.Count;
                                            matGod.ChangeMaterial(MaterialGOD.Materials.Rail, railArray.Count);
                                            DeleteMat(railArray);
                                        }

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
                                            for (int i = 0; i < matGod.mat.Count; i++)
                                            {
                                                Destroy(matGod.mat[i]);
                                            }
                                            matGod.mat.Clear();
                                            matGod.railCount = railArray.Count;
                                            matGod.ChangeMaterial(MaterialGOD.Materials.Rail, railArray.Count);
                                            DeleteMat(railArray);
                                        }

                                    }
                                }
                                // �ٴ� ���°� idle�̶��
                                if (toolGOD.toolsState == ToolGOD.Tools.Idle && matGod.matState == MaterialGOD.Materials.Idle)
                                {
                                    if (Input.GetButtonDown("Jump"))
                                    {
                                        matGod.railCount = railArray.Count;
                                        matGod.ChangeMaterial(MaterialGOD.Materials.Rail, railArray.Count);
                                        DeleteMat(railArray);
                                    }

                                }
                                // �ٴ� ���°� Ax���
                                if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                                {
                                    if (Input.GetButtonDown("Jump"))
                                    {
                                        ChangeState(PlayerItemDown.Hold.Ax, ToolGOD.Tools.Idle, railArray, MaterialGOD.Materials.Rail, matGod.railCount);
                                        DeleteMat(railArray);
                                    }
                                }
                                // �ٴ� ���°� Pick���
                                if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                                {
                                    if (Input.GetButtonDown("Jump"))
                                    {
                                        ChangeState(PlayerItemDown.Hold.Pick, ToolGOD.Tools.Idle, railArray, MaterialGOD.Materials.Rail, matGod.railCount);
                                        DeleteMat(railArray);
                                    }
                                }
                                // �ٴ� ���°� Pail���
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

                    // �տ� ����
                    else
                    {
                        // �������� �Ÿ��� ���� ������ ���
                        if (dis < 1.5f)
                        {
                            //&& rail.railCount > 0
                            if (Input.GetButtonDown("Jump"))
                            {
                                /*for (int i = 0; i < rail.railArray.Count; i++)
                                {
                                    MakeMat("CHAN_Prefab/Rail", railArray);
                                    railArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                    railArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                                }*/
                                // �÷��̾� �ջ��� ��ȯ
                                playerItem.holdState = PlayerItemDown.Hold.Mat;
                                // rail.railCount = 0;

                            }
                        }

                        // ����Ű�� ������ ��,
                        if (Input.GetButtonDown("Jump"))
                        {

                            #region Branch 
                            // �ٴ� ���°� Branch���
                            if (matGod.matState == MaterialGOD.Materials.Branch)
                            {
                                ChangeToolGod();
                                // �ٴڿ� branch�� �������� ���,
                                // ���� �ٴڿ� branch�� 3�� ������ ���
                                if (matGod.branchCount > 0)
                                {
                                    for (int i = 0; i < matGod.branchCount; i++)
                                    {
                                        // Array�� �߰��ϱ�
                                        // branch = PhotonNetwork.Instantiate("MK_Prefab/Branch", itemPos.position + new Vector3(0, i * 0.2f, 0), default);
                                        MakeMat("MK_Prefab/Branch", branchArray);
                                        print(branchArray.Count);
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
                                if (matGod.steelCount > 0)
                                {
                                    for (int i = 0; i < matGod.steelCount; i++)
                                    {
                                        MakeMat("MK_Prefab/Steel", steelArray);
                                        steelArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                        steelArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                                    }

                                }
                                // �÷��̾� �ջ��� ��ȯ
                                playerItem.holdState = PlayerItemDown.Hold.Mat;
                                // �ٴڻ��� ��ȯ
                                matGod.ChangeMaterial(MaterialGOD.Materials.None, 0);

                            }
                            #endregion
                            #region Rail
                            // ����Ű�� ������ ��,
                            else if (matGod.matState == MaterialGOD.Materials.Rail)
                            {
                                //ChangeToolGod();
                                // rail
                                // �ٴڿ� rail�� �������� ���
                                if (matGod.railCount > 0)
                                {
                                    for (int i = 0; i < matGod.railCount; i++)
                                    {
                                        MakeMat("CHAN_Prefab/Rail", railArray);
                                        railArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                        railArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                                    }

                                }
                                // �÷��̾� �ջ��� ��ȯ
                                playerItem.holdState = PlayerItemDown.Hold.Mat;
                                // �ٴڻ��� ��ȯ
                                matGod.ChangeMaterial(MaterialGOD.Materials.None, 0);
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
        else
        {

            if (brn > 0)
            {
                for (int i = 0; i < brn; i++)
                {
                    branch = Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"));
                    branchArray.Add(branch);
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

    void MakeMat(string s, List<GameObject> matArray)
    {
        // Array�� �߰��ϱ�
        GameObject mat = Instantiate(Resources.Load<GameObject>(s));
        mat.transform.parent = itemPos;
        matArray.Add(mat);
    }

    void DeleteMat(List<GameObject> matArray)
    {
        // �ٴڿ� �տ� �ִ� ������ŭ ����
        // �տ� �ִ� ��� �͵��� ����
        for (int i = 0; i < matArray.Count; i++)
        {
            PhotonNetwork.Destroy(matArray[i].gameObject);
        }

        if(matArray.Count <= 0)
        {
            playerItem.holdState = PlayerItemDown.Hold.ChangeIdle;
        }
        matArray.Clear();
    }
    // ���� ��ȭ
    void ChangeState(PlayerItemDown.Hold player, ToolGOD.Tools tool, List<GameObject> mat, MaterialGOD.Materials mats, int count)
    {
        playerItem.holdState = player;
        toolGOD.ChangeState(tool);
        count = mat.Count;
        matGod.ChangeMaterial(mats, count);
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
            // isBranch = true;
        }

    }
    int brn;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(r);
        }
        else
        {
            brn = (int)stream.ReceiveNext();
        }
    }
}
