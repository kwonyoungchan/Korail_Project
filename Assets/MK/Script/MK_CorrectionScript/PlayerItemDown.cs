using UnityEngine;
using Photon.Pun;

// PutDownItem ����
public class PlayerItemDown : MonoBehaviourPun
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
    [HideInInspector]
    public int num = 1;
    #endregion

    public int f;
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
        // ������ ���� : ������ �ƴϸ� ��ȯ
        if (!photonView.IsMine)
        {
            return;
        }
        PlayerFSM();
        // ���̸� �߻��ϰ�
        Ray pRay = new Ray(rayPos.position + new Vector3(-0.2f, 0, 0), -transform.up);
        RaycastHit cubeInfo;
        // �����̽� �ٸ� ������
        if (Input.GetButtonDown("Jump"))
        {

            if (Physics.Raycast(pRay, out cubeInfo))
            {
                toolGOD = cubeInfo.transform.gameObject.GetComponent<ToolGOD>();
                matGOD = cubeInfo.transform.gameObject.GetComponent<MaterialGOD>();
                itemGOD = cubeInfo.transform.gameObject.GetComponent<ItemGOD>();
                if (matGOD.matState != MaterialGOD.Materials.Idle) return;
                // �ٴ� ���� : �ƹ��͵� ����
                if (toolGOD.toolsState == ToolGOD.Tools.Idle)
                {
                    // ���� ���� : �տ� ��Ḧ ��� ���� ���, �߻�
                    // �տ� ���� ��� ���� ��,
                    // �տ� �ִ� �Ϳ� ���� �ٴ��� ��ȭ
                    // ����
                    if (tool[0].activeSelf)
                    {
                        holdState = Hold.ChangeIdle;
                        toolGOD.toolsState = ToolGOD.Tools.Ax;
                        return;
                    }
                    // ���
                    else if (tool[1].activeSelf)
                    {
                        holdState = Hold.ChangeIdle;
                        toolGOD.toolsState = ToolGOD.Tools.Pick;
                        return;
                    }
                    // �絿��
                    else if (tool[2].activeSelf)
                    {
                        holdState = Hold.ChangeIdle;
                        toolGOD.toolsState = ToolGOD.Tools.Pail;
                        return;
                    }
                    else
                    {
                        holdState = Hold.ChangeIdle;
                    }

                }
                // �ٴ� ���� : ����
                else if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                {
                    // �տ� ���� ��� ���� ��
                    hand = CheckHand();
                    // ��̸� ��� �ִٸ�
                    if (hand == 1)
                    {
                        toolGOD.toolsState = ToolGOD.Tools.Pick;
                        holdState = Hold.Ax;
                    }
                    // �絿�̸� ��� �ִٸ�
                    else if (hand == 2)
                    {
                        toolGOD.toolsState = ToolGOD.Tools.Pail;
                        holdState = Hold.Ax;
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

                    hand = CheckHand();
                    // ������ ��� �ִٸ�
                    if (hand == 0)
                    {
                        toolGOD.toolsState = ToolGOD.Tools.Ax;
                        holdState = Hold.Pick;
                    }
                    // �絿�̸� ��� �ִٸ�
                    else if (hand == 2)
                    {
                        toolGOD.toolsState = ToolGOD.Tools.Pail;
                        holdState = Hold.Pick;
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
                    // �տ� ���� ���� ��

                    hand = CheckHand();
                    // ������ ��� �ִٸ�
                    if (hand == 0)
                    {
                        toolGOD.toolsState = ToolGOD.Tools.Ax;
                        holdState = Hold.Pail;
                    }
                    // ��̸� ��� �ִٸ�
                    else if (hand == 1)
                    {
                        toolGOD.toolsState = ToolGOD.Tools.Pick;
                        holdState = Hold.Pail;
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
                if(lArm.transform.localEulerAngles != new Vector3(-80, 0, 0) || rArm.transform.localEulerAngles != new Vector3(-85, 0, 0))
                {
                    RotArm(lArm, -80, 0);
                    RotArm(rArm, -85, 0);

                }
                // Idle ���·� ��ȯ
                holdState = Hold.Idle;
                break;
            #region ����
            // ������ ��� ���� ��,
            case Hold.Ax:
                // ���� Ȱ��ȭ
                // �� ������
                RotArm(lArm, -80, 0);
                RotArm(rArm, -85, 0);
                tool[0].SetActive(true);
                break;
            // ��̸� ��� ���� ��
            case Hold.Pick:
                // ���� Ȱ��ȭ
                // �� ������
                RotArm(lArm, -80, 0);
                RotArm(rArm, -85, 0);
                tool[1].SetActive(true);
                break;
            // �絿�̸� ��� ���� ��,
            case Hold.Pail:
                // �� ������
                RotArm(rArm, -85, 90);
                RotArm(lArm, -80, -90);
                tool[2].SetActive(true);
                break;
            #endregion
            #region ���
            // ���θ� ��� ���� ��
            case Hold.Mat:
                for (int i = 0; i < tool.Length; i++)
                {
                    tool[i].SetActive(false);
                }
                RotArm(rArm, -85, 90);
                RotArm(lArm, -80, -90);
                break;
            #endregion
        }
    }
    // �÷��̾� ���� ���� �ö󰡰� ����� �Լ�
    void RotArm(GameObject arm, float rotX, float rotAngle)
    {
        // �� ȸ�� ��Ű��
        arm.transform.localEulerAngles = new Vector3(rotX, 0, rotAngle);
        // �� ȸ�� ����
        // num++;
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
