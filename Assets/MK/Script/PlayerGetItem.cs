using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����̵�ٸ� ������ ���� ���ø��鼭 �������� ����
public class PlayerGetItem : MonoBehaviour
{
    #region �� ȸ�� ����
    // ��
    public GameObject rArm;
    public GameObject lArm;

    // ȸ�� �ӵ� 
    public float rotSpeed = 3;

    // �� ����
    public int armState;

    // ������ �� ����
    public int curArm;
    #endregion

    #region ���� ����
    // �ȿ� �ִ� ������
    public GameObject[] tool = new GameObject[3];

    // ���� ����
    public GameObject axFact;
    public GameObject pickFact;
    public GameObject pailFact;

    // ���� y ��ġ
    public float y = 0.5f;

    // ���� ����
    bool[] isTool = new bool[3];

    // ����
    ToolItem ax;
    ToolItem pick;
    ToolItem pail;

    // ����
    GameObject createAx;
    GameObject createPick;
    GameObject createPail;
    #endregion    
    
    private void Start()
    {
        #region ���� �̸� ã��
        // �����̸� ã��
        ax = GameObject.Find("Ax").GetComponent<ToolItem>();
        pick = GameObject.Find("Pick").GetComponent<ToolItem>();
        pail = GameObject.Find("Pail").GetComponent<ToolItem>();
        #endregion
    }
    // Update is called once per frame
    void Update()
    {
        // ���� ������Ʈ
        isTool[0] = ax.isAx;
        isTool[1] = pick.isPick;
        isTool[2] = pail.isPail;

        if (isTool[0] || isTool[1] || isTool[2])
        {
            // ����Ű�� �����ٸ�
            if (Input.GetButtonDown("Jump"))
            {
                if (isTool[2])
                {
                    if (armState < 1)
                    {
                        // �÷��̾��� ���� ���� �ö�
                        RotArm(rArm, -90);
                        RotArm(lArm, -90);
                        ToolSwitch(2);
                    }
                    else
                    {
                        PutTool();
                        ChageTool();
                    }

                }
                // �絿�̰� �ƴ� ��,
                else if(isTool[0] || isTool[1])
                {
                    // ���� rotate ���� -90, 0, 0�̸� 0, 0, 0���� ��������
                    if (armState < 1)
                    {
                        // �÷��̾��� ���� ���� �ö�
                        RotArm(rArm, -90);
                        // ���� on / off
                        if (isTool[0])
                        {
                            ToolSwitch(0);
                        }
                        // ��� on / off
                        if (isTool[1])
                        {
                            ToolSwitch(1);
                        }
                    }
                    else
                    {
                        PutTool();
                        ChageTool();
                    }
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && (armState > 0))
            {
                if (armState < 2)
                {
                    RotArm(rArm, 0);
                    armState = 0;
                    curArm = 0;
                    PutTool();
                }
                if(armState > 1)
                {
                    RotArm(lArm, 0);
                    RotArm(rArm, 0);
                    armState = 0;
                    curArm = 0;
                    PutTool();
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
    // ���� on/off
    void ToolSwitch(int n)
    {
        tool[n].SetActive(true);
        curArm = 1;
    }

    // ���� �ٴڿ� �α�
    void PutTool()
    {
        // ������ ��̰� ���� ���� ���� ���
        if (tool[0].activeSelf && isTool[1] == false && isTool[2] == false)
        {
            // �տ� �ִ� ������
            tool[0].SetActive(false);
            // �ٴڿ� �������� ������
            createAx = Instantiate(axFact);
            createAx.transform.position = transform.position + new Vector3(0, y, 0);
            ax = createAx.GetComponent<ToolItem>();
        }
        // ��̸� ������ ���� ���� ���� ���
        if (tool[1].activeSelf && isTool[0] == false && isTool[2] == false)
        {
            // �տ� �ִ� ������
            tool[1].SetActive(false);
            // �ٴڿ� �������� ������
            createPick = Instantiate(pickFact);
            createPick.transform.position = transform.position + new Vector3(0, y, 0);
            pick = createPick.GetComponent<ToolItem>();
        }
        // �絿�̸� �ٴڿ� ���� ���
        if(tool[2].activeSelf && isTool[0] == false && isTool[1] == false)
        {
            // �տ� �ִ� ������
            tool[2].SetActive(false);
            // �ٴڿ� �������� ������
            createPail = Instantiate(pailFact);
            createPail.transform.position = transform.position + new Vector3(0, y, 0);
            pail = createPail.GetComponent<ToolItem>();
        }
    }

    // ���� �ٲٱ�
    void ChageTool()
    {
        // ������ �� ���¿��� ��̸� ��� ���
        if (tool[0].activeSelf && isTool[1] && isTool[0] == false) 
        {
            // �տ� �ִ� ������
            tool[0].SetActive(false);
            tool[1].SetActive(true);
            // �ٴڿ� �������� ������
            createAx = Instantiate(axFact);
            createAx.transform.position = transform.position + new Vector3(0, y, 0);
            ax = createAx.GetComponent<ToolItem>();
            // �ٴڿ� �ִ� ������ ����
            Destroy(pick.gameObject);
        }
        // ������ �� ���¿��� �絿�̸� ��� ���
        if (tool[0].activeSelf && isTool[2] && isTool[1] == false)
        {
            // �ճ�����
            RotArm(lArm, -90);
            // �տ� �ִ� ������ ��ü
            tool[0].SetActive(false);
            tool[2].SetActive(true);
            // �ٴڿ� �������� ������
            createAx = Instantiate(axFact);
            createAx.transform.position = transform.position + new Vector3(0, y, 0);
            ax = createAx.GetComponent<ToolItem>();
            // �ٴڿ� �ִ� ������ ����
            Destroy(pail.gameObject);
        }
        // ��̸� ����ִ� ���¿��� ������ ��� ���
        if (tool[1].activeSelf && isTool[0] && isTool[2] == false)
        {
            // �տ� �ִ� ������ ��ü
            tool[1].SetActive(false);
            tool[0].SetActive(true);
            // �ٴڿ� ���� ������
            createPick = Instantiate(pickFact);
            createPick.transform.position = transform.position + new Vector3(0, y, 0);
            pick = createPick.GetComponent<ToolItem>();
            // �ٴڿ� �ִ� ������ ����
            Destroy(ax.gameObject);
        }
        // ��̸� ����ִ� ���¿��� �絿�̸� ��� ���
        if (tool[1].activeSelf && isTool[2] && isTool[0] == false)
        {
            // �� ������
            RotArm(lArm, -90);
            // �տ� ��� �ִ� ������ �ٲٱ�
            tool[1].SetActive(false);
            tool[2].SetActive(true);
            // �ٴڿ� �������� ������
            createPick = Instantiate(pickFact);
            createPick.transform.position = transform.position + new Vector3(0, y, 0);
            pick = createPick.GetComponent<ToolItem>();
            // �ٴڿ� �ִ� ������ ����
            Destroy(pail.gameObject);
        }
        // �絿�̸� ����ִ� ���¿��� ������ ��� ���
        if (tool[2].activeSelf && isTool[0] && isTool[1] == false)
        {
            // �ճ�����
            RotArm(lArm, 0);
            // �տ� �ִ� ������ ��ü
            tool[2].SetActive(false);
            tool[0].SetActive(true);
            // �ٴڿ� �������� ������
            createPail = Instantiate(pailFact);
            createPail.transform.position = transform.position + new Vector3(0, y, 0);
            pail = createPail.GetComponent<ToolItem>();
            armState = 1;
            // �ٴڿ� �ִ� ������ ����
            Destroy(ax.gameObject);
        }
        // �絿�̸� ����ִ� ���¿��� ��� ��� ���
        if (tool[2].activeSelf && isTool[1] && isTool[0] == false)
        {
            // �ճ�����
            RotArm(lArm, 0);
            // �տ� �ִ� ������ ��ü
            tool[2].SetActive(false);
            tool[1].SetActive(true);
            // �ٴڿ� �������� ������
            createPail = Instantiate(pailFact);
            createPail.transform.position = transform.position + new Vector3(0, y, 0);
            pail = createPail.GetComponent<ToolItem>();
            armState = 1;
            // �ٴڿ� �ִ� ������ ����
            Destroy(pick.gameObject);
        }
    }


    /*
        bool rotate = false;
        // ���� ����ϱ�
        public void UseTool(float currentTime)
        {

            if(rArm.transform.localRotation.x >= 0)
            {
                rotate = true;
            }
            else if(rArm.transform.localRotation.x <= -90)
            {
                rotate = false;

            }

        }*/
}
