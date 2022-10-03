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
    public int matCount = 1;

    public float y = 0.55f;

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
                if (mat.Count == matCount) return;
                // Resources파일에 있는 나뭇가지 생성
                if (matCount >= 1)
                {
                    if (transform.childCount > 0)
                    {
                        DestroyChild("MK_Prefab/Branch", y);
                    }
                    else
                    {
                        for (int i = 0; i < matCount; i++)
                        {
                            //  GameObject ingredient = PhotonNetwork.Instantiate("MK_Prefab/Branch", transform.position + new Vector3(0, y + i * 0.2f, 0), default);
                            CreateMat("MK_Prefab/Branch", y, i);
                        }
                    }
                }
                break;
            case Materials.Steel:
                // 게임오브젝트가 있으면 return
                if (mat.Count == matCount) return;
                // Resources파일에 있는 나뭇가지 생성
                if (matCount >= 1)
                {
                    if (transform.childCount > 0)
                    {
                        DestroyChild("MK_Prefab/Steel", y);
                    }
                    else
                    {
                        for (int i = 0; i < matCount; i++)
                        {
                            CreateMat("MK_Prefab/Steel", y, i);
                        }
                    }
                }

                break;
            case Materials.Rail:
                // 게임오브젝트가 있으면 return
                if (mat.Count == matCount) return;
                // Resources파일에 있는 나뭇가지 생성
                if (matCount >= 1)
                {
                    if (transform.childCount > 0)
                    {
                        DestroyChild("CHAN_Prefab/Rail", y);
                    }
                    else
                    {
                        for (int i = 0; i < matCount; i++)
                        {
                            CreateMat("CHAN_Prefab/Rail", y, i);
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
    void DestroyChild(string s, float y)
    {
        Destroy(gameObject.transform.GetChild(0).gameObject);
        matCount = 1;
        GameObject material = Instantiate(Resources.Load<GameObject>(s));
        mat.Add(material);
        material.transform.position = transform.position + new Vector3(0, y, 0);
    }
    void CreateMat(string s, float y, int i)
    {
        GameObject ingredient = Instantiate(Resources.Load<GameObject>(s));
        mat.Insert(i, ingredient);
        mat[i].transform.position = transform.position + new Vector3(0, y + i * 0.2f, 0);
    }
    public void DeletMaterial()
    {
        photonView.RPC("PunDeletMaterial", RpcTarget.All);
    }
    [PunRPC]
    void PunDeletMaterial()
    {
        for (int i = 0; i < mat.Count; i++)
        {
            Destroy(mat[i]);
        }
        mat.Clear();
    }

    public void ChangeMaterial(Materials s, int count)
    {
        photonView.RPC("PUNChangeMaterial", RpcTarget.All, s, count);
    } 

    [PunRPC]
    void PUNChangeMaterial(Materials s, int count)
    {
        matState = s;
        matCount = count;
    }
}
