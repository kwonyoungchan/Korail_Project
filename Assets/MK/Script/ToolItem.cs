using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어와 가까워지면 획득가능하게 만들기
public class ToolItem : MonoBehaviour
{
    // 획득 가능
    public bool isAx = false;
    public bool isPick = false;

    // 횟수 증가
    int cnt = 0;
    private void Update()
    {
        cnt = GameObject.Find("Player").GetComponent<PlayerGetItem>().curArm;

    }

    private void OnTriggerStay(Collider other)
    {
        PlayerGetItem player = GameObject.Find("Player").GetComponent<PlayerGetItem>();
        if (other.gameObject.name.Contains("Player"))
        {
            if (gameObject.name.Contains("Ax"))
            {
                isAx = true;
                if(cnt > 0)
                {
                    player.curArm = 0;
                    isAx = false;
                    Destroy(gameObject);
                }
            }
            if (gameObject.name.Contains("Pick"))
            {
                isPick = true;
                if (cnt > 0)
                {
                    player.curArm = 0;
                    isPick = false;
                    Destroy(gameObject);
                }
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            isAx = false;
            isPick = false;
        }
    }
    private void OnDestroy()
    {
        isPick = false;
        isAx = false;
    }

}
