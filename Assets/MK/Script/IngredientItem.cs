using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �÷��̾ ������ ��̸� ���� ������ HP�� 1�� ���δ�
public class IngredientItem : MonoBehaviour
{
    public GameObject[] itemFact = new GameObject[2];

    public float y = 0.5f;
    // �ð�
    float maxTime = 1;
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

    }

    // Update is called once per frame
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "ProtoType")
            player = GameObject.Find("Player(Clone)").GetComponent<PlayerItemDown>();
        if (player.holdState == PlayerItemDown.Hold.Ax) 
        {
            axDis = Vector3.Distance(player.transform.position, transform.position);

            if (gameObject.name.Contains("Tree"))
            {
                if (axDis < 1.5f)
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
                if (pickDis < 1.5f)
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
    // 0 ���϶��� ������ ����
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
