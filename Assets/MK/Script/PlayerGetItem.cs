using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����̵�ٸ� ������ ���� ���ø��鼭 �������� ����
public class PlayerGetItem : MonoBehaviour
{
    // ��
    public GameObject rArm;
    public GameObject lArm;
    // �ȿ� �ִ� ������
    public GameObject[] tool = new GameObject[2];
    public GameObject[] material = new GameObject[2];

    // ȸ�� �ӵ� 
    public float rotSpeed = 3;

    // �� ����
    public int armState;

    // ������ �� ����
    public int curArm;

    // ���� ����
    public GameObject axFact;
    public GameObject pickFact;

    // ��� ����
    public GameObject branchFact;
    public GameObject steelFact;

    // ���� y ��ġ
    public float y = 0.5f;

    // ���� ����
    bool[] isTool = new bool[2];
    bool[] isMaterial = new bool[2];

    // ����
    ToolItem ax;
    ToolItem pick;

    // ���
    Material branch;
    Material steel;

    // ����
    GameObject createAx;
    GameObject createPick;
    // ���
    GameObject putBranch;
    GameObject putSteel;

    private void Start()
    {
        // �����̸� ã��
        ax = GameObject.Find("Ax").GetComponent<ToolItem>();
        pick = GameObject.Find("Pick").GetComponent<ToolItem>();
    }
    // Update is called once per frame
    void Update()
    {
        // ���� ������Ʈ
        isTool[0] = ax.isAx;
        isTool[1] = pick.isPick;

        if (GameObject.Find("Branch(Clone)"))
        {   
            branch = GameObject.Find("Branch(Clone)").GetComponent<Material>();

            isMaterial[0] = branch.isIngredient[0];
        }

        if (GameObject.Find("Steel(Clone)"))
        {
            steel = GameObject.Find("Steel(Clone)").GetComponent<Material>();
            isMaterial[1] = branch.isIngredient[1];
        }
        print(isMaterial[0]);
        if (isTool[0] || isTool[1])
        {
            // ����Ű�� �����ٸ�
            if (Input.GetButtonDown("Jump"))
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
        else
        {
            if (Input.GetButtonDown("Jump") && (armState > 0 && armState < 2))
            {
                RotArm(rArm, 0);
                armState = 0;
                curArm = 0;
                PutTool();
            }
        }

        if (isMaterial[0] || isMaterial[1])
        {
            // ����Ű�� �����ٸ�
            if (Input.GetButtonDown("Jump"))
            {
                // ���� rotate ���� -90, 0, 0�̸� 0, 0, 0���� ��������
                if (armState < 2)
                {
                    // �÷��̾��� ���� ���� �ö�
                    RotArm(rArm, -90);
                    RotArm(lArm, -90);
                    // ���� on / off
                    if (isMaterial[0])
                    {
                        MaterialSwich(0);
                    }
                    // ��� on / off
                    if (isMaterial[1])
                    {
                        MaterialSwich(1);
                    }
                }
                else
                {
                    RotArm(rArm, 0);
                    RotArm(lArm, 0);
                    armState = 0;
                    PutMaterial();
                    ChangeMaterial();
                }

            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && armState >= 2)
            {
                RotArm(rArm, 0);
                RotArm(lArm, 0);
                armState = 0;
                curArm = 0;
                PutMaterial();
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
    // ��� on / off
    void MaterialSwich(int n)
    {
        material[n].SetActive(true);
        curArm = 2;
    }

    // ���� �ٴڿ� �α�
    void PutTool()
    {
        // ������ ��̰� ���� ���� ���� ���
        if (tool[0].activeSelf && isTool[1] == false)
        {
            tool[0].SetActive(false);
            createAx = Instantiate(axFact);
            createAx.transform.position = transform.position + new Vector3(0, y, 0);
            ax = createAx.GetComponent<ToolItem>();
        }
        // ��̸� ������ ���� ���� ���� ���
        if (tool[1].activeSelf && isTool[0] == false)
        {
            tool[1].SetActive(false);
            createPick = Instantiate(pickFact);
            createPick.transform.position = transform.position + new Vector3(0, y, 0);
            pick = createPick.GetComponent<ToolItem>();
        }
    }
    // ��� �ٴڿ� �α�
    void PutMaterial()
    {

        // ������ ��̰� ���� ���� ���� ���
        if (material[0].activeSelf && isMaterial[1] == false)
        {
            material[0].SetActive(false);
            
            putBranch = Instantiate(branchFact);
            putBranch.transform.position = transform.position + new Vector3(0, y, 0);
            
        }
        // ��̸� ������ ���� ���� ���� ���
        if (material[1].activeSelf && isMaterial[0] == false)
        {
            material[1].SetActive(false);
            
            putSteel = Instantiate(steelFact);
            putSteel.transform.position = transform.position + new Vector3(0, y, 0);
            
        }
    }

    // ���� �ٲٱ�
    void ChageTool()
    {
        // ������ �� ���¿��� ��̸� ��� ���
        if (tool[0].activeSelf && isTool[1]) 
        { 
            tool[0].SetActive(false);
            tool[1].SetActive(true);
            createAx = Instantiate(axFact);
            createAx.transform.position = transform.position + new Vector3(0, y, 0);
            ax = createAx.GetComponent<ToolItem>();
            Destroy(pick.gameObject);
        }
        // ��̸� ����ִ� ���¿��� ������ ��� ���
        if (tool[1].activeSelf && isTool[0])
        {
            tool[1].SetActive(false);
            tool[0].SetActive(true);
            createPick = Instantiate(pickFact);
            createPick.transform.position = transform.position + new Vector3(0, y, 0);
            pick = createPick.GetComponent<ToolItem>();
            Destroy(ax.gameObject);
        }
    }

    // ��� �ٲٱ�
    void ChangeMaterial()
    {
        // ������ �� ���¿��� ��̸� ��� ���
        if (material[0].activeSelf && isMaterial[1])
        {
            material[0].SetActive(false);
            material[1].SetActive(true);
            putBranch = Instantiate(branchFact);
            putBranch.transform.position = transform.position + new Vector3(0, y, 0);

            Destroy(material[1].gameObject);
        }
        // ��̸� ����ִ� ���¿��� ������ ��� ���
        if (material[1].activeSelf && isMaterial[0])
        {
            material[1].SetActive(false);
            material[0].SetActive(true);
            putSteel = Instantiate(steelFact);
            putSteel.transform.position = transform.position + new Vector3(0, y, 0);

            Destroy(material[0].gameObject);
        }
    }


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

    }
}
