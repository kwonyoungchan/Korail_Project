using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾��� ��ɿ� ���� cube(��)�� ���¸� ��ȯ��
public class MaterialGOD : MonoBehaviour
{
    // Material�� ���� ���� ��ȭ
    public enum Materials
    {
        Idle,
        Branch,
        Steel,
        None
    }
    public Materials matState = Materials.Idle;

    // ������ ������Ʈ ���� ����
    public int branchCount = 0;

    // ������ ���ӿ�����Ʈ
    List<GameObject> mat = new List<GameObject>();

    // �÷��̾��� PlayerMaterial ��������
    PlayerMaterial player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMaterial>();
        branchCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        MaterialFSM();
    }

    // Material�� ���� FSM
    void MaterialFSM()
    {
        switch (matState)
        {
            // �ƹ��͵� ����(���� ��ᰡ �ƴ� �ٸ� ���� �ִ� ���)
            case Materials.Idle:
                break;
            // �����������
            case Materials.Branch:
                // ���ӿ�����Ʈ�� ������ return
                if (mat.Count == branchCount) return;
                // Resources���Ͽ� �ִ� �������� ����
                if (branchCount > 1)
                {
                    for (int i = 0; i < branchCount; i++)
                    {
                        GameObject branch = Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"));
                        mat.Insert(i, branch);
                        mat[i].transform.position = transform.position + new Vector3(0, 1 + i * 0.2f, 0);
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
/*                // ���ӿ�����Ʈ�� ������ return
                if (mat != null) return;
                // Resources���Ͽ� �ִ� ö ����
                mat = Instantiate(Resources.Load<GameObject>("MK_Prefab/Steel"));
                mat.transform.position = transform.position + new Vector3(0, 1, 0);*/
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
}
