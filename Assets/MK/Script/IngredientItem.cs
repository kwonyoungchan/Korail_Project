using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ ������ ��̸� ��� ������ HP�� 1�� ���δ�
public class IngredientItem : MonoBehaviour
{
    public GameObject[] itemFact = new GameObject[2];

    public float y = 0.5f;
    // �ð�
    public float maxTime = 3;
    float currentTime = 0;
    // ü��
    int maxHP = 3;
    int hp;
    float axDis;
    float pickDis;

    PlayerItemDown player;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
        player = GameObject.Find("Player").GetComponent<PlayerItemDown>();
    }

    // Update is called once per frame
    void Update()
    {

        if (player.holdState == PlayerItemDown.Hold.Ax) 
        { 
            axDis = Vector3.Distance(player.transform.position, transform.position);
            if (gameObject.name.Contains("Tree"))
            {
                if (axDis < 2f)
                {
                    currentTime += Time.deltaTime;

                    if (currentTime > maxTime)
                    {
                        DamagedObject(0);
                    }
                }
            }
        }
        if (player.holdState == PlayerItemDown.Hold.Pick)
        { 
            pickDis = Vector3.Distance(player.transform.position, transform.position);

            if (gameObject.name.Contains("Iron"))
            {
                if (pickDis < 2f)
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
            // GOD�� �ִ� State ����
            if (n == 0)
                GetComponentInParent<MaterialGOD>().matState = MaterialGOD.Materials.Branch;
            if(n == 1)
                GetComponentInParent<MaterialGOD>().matState = MaterialGOD.Materials.Steel;
        }

    }
}
