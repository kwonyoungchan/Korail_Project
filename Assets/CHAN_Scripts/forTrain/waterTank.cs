using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterTank : trainController
{
    // �� ��ũ��Ʈ�� ����ũ ��ũ��Ʈ
    // �ʱ⿡ ���� ���� �־�����, 

    [SerializeField]float maxVolume;
    [SerializeField]float curVolume;
    [SerializeField] float explosionTime;
    float curTime;

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
        if (isBoom&&!turn)
        {
            DoActive += Boom;
        }
            
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
