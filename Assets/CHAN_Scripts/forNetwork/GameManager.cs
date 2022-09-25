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
    public float amplitude;
    //������
    public float SetTime;
    public float Range;

    void Start()
    {
        //OnPhotonSerializeView ȣ�� ��
        PhotonNetwork.SerializationRate = 60;
        PhotonNetwork.SendRate = 60;
        // 1. spawnPos�� ������ �Ҵ�
        spawnPos = new Vector3[PhotonNetwork.CurrentRoom.MaxPlayers];

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            float x = Random.Range(-Range, Range);
            float z = Random.Range(-Range, Range);
            ClientManager.instance.players[i].gameObject.transform.position = new Vector3(x, 0.5f, z);
        }
        
        // ���� �濡 �����ִ� �ο����� �̿��ؼ� index ������
        int idx = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        //�÷��̾ �����Ѵ�.
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
    //���� �濡 �ִ� Player�� ��Ƴ���.
    //public List<PhotonView> players = new List<PhotonView>();


    public void LoadWaitingRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RpcLoadWaitingRoom", RpcTarget.All);
        }
        
    }

    [PunRPC]
    void RpcLoadWaitingRoom()
    {
        ClientManager.instance.DestroyPlayer();
        PhotonNetwork.LoadLevel("WaitingRoom");

    }
    #region ī�޶� ���� ���
    public virtual void DoCamShake()
    {
        StopAllCoroutines();
        photonView.RPC("RpcDoCamShake", RpcTarget.All);
    }

    [PunRPC]
    public virtual void RpcDoCamShake()
    {
        StartCoroutine(CameraShaking(amplitude, SetTime));
    }

    public IEnumerator CameraShaking(float amplitude, float setTime)
    {
        float curtime = 0;
        
        while (curtime < setTime)
        {
            Camera.main.transform.position += UnityEngine.Random.insideUnitSphere * amplitude * Time.deltaTime;
            curtime += Time.deltaTime;
            yield return null;
        }
        
    }
    #endregion



}

