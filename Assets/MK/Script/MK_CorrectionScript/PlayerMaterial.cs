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
public class PlayerMaterial : MonoBehaviour
{
    // ����Ʈ
    // ��������
    public List<GameObject> branchArray = new List<GameObject>();
    public List<GameObject> steelArray = new List<GameObject>();
    // ������ ��ġ
    public Transform itemPos;
    // MaterialGod ������Ʈ
    MaterialGOD matGod;
    // ToolGod ������Ʈ
    ToolGOD toolGOD;
    // �÷��̾� �� ���� ������Ʈ
    PlayerItemDown playerItem;

    // Start is called before the first frame update
    void Start()
    {
        playerItem = GetComponent<PlayerItemDown>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindWithTag("Branch"))
        {
            // �÷��̾ ���̸� �߻��Ѵ�
            Ray pRay = new Ray(transform.position, -transform.up);
            RaycastHit rayInfo;
            if (Physics.Raycast(pRay, out rayInfo))
            {
                matGod = rayInfo.transform.gameObject.GetComponent<MaterialGOD>();
                toolGOD = rayInfo.transform.gameObject.GetComponent<ToolGOD>();
                
                // �տ� ���� ��� �ִٸ�
                if (branchArray.Count > 0 || steelArray.Count > 0) 
                {
                    if (toolGOD == null)
                    {
                        return;
                    }
                    #region branch�� �տ� �ִ� ���
                    if (branchArray.Count > 0)
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
                    #endregion
                    #region Steel�� �տ� �ִ� ���
                    if (steelArray.Count > 0)
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
                        else if (toolGOD.toolsState == ToolGOD.Tools.Idle)
                        {
                            // ����Ű�� ������ ��,
                            if (Input.GetButtonDown("Jump"))
                            {
                                // �ٴڿ� �տ� �ִ� ������ŭ ����
                                matGod.steelCount = steelArray.Count;
                                matGod.matState = MaterialGOD.Materials.Steel;
                                // �տ� �ִ� ��� �͵��� ����
                                for (int i = 0; i < steelArray.Count; i++)
                                {
                                    Destroy(steelArray[i].gameObject);
                                }
                                steelArray.Clear();
                            }
                        }
                    }
                    #endregion
                }
                // �տ� ����
                else
                {
                    // ����Ű�� ������ ��,
                    if (Input.GetButtonDown("Jump"))
                    {
                        // �ٴ� ���°� Branch���
                        if (matGod.matState == MaterialGOD.Materials.Branch)
                        {
                            // �ٴڿ� branch�� �������� ���
                            if (matGod.branchCount > 0)
                            {
                                for(int i = 0; i < matGod.branchCount; i++)
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
                        // Steel�̶��
                        if (matGod.matState == MaterialGOD.Materials.Steel)
                        {
                            // �ٴڿ� steel�� �������� ���
                            if (matGod.steelCount > 0)
                            {
                                for (int i = 0; i < matGod.branchCount; i++)
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

                    }
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
}
