using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trainController : MonoBehaviour
{
    // ���� ȭ�� �������ִ� ��ũ��Ʈ
    // ����ũ���� �� �÷��׸� �ָ�  ��� ���ǿ� ȭ�簡 �߻��ϵ��� �Ѵ�. 
    public Transform[] firePos;
    public GameObject fireObj;
    public List<GameObject> Fires = new List<GameObject>();
    public static bool isFire;
    public Action DoActive;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DoActive != null)
        {
            DoActive();
        }
        
    }
    public virtual void DoFire()
    {
        //������ ȭ�� ��ġ�� ���� �߻���Ų��. 
        for (int i = 0; i < firePos.Length; i++)
        {
            GameObject fire = Instantiate(fireObj);
            Fires.Add(fire);
            fire.transform.position = firePos[i].position;
        }
    }
}
