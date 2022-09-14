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
        None
    }
    public Materials matState = Materials.Idle;

    // 생성된 오브젝트 만들 개수
    public int branchCount = 0;

    // 생성된 게임오브젝트
    List<GameObject> mat = new List<GameObject>();

    // 플레이어의 PlayerMaterial 가져오기
    PlayerMaterial player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMaterial>();
        branchCount = 1;
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
                        GameObject branch = Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"));
                        mat.Insert(i, branch);
                        mat[i].transform.position = transform.position + new Vector3(0, 1 + i * 0.2f, 0);
                    }
                }
                else
                {
                    branchCount = 1;
                    GameObject branch = Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"));
                    mat.Add(branch);
                    branch.transform.position = transform.position + new Vector3(0, 1, 0);
                }
                break;
            case Materials.Steel:
/*                // 게임오브젝트가 있으면 return
                if (mat != null) return;
                // Resources파일에 있는 철 생성
                mat = Instantiate(Resources.Load<GameObject>("MK_Prefab/Steel"));
                mat.transform.position = transform.position + new Vector3(0, 1, 0);*/
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
}
