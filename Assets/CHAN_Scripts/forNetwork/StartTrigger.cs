using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class StartTrigger : MonoBehaviourPun
{
    // Start is called before the first frame update
    [SerializeField] Collider[] detect;
    Vector3 boxSize = new Vector3(10, 2, 10);
    float curTime;
   [SerializeField] float setTime;
   [SerializeField] Scrollbar Scrollbar;
   [SerializeField] GameObject Handle;
    void Start()
    {
        Handle.SetActive(false);
    }

    //여기서 검출된 플레이어가 4명이 되면 게이지가 올라가고 게이지가 100% 되면 게임 씬으로 들어가게 한다.
    //그리고 게임 오버나 클리어하고 다시 해당 씬으로 돌아 올 때, 관련 요소들 다시 초기화 해야하는거 잊지 말자.

    void Update()
    {
        detect = Physics.OverlapBox(transform.position, boxSize * 0.5f, default, 1 << 6);
        if (detect.Length == 3)
        {
            // 로딩 시작
            StartLoading();
            ShowGuageUI();
        }
        else
        {
            //아니면 게이지 초기화
            curTime = 0;
            Scrollbar.size = 0;
            Handle.SetActive(false);
        }
    }
    void StartLoading()
    {
        // ui 에 게이지가 차도록 만든다. 
        // 시간은 방장 기준으로 해도 게이지 ui는 RPC로 보내야 한다. 
        curTime += Time.deltaTime;
        if (curTime > setTime)
        {
            WaitingRoomManager.instance.LoadArena();
        }
    }
    void ShowGuageUI()
    {
        Handle.SetActive(true);
        Scrollbar.size +=  Time.deltaTime/setTime;
    }
}
