using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomItem : MonoBehaviour
{
    //���� (���̸� (0/0)
    public Text roomInfo;
    // ����
    public Text roomDesc;
    // �� id
    int map_id;
    // Ŭ���� ���� �� ȣ��Ǵ� �Լ��� ������ �ִ� ����
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
        // ���࿡ onClickAction �� null�� �ƴ϶�� 
        if (OnclickAction != null)
        {
            // onClickAction�� �����ϰڴ�.
            OnclickAction(name);
        }
        ////1. InputRoomName ���� ������Ʈ ã��
        //GameObject go = GameObject.Find("InputRoomName");
        ////2. InputField ������Ʈ ��������
        //InputField inputField = go.GetComponent<InputField>();
        ////3. text�� roomName 
        //inputField.text = name;
    }
    public void SetInfo(RoomInfo info)
    {
        SetInfo((string)info.CustomProperties["room_name"], info.PlayerCount, info.MaxPlayers);

        //desc ����
        roomDesc.text = (string)info.CustomProperties["desc"];
        //map_id ����
        //map_id = (int)info.CustomProperties["map_id"];
    }
}
