using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 이동
public class PlayerMove : MonoBehaviour
{
    // 필요 속성 : 속도
    public float speed = 3;

    // 캐릭터 컨트롤러
    CharacterController playerCc;
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
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 방향 전환에 따른 캐릭터 앞?
        if(h <= -1)
        {
            transform.forward = Vector3.left;
        }
        else if(h >= 1)
        {
            transform.forward = Vector3.right;
        }   
        if(v >= 1)
        {
            transform.forward = Vector3.forward;
        }
        if(v <= -1)
        {
            transform.forward = Vector3.back;
        }

        // 2. 방향 설정하기
        Vector3 dir = h * Vector3.right + v * Vector3.forward;
        dir.Normalize();


        // 3. 플레이어 움직이기
        playerCc.Move(dir * speed * Time.deltaTime);
    }
}
