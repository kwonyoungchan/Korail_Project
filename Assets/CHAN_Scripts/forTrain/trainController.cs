using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class trainController : MonoBehaviourPun
{
    // ���� ȭ�� �������ִ� ��ũ��Ʈ
    // ����ũ���� �� �÷��׸� �ָ�  ��� ���ǿ� ȭ�簡 �߻��ϵ��� �Ѵ�. 
    public Transform[] firePos;
    public GameObject fireObj;
    public List<GameObject> Fires = new List<GameObject>();
    public static bool isFire;
    public static bool isBoom;
    public static bool boomTurn;
    public static bool TurnedOffFire;
    public static bool turn;
    public static Action DoActive;
    //����
    //public static float amplitude;
    ////������
    //public static float SetTime;
    //[SerializeField] float ampli;
    //[SerializeField] float sTime;



    void Start()
    {
        //amplitude = ampli;
        //SetTime = sTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (DoActive != null&&!turn)
        {
            DoActive();
            DoActive = null;
            turn = true;
            if (isBoom)
                boomTurn = true;
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
    //public virtual void DoCamShake()
    //{
    //    StopAllCoroutines();
    //    photonView.RPC("RpcDoCamShake", RpcTarget.All);
    //}

    //[PunRPC]
    //public virtual void RpcDoCamShake()
    //{
    //    StartCoroutine(CameraShaking(amplitude, SetTime));
    //}

    //public  IEnumerator CameraShaking(float amplitude, float setTime)
    //{
    //    float curtime = 0;
    //    while (curtime < setTime)
    //    {
    //        Camera.main.transform.position += UnityEngine.Random.insideUnitSphere * amplitude * Time.deltaTime;
    //        curtime += Time.deltaTime;
    //        yield return null;
    //    }
    //    turn = false;
    //}
    #endregion

}
