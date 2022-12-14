using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




// 플레이어 이동
// 1. 기본적인 이동
// 2. 플레이어가 바라보는 부분
// 3. 대쉬
public class PlayerMove_Lobby : MonoBehaviour
{
    // 필요 속성 : 속도
    public float speed = 3;
    // 대쉬 속도
    public float a = 2;

    // 최종 속도
    public float finSpeed;
    public Transform IntroUI;
    public Transform IntroUI2;
    int n = -1;
    GameObject ui;

    // 대쉬 시간
    public float dashTime = 0.3f;

    // 입력값 받아오는 변수
    float h;
    float v;




    // 보간 속력
    public float lerpSpeed = 10;
    float curTime;
    public float SetTime=5;

    // Start is called before the first frame update
    void Start()
    {
        // 속도 초기화
        finSpeed = speed;
        IntroUI.transform.gameObject.SetActive(false);
        IntroUI2.transform.gameObject.SetActive(false);
        ui = IntroUI.transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (n == -1)
        {
            ui = IntroUI2.transform.gameObject;
        }
        else
        {
            ui = IntroUI.transform.gameObject;
        }
        curTime += Time.deltaTime;
        if (curTime > SetTime)
        {
            ui.gameObject.SetActive(true);
        }
        if (curTime > 2 * SetTime)
        {
            ui.gameObject.SetActive(false);
            n *= -1;
            curTime = 0;
        }
        // 움직임 연동 : 내것이 아니면 반환

        // 1. 사용자 입력값 받기
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dashing();
        }
        LookPlayer();

        // 2. 방향 설정하기
        Vector3 dir = h * Vector3.right + v * Vector3.forward;
        dir.Normalize();

        // 3. 플레이어 움직이기
        transform.position += dir * finSpeed * Time.deltaTime;
        ui.transform.position = transform.position+new Vector3(1,3,0);
        


    }

    // 플레이어의 앞방향 전환하는 함수

    // 대쉬 기능
    void Dashing()
    {
        StartCoroutine(IEDash());
    }
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
        if (h > 0 && v > 0)
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

    IEnumerator IEDash()
    {
        finSpeed = speed * a;
        yield return new WaitForSeconds(dashTime);
        finSpeed = speed;
    }
}

   