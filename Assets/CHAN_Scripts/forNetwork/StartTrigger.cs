using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]Collider[] detect;
    Vector3 boxSize = new Vector3(2, 2, 2);
    void Start()
    {
        
    }

    //여기서 검출된 플레이어가 4명이 되면 게이지가 올라가고 게이지가 100% 되면 게임 씬으로 들어가게 한다.
    //그리고 게임 오버나 클리어하고 다시 해당 씬으로 돌아 올 때, 관련 요소들 다시 초기화 해야하는거 잊지 말자.

    void Update()
    {
        detect = Physics.OverlapBox(transform.position,boxSize*0.5f,default,1<<6);
    }
    private void OnTriggerEnter(Collider other)
    {
     
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}
