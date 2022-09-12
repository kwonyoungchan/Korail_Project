using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스페이드바를 누르면 팔을 들어올리면서 아이템을 먹음
public class PlayerGetItem : MonoBehaviour
{
    // 팔
    public GameObject rArm;
    public GameObject lArm;
    // 팔에 있는 도구들
    public GameObject[] tool = new GameObject[3];
    public GameObject[] material = new GameObject[2];

    // 회전 속도 
    public float rotSpeed = 3;

    // 팔 상태
    public int armState;

    // 도구를 든 상태
    public int curArm;

    // 도구 생성
    public GameObject axFact;
    public GameObject pickFact;
    public GameObject pailFact;

    // 재료 생성
    public GameObject branchFact;
    public GameObject steelFact;

    // 도구 y 위치
    public float y = 0.5f;

    // 도구 상태
    bool[] isTool = new bool[3];
    bool[] isMaterial = new bool[2];

    // 도구
    ToolItem ax;
    ToolItem pick;
    ToolItem pail;

    // 재료
    Material branch;
    Material steel;

    // 도구
    GameObject createAx;
    GameObject createPick;
    GameObject createPail;
    // 재료
    GameObject putBranch;
    GameObject putSteel;

    private void Start()
    {
        // 도구이름 찾기
        ax = GameObject.Find("Ax").GetComponent<ToolItem>();
        pick = GameObject.Find("Pick").GetComponent<ToolItem>();
        pail = GameObject.Find("Pail").GetComponent<ToolItem>();
    }
    // Update is called once per frame
    void Update()
    {
        // 도구 업데이트
        isTool[0] = ax.isAx;
        isTool[1] = pick.isPick;
        isTool[2] = pail.isPail;

        if (GameObject.Find("Branch(Clone)"))
        {   
            branch = GameObject.Find("Branch(Clone)").GetComponent<Material>();

            isMaterial[0] = branch.isIngredient[0];
        }
        if (GameObject.Find("Steel(Clone)"))
        {
            steel = GameObject.Find("Steel(Clone)").GetComponent<Material>();
            isMaterial[1] = steel.isIngredient[1];
        }

        if (isTool[0] || isTool[1] || isTool[2])
        {
            // 점프키를 누른다면
            if (Input.GetButtonDown("Jump"))
            {
                // 팔의 rotate 값이 -90, 0, 0이면 0, 0, 0으로 돌려놓기
                if (armState < 1)
                {
                    // 플레이어의 팔이 위로 올라감
                    RotArm(rArm, -90);
                    // 도끼 on / off
                    if (isTool[0])
                    {
                        ToolSwitch(0);
                    }
                    // 곡갱이 on / off
                    if (isTool[1])
                    {
                        ToolSwitch(1);
                    }
                    if (isTool[2])
                    {
                        ToolSwitch(2);
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
            // 점프키를 누른다면
            if (Input.GetButtonDown("Jump"))
            {
                // 팔의 rotate 값이 -90, 0, 0이면 0, 0, 0으로 돌려놓기
                if (armState < 2)
                {
                    // 플레이어의 팔이 위로 올라감
                    RotArm(rArm, -90);
                    RotArm(lArm, -90);
                    // 도끼 on / off
                    if (isMaterial[0])
                    {
                        MaterialSwich(0);
                    }
                    // 곡갱이 on / off
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

    // 플레이어 팔이 위로 올라가게 만드는 함수
    void RotArm(GameObject arm, float rotAngle)
    {
        // 팔 회전 시키기
        arm.transform.localEulerAngles = new Vector3(rotAngle, 0, 0);
        // 팔 회전 상태
        armState++;
    }
    // 도구 on/off
    void ToolSwitch(int n)
    {
        tool[n].SetActive(true);
        curArm = 1;
    }
    // 재료 on / off
    void MaterialSwich(int n)
    {
        material[n].SetActive(true);
        curArm = 2;
    }

    // 도구 바닥에 두기
    void PutTool()
    {
        // 도끼를 곡갱이가 없는 곳에 놨을 경우
        if (tool[0].activeSelf && isTool[1] == false && isTool[2] == false)
        {
            tool[0].SetActive(false);
            createAx = Instantiate(axFact);
            createAx.transform.position = transform.position + new Vector3(0, y, 0);
            ax = createAx.GetComponent<ToolItem>();
        }
        // 곡갱이를 도끼가 없는 곳에 놨을 경우
        if (tool[1].activeSelf && isTool[0] == false && isTool[2] == false)
        {
            tool[1].SetActive(false);
            createPick = Instantiate(pickFact);
            createPick.transform.position = transform.position + new Vector3(0, y, 0);
            pick = createPick.GetComponent<ToolItem>();
        }
        if(tool[2].activeSelf && isTool[0] == false && isTool[1] == false)
        {
            tool[2].SetActive(false);
            createPail = Instantiate(pailFact);
            createPail.transform.position = transform.position + new Vector3(0, y, 0);
            pail = createPail.GetComponent<ToolItem>();
        }
    }
    // 재료 바닥에 두기
    void PutMaterial()
    {

        // 나뭇가지를 바닥에 놓는 경우
        if (material[0].activeSelf && isMaterial[1] == false)
        {
            material[0].SetActive(false);
            
            putBranch = Instantiate(branchFact);
            putBranch.transform.position = transform.position + new Vector3(0, y, 0);
            
        }
        // 철을 바닥에 놓는 경우
        if (material[1].activeSelf && isMaterial[0] == false)
        {
            material[1].SetActive(false);
            
            putSteel = Instantiate(steelFact);
            putSteel.transform.position = transform.position + new Vector3(0, y, 0);
            
        }
    }

    // 도구 바꾸기
    void ChageTool()
    {
        // 도끼를 든 상태에서 곡갱이를 드는 경우
        if (tool[0].activeSelf && isTool[1]) 
        { 
            tool[0].SetActive(false);
            tool[1].SetActive(true);
            createAx = Instantiate(axFact);
            createAx.transform.position = transform.position + new Vector3(0, y, 0);
            ax = createAx.GetComponent<ToolItem>();
            Destroy(pick.gameObject);
        }
        // 도끼를 든 상태에서 양동이를 드는 경우
        if (tool[0].activeSelf && isTool[2])
        {
            tool[0].SetActive(false);
            tool[2].SetActive(true);
            createAx = Instantiate(axFact);
            createAx.transform.position = transform.position + new Vector3(0, y, 0);
            ax = createAx.GetComponent<ToolItem>();
            Destroy(pail.gameObject);
        }
        // 곡갱이를 들고있는 상태에서 도끼를 드는 경우
        if (tool[1].activeSelf && isTool[0])
        {
            tool[1].SetActive(false);
            tool[0].SetActive(true);
            createPick = Instantiate(pickFact);
            createPick.transform.position = transform.position + new Vector3(0, y, 0);
            pick = createPick.GetComponent<ToolItem>();
            Destroy(ax.gameObject);
        }
        // 곡갱이를 들고있는 상태에서 양동이를 드는 경우
        if (tool[1].activeSelf && isTool[2])
        {
            tool[1].SetActive(false);
            tool[2].SetActive(true);
            createPick = Instantiate(pickFact);
            createPick.transform.position = transform.position + new Vector3(0, y, 0);
            pick = createPick.GetComponent<ToolItem>();
            Destroy(pail.gameObject);
        }
        // 양동이를 들고있는 상태에서 도끼를 드는 경우
        if (tool[2].activeSelf && isTool[0])
        {
            tool[2].SetActive(false);
            tool[0].SetActive(true);
            createPail = Instantiate(pailFact);
            createPail.transform.position = transform.position + new Vector3(0, y, 0);
            pail = createPail.GetComponent<ToolItem>();
            Destroy(ax.gameObject);
        }
        // 양동이를 들고있는 상태에서 곡갱이 드는 경우
        if (tool[2].activeSelf && isTool[1])
        {
            tool[2].SetActive(false);
            tool[1].SetActive(true);
            createPail = Instantiate(pickFact);
            createPail.transform.position = transform.position + new Vector3(0, y, 0);
            pail = createPail.GetComponent<ToolItem>();
            Destroy(pick.gameObject);
        }
    }

    // 재료 바꾸기
    void ChangeMaterial()
    {
        // 도끼를 든 상태에서 곡갱이를 드는 경우
        if (material[0].activeSelf && isMaterial[1])
        {
            material[0].SetActive(false);
            material[1].SetActive(true);
            putBranch = Instantiate(branchFact);
            putBranch.transform.position = transform.position + new Vector3(0, y, 0);

            Destroy(material[1].gameObject);
        }
        // 곡갱이를 들고있는 상태에서 도끼를 드는 경우
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
    // 도구 사용하기
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
