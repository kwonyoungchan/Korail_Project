using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

// �÷��̾ ������ ��̸� ���� ������ HP�� 1�� ���δ�
public class IngredientItem : MonoBehaviourPun
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

    bool isGathering;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;

    }

    // Update is called once per frame
    void Update()
    {
        
        player = GameObject.Find("Player(Clone)").GetComponent<PlayerItemDown>();
        isGathering = GameObject.Find("Player(Clone)").GetComponent<PlayerForwardRay>().isGathering;
        
        if (player.holdState == PlayerItemDown.Hold.Ax) 
        {
            axDis = Vector3.Distance(player.transform.position, transform.position);

            if (gameObject.name.Contains("Tree"))
            {
                if (axDis < 1f && isGathering)
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
                if (pickDis < 1.2f && isGathering)
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
        isGathering = false;
        if (hp <= 0)
        {
            Destroy(gameObject);
            isGathering = false;
            // GOD�� �ִ� State ����
            if (n == 0)
                GetComponentInParent<MaterialGOD>().MaterialFSM(MaterialGOD.Materials.Branch);
            if(n == 1)
                GetComponentInParent<MaterialGOD>().MaterialFSM(MaterialGOD.Materials.Steel);
        }

    }
}
