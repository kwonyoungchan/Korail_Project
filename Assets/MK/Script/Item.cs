using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. �÷��̾ �������� ��� ���� ������,
// 2. �÷��̾ �������� ��� ���� ��,
public class Item : MonoBehaviour
{
    // �迭 ���� �������� ������ Ȯ���Ѵ�
    // 0 - ����, 1 - ö
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
        // Hierarchy â Ȯ���ؼ� ������ ���� ����
        item[0] = treeCnt;
        item[1] = steelCnt;

        print(treeCnt + " / " + steelCnt);
    }

    // �÷��̾ ������ ���� ����
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            print("��");
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
