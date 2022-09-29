using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasItems : MonoBehaviour
{
    // ���⼭�� �÷��̾ ���� ���� �������� ����ְ�
    // � ��� �ִ��� �˷��ִ� ��ũ��Ʈ
    public static HasItems instance;

    private void Awake()
    {
        instance = this;
    }
    [SerializeField] GameObject Rails;
    public List<GameObject> rails = new List<GameObject>();
    [SerializeField]int count;
    [SerializeField] GameObject player;
    private void Start()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject rail = Instantiate(Rails);
            rails.Add(rail);
            rail.transform.position = gameObject.transform.up+transform.up * (0.1f * i);
        }
    }
    private void Update()
    {
        if (rails.Count > 0)
        {
            for (int i = 0; i < rails.Count; i++)
            {
                rails[i].transform.position = player.transform.position + new Vector3(0, 1 + (0.2f * i), 0);
            }
        }

    }
    public void AddRail()
    {
        GameObject rail = Instantiate(Rails);
        rails.Add(rail);
    }
    public void RemoveRail()
    {
        if (rails.Count > 0)
        {
            GameObject rail = rails[rails.Count - 1];
            rails.RemoveAt(rails.Count - 1);
            Destroy(rail);
        }
    }


}
