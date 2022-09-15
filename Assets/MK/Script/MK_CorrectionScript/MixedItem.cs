using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 조합하기
// 나무와 철이 1개씩 있다면 레일 1개를 만든다

public class MixedItem : MonoBehaviour
{
    // 필요속성 : 나무, 철, 레일공장, 일정시간
    // 레일공장
    public GameObject railFact;
    // 제작 시간
    public float createTime = 3;
    // 나무개수
    [HideInInspector]
    public int branchCount;
    [HideInInspector]
    public int steelCount;
    public int railCount;

    // 나무랑 철 위치
    [SerializeField]
    Transform[] matPos = new Transform[3];

    // 기차 위에 재료 리스트
    public List<GameObject> branchArray = new List<GameObject>();
    public List<GameObject> steelArray = new List<GameObject>();

    // 레일 리스트
    public List<GameObject> railArray = new List<GameObject>();

    // 현재시간
    float currentTime = 0;

    // 위에 개수 체크
    int bCount;
    int sCount;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(railCount != railArray.Count)
        {
            for(int i = 0; i < railArray.Count; i++)
            {
                Destroy(railArray[i].gameObject);
            }
            railArray.Clear();
        }
        // 기차 위에 나무 개수 증가
        if(branchCount > 0 && bCount <= 0)
        {
            if (branchArray.Count == branchCount) return;
            for(int i = 0; i< branchCount; i++)
            {
                CreateMat("MK_Prefab/Branch", i, matPos[0], branchArray);
            }
            bCount++;
        }
        if (steelCount > 0 && sCount <= 0)
        {
            if (steelArray.Count == steelCount) return;
            for (int i = 0; i < steelCount; i++)
            {
                CreateMat("MK_Prefab/Steel", i, matPos[1], steelArray);
            }
            sCount++;
        }
        // 기차 위에 철과 나무가 있다면,
        if (branchArray.Count > 0 && steelArray.Count > 0)
        {
            currentTime += Time.deltaTime;
            // 일정시간이 지난 후에 
            if (currentTime > createTime)
            {
                // 레일이 나오게 만든다
                GameObject rail = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail"), matPos[2]);
                railArray.Add(rail);
                for (int i = 0; i < railArray.Count; i++)
                {
                    rail.transform.position = matPos[2].position + new Vector3(0, i * 0.2f, 0);
                }
                // railCount
                railCount = railArray.Count;
                // 오브젝트 제거
                Destroy(branchArray[branchArray.Count - 1].gameObject);
                Destroy(steelArray[steelArray.Count - 1].gameObject);
                // 리스트 제거
                branchArray.RemoveAt(branchArray.Count - 1);
                steelArray.RemoveAt(steelArray.Count - 1);
                // 카운트 감소
                branchCount -= 1;
                steelCount -= 1;
                currentTime = 0;
            }
        }
        else if(GameObject.Find("Rail") && (branchArray.Count > 0 || steelArray.Count > 0))
        {
            bCount = 0;
            sCount = 0;
        }
    }
    void CreateMat(string s, int i, Transform pos, List<GameObject> mat)
    {
        GameObject ingredient = Instantiate(Resources.Load<GameObject>(s));
        ingredient.transform.parent = pos;
        mat.Insert(i, ingredient);
        mat[i].transform.position = pos.position + new Vector3(0, i * 0.2f, 0);
    }
}
