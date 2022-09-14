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
    GameObject mat;

    // �÷��̾��� PlayerMaterial ��������
    PlayerMaterial player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMaterial>();
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
                if (mat != null) return;
                // Resources���Ͽ� �ִ� �������� ����
                if (branchCount > 1)
                {
                    for (int i = 0; i < branchCount; i++)
                    {
                        mat = Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"));
                        mat.transform.position = transform.position + new Vector3(0, 1 + i * 0.3f, 0);
                    }
                }
                else
                {
                    mat = Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"));
                    mat.transform.position = transform.position + new Vector3(0, 1, 0);
                }
                break;
            case Materials.Steel:
                // ���ӿ�����Ʈ�� ������ return
                if (mat != null) return;
                // Resources���Ͽ� �ִ� ö ����
                mat = Instantiate(Resources.Load<GameObject>("MK_Prefab/Steel"));
                mat.transform.position = transform.position + new Vector3(0, 1, 0);
                break;
            case Materials.None:
                if(mat != null)
                {
                    Destroy(mat);
                }
                else
                {
                    matState = Materials.Idle;
                }
                break;
        }
    }
}
