using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PutDownItem 역할
public class PlayerItemDown : MonoBehaviour
{
    // 아이템 정보 확인
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
    // 팔에 있는 도구 활성화
    public GameObject[] tool = new GameObject[3];
    // 팔에 있는 mat 활성화
    public GameObject[] mat;

    #region 팔 회전 관련
    // 팔
    public GameObject rArm;
    public GameObject lArm;

    // 회전 속도 
    public float rotSpeed = 3;

    // 팔 상태
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
        // 레이를 발사하고
        Ray pRay = new Ray(transform.position, -transform.up);
        RaycastHit cubeInfo;
        // 스페이스 바를 누르면
        if (Physics.Raycast(pRay, out cubeInfo))
        {

            if(Input.GetButtonDown("Jump"))
            {
                if(cubeInfo.transform.gameObject.GetComponent<ToolGOD>() == null)
                {
                    return;
                }
                // 바닥 상태 : 아무것도 없음
                if(cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState == ToolGOD.Tools.Idle)
                {
                    // 오류 사항 : 손에 재료를 들고 있을 경우, 발생
                    // 손에 무언갈 들고 있을 때,
                    if (armState > 0)
                    {
                        holdState = Hold.ChangeIdle;
                        // 손에 있는 것에 따른 바닥의 변화
                        // 도끼
                        if (tool[0].activeSelf)
                        {
                            cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Ax;
                            return;
                        }
                        // 곡갱이
                        if (tool[1].activeSelf)
                        {
                            cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Pick;
                            return;
                        }
                        // 양동이
                        if (tool[2].activeSelf)
                        {
                            cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Pail;
                            return;
                        }
                    }
                    // 손이 비어있을 때
                    else
                    {
                        holdState = Hold.Idle;
                    }
                }
                // 바닥 상태 : 도끼
                if (cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState == ToolGOD.Tools.Ax)
                {
                    // 손에 무언갈 들고 있을 때
                    if (armState > 0)
                    {
                        hand = CheckHand();
                        // 곡갱이를 들고 있다면
                        if(hand == 1)
                        {
                            cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Pick;
                            holdState = Hold.Ax;
                        }
                        // 양동이를 들고 있다면
                        if(hand == 2)
                        {
                            cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Pail;
                            holdState = Hold.Ax;
                        }

                    }
                    else
                    {
                        // 플레이어 상태를 변환한다
                        holdState = Hold.Ax;
                        // 레이의 상태도 변화
                        cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Idle;
                    }
                }
                // 바닥 상태 : 곡갱이
                if (cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState == ToolGOD.Tools.Pick)
                {
                    // 손에 무언가 있을 때
                    if (armState > 0)
                    {
                        hand = CheckHand();
                        // 도끼를 들고 있다면
                        if (hand == 0)
                        {
                            cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Ax;
                            holdState = Hold.Pick;
                        }
                        // 양동이를 들고 있다면
                        if (hand == 2)
                        {
                            cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Pail;
                            holdState = Hold.Pick;
                        }

                    }
                    else
                    {
                        // 플레이어 상태를 변환한다
                        holdState = Hold.Pick;
                        // 레이의 상태도 변화
                        cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Idle;
                    }
                }
                // 바닥 상태 : 양동이
                if (cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState == ToolGOD.Tools.Pail)
                {
                    // 손에 무언가 있을 때
                    if (armState > 0)
                    {
                        hand = CheckHand();
                        // 도끼를 들고 있다면
                        if (hand == 0)
                        {
                            cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Ax;
                            holdState = Hold.Pail;
                        }
                        // 곡갱이를 들고 있다면
                        if (hand == 1)
                        {
                            cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Pick;
                            holdState = Hold.Pail;
                        }

                    }
                    else
                    {
                        // 플레이어 상태를 변환한다
                        holdState = Hold.Pail;
                        // 레이의 상태도 변화
                        cubeInfo.transform.gameObject.GetComponent<ToolGOD>().toolsState = ToolGOD.Tools.Idle;
                    }
                }
            }
        }
    }

    // 플레이어 상태
    void PlayerFSM()
    {
        switch (holdState)
        {
            // 아무것도 들고 있지 않을 때,
            case Hold.Idle:
                for(int i = 0; i < tool.Length; i++)
                {
                    tool[i].SetActive(false);
                }
                break;
            // 도구를 들고 있다가 내려 놓을 때
            case Hold.ChangeIdle:
                // 한쪽팔만 들 때
                if (armState > 0 && armState < 2)
                {
                    RotArm(rArm, 0);
                }
                // 양쪽팔을 들고 있을 때
                else
                {
                    RotArm(lArm, 0);
                    RotArm(rArm, 0);
                }
                // Idle 상태로 변환
                holdState = Hold.Idle;
                armState = 0;
                break;
            #region 도구
            // 도끼를 들고 있을 때,
            case Hold.Ax:
                // 도끼 활성화
                if(armState > 1)
                {
                    if(lArm.transform.localEulerAngles != new Vector3(0, 0, 0))
                    {
                        RotArm(lArm, 0);
                    }
                    tool[0].SetActive(true);
                    return;
                }
                // 팔 돌리기
                RotArm(lArm, 0);
                RotArm(rArm, -90);
                tool[0].SetActive(true);
                break;
            // 곡갱이를 들고 있을 때
            case Hold.Pick:
                // 도구 활성화
                if (armState > 1)
                {
                    if (lArm.transform.localEulerAngles != new Vector3(0, 0, 0))
                    {
                        RotArm(lArm, 0);
                    }
                    tool[1].SetActive(true);
                    return;
                }
                // 팔 돌리기
                RotArm(lArm, 0);
                RotArm(rArm, -90);
                tool[1].SetActive(true);
                break;
            // 양동이를 들고 있을 때,
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
                // 팔 돌리기
                RotArm(rArm, -90);
                RotArm(lArm, -90);
                tool[2].SetActive(true);
                break;
            #endregion
            #region 선로
            // 선로를 들고 있을 때
            case Hold.Rail:
                if (armState > 2)
                {
                    return;
                }
                RotArm(rArm, -90);
                RotArm(lArm, -90);
                break;
            #endregion
            #region 재료
            // 나뭇가지를 들고 있을 때
            case Hold.Branch:
                if (armState > 2)
                {
                    return;
                }
                RotArm(rArm, -90);
                RotArm(lArm, -90);
                break;
            // 철을 들고 있을 때,
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
    // 플레이어 팔이 위로 올라가게 만드는 함수
    void RotArm(GameObject arm, float rotAngle)
    {
        // 팔 회전 시키기
        arm.transform.localEulerAngles = new Vector3(rotAngle, 0, 0);
        // 팔 회전 상태
        armState++;
    }

    // 플레이어 손에 들고 있는 것 확인
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
