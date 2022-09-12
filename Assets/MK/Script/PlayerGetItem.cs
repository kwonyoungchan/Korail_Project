using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스페이드바를 누르면 팔을 들어올리면서 아이템을 먹음
public class PlayerGetItem : MonoBehaviour
{
    #region 팔 회전 관련
    // 팔
    public GameObject rArm;
    public GameObject lArm;

    // 회전 속도 
    public float rotSpeed = 3;

    // 팔 상태
    public int armState;

    // 도구를 든 상태
    public int curArm;
    #endregion

    #region 도구 관련
    // 팔에 있는 도구들
    public GameObject[] tool = new GameObject[3];

    // 도구 생성
    public GameObject axFact;
    public GameObject pickFact;
    public GameObject pailFact;

    // 도구 y 위치
    public float y = 0.5f;

    // 도구 상태
    bool[] isTool = new bool[3];

    // 도구
    ToolItem ax;
    ToolItem pick;
    ToolItem pail;

    // 도구
    GameObject createAx;
    GameObject createPick;
    GameObject createPail;
    #endregion    
    
    private void Start()
    {
        #region 도구 이름 찾기
        // 도구이름 찾기
        ax = GameObject.Find("Ax").GetComponent<ToolItem>();
        pick = GameObject.Find("Pick").GetComponent<ToolItem>();
        pail = GameObject.Find("Pail").GetComponent<ToolItem>();
        #endregion
    }
    // Update is called once per frame
    void Update()
    {
        // 도구 업데이트
        isTool[0] = ax.isAx;
        isTool[1] = pick.isPick;
        isTool[2] = pail.isPail;

        if (isTool[0] || isTool[1] || isTool[2])
        {
            // 점프키를 누른다면
            if (Input.GetButtonDown("Jump"))
            {
                if (isTool[2])
                {
                    if (armState < 1)
                    {
                        // 플레이어의 팔이 위로 올라감
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
                // 양동이가 아닐 때,
                else if(isTool[0] || isTool[1])
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

    // 도구 바닥에 두기
    void PutTool()
    {
        // 도끼를 곡갱이가 없는 곳에 놨을 경우
        if (tool[0].activeSelf && isTool[1] == false && isTool[2] == false)
        {
            // 손에 있는 아이템
            tool[0].SetActive(false);
            // 바닥에 내려놓을 아이템
            createAx = Instantiate(axFact);
            createAx.transform.position = transform.position + new Vector3(0, y, 0);
            ax = createAx.GetComponent<ToolItem>();
        }
        // 곡갱이를 도끼가 없는 곳에 놨을 경우
        if (tool[1].activeSelf && isTool[0] == false && isTool[2] == false)
        {
            // 손에 있는 아이템
            tool[1].SetActive(false);
            // 바닥에 내려놓을 아이템
            createPick = Instantiate(pickFact);
            createPick.transform.position = transform.position + new Vector3(0, y, 0);
            pick = createPick.GetComponent<ToolItem>();
        }
        // 양동이를 바닥에 놓는 경우
        if(tool[2].activeSelf && isTool[0] == false && isTool[1] == false)
        {
            // 손에 있는 아이템
            tool[2].SetActive(false);
            // 바닥에 내려놓을 아이템
            createPail = Instantiate(pailFact);
            createPail.transform.position = transform.position + new Vector3(0, y, 0);
            pail = createPail.GetComponent<ToolItem>();
        }
    }

    // 도구 바꾸기
    void ChageTool()
    {
        // 도끼를 든 상태에서 곡갱이를 드는 경우
        if (tool[0].activeSelf && isTool[1] && isTool[0] == false) 
        {
            // 손에 있는 아이템
            tool[0].SetActive(false);
            tool[1].SetActive(true);
            // 바닥에 내려놓을 아이템
            createAx = Instantiate(axFact);
            createAx.transform.position = transform.position + new Vector3(0, y, 0);
            ax = createAx.GetComponent<ToolItem>();
            // 바닥에 있는 아이템 제거
            Destroy(pick.gameObject);
        }
        // 도끼를 든 상태에서 양동이를 드는 경우
        if (tool[0].activeSelf && isTool[2] && isTool[1] == false)
        {
            // 손내리기
            RotArm(lArm, -90);
            // 손에 있는 아이템 교체
            tool[0].SetActive(false);
            tool[2].SetActive(true);
            // 바닥에 내려놓을 아이템
            createAx = Instantiate(axFact);
            createAx.transform.position = transform.position + new Vector3(0, y, 0);
            ax = createAx.GetComponent<ToolItem>();
            // 바닥에 있는 아이템 제거
            Destroy(pail.gameObject);
        }
        // 곡갱이를 들고있는 상태에서 도끼를 드는 경우
        if (tool[1].activeSelf && isTool[0] && isTool[2] == false)
        {
            // 손에 있는 아이템 교체
            tool[1].SetActive(false);
            tool[0].SetActive(true);
            // 바닥에 놓을 아이템
            createPick = Instantiate(pickFact);
            createPick.transform.position = transform.position + new Vector3(0, y, 0);
            pick = createPick.GetComponent<ToolItem>();
            // 바닥에 있는 아이템 제거
            Destroy(ax.gameObject);
        }
        // 곡갱이를 들고있는 상태에서 양동이를 드는 경우
        if (tool[1].activeSelf && isTool[2] && isTool[0] == false)
        {
            // 손 내리기
            RotArm(lArm, -90);
            // 손에 들고 있는 아이템 바꾸기
            tool[1].SetActive(false);
            tool[2].SetActive(true);
            // 바닥에 내려놓을 아이템
            createPick = Instantiate(pickFact);
            createPick.transform.position = transform.position + new Vector3(0, y, 0);
            pick = createPick.GetComponent<ToolItem>();
            // 바닥에 있는 아이템 제거
            Destroy(pail.gameObject);
        }
        // 양동이를 들고있는 상태에서 도끼를 드는 경우
        if (tool[2].activeSelf && isTool[0] && isTool[1] == false)
        {
            // 손내리기
            RotArm(lArm, 0);
            // 손에 있는 아이템 교체
            tool[2].SetActive(false);
            tool[0].SetActive(true);
            // 바닥에 내려놓을 아이템
            createPail = Instantiate(pailFact);
            createPail.transform.position = transform.position + new Vector3(0, y, 0);
            pail = createPail.GetComponent<ToolItem>();
            armState = 1;
            // 바닥에 있는 아이템 제거
            Destroy(ax.gameObject);
        }
        // 양동이를 들고있는 상태에서 곡갱이 드는 경우
        if (tool[2].activeSelf && isTool[1] && isTool[0] == false)
        {
            // 손내리기
            RotArm(lArm, 0);
            // 손에 있는 아이템 교체
            tool[2].SetActive(false);
            tool[1].SetActive(true);
            // 바닥에 내려놓을 아이템
            createPail = Instantiate(pailFact);
            createPail.transform.position = transform.position + new Vector3(0, y, 0);
            pail = createPail.GetComponent<ToolItem>();
            armState = 1;
            // 바닥에 있는 아이템 제거
            Destroy(pick.gameObject);
        }
    }


    /*
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

        }*/
}
