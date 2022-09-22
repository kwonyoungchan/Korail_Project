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
        MaterialFSM(Materials.Idle);
    }
    public void MaterialFSM(Materials s)
    {
        //photonView.RPC("PunMaterialFSM", RpcTarget.All, s);
        PunMaterialFSM(s);
    }

    [PunRPC]
    // Material의 따른 FSM
    void PunMaterialFSM(Materials s)
    {
        switch (s)
        {
            // 아무것도 안함(위에 재료가 아닌 다른 것이 있는 경우)
            case Materials.Idle:
                break;
            // 나무가지라면
            case Materials.Branch:
                // 게임오브젝트가 있으면 return
                //if (branchCount == mat.Count) return;
                // Resources파일에 있는 나뭇가지 생성
                if (branchCount > 1)
                {

                    for (int i = 0; i < branchCount; i++)
                    {
                        GameObject ingredient = PhotonNetwork.Instantiate("MK_Prefab/Branch", transform.position + new Vector3(0, y + i * 0.2f, 0), default);
                        mat.Insert(i, ingredient);
                    }
                }
                else
                {
                    branchCount = 1;
                    GameObject branch = PhotonNetwork.Instantiate("MK_Prefab/Branch", transform.position + new Vector3(0, y, 0), default);
                    mat.Add(branch);
                    // branch.transform.position = transform.position + new Vector3(0, y, 0);
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
                    GameObject steel = PhotonNetwork.Instantiate("MK_Prefab/Steel", transform.position + new Vector3(0, y, 0), default);
                    mat.Add(steel);
                    // steel.transform.position = transform.position + new Vector3(0, y, 0);
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
                    GameObject rail = PhotonNetwork.Instantiate("CHAN_Prefab/Rail", transform.position + new Vector3(0, y, 0), default);
                    mat.Add(rail);
                    
                }
                break;
            case Materials.None:
                if(mat.Count > 0)
                {
                    for (int i = 0; i < mat.Count; i++) 
                    { 
                        PhotonNetwork.Destroy(mat[i]);
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
    void CreateMat(string s, int i)
    {
        GameObject ingredient = PhotonNetwork.Instantiate(s, transform.position + new Vector3(0, y + i * 0.2f, 0), default);
        mat.Insert(i, ingredient);
        // mat[i].transform.position = transform.position + new Vector3(0, y + i * 0.2f, 0);
    }
}
