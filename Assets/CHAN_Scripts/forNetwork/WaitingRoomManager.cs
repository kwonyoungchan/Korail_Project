using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    public static WaitingRoomManager instance;
    private void Awake()
    {
        instance = this;
    }
    // spawnPos �� ����
    public Vector3[] spawnPos;
    public Button GoBtn;
   
    void Start()
    {
        //OnPhotonSerializeView ȣ�� ��
        PhotonNetwork.SerializationRate = 60;
        PhotonNetwork.SendRate = 60;
        // 1. spawnPos�� ������ �Ҵ�
        spawnPos = new Vector3[PhotonNetwork.CurrentRoom.MaxPlayers];
        float angle = 360.0f / spawnPos.Length;
        // 2. �� Ƚ���� ȸ�� ������ ���
        for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
        {
            spawnPos[i] = transform.position + transform.forward * 5;
            transform.Rotate(0, angle, 0);
        }
        // ���� �濡 �����ִ� �ο����� �̿��ؼ� index ������
        int idx = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        //�÷��̾ �����Ѵ�.
       // if(ClientManager.instance.IsExit() == false)
        {
            PhotonNetwork.Instantiate("MK_Prefab/Player", spawnPos[idx], Quaternion.identity);
        }
       
        
       
        

        //players.Add(obj.GetPhotonView());
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //�濡 �÷��̾ ���� ���� �� ȣ�����ִ� �Լ�
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        print(newPlayer.NickName + " ���� �� �濡 ���Խ��ϴ�.");
    }
   

    public void LoadArena()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                if(photonView.IsMine)
                LoadGameScene();
            }
        }
    }
    void LoadGameScene()
    {
        photonView.RPC("RpcLoadGameScene", RpcTarget.All);
    }

    [PunRPC]
    void RpcLoadGameScene()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }




}