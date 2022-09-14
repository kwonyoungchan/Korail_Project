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
        Rail,
        Branch,
        Steel
    }
    public Hold holdState = Hold.Idle;
    // �ȿ� �ִ� ���� Ȱ��ȭ
    public GameObject[] tool = new GameObject[3];
    // �ȿ� �ִ� mat Ȱ��ȭ
    public GameObject[] mat;

    #region �� ȸ�� ����
    // ��
    public GameObject rArm;
    public GameObject lArm;

    // ȸ�� �ӵ� 
    public float rotSpeed = 3;

    // �� ����
    public int armState;
    #endregion

    int hand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerFSM();
        // ���̸� �߻��ϰ�
        Ray pRay = new Ray(transform.position, -transform.up);
        RaycastHit cubeInfo;
        // �����̽� �ٸ� ������
        if (Physics.Raycast(pRay, out cubeInfo))
        {

            if(Input.GetButtonDown("Jump"))
            {
                if(cubeInfo.transform.gameObject.GetComponent<ToolGOD>() == null)
                {
                    return;
                }
                // �ٴ� ���� : �ƹ��͵� ����
                if(cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState == ToolGOD.Tools.Idle)
                {
                    // ���� ���� : �տ� ��Ḧ ��� ���� ���, �߻�
                    // �տ� ���� ��� ���� ��,
                    if (armState > 0)
                    {
                        holdState = Hold.ChangeIdle;
                        // �տ� �ִ� �Ϳ� ���� �ٴ��� ��ȭ
                        // ����
                        if (tool[0].activeSelf)
                        {
                            cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Ax;
                            return;
                        }
                        // ���
                        if (tool[1].activeSelf)
                        {
                            cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Pick;
                            return;
                        }
                        // �絿��
                        if (tool[2].activeSelf)
                        {
                            cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Pail;
                            return;
                        }
                    }
                    // ���� ������� ��
                    else
                    {
                        holdState = Hold.Idle;
                    }
                }
                // �ٴ� ���� : ����
                if (cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState == ToolGOD.Tools.Ax)
                {
                    // �տ� ���� ��� ���� ��
                    if (armState > 0)
                    {
                        hand = CheckHand();
                        // ��̸� ��� �ִٸ�
                        if(hand == 1)
                        {
                            cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Pick;
                            holdState = Hold.Ax;
                        }
                        // �絿�̸� ��� �ִٸ�
                        if(hand == 2)
                        {
                            cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Pail;
                            holdState = Hold.Ax;
                        }

                    }
                    else
                    {
                        // �÷��̾� ���¸� ��ȯ�Ѵ�
                        holdState = Hold.Ax;
                        // ������ ���µ� ��ȭ
                        cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Idle;
                    }
                }
                // �ٴ� ���� : ���
                if (cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState == ToolGOD.Tools.Pick)
                {
                    // �տ� ���� ���� ��
                    if (armState > 0)
                    {
                        hand = CheckHand();
                        // ������ ��� �ִٸ�
                        if (hand == 0)
                        {
                            cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Ax;
                            holdState = Hold.Pick;
                        }
                        // �絿�̸� ��� �ִٸ�
                        if (hand == 2)
                        {
                            cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Pail;
                            holdState = Hold.Pick;
                        }

                    }
                    else
                    {
                        // �÷��̾� ���¸� ��ȯ�Ѵ�
                        holdState = Hold.Pick;
                        // ������ ���µ� ��ȭ
                        cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Idle;
                    }
                }
                // �ٴ� ���� : �絿��
                if (cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState == ToolGOD.Tools.Pail)
                {
                    // �տ� ���� ���� ��
                    if (armState > 0)
                    {
                        hand = CheckHand();
                        // ������ ��� �ִٸ�
                        if (hand == 0)
                        {
                            cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Ax;
                            holdState = Hold.Pail;
                        }
                        // ��̸� ��� �ִٸ�
                        if (hand == 1)
                        {
                            cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Pick;
                            holdState = Hold.Pail;
                        }

                    }
                    else
                    {
                        // �÷��̾� ���¸� ��ȯ�Ѵ�
                        holdState = Hold.Pail;
                        // ������ ���µ� ��ȭ
                        cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Idle;
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
            // �ƹ��͵� ��� ���� ���� ��,
            case Hold.Idle:
                for(int i = 0; i < tool.Length; i++)
                {
                    tool[i].SetActive(false);
                }
                break;
            // ������ ��� �ִٰ� ���� ���� ��
            case Hold.ChangeIdle:
                // �����ȸ� �� ��
                if (armState > 0 && armState < 2)
                {
                    RotArm(rArm, 0);
                }
                // �������� ��� ���� ��
                else
                {
                    RotArm(lArm, 0);
                    RotArm(rArm, 0);
                }
                // Idle ���·� ��ȯ
                holdState = Hold.Idle;
                armState = 0;
                break;
            #region ����
            // ������ ��� ���� ��,
            case Hold.Ax:
                // ���� Ȱ��ȭ
                if(armState > 1)
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
            // ��̸� ��� ���� ��
            case Hold.Pick:
                // ���� Ȱ��ȭ
                if (armState > 1)
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
            // �絿�̸� ��� ���� ��,
            case Hold.Pail:
                if (armState > 2)
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
            // ���θ� ��� ���� ��
            case Hold.Rail:
                if (armState > 2)
                {
                    return;
                }
                RotArm(rArm, -90);
                RotArm(lArm, -90);
                break;
            #endregion
            #region ���
            // ���������� ��� ���� ��
            case Hold.Branch:
                if (armState > 2)
                {
                    return;
                }
                RotArm(rArm, -90);
                RotArm(lArm, -90);
                break;
            // ö�� ��� ���� ��,
            case Hold.Steel:
                if (armState > 2)
                {
                    return;
                }
                RotArm(rArm, -90);
                RotArm(lArm, -90);
                break;
                #endregion
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

    // �÷��̾� �տ� ��� �ִ� �� Ȯ��
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
