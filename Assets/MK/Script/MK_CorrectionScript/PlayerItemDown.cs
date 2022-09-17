using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PutDownItem ����
public class PlayerItemDown : MonoBehaviour
{
    // ������ ���� Ȯ��
    public enum Hold
    {
        Idle,
        ChangeIdle,
        Ax,
        Pick,
        Pail,
        Mat,
    }
    public Hold holdState = Hold.Idle;
    // �ȿ� �ִ� ���� Ȱ��ȭ
    public GameObject[] tool = new GameObject[3];
    // �ȿ� �ִ� mat Ȱ��ȭ
    public GameObject[] mat;
    // ���� �߻� ��ġ
    public Transform rayPos;

    #region �� ȸ�� ����
    // ��
    public GameObject rArm;
    public GameObject lArm;

    // ȸ�� �ӵ�
    public float rotSpeed = 3;

    // �� ����
    public int num;
    #endregion

    int hand;

    // ToolGod ������Ʈ
    ToolGOD toolGOD;
    MaterialGOD matGOD;
    ItemGOD itemGOD;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayerFSM();
        // ���̸� �߻��ϰ�
        Ray pRay = new Ray(rayPos.position, -transform.up);
        RaycastHit cubeInfo;
        // �����̽� �ٸ� ������
        if (Input.GetButtonDown("Jump"))
        {

            if (Physics.Raycast(pRay, out cubeInfo))
            {
                toolGOD = cubeInfo.transform.gameObject.GetComponent<ToolGOD>();
                matGOD = cubeInfo.transform.gameObject.GetComponent<MaterialGOD>();
                itemGOD = cubeInfo.transform.gameObject.GetComponent<ItemGOD>();
                if (matGOD.matState != MaterialGOD.Materials.Idle || holdState == Hold.Mat)
                {
                    return;
                }

                // �ٴ� ���� : �ƹ��͵� ����
                if (toolGOD.toolsState == ToolGOD.Tools.Idle)
                {
                    // ���� ���� : �տ� ���Ḧ ���� ���� ����, �߻�
                    // �տ� ������ ���� ���� ��,
                    if (num > 0)
                    {
                        holdState = Hold.ChangeIdle;
                        // �տ� �ִ� �Ϳ� ���� �ٴ��� ��ȭ
                        // ����
                        if (tool[0].activeSelf)
                        {
                            toolGOD.toolsState = ToolGOD.Tools.Ax;
                            return;
                        }
                        // ���
                        if (tool[1].activeSelf)
                        {
                            toolGOD.toolsState = ToolGOD.Tools.Pick;
                            return;
                        }
                        // �絿��
                        if (tool[2].activeSelf)
                        {
                            toolGOD.toolsState = ToolGOD.Tools.Pail;
                            return;
                        }
                    }
                    // ���� �������� ��
                    else
                    {
                        holdState = Hold.Idle;
                    }
                }
                // �ٴ� ���� : ����
                else if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                {
                    // �տ� ������ ���� ���� ��
                    if (num > 0)
                    {
                        hand = CheckHand();
                        // ��̸� ���� �ִٸ�
                        if (hand == 1)
                        {
                            toolGOD.toolsState = ToolGOD.Tools.Pick;
                            holdState = Hold.Ax;
                        }
                        // �絿�̸� ���� �ִٸ�
                        if (hand == 2)
                        {
                            toolGOD.toolsState = ToolGOD.Tools.Pail;
                            holdState = Hold.Ax;
                        }

                    }
                    else
                    {
                        // �÷��̾� ���¸� ��ȯ�Ѵ�
                        holdState = Hold.Ax;
                        // ������ ���µ� ��ȭ
                        toolGOD.toolsState = ToolGOD.Tools.Idle;
                    }
                }
                // �ٴ� ���� : ���
                else if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                {
                    // �տ� ������ ���� ��
                    if (num > 0)
                    {
                        hand = CheckHand();
                        // ������ ���� �ִٸ�
                        if (hand == 0)
                        {
                            toolGOD.toolsState = ToolGOD.Tools.Ax;
                            holdState = Hold.Pick;
                        }
                        // �絿�̸� ���� �ִٸ�
                        if (hand == 2)
                        {
                            toolGOD.toolsState = ToolGOD.Tools.Pail;
                            holdState = Hold.Pick;
                        }

                    }
                    else
                    {
                        // �÷��̾� ���¸� ��ȯ�Ѵ�
                        holdState = Hold.Pick;
                        // ������ ���µ� ��ȭ
                        toolGOD.toolsState = ToolGOD.Tools.Idle;
                    }
                }
                // �ٴ� ���� : �絿��
                else if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                {
                    // �տ� ������ ���� ��
                    if (num > 0)
                    {
                        hand = CheckHand();
                        // ������ ���� �ִٸ�
                        if (hand == 0)
                        {
                            toolGOD.toolsState = ToolGOD.Tools.Ax;
                            holdState = Hold.Pail;
                        }
                        // ��̸� ���� �ִٸ�
                        if (hand == 1)
                        {
                            toolGOD.toolsState = ToolGOD.Tools.Pick;
                            holdState = Hold.Pail;
                        }

                    }
                    else
                    {
                        // �÷��̾� ���¸� ��ȯ�Ѵ�
                        holdState = Hold.Pail;
                        // ������ ���µ� ��ȭ
                        toolGOD.toolsState = ToolGOD.Tools.Idle;
                    }
                }

            }
        }
    }

    // �÷��̾� ����
    void PlayerFSM()
    {
        switch (holdState)
        {
            // �ƹ��͵� ���� ���� ���� ��,
            case Hold.Idle:
                for(int i = 0; i < tool.Length; i++)
                {
                    tool[i].SetActive(false);
                }

                break;
            // ������ ���� �ִٰ� ���� ���� ��
            case Hold.ChangeIdle:
                // �����ȸ� �� ��
                 if (num > 0 && num < 2)
                {
                    RotArm(rArm, 0);
                }
                // �������� ���� ���� ��
                else
                {
                    RotArm(lArm, 0);
                    RotArm(rArm, 0);
                }
                // Idle ���·� ��ȯ
                holdState = Hold.Idle;
                num = 0;
                break;
            #region ����
            // ������ ���� ���� ��,
            case Hold.Ax:
                // ���� Ȱ��ȭ
                if(num > 1)
                {
                    if(lArm.transform.localEulerAngles != new Vector3(0, 0, 0))
                    {
                        RotArm(lArm, 0);
                    }
                    tool[0].SetActive(true);
                    return;
                }
                // �� ������
                RotArm(lArm, 0);
                RotArm(rArm, -90);
                tool[0].SetActive(true);
                break;
            // ��̸� ���� ���� ��
            case Hold.Pick:
                // ���� Ȱ��ȭ
                if (num > 1)
                {
                    if (lArm.transform.localEulerAngles != new Vector3(0, 0, 0))
                    {
                        RotArm(lArm, 0);
                    }
                    tool[1].SetActive(true);
                    return;
                }
                // �� ������
                RotArm(lArm, 0);
                RotArm(rArm, -90);
                tool[1].SetActive(true);
                break;
            // �絿�̸� ���� ���� ��,
            case Hold.Pail:
                if (num > 2)
                {
                    if (lArm.transform.localEulerAngles != new Vector3(-90, 0, 0))
                    {
                        RotArm(lArm, -90);
                    }
                    tool[2].SetActive(true);
                    return;
                }
                // �� ������
                RotArm(rArm, -90);
                RotArm(lArm, -90);
                tool[2].SetActive(true);
                break;
            #endregion
            #region ����
            // ���θ� ���� ���� ��
            case Hold.Mat:
                if (num > 2)
                {
                    if (lArm.transform.localEulerAngles != new Vector3(-90, 0, 0))
                    {
                        RotArm(lArm, -90);
                    }
                    for (int i = 0; i < tool.Length; i++)
                    {
                        tool[i].SetActive(false);
                    }
                    return;
                }
                RotArm(rArm, -90);
                RotArm(lArm, -90);
                break;
            #endregion
        }
    }
    // �÷��̾� ���� ���� �ö󰡰� ������ �Լ�
    void RotArm(GameObject arm, float rotAngle)
    {
        // �� ȸ�� ��Ű��
        arm.transform.localEulerAngles = new Vector3(rotAngle, 0, 0);
        // �� ȸ�� ����
        num++;
    }

    // �÷��̾� �տ� ���� �ִ� �� Ȯ��
    int CheckHand()
    {
        for (int i = 0; i < tool.Length; i++)
        {
            if (tool[i].activeSelf)
            {
                tool[i].SetActive(false);

                return i;
            }
        }
        return -1;
    }
}
