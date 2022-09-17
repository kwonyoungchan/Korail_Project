using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾��� ���ɿ� ���� cube(��)�� ���¸� ��ȯ��
public class MaterialGOD : MonoBehaviour
{
    // Material�� ���� ���� ��ȭ
    public enum Materials
    {
        Idle,
        Branch,
        Steel,
        Rail,
        None
    }
    public Materials matState = Materials.Idle;

    // ������ ������Ʈ ���� ����
    public int branchCount = 0;
    public int steelCount = 0;
    public int railCount = 0;

    int preBCount;

    float y = 0.55f;

    // ������ ���ӿ�����Ʈ
    public List<GameObject> mat = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        branchCount = 1;
        steelCount = 1;
        railCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //MaterialFSM();
    }

    // Material�� ���� FSM
    void MaterialFSM()
    {
        switch (matState)
        {
            // �ƹ��͵� ����(���� ���ᰡ �ƴ� �ٸ� ���� �ִ� ����)
            case Materials.Idle:
                break;
            // ������������
            case Materials.Branch:
                // ���ӿ�����Ʈ�� ������ return
                if (mat.Count == branchCount) return;
                // Resources���Ͽ� �ִ� �������� ����
                if (branchCount > 1)
                {
                    for (int i = 0; i < branchCount; i++)
                    {
                        CreateMat("MK_Prefab/Branch", i);
                    }
                }
                else
                {
                    branchCount = 1;
                    GameObject branch = Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"));
                    mat.Add(branch);
                    branch.transform.position = transform.position + new Vector3(0, 1, 0);
                }
                break;
            case Materials.Steel:
                // ���ӿ�����Ʈ�� ������ return
                if (mat.Count == steelCount) return;
                // Resources���Ͽ� �ִ� �������� ����
                if (steelCount > 1)
                {
                    for (int i = 0; i < steelCount; i++)
                    {
                        CreateMat("MK_Prefab/Steel", i);
                    }
                }
                else
                {
                    steelCount = 1;
                    GameObject steel = Instantiate(Resources.Load<GameObject>("MK_Prefab/Steel"));
                    mat.Add(steel);
                    steel.transform.position = transform.position + new Vector3(0, 1, 0);
                }
                break;
            case Materials.Rail:
                // ���ӿ�����Ʈ�� ������ return
                if (mat.Count == railCount) return;
                // Resources���Ͽ� �ִ� �������� ����
                if (railCount > 1)
                {
                    for (int i = 0; i < railCount; i++)
                    {
                        CreateMat("CHAN_Prefab/Rail", i);
                    }
                }
                else
                {
                    railCount = 1;
                    GameObject rail = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail"));
                    mat.Add(rail);
                    rail.transform.position = transform.position + new Vector3(0, 1, 0);
                }
                break;
            case Materials.None:
                if(mat.Count > 0)
                {
                    for (int i = 0; i < mat.Count; i++)
                    {
                        Destroy(mat[i]);
                    }
                    mat.Clear();
                }
                else
                {
                    matState = Materials.Idle;
                }
                break;
        }
    }
    public void CreateMat(string s, int i)
    {
        GameObject ingredient = Instantiate(Resources.Load<GameObject>(s));
        mat.Insert(i, ingredient);
        mat[i].transform.position = transform.position + new Vector3(0, 1 + i * 0.2f, 0);
    }
}
