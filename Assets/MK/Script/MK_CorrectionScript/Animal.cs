using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 패트롤하게 만들기 =
// 체력 만들기
public class Animal : MonoBehaviour
{
    public enum Animals
    {
        Idle,
        Move,
        Rot
    }
    public Animals animalState = Animals.Idle;
    // 동물 hp
    public int animalHP = 5;
    // 동물 이동 속도
    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 데미지
    void Damage()
    {
        animalHP--;
        if(animalHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
