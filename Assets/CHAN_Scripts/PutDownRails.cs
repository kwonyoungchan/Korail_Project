using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutDownRails : MonoBehaviour
{
    [SerializeField ]GameObject RailFac;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //스페이스바를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //사각형의 위치는 Raycasthit으로 결정한다.
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (!hit.transform.name.Contains("rail"))
                {
                    //선로가 생성되고
                    //생성된 선로는 인근의 사각형위에 위치하게 된다.
                    // 나중에 합치게 되면 아래의 생성 대신 위치시키는 기능만 넣도록 하자.
                    GameObject rail = Instantiate(RailFac);
                    // hitPoint를 보간해야 할 필요가 있음
                    // 좌표의 단위가 소수점이 나왔으면
                    // 그 값을 내림하고 다시 0.5를 더한다.
                    Vector3 interpolatePos;
                    interpolatePos.x = Mathf.Floor(hit.point.x) + 0.5f;
                    interpolatePos.y = 0.5f;
                    interpolatePos.z = Mathf.Floor(hit.point.z) + 0.5f;
                    rail.transform.position = interpolatePos;
                }
                else
                {
                    print("겹침");
                }
            }
            
            
        }
    }
}
