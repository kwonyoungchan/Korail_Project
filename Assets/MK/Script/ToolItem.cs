using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어와 가까워지면 획득가능하게 만들기
public class ToolItem : MonoBehaviour
{
    // 플레이어와의 거리
    public float itemDis = 0.2f;
    // 획득 가능
    public bool isAx = false;
    public bool isPick = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            if (gameObject.name.Equals("Ax"))
            {
                isAx = true;
            }
            if (gameObject.name.Equals("Pick"))
            {
                isPick = true;
            }
        }
    }
}
