using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 플레이어의 명령에 따라 cube(맵)의 상태를 전환함
public class MaterialGOD : MonoBehaviourPun
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

    float y = 0.55f;

    // 생성된 게임오브젝트
    public List<GameObject> mat = new List<GameObject>();
    private void Update()
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
                print(mat.Count + "/" + branchCount);
                // Resources파일에 있는 나뭇가지 생성
                if (branchCount >= 1)
                {
                    if (transform.childCount > 0)
                    {
                        DestroyChild();
                    }
                    else
                    {
                        for (int i = 0; i < branchCount; i++)
                        {
                            //  GameObject ingredient = PhotonNetwork.Instantiate("MK_Prefab/Branch", transform.position + new Vector3(0, y + i * 0.2f, 0), default);
                            CreateMat("MK_Prefab/Branch", i);
                        }
                    }
                }
                break;
            case Materials.Steel:
                // 게임오브젝트가 있으면 return
                if (mat.Count == steelCount) return;
                // Resources파일에 있는 나뭇가지 생성
                if (steelCount > 1)
                {
                    if (transform.childCount > 0)
                    {
                        DestroyChild();
                    }
                    else
                    {
                        for (int i = 0; i < steelCount; i++)
                        {
                            CreateMat("MK_Prefab/Steel", i);
                        }
                    }
                }

                break;
            case Materials.Rail:
                // 게임오브젝트가 있으면 return
                if (mat.Count == railCount) return;
                // Resources파일에 있는 나뭇가지 생성
                if (railCount > 1)
                {
                    if (transform.childCount > 0)
                    {
                        DestroyChild();
                    }
                    else
                    {
                        for (int i = 0; i < railCount; i++)
                        {
                            CreateMat("CHAN_Prefab/Rail", i);
                        }
                    }
                }
                break;
            case Materials.None:
                if(mat.Count > 0)
                {
                    for (int i = 0; i < mat.Count; i++) 
                    {
                        Destroy(mat[i].gameObject);
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
    void DestroyChild()
    {
        Destroy(gameObject.transform.GetChild(0).gameObject);
        branchCount = 1;
        GameObject branch = Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"));
        mat.Add(branch);
        branch.transform.position = transform.position + new Vector3(0, y, 0);
    }
    void CreateMat(string s, int i)
    {
        GameObject ingredient = Instantiate(Resources.Load<GameObject>(s));
        mat.Insert(i, ingredient);
        mat[i].transform.position = transform.position + new Vector3(0, y + i * 0.2f, 0);
    }

    public void ChangeMaterial(Materials s, int count)
    {
        photonView.RPC("PUNChangeMaterial", RpcTarget.All, s, count);
    } 

    [PunRPC]
    void PUNChangeMaterial(Materials s, int count)
    {
        matState = s;
        branchCount = count;
    }
}
