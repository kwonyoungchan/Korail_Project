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
    public Transform triggerPos;
    Vector3 pos;
    void Start()
    {
        Camera.main.transform.position = TrainPos.position + setPos;
        pos = Vector3.zero;
    }

    void Update()
    {
        //카메라이동은 기차의 x축방향 이동에만 관여한다.
        // 기차가 스크린 좌표상에서 특정지점 넘어가면 카메라가 따라서 이동한다. 
        
        //카메라 이동은 Lerp로 보간하면서 이동한다.
        if (TrainPos.gameObject.activeSelf)
        {
            if (TrainPos.position.x > triggerPos.position.x)
            {
                pos.x = Mathf.Lerp(pos.x, TrainPos.position.x, 10 * Time.deltaTime);
            }
        }
        Camera.main.transform.position = pos + setPos;
        
    }
}
