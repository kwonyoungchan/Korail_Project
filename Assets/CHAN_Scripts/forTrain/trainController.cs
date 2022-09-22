using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class trainController : MonoBehaviour
{
    // 기차 화재 관리해주는 스크립트
    // 물탱크에서 고갈 플래그를 주면  모든 객실에 화재가 발생하도록 한다. 
    public Transform[] firePos;
    public GameObject fireObj;
    public List<GameObject> Fires = new List<GameObject>();
    public static bool isFire;
    public static bool isBoom;
    public static bool turn;
    public static Action DoActive;

    //진폭
    public static float amplitude;
    //진동수
    public static float frequency;

    public static float setTime;
    [SerializeField] float ampli;
    [SerializeField] float freq;
    [SerializeField] float sTime;

    float curtime;


    void Start()
    {
        amplitude = ampli;
        frequency = freq;
        setTime = sTime;
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
    public virtual void DoFire()
    {
        //지정한 화재 위치에 불을 발생시킨다. 
        for (int i = 0; i < firePos.Length; i++)
        {
            Fires[i].SetActive(true);
            //Fires[i].transform.position = firePos[i].position;
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
    public virtual void Boom()
    {
        GameObject explosion = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/train_explosion"));
        explosion.transform.position = transform.position;
        Destroy(explosion, 1);
        gameObject.SetActive(false);
    }

    public virtual void CameraShaking(float amplitude, float setTime)
    {
        //기차가 터질 때, 카메라가 흔들리는 함수
        
        while (curtime < setTime)
        { 
            transform.position = Camera.main.transform.position + UnityEngine.Random.insideUnitSphere * amplitude * Time.deltaTime;
            curtime += Time.deltaTime;
        }
        curtime = 0;
        return;

    }
    
}
