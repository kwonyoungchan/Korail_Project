using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    public static WaitingRoomManager instance;
    public string[] levels;
    int num;
    private void Awake()
    {
        instance = this;
    }
    // spawnPos �� ����
    public Vector3[] spawnPos;
    public Button GoBtn;
    public Button BackBtn;

    // ���̵� ���� ��ư ���� ����
    // ���� ����
    public Button RBtn;
    // ���� ����
    public Button LBtn;
    // �ؽ�Ʈ ǥ��
    public Text Level;
    public float minSpeed;
    public float maxSpeed;


    void Start()
    {
        //�ʱ⿡�� ���������� �Ʊ����� �����Ѵ�.
        Level.text = levels[0];
        //�ʱ� �����ӵ��� ����
        minSpeed = GameInfo.instance.trainSpeed;
        maxSpeed = GameInfo.instance.trainSpeed + levels.Length;
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
        string s = GameInfo.instance.CallBackName();
        PhotonNetwork.Instantiate("MK_Prefab" + "/" + "PlayerPrefabs" + "/" + s, GameInfo.instance.curPos + new Vector3(0, 1, 0), Quaternion.identity);
    }
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

            }
            if (photonView.IsMine)
                LoadGameScene();
        }
    }
    void LoadGameScene()
    {
        photonView.RPC("RpcLoadGameScene", RpcTarget.All);
    }

    //������ ��ư ������ ��
    public void ClickedRightButton()
    {
        num++;
        if (num > levels.Length - 1)
        {
            num = 0;
            GameInfo.instance.trainSpeed = minSpeed;
            print(num);
        }
        GameInfo.instance.trainSpeed += 1;
        Level.text = levels[num];
        print(GameInfo.instance.trainSpeed);


    }
    //���� ��ư ������ ��
    public void ClickedLeftButton()
    {
        num--;
        if (num < 0)
        {
            num = levels.Length - 1;
            GameInfo.instance.trainSpeed = maxSpeed;
            print(num);
        }
        GameInfo.instance.trainSpeed -= 1;
        Level.text = levels[num];
        print(GameInfo.instance.trainSpeed);
    }

    [PunRPC]
    void RpcLoadGameScene()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }

    public void ReJoinLobby()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        GameObject clientManager = GameObject.Find("clientManager");
        GameObject player = GameObject.Find("Player(Clone)");
        Destroy(clientManager);
        Destroy(player);
        GameObject lobbyPlayer=Instantiate(GameInfo.instance.charaters_Lobby[GameInfo.instance.CharacterCount]);
        lobbyPlayer.transform.position = GameInfo.instance.curPos;
        PhotonNetwork.LoadLevel("MainLobby");
    }

}
