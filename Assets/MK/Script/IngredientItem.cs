using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 도끼나 곡갱이를 들고 있으면 HP가 1씩 까인다
public class IngredientItem : MonoBehaviour
{
    public GameObject[] itemFact = new GameObject[2];
    public List<GameObject> branch = new List<GameObject>();
    public List<GameObject> steel = new List<GameObject>();

    public float y = 0.5f;
    // 시간
    public float maxTime = 3;
    float currentTime = 0;
    // 체력
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
        // 플레이어가 아이템을 들고 있을 때
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
                    
                    print(currentTime);
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

    // 게임오브젝트가 타격을 입을때
    // 0 이하라면 아이템 생성
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
