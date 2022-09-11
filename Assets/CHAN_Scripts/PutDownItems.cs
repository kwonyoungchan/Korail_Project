using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutDownItems : MonoBehaviour
{

    // 플레이어가 현재 들고 있는 아이템의 정보를 알아야 한다.
    int n = -1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        { 
            n*=-1;
        }
        //스페이스바를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //사각형의 위치는 Raycasthit으로 결정한다.
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                #region 이부분은 플레이어가 바로 아이템을 생성하는 코드
                //if (!hit.transform.name.Contains("rail"))
                //{
                //    //선로가 생성되고
                //    //생성된 선로는 인근의 사각형위에 위치하게 된다.
                //    // 나중에 합치게 되면 아래의 생성 대신 위치시키는 기능만 넣도록 하자.
                //    GameObject rail = Instantiate(RailFac);
                //    // hitPoint를 보간해야 할 필요가 있음
                //    // 좌표의 단위가 소수점이 나왔으면
                //    // 그 값을 내림하고 다시 0.5를 더한다.
                //    Vector3 interpolatePos;
                //    interpolatePos.x = Mathf.Floor(hit.point.x) + 0.5f;
                //    interpolatePos.y = 0.5f;
                //    interpolatePos.z = Mathf.Floor(hit.point.z) + 0.5f;
                //    rail.transform.position = interpolatePos;
                //}
                //else
                //{
                //    print("겹침");
                //}
                #endregion
                //바로 rail를 설치한다고 가정
                if (n == -1)
                {
                    if (HasItems.instance.rails.Count > 0
                        &&hit.transform.gameObject.GetComponent<ItemGOD>().items!=ItemGOD.Items.Rail)
                    {
                        hit.transform.gameObject.GetComponent<ItemGOD>()
                       .ChangeState(ItemGOD.Items.Rail);
                        HasItems.instance.RemoveRail();
                    }
                }
                else
                {
                    if (hit.transform.gameObject.GetComponent<ItemGOD>().items == ItemGOD.Items.Rail)
                    {
                        hit.transform.gameObject.GetComponent<ItemGOD>()
                           .ChangeState(ItemGOD.Items.Idle);
                        HasItems.instance.AddRail();
                    }
                }
                
            }


        }
    }


    
}
