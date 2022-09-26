using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maker : trainController, IPunObservable
{


    // Update is called once per frame
    [HideInInspector]
    public int branchCount;
    [HideInInspector]
    public int steelCount;
    public int railCount;


    // 나무랑 철 위치
    [SerializeField]
    public Transform[] matPos = new Transform[2];

    // 기차 위에 재료 리스트
    public List<GameObject> branchArray = new List<GameObject>();
    public List<GameObject> steelArray = new List<GameObject>();

    // 레일 리스트
    public List<GameObject> railArray = new List<GameObject>();
    // 위에 개수 체크
    public int bCount;
    public int sCount;
    // Start is called before the first frame update
    void Start()
    {
        MakeFire();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (isFire && !turn)
            {
                DoActive += DoFire;

            }
            if (isBoom && !boomTurn)
            {
                DoActive += Boom;
            }
            if (TurnedOffFire && !turn)
            {
                DoActive += TurnOffFire;
            }
            if (railCount != railArray.Count)
            {
                for (int i = 0; i < railArray.Count; i++)
                {
                    Destroy(railArray[i].gameObject);
                }
                railArray.Clear();
            }
            // 기차 위에 나무 개수 증가   
            if (branchCount > 0 && bCount <= 0)
            {
                if (branchArray.Count == branchCount) return;
                for (int i = 0; i < branchCount; i++)
                {
                    CreateBranch("MK_Prefab/Branch", i, 0);
                    bCount++;
                }
            }
            if (steelCount > 0 && sCount <= 0)
            {
                if (steelArray.Count == steelCount) return;
                for (int i = 0; i < steelCount; i++)
                {
                    CreateSteel("MK_Prefab/Steel", i, 1);
                    sCount++;
                }
            }
        }
        else
        {
            matPos[0].transform.position = pos[0];
            matPos[1].transform.position = pos[1];
            matPos[2].transform.position = pos[2];
            branchCount = a;
            steelCount = b;
            railCount = c;

            if(branchCount > 0)
            {
                if(steelCount > 0)
                {
                    if (steelArray.Count == steelCount) return;
                    for (int i = 0; i < steelCount; i++)
                    {
                        CreateSteel("MK_Prefab/Steel", i, 1);
                    }
                }
                if (branchArray.Count == branchCount) return;
                for (int i = 0; i < branchCount; i++)
                {
                    CreateBranch("MK_Prefab/Branch", i, 0);
                }
                if (steelCount > 0)
                {
                    if (steelArray.Count == steelCount) return;
                    for (int i = 0; i < steelCount; i++)
                    {
                        CreateSteel("MK_Prefab/Steel", i, 1);
                    }
                }
            }
            if (steelCount > 0)
            {
                if (steelArray.Count == steelCount) return;
                for (int i = 0; i < steelCount; i++)
                {
                    CreateSteel("MK_Prefab/Steel", i, 1);
                }
            }
            if (branchCount > 0 && steelCount > 0)
            {
                if (railCount == railArray.Count) return;
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
                bCount--;
                sCount--;
            }

            if (railCount != railArray.Count)
            {
                for (int i = 0; i < railArray.Count; i++)
                {
                    Destroy(railArray[i].gameObject);
                }
                railArray.Clear();
            }
        }
        // 기차 위에 철과 나무가 있다면,

    }
    void CreateBranch(string s, int i, int pos)
    {
        GameObject ingredient = Instantiate(Resources.Load<GameObject>(s));
        ingredient.transform.parent = matPos[pos];
        branchArray.Insert(i, ingredient);
        branchArray[i].transform.position = matPos[pos].position + new Vector3(0, i * 0.2f, 0);
    }

    void CreateSteel(string s, int i, int pos)
    {
        GameObject ingredient = Instantiate(Resources.Load<GameObject>(s));
        ingredient.transform.parent = matPos[pos];
        steelArray.Insert(i, ingredient);
        steelArray[i].transform.position = matPos[pos].position + new Vector3(0, i * 0.2f, 0);
    }


    public override void DoFire()
    {
        base.DoFire();
        print("메이커 화재");
    }
    public override void MakeFire()
    {
        base.MakeFire();
    }
    public override void Boom()
    {
        base.Boom();
    }
    public override void TurnOffFire()
    {
        base.TurnOffFire();
    }
    [PunRPC]
    public override void RpcDofire()
    {
        base.RpcDofire();
    }
    [PunRPC]
    public override void RpcTurnOffFire()
    {
        base.RpcTurnOffFire();
    }
    [PunRPC]
    public override void RpcBoom()
    {
        base.RpcBoom();
    }

    Vector3[] pos = new Vector3[3];
    int a;
    int b;
    int c;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(matPos[0].transform.position);
            stream.SendNext(matPos[1].transform.position);
            stream.SendNext(matPos[2].transform.position);
            stream.SendNext(branchCount);
            stream.SendNext(steelCount);
            stream.SendNext(railCount);
            
        }
        else
        {
            pos[0] = (Vector3)stream.ReceiveNext();
            pos[1] = (Vector3)stream.ReceiveNext();
            pos[2] = (Vector3)stream.ReceiveNext();
            a = (int)stream.ReceiveNext();
            b = (int)stream.ReceiveNext();
            c = (int)stream.ReceiveNext();
        }
    }
}
