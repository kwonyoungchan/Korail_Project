using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // 카메라가 이동하는 스크립트
    // 카메라가 움직이는 기준 : 기차
    // 기차가 x축 방향으로 이동하는것만 카메라가 판별해서 이동한다.
    // 단 카메라의 방위와 고도는 변하지 않는다. 
    // 필요 속성
    // 기차의 위치 
    public Transform TrainPos;
    [SerializeField] Vector3 setPos;
    void Start()
    {
        transform.position = TrainPos.position + setPos; 
    }

    void Update()
    {
        //카메라이동은 기차의 x축방향 이동에만 관여한다.
        Vector3 pos = Vector3.zero;
        if (!TrainPos.gameObject.activeSelf)
        {
            pos.x = TrainPos.position.x;
        }
        transform.position = pos + setPos;
    }
}
