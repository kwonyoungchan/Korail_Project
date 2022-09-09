using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스페이드바를 누르면 팔을 들어올리면서 아이템을 먹음
public class PlayerGetItem : MonoBehaviour
{
    // 팔
    public GameObject rArm;
    public GameObject lArm;

    // 회전 속도 
    public float rotSpeed = 3;

    // 팔 상태
    int armState = 1;

    // 도구 상태
    bool isTool;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isTool = GameObject.FindWithTag("Tool").GetComponent<ToolItem>().isAx || GameObject.FindWithTag("Tool").GetComponent<ToolItem>().isPick;
        if (isTool)
        {
            // 점프키를 누른다면
            if (Input.GetButtonDown("Jump"))
            {
                // 팔의 rotate 값이 -90, 0, 0이면 0, 0, 0으로 돌려놓기
                if (armState > 1)
                {
                    RotArm(rArm, 0);

                    armState = 0;
                }
                else
                {
                    // 플레이어의 팔이 위로 올라감
                    RotArm(rArm, -90);
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

    // 아이템 얻고 들고 있는 모션 만들기
    void ItemOnArm()
    {

    }
}
