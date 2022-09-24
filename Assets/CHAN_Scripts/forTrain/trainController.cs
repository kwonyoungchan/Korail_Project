using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class trainController : MonoBehaviourPun, IPunObservable
{
    // 기차 화재 관리해주는 스크립트
    // 물탱크에서 고갈 플래그를 주면  모든 객실에 화재가 발생하도록 한다. 
    public Transform[] firePos;
    public GameObject fireObj;
    public List<GameObject> Fires = new List<GameObject>();
    public static bool isFire;
    public static bool isBoom;
    public static bool TurnedOffFire;
    public static bool turn;
    public static Action DoActive;
    //진폭
    public static float amplitude;
    //진동수
    public static float SetTime;
    [SerializeField] float ampli;
    [SerializeField] float sTime;
    [Header ("포톤 UDP 정보 모음")]
    [SerializeField] public static GameObject[] trains;
    public static Vector3[] recievePos;
    public static Quaternion[] recieveRot;
    public static Vector3 CamRecievePos;




    void Start()
    {
        amplitude = ampli;
        SetTime = sTime;
        recievePos = new Vector3[trains.Length];
        
        recieveRot = new Quaternion[trains.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (DoActive != null&&!turn)
        {
            DoActive();
            DoActive = null;
            turn = true;
            
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
    #region 카메라 흔드는 기능
    public virtual IEnumerator CameraShaking(float amplitude, float setTime)
    {
        float curtime = 0;
        isBoom = true;
        while (curtime < setTime)
        {
            Camera.main.transform.position += UnityEngine.Random.insideUnitSphere * amplitude * Time.deltaTime;
            curtime += Time.deltaTime;
            yield return null;
        }
        turn = false;
        
    }


    #endregion
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        for (int i = 0; i < trains.Length; i++)
        {
            //데이터 보내기
            if (stream.IsWriting)//IsMine==true;
            {
                stream.SendNext(trains[i].transform.position);
                stream.SendNext(trains[i].transform.rotation);
            }
            //데이터 받기
            else if (stream.IsReading)//IsMine==false  
            {
                recievePos[i] = (Vector3)stream.ReceiveNext();
                recieveRot[i] = (Quaternion)stream.ReceiveNext();
            }
        }

    }

}
