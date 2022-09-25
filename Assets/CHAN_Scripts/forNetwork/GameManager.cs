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
    // spawnPos 둘 변수
    public Vector3[] spawnPos;
    public float amplitude;
    //진동수
    public float SetTime;
    public float Range;

    void Start()
    {
        //OnPhotonSerializeView 호출 빈도
        PhotonNetwork.SerializationRate = 60;
        PhotonNetwork.SendRate = 60;
        // 1. spawnPos의 갯수를 할당
        spawnPos = new Vector3[PhotonNetwork.CurrentRoom.MaxPlayers];

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            float x = Random.Range(-Range, Range);
            float z = Random.Range(-Range, Range);
            ClientManager.instance.players[i].gameObject.transform.position = new Vector3(x, 0.5f, z);
        }
        
        // 현재 방에 들어와있는 인원수를 이용해서 index 구하자
        int idx = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        //플레이어를 생성한다.
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
    //현재 방에 있는 Player를 담아놓자.
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
    #region 카메라 흔드는 기능
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

