using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 명령에 따라 cube(맵)의 상태를 전환함
public class MaterialGOD : MonoBehaviour
{
    // Material에 따른 상태 변화
    public enum Materials
    {
        Idle,
        Branch,
        Steel,
        Rail,
        None
    }
    public Materials matState = Materials.Idle;

    // 생성된 오브젝트 만들 개수
    public int branchCount = 0;
    public int steelCount = 0;
    public int railCount = 0;

    int preBCount;

    float y = 0.55f;

    // 생성된 게임오브젝트
    public List<GameObject> mat = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        branchCount = 1;
        steelCount = 1;
        railCount = 1; 
    }

    // Update is called once per frame
    void Update()
    {
        MaterialFSM();
    }

    // Material의 따른 FSM
    void MaterialFSM()
    {
        switch (matState)
        {
            // 아무것도 안함(위에 재료가 아닌 다른 것이 있는 경우)
            case Materials.Idle:
                break;
            // 나무가지라면
            case Materials.Branch:
                // 게임오브젝트가 있으면 return
                if (mat.Count == branchCount) return;
                // Resources파일에 있는 나뭇가지 생성
                if (branchCount > 1)
                {
                    for (int i = 0; i < branchCount; i++)
                    {
                        CreateMat("MK_Prefab/Branch", i);
                    }
                }
                else
                {
                    branchCount = 1;
                    GameObject branch = Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"));
                    mat.Add(branch);
                    branch.transform.position = transform.position + new Vector3(0, y, 0);
                }
                break;
            case Materials.Steel:
                // 게임오브젝트가 있으면 return
                if (mat.Count == steelCount) return;
                // Resources파일에 있는 나뭇가지 생성
                if (steelCount > 1)
                {
                    for (int i = 0; i < steelCount; i++)
                    {
                        CreateMat("MK_Prefab/Steel", i);
                    }
                }
                else
                {
                    steelCount = 1;
                    GameObject steel = Instantiate(Resources.Load<GameObject>("MK_Prefab/Steel"));
                    mat.Add(steel);
                    steel.transform.position = transform.position + new Vector3(0, y, 0);
                }
                break;
            case Materials.Rail:
                // 게임오브젝트가 있으면 return
                if (mat.Count == railCount) return;
                // Resources파일에 있는 나뭇가지 생성
                if (railCount > 1)
                {
                    for (int i = 0; i < railCount; i++)
                    {
                        CreateMat("CHAN_Prefab/Rail", i);
                    }
                }
                else
                {
                    railCount = 1;
                    GameObject rail = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail"));
                    mat.Add(rail);
                    rail.transform.position = transform.position + new Vector3(0, y, 0);
                }
                break;
            case Materials.None:
                if(mat.Count > 0)
                {
                    for (int i = 0; i < mat.Count; i++) 
                    { 
                        Destroy(mat[i]);
                    }
                    mat.Clear();
                }
                else
                {
                    matState = Materials.Idle;
                }
                break;
        }
    }
    public void CreateMat(string s, int i)
    {
        GameObject ingredient = Instantiate(Resources.Load<GameObject>(s));
        mat.Insert(i, ingredient);
        mat[i].transform.position = transform.position + new Vector3(0, y + i * 0.2f, 0);
    }
}
