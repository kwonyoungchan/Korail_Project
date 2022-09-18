using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 플레이어가 도끼나 곡갱이를 들고 있으면 HP가 1씩 까인다
public class IngredientItem : MonoBehaviour
{
    public GameObject[] itemFact = new GameObject[2];

    public float y = 0.5f;
    // 시간
    float maxTime = 1;
    float currentTime = 0;
    // 체력
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
        //(Clone)
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

    // 게임오브젝트가 타격을 입을때
    // 0 이하라면 아이템 생성
    void DamagedObject(int n)
    {
        currentTime = 0;
        hp--;
        if(hp <= 0)
        {
            Destroy(gameObject);
            // GOD에 있는 State 변경
            if (n == 0)
                GetComponentInParent<MaterialGOD>().matState = MaterialGOD.Materials.Branch;
            if(n == 1)
                GetComponentInParent<MaterialGOD>().matState = MaterialGOD.Materials.Steel;
        }

    }
}
