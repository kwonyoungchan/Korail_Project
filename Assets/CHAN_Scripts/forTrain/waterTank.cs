using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class waterTank : trainController
{
    // 본 스크립트는 물탱크 스크립트
    // 초기에 물의 양이 주어지고, 

    [SerializeField]float maxVolume;
    [SerializeField]float curVolume;
    [SerializeField]float explosionTime;
    [SerializeField]float detectRange;
    float curTime;
    [SerializeField] Collider[] detect;

    // 기차가 이동하기 시작하는 순간에 물의 양은 줄어들기 시작한다.
    [SerializeField] trainMove tMove;
    // 물이 고갈되면 모든 기차에서 불이나기 시작한다.
    // 불이 나는 것은 train control에서 제어 할 것이라 train control 에게 flag만 주게 하자
    // 물이 채워지면 물은 다시 충전되고, 기차의 상태를 원상태로 유지시킨다. 
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
        else
        {
            curTime = 0;
            turn = false;
            isBoom = false;
            isFire = false;
            boomTurn = false;
            TurnedOffFire = false;
        }
        if (isFire&&!turn)
        {
            DoActive += DoFire;
        }
        if (isBoom&& !boomTurn)
        {
            DoActive += Boom;
        }

        // 여기서부터 플레이어 탐지를 시작한다.
        // waterTank가 플레이어를 감지하면 플레이어의 컴포넌트에 접근한다.
        detect=Physics.OverlapSphere(transform.position, detectRange, 1<<6);
        // 스크립트 PlayerForwardRay 내부의 is water 여부를 검사한다.
        if (detect.Length !=0)
        {
            if (detect[0].GetComponent<PlayerForwardRay>().isWater)
            {
                curVolume = maxVolume;
                TurnedOffFire = true;
                turn = false;
            }
        }
        if (TurnedOffFire && !turn)
        {
            DoActive += TurnOffFire;
        }
        // 만약 true 이면 isFire를 false 시킨다.
        // 물을 다시 채운다.
        // 만약 false 면 그냥 넘긴다. 

    }
    void drainWater()
    {
        
        //만약 물탱크의 양이 0이하로 떨어지면 trainscontrol에게 불이 났다고 전달한다. 
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
                // 이때 기차는 폭발한다.
                
                if (photonView.IsMine)
                {
                    if (!isBoom)
                    {
                        isBoom = true;
                        turn = false;
                        GameManager.instance.DoCamShake();
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

}
