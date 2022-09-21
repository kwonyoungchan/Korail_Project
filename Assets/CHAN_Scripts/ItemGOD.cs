using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 블럭 주변의 rail 정보 클래스
public class RailInfo
{
    public string info;
    public float rotaion;
}


public class ItemGOD : MonoBehaviourPun
{
    
    public enum Items
    {
        Idle,
        StartRail,
        Rail,
        CornerRail,
        EndRail

    }
    public Items items;
    public bool turn;
    public bool isConnected;
    public float railHeight;
    GameObject createItem;
    Quaternion setRot;
    Renderer rd;
    Color defaultColor;
    
    
    void Start()
    {
        rd = GetComponent<Renderer>();
        //defaultColor = Color.green;
    }

    
    void Update()
    {
        StateMachine();
        //오브젝트가 레일리스트 제일 마지막에 있을 경우  제거 가능하도록 isconnected =false
        if (gameObject == connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1])
        {
            isConnected = false;
        }
        //if (isConnected)
        //{
        //    rd.material.color = Color.red;
        //}
        //else if(isConnected==false&&items==Items.Idle)
        //{
        //    rd.material.color = defaultColor;
        //}
    }

    void StateMachine()
    {
        switch (items)
        {
            case Items.Idle:
                if (createItem != null)
                {
                    Destroy(createItem);
                    break;
                }
                else
                {
                    break;
                }
            case Items.StartRail:
                if (!turn)
                {
                    createItem = PhotonNetwork.Instantiate("CHAN_Prefab/Rail", transform.position + new Vector3(0, 0.5f, 0), setRot);
                    //createItem = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail"));
                    //createItem.transform.position = transform.position + new Vector3(0, railHeight, 0);
                    //createItem.transform.rotation = setRot;
                    //rd.material.color = Color.blue;
                    turn = true;
                }
                break;

            case Items.Rail:
                if (!turn)
                {
                    if (createItem)
                    {
                        PhotonNetwork.Destroy(createItem);
                    }
                    createItem = PhotonNetwork.Instantiate("CHAN_Prefab/Rail", transform.position + new Vector3(0, railHeight, 0), setRot);
                    //createItem = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail"));
                    //createItem.transform.position = transform.position + new Vector3(0, railHeight, 0);
                    createItem.transform.rotation = setRot;
                    turn = true;
                }
                break;
            case Items.CornerRail:
                if (!turn)
                {
                    if (createItem)
                    {
                        PhotonNetwork.Destroy(createItem);
                    }
                    createItem = PhotonNetwork.Instantiate("CHAN_Prefab/cornerRail", transform.position + new Vector3(0, railHeight, 0), setRot);
                    //createItem = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/cornerRail"));
                    //createItem.transform.position = transform.position + new Vector3(0, railHeight, 0);
                    createItem.transform.rotation = setRot;
                    turn = true;
                }
                break;
            case Items.EndRail:
                if (!turn)
                {
                    createItem = PhotonNetwork.Instantiate("CHAN_Prefab/Rail", transform.position + new Vector3(0, 0.5f, 0),setRot);
                    //createItem = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail"));
                    //createItem.transform.position = transform.position + new Vector3(0, railHeight, 0);
                   // rd.material.color = Color.black;
                    turn = true;
                }
                break;

        }


    }



    public void ChangeState(Items item, Quaternion Rot = default, float Height = 0.5f, bool Turn = false)
    {
        turn = false;
        items = item;
        setRot = Rot;
        railHeight = Height;
        photonView.RPC("RPCChangeState", RpcTarget.All,items, setRot, railHeight, turn);
    }
    [PunRPC]
    void RPCChangeState(Items item, Quaternion Rot = default, float Height = 0.5f, bool Turn = false)
    {
        turn = false;
        items = item;
        setRot = Rot;       
        railHeight = Height;
    }

}
