using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class trainController : MonoBehaviourPun
{
    // ���� ȭ�� �������ִ� ��ũ��Ʈ
    // ����ũ���� ���� �÷��׸� �ָ�  ��� ���ǿ� ȭ�簡 �߻��ϵ��� �Ѵ�. 
    public Transform[] firePos;
    public GameObject fireObj;
    public List<GameObject> Fires = new List<GameObject>();
    public static bool turn;
    public static Action DoActive;
    public GameObject FireUI;
    public static AudioSource audio;
    public GameObject FireSound;
    public UnityEngine.Object[] sounds;
    public static List<AudioClip> soundClips = new List<AudioClip>();
    //ublic static AudioSource audio;
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
        audio = GetComponent<AudioSource>();
        for (int i = 0; i < sounds.Length; i++)
        {
            soundClips.Add((AudioClip)sounds[i]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DoActive != null&&!turn)
        {
            DoActive();
            audio.PlayOneShot(audio.clip);
            turn = true;
            DoActive = null;
            StateMachine(TrainState.Idle);
        }
        // �ҳ��ٴ� ��ȣ�� �߻��ϸ� ���� UI �߻��ϵ���
        if (trainState == TrainState.isFire)
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
    // ���¸ӽ�
    public  void StateMachine(TrainState s)
    {
        if (turn) return;
        switch (s)
        {
            case TrainState.Idle:
                trainState = TrainState.Idle;
                break;
            case TrainState.isFire:
                trainState = TrainState.isFire;
                audio.clip = soundClips[1];
                break;
            case TrainState.isBoom:
                trainState = TrainState.isBoom;
                audio.clip = soundClips[2];
                break;
            case TrainState.TurnOffFire:
                trainState = TrainState.TurnOffFire;
                audio.clip = soundClips[3];
                break;
        }
    }

}
