using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomItem : MonoBehaviour
{
    //내용 (방이름 (0/0)
    public Text roomInfo;
    // 설명
    public Text roomDesc;
    // 맵 id
    int map_id;
    // 클릭이 됐을 때 호출되는 함수를 가지고 있는 변수
    public Action<string> OnclickAction;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetInfo(string roomName, int currPlayer, byte maxPlayer)
    {
        name = roomName;
        roomInfo.text = roomName + "(" + currPlayer + " / " + maxPlayer + ")";
    }
    public void OnClick()
    {
        // 만약에 onClickAction 가 null이 아니라면 
        if (OnclickAction != null)
        {
            // onClickAction을 실행하겠다.
            OnclickAction(name);
        }
        ////1. InputRoomName 게임 오브젝트 찾자
        //GameObject go = GameObject.Find("InputRoomName");
        ////2. InputField 컴포넌트 가져오자
        //InputField inputField = go.GetComponent<InputField>();
        ////3. text에 roomName 
        //inputField.text = name;
    }
    public void SetInfo(RoomInfo info)
    {
        SetInfo((string)info.CustomProperties["room_name"], info.PlayerCount, info.MaxPlayers);

        //desc 설정
        roomDesc.text = (string)info.CustomProperties["desc"];
        //map_id 설정
        //map_id = (int)info.CustomProperties["map_id"];
    }
}
