using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��Ʈ���ϰ� ����� =
// ü�� �����
public class Animal : MonoBehaviour
{
    public enum Animals
    {
        Idle,
        Move,
        Rot
    }
    public Animals animalState = Animals.Idle;
    // ���� hp
    public int animalHP = 5;
    // ���� �̵� �ӵ�
    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // ������
    void Damage()
    {
        animalHP--;
        if(animalHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
