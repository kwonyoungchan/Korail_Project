using System.Collections;
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
public class PlayerMaterial : MonoBehaviour
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

    // ���� �� ������ ��ġ
    public Transform bPos;
    public Transform sPos;
    // ���� ��ġ
    public Transform rayPos;

    // MaterialGod ������Ʈ
    MaterialGOD matGod;
    // ToolGod ������Ʈ
    ToolGOD toolGOD;
    // ItemGod ������Ʈ
    ItemGOD itemGOD;

    // �÷��̾� �� ���� ������Ʈ
    PlayerItemDown playerItem;
    // ���� ��ġ
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
        // ���� ��ġ
        matTrain = GameObject.Find("Train").transform;
        rail = GameObject.Find("Train").GetComponent<MixedItem>();

        // �÷��̾ ���̸� �߻��Ѵ�
        Ray pRay = new Ray(rayPos.position, -transform.up);
        RaycastHit rayInfo;
        if (Physics.Raycast(pRay, out rayInfo))
        {
            matGod = rayInfo.transform.gameObject.GetComponent<MaterialGOD>();
            toolGOD = rayInfo.transform.gameObject.GetComponent<ToolGOD>();
            itemGOD = rayInfo.transform.gameObject.GetComponent<ItemGOD>();
            // �տ� ���� ��� �ִٸ�
            if (branchArray.Count > 0 || steelArray.Count > 0 || railArray.Count > 0)
            {
                #region branch�� �տ� �ִ� ���
                if (branchArray.Count > 0)
                {
                    // �������� �Ÿ� �Ǻ�
                    float dis = Vector3.Distance(transform.position, matTrain.position);
                    // �������� �Ÿ��� ������
                    if (dis < 1.5f)
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            matTrain.GetComponent<MixedItem>().branchCount = branchArray.Count;
                            // ���� ���� branch �ױ�
                            for (int i = 0; i < branchArray.Count; i++)
                            {
                                Destroy(branchArray[i].gameObject);
                            }
                            branchArray.Clear();
                        }
                    }
                    else
                    {

                        // �ٴ� ���°� Branch���
                        if (matGod.matState == MaterialGOD.Materials.Branch)
                        {
                            // Array�� �߰��ϱ�
                            MakeMat("MK_Prefab/Branch", branchArray);
                            // �� ��ġ ���� ������ �װ� �����
                            for (int i = 0; i < branchArray.Count; i++)
                            {
                                branchArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                branchArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                            }
                            matGod.matState = MaterialGOD.Materials.None;

                        }
                        // �ٴ� ���°� idle�̶��
                        if (toolGOD.toolsState == ToolGOD.Tools.Idle)
                        {
                            // ����Ű�� ������ ��,
                            if (Input.GetButtonDown("Jump"))
                            {
                                DeleteBranch();
                            }
                        }
                        // �ٴ� ���°� Ax���
                        if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                        {
                            // ����Ű�� ������ ��,
                            if (Input.GetButtonDown("Jump"))
                            {
                                playerItem.holdState = PlayerItemDown.Hold.Ax;
                                toolGOD.toolsState = ToolGOD.Tools.Idle;
                                DeleteBranch();
                            }
                        }
                        // �ٴ� ���°� Pick���
                        if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                        {
                            // ����Ű�� ������ ��,
                            if (Input.GetButtonDown("Jump"))
                            {
                                playerItem.holdState = PlayerItemDown.Hold.Pick;
                                toolGOD.toolsState = ToolGOD.Tools.Idle;
                                DeleteBranch();
                            }
                        }
                        // �ٴ� ���°� Pail���
                        if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                        {
                            // ����Ű�� ������ ��,
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
                #region Steel�� �տ� �ִ� ���
                if (steelArray.Count > 0)
                {
                    // �������� �Ÿ� �Ǻ�
                    float dis = Vector3.Distance(transform.position, matTrain.position);
                    // �������� �Ÿ��� ������
                    if (dis < 1.5f)
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            matTrain.GetComponent<MixedItem>().steelCount = steelArray.Count;
                            // ���� ���� branch �ױ�
                            for (int i = 0; i < steelArray.Count; i++)
                            {
                                Destroy(steelArray[i].gameObject);
                            }
                            steelArray.Clear();
                        }
                    }
                    else
                    {
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
                            matGod.matState = MaterialGOD.Materials.None;

                        }
                        // �ٴ� ���°� idle�̶��
                        if (toolGOD.toolsState == ToolGOD.Tools.Idle)
                        {
                            // ����Ű�� ������ ��,
                            if (Input.GetButtonDown("Jump"))
                            {
                                DeleteSteel();
                            }
                        }
                        // �ٴ� ���°� Ax���
                        if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                        {
                            // ����Ű�� ������ ��,
                            if (Input.GetButtonDown("Jump"))
                            {
                                playerItem.holdState = PlayerItemDown.Hold.Ax;
                                toolGOD.toolsState = ToolGOD.Tools.Idle;
                                DeleteSteel();
                            }
                        }
                        // �ٴ� ���°� Pick���
                        if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                        {
                            // ����Ű�� ������ ��,
                            if (Input.GetButtonDown("Jump"))
                            {
                                playerItem.holdState = PlayerItemDown.Hold.Pick;
                                toolGOD.toolsState = ToolGOD.Tools.Idle;
                                DeleteSteel();
                            }
                        }
                        // �ٴ� ���°� Pail���
                        if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                        {
                            // ����Ű�� ������ ��,
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
                #region rail�� �տ� �ִ� ���
                if (railArray.Count > 0)
                {
                    #region ���� ��ġ x
                    // �ٴ� ���°� Rail��� => ���� ��ġ x
                    if (matGod.matState == MaterialGOD.Materials.Rail)
                    {
                        // Array�� �߰��ϱ�
                        MakeMat("CHAN_Prefab/Rail", railArray);
                        // �� ��ġ ���� ������ �װ� �����
                        for (int i = 0; i < railArray.Count; i++)
                        {
                            railArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                            railArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                        }
                        matGod.matState = MaterialGOD.Materials.None;

                    }
                    // �ٴ� ���°� idle�̶��
                    if (toolGOD.toolsState == ToolGOD.Tools.Idle)
                    {
                        // ����Ű�� ������ ��,
                        if (Input.GetButtonDown("Jump"))
                        {
                            DeleteRail();
                        }
                    }
                    // �ٴ� ���°� Ax���
                    if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                    {
                        // ����Ű�� ������ ��,
                        if (Input.GetButtonDown("Jump"))
                        {
                            playerItem.holdState = PlayerItemDown.Hold.Ax;
                            toolGOD.toolsState = ToolGOD.Tools.Idle;
                            DeleteRail();
                        }
                    }
                    // �ٴ� ���°� Pick���
                    if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                    {
                        // ����Ű�� ������ ��,
                        if (Input.GetButtonDown("Jump"))
                        {
                            playerItem.holdState = PlayerItemDown.Hold.Pick;
                            toolGOD.toolsState = ToolGOD.Tools.Idle;
                            DeleteRail();
                        }
                    }
                    // �ٴ� ���°� Pail���
                    if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                    {
                        // ����Ű�� ������ ��,
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
            // �տ� ����
            else
            {
                // RailTrain���� �Ÿ��� ������ 
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
                            // �÷��̾� �ջ��� ��ȯ
                            playerItem.holdState = PlayerItemDown.Hold.Rail;
                            rail.railCount = 0;
                        }
                    }
                }

                // ����Ű�� ������ ��,
                if (Input.GetButtonDown("Jump"))
                {
                    #region Branch 
                    // �ٴ� ���°� Branch���
                    if (matGod.matState == MaterialGOD.Materials.Branch)
                    {
                        // �ٴڿ� branch�� �������� ���,
                        // ���� �ٴڿ� branch�� 3�� ������ ���
                        if (matGod.branchCount > 0)
                        {
                            for (int i = 0; i < matGod.branchCount; i++)
                            {
                                MakeMat("MK_Prefab/Branch", branchArray);
                                branchArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                branchArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                            }
                        }
                        // �÷��̾� �ջ��� ��ȯ
                        playerItem.holdState = PlayerItemDown.Hold.Branch;
                        // �ٴڻ��� ��ȯ
                        matGod.matState = MaterialGOD.Materials.None;
                    }
                    #endregion
                    #region Steel
                    // ����Ű�� ������ ��,
                    if (matGod.matState == MaterialGOD.Materials.Steel)
                    {
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
                            playerItem.holdState = PlayerItemDown.Hold.Steel;
                            // �ٴڻ��� ��ȯ
                            matGod.matState = MaterialGOD.Materials.None;
                        
                    }
                    #endregion
                    #region Rail
                    // ����Ű�� ������ ��,
                    if (matGod.matState == MaterialGOD.Materials.Rail)
                    {
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
                        playerItem.holdState = PlayerItemDown.Hold.Rail;
                        // �ٴڻ��� ��ȯ
                        matGod.matState = MaterialGOD.Materials.None;
                    }
                    #endregion
                }
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

    void DeleteBranch()
    {
        matGod.branchCount = branchArray.Count;
        matGod.matState = MaterialGOD.Materials.Branch;
        // �ٴڿ� �տ� �ִ� ������ŭ ����
        // �տ� �ִ� ��� �͵��� ����
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
        // �ٴڿ� �տ� �ִ� ������ŭ ����
        // �տ� �ִ� ��� �͵��� ����
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
        // �ٴڿ� �տ� �ִ� ������ŭ ����
        // �տ� �ִ� ��� �͵��� ����
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
