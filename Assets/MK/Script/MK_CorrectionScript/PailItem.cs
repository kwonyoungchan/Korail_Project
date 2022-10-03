using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 플레이어 손에 있는 양동이와 연동 시키기
public class PailItem : MonoBehaviourPun
{
    // 플레이어 찾기
    public GameObject[] player;
 
    // 물 오브젝트
    public GameObject water;

    bool isPail = false;

    bool isWater;

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < player.Length; i++)
        {
            if (player[i].GetComponent<PlayerForwardRay>().isWater)
            {
                isPail = true;
            }
        }

        if (isPail)
        {
            water.SetActive(true);
        }
        else
        {
            water.SetActive(false);
        }
        

    }
}
