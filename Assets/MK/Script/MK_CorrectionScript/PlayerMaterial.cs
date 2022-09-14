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

                    // �ٴ� ���°� Branch���
                    if (branchArray.Count > 0 && matGod.matState == MaterialGOD.Materials.Branch)
                    {
                        // Array�� �߰��ϱ�
                        GameObject branch = Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"));
                        branch.transform.parent = itemPos;
                        branchArray.Add(branch);
                        // �� ��ġ ���� ������ �װ� �����
                        for (int i = 0; i < branchArray.Count; i++)
                        {
                            branchArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                            branchArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                        }
                        matGod.matState = MaterialGOD.Materials.None;

                    }
                    // �ٴ� ���°� idle�̶��
                    else if (branchArray.Count > 0 && toolGOD.toolsState == ToolGOD.Tools.Idle)
                    {
                        // ����Ű�� ������ ��,
                        if (Input.GetButtonDown("Jump"))
                        {
                            // �ٴڿ� �տ� �ִ� ������ŭ ����
                            matGod.branchCount = branchArray.Count;
                            matGod.matState = MaterialGOD.Materials.Branch;
                            // �տ� �ִ� ��� �͵��� ����
                            for (int i = 0; i < branchArray.Count; i++)
                            {
                                Destroy(branchArray[i].gameObject);
                            }
                            branchArray.Clear();
                        }
                    }
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
                            // Array�� �߰��ϱ�
                            GameObject branch = Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"));
                            branchArray.Add(branch);
                            // ������ �ø���
                            branch.transform.parent = itemPos;
                            branch.transform.position = itemPos.position;
                            
                            // �÷��̾� �ջ��� ��ȯ
                            playerItem.holdState = PlayerItemDown.Hold.Branch;
                            // �ٴڻ��� ��ȯ
                            matGod.matState = MaterialGOD.Materials.None;
                        }
                        // Steel�̶��
                        if (matGod.matState == MaterialGOD.Materials.Steel)
                        {
                            // Array�� �߰��ϱ�
                            GameObject steel = Instantiate(Resources.Load<GameObject>("MK_Prefab/Steel"));
                            steelArray.Add(steel);
                            steel.transform.position = itemPos.position;
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
}
