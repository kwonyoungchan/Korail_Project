using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ConnectionManager : MonoBehaviourPunCallbacks
{
    //닉네임  InputField
    public InputField inputNickName;
    //접속 Button
    public Button btnConnect;
    void Start()
    {
        //닉네임이 변경될 때, 호출되는 함수 등록
        // inputNickName.onValueChanged.AddListener(OnValueChanged);
        //닉네임에서 Enter를 쳤을 때 호출되는 함수 등록
        inputNickName.onSubmit.AddListener(OnSubmit);

        //닉네임에서 Foucusing을 잃었을 때 호출되는 함수 등록
        // inputNickName.onValueChanged.AddListener(OnEndEdit);

    }

    #region 여기는 InputField관련 설정 모음

    public void OnSubmit(string s)
    {
        //만약 s의 길이가 0보다 크다면
        if (s.Length > 0)
        {
            //접속하자
            OnClickConnect();
        }
        print("OnSubmit: " + s);
    }
    #endregion
    public void OnClickConnect()
    {
        // setting inspector 에서 해도 됨 
        PhotonNetwork.GameVersion = "1";
        //NameServer 접속,(AppID, GameVersion, 지역)
        PhotonNetwork.ConnectUsingSettings();
    }
    // 마스터 서버에 접속 성공,  아직 로비를 만들거나 진입할 수 없는 상태
    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }
    // 마스터서버에 접속, 로비생성 및 진입이 가능
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        //닉네임 설정
        PhotonNetwork.NickName = inputNickName.text;
        //기본 로비 진입 
        PhotonNetwork.JoinLobby();
        //특정 로비 진입
        //PhotonNetwork.JoinLobby(new TypedLobby("권영찬 로비",LobbyType.Default));
    }
    // 로비 접속 성공시 호출
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        //SceneManager.LoadScene("LobbyScene");
        PhotonNetwork.LoadLevel("MainLobby");
    }
}

