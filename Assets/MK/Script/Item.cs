using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 플레이어가 아이템을 들고 있지 않을때,
// 2. 플레이어가 아이템을 들고 있을 때,
public class Item : MonoBehaviour
{
    // 배열 통해 아이템의 종류를 확인한다
    // 0 - 나무, 1 - 철
    public int[] item = new int[2];

    public int treeCnt = 0;
    public int steelCnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Hierarchy 창 확인해서 아이템 상태 보기
        item[0] = treeCnt;
        item[1] = steelCnt;

        print(treeCnt + " / " + steelCnt);
    }

    // 플레이어가 닿으면 개수 증가
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            print("항");
            if (gameObject.name.Contains("Branch"))
            {
                treeCnt++;
            }
            if (gameObject.name.Contains("Steel"))
            {
                steelCnt++;
            }
        }
    }
}
