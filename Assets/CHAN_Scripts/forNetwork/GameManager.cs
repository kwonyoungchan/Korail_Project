using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    // spawnPos �� ����
    public Vector3[] spawnPos;
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
        PhotonNetwork.Instantiate("Player", spawnPos[idx], Quaternion.identity);
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
    //���� �濡 �ִ� Player�� ��Ƴ���.
    public List<PhotonView> players = new List<PhotonView>();
 



}

