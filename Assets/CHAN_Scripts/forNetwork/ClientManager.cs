using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ClientManager : MonoBehaviour
{
    public static ClientManager instance;
    //현재 방에 있는 Player를 담아놓자.
    public List<PhotonView> players = new List<PhotonView>();


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void AddPlayer(PhotonView pv)
    {
        players.Add(pv);
    }

    public bool IsExit()
    {
        for(int i = 0; i < players.Count; i++)
        {
            if(players[i].Owner.NickName == PhotonNetwork.NickName)
            {
                return true;
            }
        }
        return false;
    }

    public void DestroyPlayer()
    {
        for(int i = 0; i < players.Count; i++)
        {
            if(players[i].IsMine)
            {
                PhotonNetwork.Destroy(players[i].gameObject);
                
                break;
            }
        }
        players.Clear();
    }
    
}
