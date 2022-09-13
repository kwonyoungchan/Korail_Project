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
                mat = Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"));
                mat.transform.position = transform.position + new Vector3(0, 1, 0);
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
                break;
        }
    }
}
