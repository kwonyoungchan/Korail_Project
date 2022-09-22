using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public enum Animals
    {
        Idle,
        Move,
        Rot,
        Stop
    }
    public Animals animalState = Animals.Idle;
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
        AnimalFSM();
        if (animalState != Animals.Stop)
        {
            currentTime += Time.deltaTime;
        }

        if (currentTime > changeTime)
        {
            currentTime = 0;
            int rnd = Random.Range(0, 2);
            print(rnd);
            switch (rnd)
            {
                case 0:
                    animalState = Animals.Idle;
                    break;
                case 1:
                    animalState = Animals.Rot;
                    break;
            }
        }
        

    }

    void AnimalFSM()
    {
        switch (animalState)
        {
            case Animals.Idle:
                break;
            case Animals.Move:
                Move();
                break;
            case Animals.Rot:
                Rot();
                break;
            case Animals.Stop:
                currentTime = 0;
                break;
        }
    }
    
    // 움직이기
    void Move()
    {
        Vector3 dir = rndPos - transform.position;
        transform.position += dir * speed * Time.deltaTime;
    }

    void Rot()
    {
        // 랜덤 위치 정하기
        rndPos = UnityEngine.Random.insideUnitSphere * range;
        rndPos.y = 1.5f;
        transform.LookAt(rndPos);
        animalState = Animals.Move;
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
}
