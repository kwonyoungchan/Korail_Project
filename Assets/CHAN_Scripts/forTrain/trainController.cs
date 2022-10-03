using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class trainController : MonoBehaviourPun
{
    // 기차 화재 관리해주는 스크립트
    // 물탱크에서 고갈 플래그를 주면  모든 객실에 화재가 발생하도록 한다. 
    public Transform[] firePos;
    public GameObject fireObj;
    public List<GameObject> Fires = new List<GameObject>();
    public static bool isFire;
    public static bool isBoom;
    public static bool boomTurn;
    public static bool TurnedOffFire;
    public static bool turn;
    public static Action DoActive;
    public GameObject FireUI;
    public enum TrainState
    { 
        Idle,
        isFire,
        isBoom,
        TurnOffFire
    }
    public static TrainState trainState;



    void Start()
    {
        //amplitude = ampli;
        //SetTime = sTime;
        FireUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (DoActive != null&&!turn)
        {
            DoActive();
            DoActive = null;
            StateMachine(TrainState.Idle);
        }
        // 불났다는 신호가 발생하면 빨간 UI 발생하도록
        if (isFire)
        {
            FireUI.SetActive(true);
        }
        else
        {
            FireUI.SetActive(false);
        }
        
    }
    public virtual void MakeFire()
    {
        for (int i = 0; i < firePos.Length; i++)
        {
            GameObject fire = Instantiate(fireObj, firePos[i]);
            Fires.Add(fire);
            fire.transform.position = firePos[i].position;
            fire.SetActive(false);
        }
    }

    #region 기차에 불내는 함수 영역
    public virtual void DoFire()
    {
        photonView.RPC("RpcDofire", RpcTarget.All);
        ////지정한 화재 위치에 불을 발생시킨다. 
        //for (int i = 0; i < firePos.Length; i++)
        //{
        //    Fires[i].SetActive(true);
        //}
    }
    [PunRPC]
    public virtual void RpcDofire()
    {
        //지정한 화재 위치에 불을 발생시킨다. 
        for (int i = 0; i < firePos.Length; i++)
        {
            Fires[i].SetActive(true);
        }
    }
    #endregion
    #region 기차 불끄는 함수 영역
    public virtual void TurnOffFire()
    {
        photonView.RPC("RpcTurnOffFire", RpcTarget.All);
        //for (int i = 0; i < firePos.Length; i++)
        //{
        //    Fires[i].SetActive(false);
        //    //나중에 자연스럽게 꺼지도록 기능을 구현하자
        //}
    }
    [PunRPC]
    public virtual void RpcTurnOffFire()
    {
        for (int i = 0; i < firePos.Length; i++)
        {
            Fires[i].SetActive(false);
            //나중에 자연스럽게 꺼지도록 기능을 구현하자
        }
        isFire = false;
    }
    #endregion
    #region 기차 터지는 함수 영역
    public virtual void Boom()
    {
        photonView.RPC("RpcBoom", RpcTarget.All);
        //GameObject explosion = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/train_explosion"));
        //explosion.transform.position = transform.position;
        //Destroy(explosion, 1);
        //gameObject.SetActive(false);
    }
    [PunRPC]
    public virtual void RpcBoom()
    {
        GameObject explosion = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/train_explosion"));
        explosion.transform.position = transform.position;
        Destroy(explosion, 1);
        gameObject.SetActive(false);
    }
    #endregion
    // 상태머신
    public static void StateMachine(TrainState s)
    {
        switch (s)
        {
            case TrainState.Idle:
                trainState = TrainState.Idle;
                break;
            case TrainState.isFire:
                trainState = TrainState.isFire;
                break;
            case TrainState.isBoom:
                trainState = TrainState.isBoom;
                break;
            case TrainState.TurnOffFire:
                trainState = TrainState.TurnOffFire;
                break;
        }
    }

}
