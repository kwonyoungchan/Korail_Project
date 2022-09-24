using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class trainController : MonoBehaviourPun, IPunObservable
{
    // ���� ȭ�� �������ִ� ��ũ��Ʈ
    // ����ũ���� �� �÷��׸� �ָ�  ��� ���ǿ� ȭ�簡 �߻��ϵ��� �Ѵ�. 
    public Transform[] firePos;
    public GameObject fireObj;
    public List<GameObject> Fires = new List<GameObject>();
    public static bool isFire;
    public static bool isBoom;
    public static bool TurnedOffFire;
    public static bool turn;
    public static Action DoActive;
    //����
    public static float amplitude;
    //������
    public static float SetTime;
    [SerializeField] float ampli;
    [SerializeField] float sTime;
    [Header ("���� UDP ���� ����")]
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

    #region ������ �ҳ��� �Լ� ����
    public virtual void DoFire()
    {
        photonView.RPC("RpcDofire", RpcTarget.All);
        ////������ ȭ�� ��ġ�� ���� �߻���Ų��. 
        //for (int i = 0; i < firePos.Length; i++)
        //{
        //    Fires[i].SetActive(true);
        //}
    }
    [PunRPC]
    public virtual void RpcDofire()
    {
        //������ ȭ�� ��ġ�� ���� �߻���Ų��. 
        for (int i = 0; i < firePos.Length; i++)
        {
            Fires[i].SetActive(true);
        }
    }
    #endregion
    #region ���� �Ҳ��� �Լ� ����
    public virtual void TurnOffFire()
    {
        photonView.RPC("RpcTurnOffFire", RpcTarget.All);
        //for (int i = 0; i < firePos.Length; i++)
        //{
        //    Fires[i].SetActive(false);
        //    //���߿� �ڿ������� �������� ����� ��������
        //}
    }
    [PunRPC]
    public virtual void RpcTurnOffFire()
    {
        for (int i = 0; i < firePos.Length; i++)
        {
            Fires[i].SetActive(false);
            //���߿� �ڿ������� �������� ����� ��������
        }
    }
    #endregion
    #region ���� ������ �Լ� ����
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
    #region ī�޶� ���� ���
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
            //������ ������
            if (stream.IsWriting)//IsMine==true;
            {
                stream.SendNext(trains[i].transform.position);
                stream.SendNext(trains[i].transform.rotation);
            }
            //������ �ޱ�
            else if (stream.IsReading)//IsMine==false  
            {
                recievePos[i] = (Vector3)stream.ReceiveNext();
                recieveRot[i] = (Quaternion)stream.ReceiveNext();
            }
        }

    }

}
