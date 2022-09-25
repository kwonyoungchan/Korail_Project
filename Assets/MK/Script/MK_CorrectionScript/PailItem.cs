using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// �÷��̾� �տ� �ִ� �絿�̿� ���� ��Ű��
public class PailItem : MonoBehaviourPun
{
    // �÷��̾� ã��
    public GameObject[] player;
 
    // �� ������Ʈ
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
