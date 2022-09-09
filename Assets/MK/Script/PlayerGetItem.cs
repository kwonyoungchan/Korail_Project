using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����̵�ٸ� ������ ���� ���ø��鼭 �������� ����
public class PlayerGetItem : MonoBehaviour
{
    // ��
    public GameObject rArm;
    public GameObject lArm;

    // ȸ�� �ӵ� 
    public float rotSpeed = 3;

    // �� ����
    int armState = 1;

    // ���� ����
    bool isTool;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isTool = GameObject.FindWithTag("Tool").GetComponent<ToolItem>().isAx || GameObject.FindWithTag("Tool").GetComponent<ToolItem>().isPick;
        if (isTool)
        {
            // ����Ű�� �����ٸ�
            if (Input.GetButtonDown("Jump"))
            {
                // ���� rotate ���� -90, 0, 0�̸� 0, 0, 0���� ��������
                if (armState > 1)
                {
                    RotArm(rArm, 0);

                    armState = 0;
                }
                else
                {
                    // �÷��̾��� ���� ���� �ö�
                    RotArm(rArm, -90);
                }
            }
        }
    }

    // �÷��̾� ���� ���� �ö󰡰� ����� �Լ�
    void RotArm(GameObject arm, float rotAngle)
    {
        // �� ȸ�� ��Ű��
        arm.transform.localEulerAngles = new Vector3(rotAngle, 0, 0);
        // �� ȸ�� ����
        armState++;
    }

    // ������ ��� ��� �ִ� ��� �����
    void ItemOnArm()
    {

    }
}
