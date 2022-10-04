using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Animal : MonoBehaviourPun, IPunObservable
{
    public enum Animals
    {
        Idle,
        Move,
        Rot,
        Stop
    }
    public Animals animalState = Animals.Idle;

    public Animator anim;
    // 동물 hp
    public int animalHP = 5;
    // 속도
    public float speed = 1;

    // 랜덤 위치
    Vector3 rndPos;
    bool res;
    // 랜덤 위치 범위
    public float range = 3;

    float currentTime = 0;
    float currentTime1 = 0;
    public float changeTime = 3;
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (isCollision)
            {
                rndPos = Vector3.zero;
                AnimalFSM(Animals.Rot);
            }
            switch (animalState)
            {
                case Animals.Idle:
                    anim.SetTrigger("Stop");
                    currentTime += Time.deltaTime;
                    if (currentTime > changeTime)
                    {
                        currentTime = 0;
                        AnimalFSM(Animals.Rot);
                    }
                    break;
                case Animals.Move:
                    Move();
                    break;
                case Animals.Rot:
                    anim.SetTrigger("Move");
                    Rot();
                    break;
                case Animals.Stop:
                    break;
            }
        }
        else
        {
            // Lerp를 이용해서 목적지, 목적방향까지 이동 및 회전
            transform.position = Vector3.Lerp(transform.position, receivePos, 5 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, 5 * Time.deltaTime);
        }
        

    }

    public void AnimalFSM(Animals s)
    {
        photonView.RPC("RpcAnimalState", RpcTarget.All, s);
    }
    [PunRPC]
    void RpcAnimalState(Animals s)
    {
        animalState = s;

    }
    
    // 움직이기
    void Move()
    {
        Vector3 dir = rndPos - transform.position;
        if (Vector3.Distance(transform.position, rndPos) < 0.1f)
        {
            transform.position = rndPos;
            rndPos = Vector3.zero;
            AnimalFSM(Animals.Rot);
        }
        else
        {
            transform.position += dir.normalized * speed * Time.deltaTime;
        }
    }

    void Rot()
    {
        if (rndPos == Vector3.zero)
        {
            // 랜덤 위치 정하기
            rndPos = UnityEngine.Random.insideUnitSphere * range;
            rndPos.y = 1.5f;
        }
        else
        {
            transform.LookAt(rndPos);
            AnimalFSM(Animals.Move);
        }
    }
    

    // ������
    public void Damage()
    {
        currentTime1 += Time.deltaTime;
        if (currentTime1 > 1)
        {
            animalHP--;
            if (animalHP <= 0)
            {
                Destroy(gameObject);
            }
            currentTime1 = 0;
        }
    }
    bool isCollision;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Gather") || collision.gameObject.layer == LayerMask.NameToLayer("Prevent")
            || collision.gameObject.layer == LayerMask.NameToLayer("Movement") || collision.gameObject.layer == LayerMask.NameToLayer("Train"))
        {
            isCollision = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        isCollision = false;
    }
    // 위치
    Vector3 receivePos;
    // 회전
    Quaternion receiveRot;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 데이터 보내기와 받기는 한가지만 ture가 됨
        // 데이터 보내기
        if (stream.IsWriting)
        {
            // position, rotation => class 못넘김 value만 가능, value 타입 배열이나 리스트 가능
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

        }
        // 데이터 받기
        else if (stream.IsReading) // = if (stream.IsReading)
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }

}
