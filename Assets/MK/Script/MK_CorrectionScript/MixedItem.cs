using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �����ϱ�
// ������ ö�� 1���� �ִٸ� ���� 1���� �����

public class MixedItem : MonoBehaviour
{
    // �ʿ�Ӽ� : ����, ö, ���ϰ���, �����ð�
    // ���ϰ���
    public GameObject railFact;
    // ���� �ð�
    public float createTime = 3;
    // ��������
    [HideInInspector]
    public int branchCount;
    [HideInInspector]
    public int steelCount;
    public int railCount;

    // ������ ö ��ġ
    [SerializeField]
    Transform[] matPos = new Transform[3];

    // ���� ���� ��� ����Ʈ
    public List<GameObject> branchArray = new List<GameObject>();
    public List<GameObject> steelArray = new List<GameObject>();

    // ���� ����Ʈ
    public List<GameObject> railArray = new List<GameObject>();

    // ����ð�
    float currentTime = 0;

    // ���� ���� üũ
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
        // ���� ���� ���� ���� ����
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
        // ���� ���� ö�� ������ �ִٸ�,
        if (branchArray.Count > 0 && steelArray.Count > 0)
        {
            currentTime += Time.deltaTime;
            // �����ð��� ���� �Ŀ� 
            if (currentTime > createTime)
            {
                // ������ ������ �����
                GameObject rail = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail"), matPos[2]);
                railArray.Add(rail);
                for (int i = 0; i < railArray.Count; i++)
                {
                    rail.transform.position = matPos[2].position + new Vector3(0, i * 0.2f, 0);
                }
                // railCount
                railCount = railArray.Count;
                // ������Ʈ ����
                Destroy(branchArray[branchArray.Count - 1].gameObject);
                Destroy(steelArray[steelArray.Count - 1].gameObject);
                // ����Ʈ ����
                branchArray.RemoveAt(branchArray.Count - 1);
                steelArray.RemoveAt(steelArray.Count - 1);
                // ī��Ʈ ����
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
