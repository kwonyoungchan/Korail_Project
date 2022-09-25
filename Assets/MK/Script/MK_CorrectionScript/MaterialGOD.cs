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
    public int matCount = 1;

    float y = 0.55f;

    // ������ ���ӿ�����Ʈ
    public List<GameObject> mat = new List<GameObject>();
    private void Update()
    {
        MaterialFSM();
    }

    // Material�� ���� FSM
    void MaterialFSM()
    {
        switch (matState)
        {
            // �ƹ��͵� ����(���� ��ᰡ �ƴ� �ٸ� ���� �ִ� ���)
            case Materials.Idle:
                break;
            // �����������
            case Materials.Branch:
                // ���ӿ�����Ʈ�� ������ return
                if (mat.Count == matCount) return;
                // Resources���Ͽ� �ִ� �������� ����
                if (matCount >= 1)
                {
                    if (transform.childCount > 0)
                    {
                        DestroyChild();
                    }
                    else
                    {
                        for (int i = 0; i < matCount; i++)
                        {
                            //  GameObject ingredient = PhotonNetwork.Instantiate("MK_Prefab/Branch", transform.position + new Vector3(0, y + i * 0.2f, 0), default);
                            CreateMat("MK_Prefab/Branch", i);
                        }
                    }
                }
                break;
            case Materials.Steel:
                // ���ӿ�����Ʈ�� ������ return
                if (mat.Count == matCount) return;
                // Resources���Ͽ� �ִ� �������� ����
                if (matCount >= 1)
                {
                    if (transform.childCount > 0)
                    {
                        DestroyChild();
                    }
                    else
                    {
                        for (int i = 0; i < matCount; i++)
                        {
                            CreateMat("MK_Prefab/Steel", i);
                        }
                    }
                }

                break;
            case Materials.Rail:
                // ���ӿ�����Ʈ�� ������ return
                if (mat.Count == matCount) return;
                // Resources���Ͽ� �ִ� �������� ����
                if (matCount >= 1)
                {
                    if (transform.childCount > 0)
                    {
                        DestroyChild();
                    }
                    else
                    {
                        for (int i = 0; i < matCount; i++)
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
        matCount = 1;
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
        matCount = count;
    }
}
