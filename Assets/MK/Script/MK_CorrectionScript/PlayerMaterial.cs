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

    // Start is called before the first frame update
    void Start()
    {
        
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
                if(rayInfo.transform.gameObject.GetComponent<MaterialGOD>() == null)
                {
                    return;a
                }
                // �տ� ���� ��� �ִٸ�
                if (branchArray.Count > 0 || steelArray.Count > 0) 
                {
                    if (branchArray.Count > 0 && rayInfo.transform.gameObject.GetComponent<MaterialGOD>().matState == MaterialGOD.Materials.Branch)
                    {
                        
                        // Array�� �߰��ϱ�
                        GameObject branch = Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"));
                        branch.transform.parent = itemPos;
                        
                        branchArray.Add(branch);
                        for(int i = 0; i < branchArray.Count; i++)
                        {
                            branchArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.5f, 0);
                            branchArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                        }
                        rayInfo.transform.gameObject.GetComponent<MaterialGOD>().matState = MaterialGOD.Materials.None;

                    }
                }
                // �տ� ����
                else
                {
                    // ����Ű�� ������ ��,
                    if (Input.GetButtonDown("Jump") && rayInfo.transform.gameObject.GetComponent<MaterialGOD>())
                    {
                        // �տ� �ִ� ���𰡸� ���
                        // �ٴ� ���°� Branch���
                        if (rayInfo.transform.gameObject.GetComponent<MaterialGOD>().matState == MaterialGOD.Materials.Branch)
                        {
                            // Array�� �߰��ϱ�
                            GameObject branch = Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"));
                            branchArray.Add(branch);
                            branch.transform.parent = itemPos;
                            branch.transform.position = itemPos.position;
                            branch.transform.eulerAngles = new Vector3(90, 0, 90);
                            
                            // �÷��̾� �ջ��� ��ȯ
                            GetComponent<PlayerItemDown>().holdState = PlayerItemDown.Hold.Branch;
                            // �ٴڻ��� ��ȯ
                            rayInfo.transform.gameObject.GetComponent<MaterialGOD>().matState = MaterialGOD.Materials.None;
                        }
                        // Steel�̶��
                        if (rayInfo.transform.gameObject.GetComponent<MaterialGOD>().matState == MaterialGOD.Materials.Steel)
                        {
                            // Array�� �߰��ϱ�
                            GameObject steel = Instantiate(Resources.Load<GameObject>("MK_Prefab/Steel"));
                            steelArray.Add(steel);
                            steel.transform.position = itemPos.position;
                            // �÷��̾� �ջ��� ��ȯ
                            GetComponent<PlayerItemDown>().holdState = PlayerItemDown.Hold.Steel;
                            // �ٴڻ��� ��ȯ
                            rayInfo.transform.gameObject.GetComponent<MaterialGOD>().matState = MaterialGOD.Materials.None;
                            
                        }
                    }
                }
            }
        }
    }
}
