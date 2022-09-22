using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// �÷��̾��� ��ɿ� ���� cube(��)�� ���¸� ��ȯ��
public class MaterialGOD : MonoBehaviourPun
{
    // Material�� ���� ���� ��ȭ
    public enum Materials
    {
        Idle,
        Branch,
        Steel,
        Rail,
        None
    }
    public Materials matState = Materials.Idle;

    // ������ ������Ʈ ���� ����
    public int branchCount = 0;
    public int steelCount = 0;
    public int railCount = 0;

    float y = 0.55f;

    // ������ ���ӿ�����Ʈ
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
    // Material�� ���� FSM
    void PunMaterialFSM(Materials s)
    {
        switch (s)
        {
            // �ƹ��͵� ����(���� ��ᰡ �ƴ� �ٸ� ���� �ִ� ���)
            case Materials.Idle:
                break;
            // �����������
            case Materials.Branch:
                // ���ӿ�����Ʈ�� ������ return
                //if (branchCount == mat.Count) return;
                // Resources���Ͽ� �ִ� �������� ����
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
                // ���ӿ�����Ʈ�� ������ return
                if (mat.Count == steelCount) return;
                // Resources���Ͽ� �ִ� �������� ����
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
                // ���ӿ�����Ʈ�� ������ return
                if (mat.Count == railCount) return;
                // Resources���Ͽ� �ִ� �������� ����
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
