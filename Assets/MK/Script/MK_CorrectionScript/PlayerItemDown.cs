using UnityEngine;
using Photon.Pun;

// PutDownItem 역할
public class PlayerItemDown : MonoBehaviourPun, IPunObservable
{
    // 아이템 정보 확인
    public enum Hold
    {
        Idle,
        ChangeIdle,
        Ax,
        Pick,
        Pail,
        Mat,
        Animal
    }
    public Hold holdState;
    // 팔에 있는 도구 활성화
    public GameObject[] tool = new GameObject[3];
    // 팔에 있는 mat 활성화
    public GameObject[] mat;
    // 레이 발사 위치
    public Transform rayPos;

    #region 팔 회전 관련
    // 팔
    public GameObject rArm;
    public GameObject lArm;

    // 회전 속도 
    public float rotSpeed = 3;

    // 팔 상태
    [HideInInspector]
    public int num = 1;
    #endregion

    public int f;
    int hand;

    // ToolGod 컴포넌트
    ToolGOD toolGOD;
    MaterialGOD matGOD;
    ItemGOD itemGOD;
    PlayerForwardRay player;
    PlayerAnim anim;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerForwardRay>();
        anim = GetComponent<PlayerAnim>();
    }

    // Update is called once per frame
    void Update()
    {
        // 움직임 연동 : 내것이 아니면 반환
        if (photonView.IsMine)
        {

            PlayerFSM(holdState);
            // 레이를 발사하고
            Ray pRay = new Ray(rayPos.position + new Vector3(-0.2f, 0, 0), -transform.up);
            RaycastHit cubeInfo;
            // 스페이스 바를 누르면
            if (Input.GetButtonDown("Jump"))
            {

                if (Physics.Raycast(pRay, out cubeInfo))
                {
                    toolGOD = cubeInfo.transform.gameObject.GetComponent<ToolGOD>();
                    matGOD = cubeInfo.transform.gameObject.GetComponent<MaterialGOD>();
                    itemGOD = cubeInfo.transform.gameObject.GetComponent<ItemGOD>();
                    RiverGOD riverGod = cubeInfo.transform.gameObject.GetComponent<RiverGOD>();
                    //if (matGOD == null) return;
                    if (matGOD == null || matGOD.matState != MaterialGOD.Materials.Idle || player.isItemDown) return;

                    // 바닥 상태 : 아무것도 없음
                    if (toolGOD.toolsState == ToolGOD.Tools.Idle)
                    {
                        // 오류 사항 : 손에 재료를 들고 있을 경우, 발생
                        // 손에 무언갈 들고 있을 때,
                        // 손에 있는 것에 따른 바닥의 변화
                        // 도끼
                        if (tool[0].activeSelf)
                        {
                            PlayerFSM(Hold.ChangeIdle);
                            // toolGOD.toolsState = ToolGOD.Tools.Ax;
                            toolGOD.ChangeState(ToolGOD.Tools.Ax);
                            return;
                        }
                        // 곡갱이
                        else if (tool[1].activeSelf)
                        {
                            PlayerFSM(Hold.ChangeIdle);
                            // toolGOD.toolsState = ToolGOD.Tools.Pick;
                            toolGOD.ChangeState(ToolGOD.Tools.Pick);
                            return;
                        }
                        // 양동이
                        else if (tool[2].activeSelf)
                        {
                            PlayerFSM(Hold.ChangeIdle);
                            // toolGOD.toolsState = ToolGOD.Tools.Pail;
                            toolGOD.ChangeState(ToolGOD.Tools.Pail);
                            return;
                        }
                        else
                        {
                            if(holdState != Hold.Mat)
                                PlayerFSM(Hold.ChangeIdle);
                        }

                    }
                    // 바닥 상태 : 도끼
                    else if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                    {
                        // 손에 무언갈 들고 있을 때
                        hand = CheckHand();
                        // 곡갱이를 들고 있다면
                        if (hand == 1)
                        {
                            // toolGOD.toolsState = ToolGOD.Tools.Pick;
                            toolGOD.ChangeState(ToolGOD.Tools.Pick);
                            PlayerFSM(Hold.Ax);
                        }
                        // 양동이를 들고 있다면
                        else if (hand == 2)
                        {
                            //toolGOD.toolsState = ToolGOD.Tools.Pail;
                            toolGOD.ChangeState(ToolGOD.Tools.Pail);
                            PlayerFSM(Hold.Ax);
                        }
                        else
                        {
                            // 플레이어 상태를 변환한다
                            PlayerFSM(Hold.Ax);
                            // 레이의 상태도 변화
                            //toolGOD.toolsState = ToolGOD.Tools.Idle;
                            toolGOD.ChangeState(ToolGOD.Tools.Idle);
                        }

                    }
                    // 바닥 상태 : 곡갱이
                    else if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                    {

                        hand = CheckHand();
                        // 도끼를 들고 있다면
                        if (hand == 0)
                        {
                            // toolGOD.toolsState = ToolGOD.Tools.Ax;
                            toolGOD.ChangeState(ToolGOD.Tools.Ax);
                            PlayerFSM(Hold.Pick);
                        }
                        // 양동이를 들고 있다면
                        else if (hand == 2)
                        {
                            //toolGOD.toolsState = ToolGOD.Tools.Pail;
                            toolGOD.ChangeState(ToolGOD.Tools.Pail);
                            PlayerFSM(Hold.Pick);
                        }
                        else
                        {
                            // 플레이어 상태를 변환한다
                            PlayerFSM(Hold.Pick);
                            // 레이의 상태도 변화
                            //toolGOD.toolsState = ToolGOD.Tools.Idle;
                            toolGOD.ChangeState(ToolGOD.Tools.Idle);
                        }

                    }
                    // 바닥 상태 : 양동이
                    else if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                    {
                        // 손에 무언가 있을 때

                        hand = CheckHand();
                        // 도끼를 들고 있다면
                        if (hand == 0)
                        {
                            // toolGOD.toolsState = ToolGOD.Tools.Ax;
                            toolGOD.ChangeState(ToolGOD.Tools.Ax);
                            PlayerFSM(Hold.Pail);
                        }
                        // 곡갱이를 들고 있다면
                        else if (hand == 1)
                        {
                            //toolGOD.toolsState = ToolGOD.Tools.Pick;
                            toolGOD.ChangeState(ToolGOD.Tools.Pick);
                            PlayerFSM(Hold.Pail);
                        }
                        else
                        {
                            // 플레이어 상태를 변환한다
                            PlayerFSM(Hold.Pail);
                            // 레이의 상태도 변화
                            //toolGOD.toolsState = ToolGOD.Tools.Idle;
                            toolGOD.ChangeState(ToolGOD.Tools.Idle);
                        }

                    }

                }
            }

        }
        else
        {
            PlayerFSM(holdState);
        }
    }
    public void PlayerFSM(Hold s)
    {
        //photonView.RPC("PUNPlayerFSM", RpcTarget.All, s);
        PUNPlayerFSM(s);
    }
    // 플레이어 상태
    void PUNPlayerFSM(Hold s)
    {

        holdState = s;
        switch (s)
        {
            // 아무것도 들고 있지 않을 때,
            case Hold.Idle:
                for(int i = 0; i < tool.Length; i++)
                {
                    tool[i].SetActive(false);
                }
                anim.AnimState(PlayerAnim.Anim.Move);
                break;
            // 도구를 들고 있다가 내려 놓을 때
            case Hold.ChangeIdle:
                // 한쪽팔만 들 때
                if(lArm.transform.localEulerAngles != new Vector3(-80, 0, 0) || rArm.transform.localEulerAngles != new Vector3(-85, 0, 0))
                {
                    RotArm(lArm, -80, 0);
                    RotArm(rArm, -85, 0);

                }
                // Idle 상태로 변환
                holdState = Hold.Idle;
                break;
            #region 도구
            // 도끼를 들고 있을 때,
            case Hold.Ax:
                // 도끼 활성화
                // 팔 돌리기
                RotArm(lArm, -80, 0);
                RotArm(rArm, -85, 0);
                tool[0].SetActive(true);
                break;
            // 곡갱이를 들고 있을 때
            case Hold.Pick:
                // 도구 활성화
                // 팔 돌리기
                RotArm(lArm, -80, 0);
                RotArm(rArm, -85, 0);
                tool[1].SetActive(true);
                break;
            // 양동이를 들고 있을 때,
            case Hold.Pail:
                // 팔 돌리기
                RotArm(rArm, -85, 90);
                RotArm(lArm, -80, -90);
                tool[2].SetActive(true);
                anim.AnimState(PlayerAnim.Anim.Idle);
                break;
            #endregion
            #region 재료
            // 선로를 들고 있을 때
            case Hold.Mat:
                for (int i = 0; i < tool.Length; i++)
                {
                    tool[i].SetActive(false);
                }
                RotArm(rArm, -85, 90);
                RotArm(lArm, -80, -90);
                anim.AnimState(PlayerAnim.Anim.Idle);
                break;
            #endregion
            case Hold.Animal:
                for (int i = 0; i < tool.Length; i++)
                {
                    tool[i].SetActive(false);
                }
                RotArm(rArm, -85, 90);
                RotArm(lArm, -80, -90);
                anim.AnimState(PlayerAnim.Anim.Idle);
                break;
        }
    }
    // 플레이어 팔이 위로 올라가게 만드는 함수
    void RotArm(GameObject arm, float rotX, float rotAngle)
    {
        // 팔 회전 시키기
        arm.transform.localEulerAngles = new Vector3(rotX, 0, rotAngle);
        // 팔 회전 상태
        // num++;
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(holdState);
        }
        else
        {
            holdState = (Hold)stream.ReceiveNext();
        }
    }
}
