using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 이동
// 1. 기본적인 이동
// 2. 플레이어가 바라보는 부분
// 3. 대쉬
public class PlayerMove : MonoBehaviour
{
    // 필요 속성 : 속도
    public float speed = 3;

    // 캐릭터 컨트롤러
    CharacterController playerCc;

    // 입력값 받아오는 변수
    float h;
    float v;

    // Start is called before the first frame update
    void Start()
    {
        // 캐릭터 컨트롤러 속성 가져오기
        playerCc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 1. 사용자 입력값 받기
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        // 방향 전환에 따른 캐릭터 바라보는 부분
        LookPlayer();

        // 2. 방향 설정하기
        Vector3 dir = h * Vector3.right + v * Vector3.forward;
        dir.Normalize();

        // 3. 플레이어 움직이기
        playerCc.Move(dir * speed * Time.deltaTime);
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
        yield return null;
    }
}
