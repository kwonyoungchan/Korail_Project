using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maker : trainController
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
        matPos[2] = GameObject.FindWithTag("RPos").transform;
        MakeFire();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFire&&!turn)
        {
            DoActive += DoFire;
            
        }
        if (isBoom)
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
                CreateMat("MK_Prefab/Branch", i, matPos[0], branchArray);
                bCount++;
            }
        }
        if (steelCount > 0 && sCount <= 0)
        {
            if (steelArray.Count == steelCount) return;
            for (int i = 0; i < steelCount; i++)
            {
                CreateMat("MK_Prefab/Steel", i, matPos[1], steelArray);
                sCount++;
            }
        }
        // 기차 위에 철과 나무가 있다면,

    }
    void CreateMat(string s, int i, Transform pos, List<GameObject> mat)
    {
        GameObject ingredient = Instantiate(Resources.Load<GameObject>(s));
        ingredient.transform.parent = pos;
        mat.Insert(i, ingredient);
        mat[i].transform.position = pos.position + new Vector3(0, i * 0.2f, 0);
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



}
