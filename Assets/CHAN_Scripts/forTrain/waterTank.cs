using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterTank : trainController
{
    // 본 스크립트는 물탱크 스크립트
    // 초기에 물의 양이 주어지고, 

    [SerializeField]float maxVolume;
    [SerializeField]float curVolume;
    [SerializeField] float explosionTime;
    float curTime;

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
        if (isFire&&!turn)
        {
            DoActive += DoFire;
        }
        if (isBoom&&!turn)
        {
            DoActive += Boom;
        }
            
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
                isBoom = true;
                StartCoroutine(CameraShaking(amplitude, SetTime));
                

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
    public override IEnumerator CameraShaking(float amplitude, float setTime)
    {
        return base.CameraShaking(amplitude, setTime);
    }

}
