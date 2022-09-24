using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class waterTank : trainController
{
    // �� ��ũ��Ʈ�� ����ũ ��ũ��Ʈ
    // �ʱ⿡ ���� ���� �־�����, 

    [SerializeField]float maxVolume;
    [SerializeField]float curVolume;
    [SerializeField]float explosionTime;
    [SerializeField]float detectRange;
    float curTime;
    [SerializeField] Collider[] detect;

    // ������ �̵��ϱ� �����ϴ� ������ ���� ���� �پ��� �����Ѵ�.
    [SerializeField] trainMove tMove;

    // ���� ���Ǹ� ��� �������� ���̳��� �����Ѵ�.
    // ���� ���� ���� train control���� ���� �� ���̶� train control ���� flag�� �ְ� ����
    // ���� ä������ ���� �ٽ� �����ǰ�, ������ ���¸� �����·� ������Ų��. 
    void Start()
    {
        curVolume = maxVolume;
        MakeFire();
    }

    // Update is called once per frame
    void Update()
    {
        if (tMove.depart)
        {
            drainWater();
        }
        if (isFire&&!turn)
        {
            DoActive += DoFire;
        }
        if (isBoom&&!boomTurn)
        {
            DoActive += Boom;
            
        }
        if (TurnedOffFire&& !turn)
        {
            DoActive += TurnOffFire;
        }
        // ���⼭���� �÷��̾� Ž���� �����Ѵ�.
        // waterTank�� �÷��̾ �����ϸ� �÷��̾��� ������Ʈ�� �����Ѵ�.
        detect=Physics.OverlapSphere(transform.position, detectRange, 1<<6);
        // ��ũ��Ʈ PlayerForwardRay ������ is water ���θ� �˻��Ѵ�.
        if (detect.Length !=0)
        {
            if (detect[0].GetComponent<PlayerForwardRay>().isWater)
            {
                curVolume = maxVolume;
                TurnedOffFire = true;
                turn = false;
            }
        }
        // ���� true �̸� isFire�� false ��Ų��.
        // ���� �ٽ� ä���.
        // ���� false �� �׳� �ѱ��. 

    }
    void drainWater()
    {
        
        //���� ����ũ�� ���� 0���Ϸ� �������� trainscontrol���� ���� ���ٰ� �����Ѵ�. 
        if (curVolume <= 0)
        {
            isFire = true;
        }
        else
        {
            curVolume -= 10f * Time.deltaTime;
        }
        if (isFire)
        {
            curTime += Time.deltaTime;
            if (curTime > explosionTime)
            {
                // �̶� ������ �����Ѵ�.
                
                if (photonView.IsMine)
                {
                    if (!isBoom)
                    {
                        isBoom = true;
                        DoCamShake();
                    }
                    
                }
            }
        }
    }
    public override void DoFire()
    {
        base.DoFire();
    }
    public override void TurnOffFire()
    {
        base.TurnOffFire();
    }
    public override void MakeFire()
    {
        base.MakeFire();
    }
    public override void Boom()
    {
        base.Boom();
    }
    public override void DoCamShake()
    {
        base.DoCamShake();
    }

    [PunRPC]
    public override void RpcDofire()
    {
        base.RpcDofire();
    }
    [PunRPC]
    public override void RpcTurnOffFire()
    {
        base.RpcTurnOffFire();
    }
    [PunRPC]
    public override void RpcBoom()
    {
        base.RpcBoom();
    }
    [PunRPC]
    public override void RpcDoCamShake()
    {
        base.RpcDoCamShake();
    }
}
