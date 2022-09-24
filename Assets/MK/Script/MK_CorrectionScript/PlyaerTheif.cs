using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 레이를 쏘다가 Theif가 맞으면 Thief의 state를 Run으로 변경
public class PlyaerTheif : MonoBehaviour
{
    // 레이 쏘는 위치 
    public GameObject rPos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어가 앞으로 레이를 쏜다
        Ray playerRay = new Ray(rPos.transform.position, transform.forward);
        RaycastHit rayInfo;
        // 만약 맞은 물체가 있다면
        if (Physics.Raycast(playerRay, out rayInfo))
        {
            // 맞은 물체가 도둑이라면 state를 Run으로 바꿈
            Theif theif = rayInfo.transform.gameObject.GetComponent<Theif>();
            if (theif)
            {
                theif.state = Theif.TState.Run;
            }
        }
    }
}
