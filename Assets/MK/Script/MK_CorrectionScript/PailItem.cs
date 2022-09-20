using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 손에 있는 양동이와 연동 시키기
public class PailItem : MonoBehaviour
{
    // 물 오브젝트
    public GameObject water;

    bool isPail = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isPail = GameObject.Find("Player(Clone)").GetComponent<PlayerForwardRay>().isWater;

        if(isPail)
        {
            water.SetActive(true);
        }
        else
        {
            water.SetActive(false);
        }
    }
}
