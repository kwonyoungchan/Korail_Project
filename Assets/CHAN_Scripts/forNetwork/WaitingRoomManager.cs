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
    // spawnPos 둘 변수
    public Vector3[] spawnPos;
    public Button GoBtn;
    public Button BackBtn;

    // 난이도 조절 버튼 관련 변수
    // 우측 선택
    public Button RBtn;
    // 좌측 선택
    public Button LBtn;
    // 텍스트 표시
    public Text Level;
    public float minSpeed;
    public float maxSpeed;


    void Start()
    {
        //초기에는 레벨설정을 아기으로 설정한다.
        Level.text = levels[0];
        //초기 기차속도를 저장
        minSpeed = GameInfo.instance.trainSpeed;
        maxSpeed = GameInfo.instance.trainSpeed + levels.Length;
        //OnPhotonSerializeView 호출 빈도
        PhotonNetwork.SerializationRate = 60;
        PhotonNetwork.SendRate = 60;
        // 1. spawnPos의 갯수를 할당
        spawnPos = new Vector3[PhotonNetwork.CurrentRoom.MaxPlayers];
        float angle = 360.0f / spawnPos.Length;
        // 2. 한 횟수당 회전 각도를 계산
        for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
        {
            spawnPos[i] = transform.position + transform.forward * 5;
            transform.Rotate(0, angle, 0);
        }
        // 현재 방에 들어와있는 인원수를 이용해서 index 구하자
        int idx = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        //플레이어를 생성한다.
        string s = GameInfo.instance.CallBackName();
        PhotonNetwork.Instantiate("MK_Prefab" + "/" + "PlayerPrefabs" + "/" + s, GameInfo.instance.curPos + new Vector3(0, 1, 0), Quaternion.identity);
    }
    void Update()
    {

    }

    //방에 플레이어가 참여 했을 때 호출해주는 함수
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        print(newPlayer.NickName + " 님이 이 방에 들어왔습니다.");
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

    //오른쪽 버튼 눌렀을 때
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
    //왼쪽 버튼 눌렀을 때
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
