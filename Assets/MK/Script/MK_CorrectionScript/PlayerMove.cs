using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 플레이어 이동
// 1. 기본적인 이동
// 2. 플레이어가 바라보는 부분
// 3. 대쉬
public class PlayerMove : MonoBehaviourPun, IPunObservable
{
    // 필요 속성 : 속도
    public float speed = 3;
    // 대쉬 속도
    public float a = 2;

    // 최종 속도
    public float finSpeed;

    // 대쉬 시간
    public float dashTime = 0.3f;

    // 입력값 받아오는 변수
    float h;
    float v;

    // 리지드바디
    Rigidbody rigid;

    // 네트워크
    // 위치
    Vector3 receivePos;
    Vector3 receiveNPos;
    // 회전
    Quaternion receiveRot;
    // 보간 속력

    public Transform NicknameUI;
    public Text nName;
    public float lerpSpeed = 10;
    float curTime;
    float SetTime=5;

    string s;

    // Start is called before the first frame update
    void Start()
    {
        // 리지드바디
        rigid = GetComponentInChildren<Rigidbody>();
        // 속도 초기화
        finSpeed = speed;
        nName.text = photonView.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        s = SceneManager.GetActiveScene().name;
        if (s == "WaitingRoom")
        {
            NicknameUI.transform.gameObject.SetActive(true);
        }
        else
        {
            NicknameUI.transform.gameObject.SetActive(false);
        }
        // 움직임 연동 : 내것이 아니면 반환
        if (photonView.IsMine)
        {
            // 1. 사용자 입력값 받기
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");

            // 방향 전환에 따른 캐릭터 바라보는 부분
            LookPlayer();

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Dashing();
            }

            // 2. 방향 설정하기
            Vector3 dir = h * Vector3.right + v * Vector3.forward;
            dir.Normalize();

            // 3. 플레이어 움직이기
            transform.position += dir * finSpeed * Time.deltaTime;
            NicknameUI.transform.position = transform.position + new Vector3(0, 3, 0);
            
        }
        else
        {
            // Lerp를 이용해서 목적지, 목적방향까지 이동 및 회전
            transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
            NicknameUI.position = Vector3.Lerp(NicknameUI.transform.position, receiveNPos, lerpSpeed * Time.deltaTime);
            
        }
    }

    // 플레이어의 앞방향 전환하는 함수
    void LookPlayer()
    {
        // 왼쪽
        if (h <= -1)
        {
            transform.forward = Vector3.left;
        }
        // 오른쪽
        if (h >= 1)
        {
            transform.forward = Vector3.right;
        }
        // 앞
        if (v >= 1)
        {
            transform.forward = Vector3.forward;
        }
        // 아래
        if (v <= -1)
        {
            transform.forward = Vector3.back;
        }
        // 오른쪽 앞대각선 
        if(h > 0 && v > 0)
        {
            transform.forward = Vector3.forward + Vector3.right;
        }
        // 오른쪽 아래 대각선
        if (h > 0 && v < 0)
        {
            transform.forward = Vector3.back + Vector3.right;
        }
        // 왼쪽 위 대각선
        if (h < 0 && v > 0)
        {
            transform.forward = Vector3.forward + Vector3.left;
        }
        if (h < 0 && v < 0)
        {
            transform.forward = Vector3.back + Vector3.left;
        }
    }

    // 대쉬 기능
    void Dashing()
    {
        StartCoroutine(IEDash());
    }

    IEnumerator IEDash()
    {
        finSpeed = speed * a;
        yield return new WaitForSeconds(dashTime);
        finSpeed = speed;
    }

    #region 네트워크
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 데이터 보내기와 받기는 한가지만 ture가 됨
        // 데이터 보내기
        if (stream.IsWriting)
        {
            // position, rotation => class 못넘김 value만 가능, value 타입 배열이나 리스트 가능
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(NicknameUI.transform.position);

        }
        // 데이터 받기
        else if (stream.IsReading) // = if (stream.IsReading)
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
            receiveNPos = (Vector3)stream.ReceiveNext();
        }
    }
    #endregion
    //void ShowNickName()
    //{
    //    photonView.RPC("RpcshowNickName", RpcTarget.All);
    //}
    //[PunRPC]
    //void RpcshowNickName()
    //{
    //    nName.text = GameInfo.instance.nickName;
    //}
}
