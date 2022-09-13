using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ ������ ��̸� ��� ������ HP�� 1�� ���δ�
public class IngredientItem : MonoBehaviour
{
    public GameObject[] itemFact = new GameObject[2];
    public List<GameObject> branch = new List<GameObject>();
    public List<GameObject> steel = new List<GameObject>();

    public float y = 0.5f;
    // �ð�
    public float maxTime = 3;
    float currentTime = 0;
    // ü��
    int maxHP = 3;
    int hp;
    float axDis;
    float pickDis;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Player").GetComponent<PlayerGetItem>().armState <= 0)
        {
            return;
        }
        // �÷��̾ �������� ��� ���� ��
        GameObject ax = GameObject.Find("ArmAx");
        GameObject pick = GameObject.Find("ArmPick");

        if (ax != null) 
        { 
            axDis = Vector3.Distance(ax.transform.position, transform.position);
            if (gameObject.name.Contains("Tree"))
            {
                if (axDis < 1.6f)
                {
                    currentTime += Time.deltaTime;

                    if (currentTime > maxTime)
                    {
                        DamagedObject(0);
                    }
                }
            }
        }
        if (pick != null)
        { 
            pickDis = Vector3.Distance(pick.transform.position, transform.position);

            if (gameObject.name.Contains("Mine"))
            {
                if (pickDis < 1.6f)
                {
                    currentTime += Time.deltaTime;
                    if (currentTime > maxTime)
                    {
                        DamagedObject(1);
                    }
                }
            }
        }
       
    }

    // ���ӿ�����Ʈ�� Ÿ���� ������
    // 0 ���϶�� ������ ����
    void DamagedObject(int n)
    {
        currentTime = 0;
        hp--;
        if(hp <= 0)
        {
            Destroy(gameObject);
            GameObject item = Instantiate(itemFact[n]);
            item.transform.position = transform.position + new Vector3(0, y, 0);
        }

    }
}