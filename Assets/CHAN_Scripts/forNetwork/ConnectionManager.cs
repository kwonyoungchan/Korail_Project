using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ConnectionManager : MonoBehaviourPunCallbacks
{
    //�г���  InputField
    public InputField inputNickName;
    //���� Button
    public Button btnConnect;
    void Start()
    {
        //�г����� ����� ��, ȣ��Ǵ� �Լ� ���
        // inputNickName.onValueChanged.AddListener(OnValueChanged);
        //�г��ӿ��� Enter�� ���� �� ȣ��Ǵ� �Լ� ���
        inputNickName.onSubmit.AddListener(OnSubmit);

        //�г��ӿ��� Foucusing�� �Ҿ��� �� ȣ��Ǵ� �Լ� ���
        // inputNickName.onValueChanged.AddListener(OnEndEdit);

    }

    #region ����� InputField���� ���� ����

    public void OnSubmit(string s)
    {
        //���� s�� ���̰� 0���� ũ�ٸ�
        if (s.Length > 0)
        {
            //��������
            OnClickConnect();
        }
        print("OnSubmit: " + s);
    }
    #endregion
    public void OnClickConnect()
    {
        // setting inspector ���� �ص� �� 
        PhotonNetwork.GameVersion = "1";
        //NameServer ����,(AppID, GameVersion, ����)
        PhotonNetwork.ConnectUsingSettings();
    }
    // ������ ������ ���� ����,  ���� �κ� ����ų� ������ �� ���� ����
    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }
    // �����ͼ����� ����, �κ���� �� ������ ����
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        //�г��� ����
        PhotonNetwork.NickName = inputNickName.text;
        //�⺻ �κ� ���� 
        PhotonNetwork.JoinLobby();
        //Ư�� �κ� ����
        //PhotonNetwork.JoinLobby(new TypedLobby("�ǿ��� �κ�",LobbyType.Default));
    }
    // �κ� ���� ������ ȣ��
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        //SceneManager.LoadScene("LobbyScene");
        PhotonNetwork.LoadLevel("MainLobby");
    }
}

