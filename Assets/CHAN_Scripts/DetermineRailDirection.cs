using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetermineRailDirection : MonoBehaviour
{
    //선로가 바닥에 놓아지는 순간 선로의 방향이 결정되어야 함
    //선로는 다시 회수되어야 함.
    //먼저 선로의 방향을 정의해보자
    //1. 수평
    //2. 수직
    //3. 커브(우회전)
    //4. 커브(좌회전)
    void Start()
    {
        //여기서 한번만 방향이 결정됨
    }

    void Update()
    {
        // 여기서 회수 함수가 발동되어야 함
        // 회수기능의 발동 조건은 마지막에 설치한 선로여야 함
        // 만약에 코너선로가 생성된다면
        // 해당 선로는 코너선로 방향에 맞도록 재정렬해야 함
    }
    void SetDirection()
    { 
        
    }
}
