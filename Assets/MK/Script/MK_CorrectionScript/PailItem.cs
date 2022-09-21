using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾� �տ� �ִ� �絿�̿� ���� ��Ű��
public class PailItem : MonoBehaviour
{
    // �� ������Ʈ
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
