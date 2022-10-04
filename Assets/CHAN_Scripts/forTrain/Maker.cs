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
    AudioSource sound;
    bool turn;


    // ������ ö ��ġ
    [SerializeField]
    public Transform[] matPos = new Transform[2];

    // ���� ���� ��� ����Ʈ
    public List<GameObject> branchArray = new List<GameObject>();
    public List<GameObject> steelArray = new List<GameObject>();

    // ���� ����Ʈ
    public List<GameObject> railArray = new List<GameObject>();
    // ���� ���� üũ
    public int bCount;
    public int sCount;

    // ���� ���� �ð�
    float createTime = 3;
    float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        MakeFire();
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (trainState == TrainState.isFire)
        {
            DoActive += DoFire;

        }
        if (trainState == TrainState.isBoom)
        {
            DoActive += Boom;
        }
        if (trainState == TrainState.TurnOffFire)
        {
            DoActive += TurnOffFire;
        }

        if (railCount != railArray.Count)
        {
            DeletRail();
        }


        // ���� ���� ö�� ������ �ִٸ�,
        if (branchArray.Count > 0 && steelArray.Count > 0)
        {
            MakeRail();
        }

    }

    List<GameObject> matArray = new List<GameObject>();
    public void CreateMat(int index, int matCount, int count, string s, int pos)
    {
        photonView.RPC("PunCreateMat", RpcTarget.All, index, matCount, count, s, pos);
    }
    public int total;
    [PunRPC]
    void PunCreateMat(int index, int matCount, int count, string s, int pos)
    {
        if (index == 0)
        {
            matArray = branchArray;
            matArray.Clear();
        }
        else if (index == 1)
        {
            matArray = steelArray;
            matArray.Clear();
        }
        else
        {
            matArray = railArray;
            matArray.Clear();
        }
        total += matCount;
        if (matArray.Count == total) return;
        for (int i = 0; i < total; i++)
        {
            GameObject ingredient = Instantiate(Resources.Load<GameObject>(s));
            ingredient.transform.parent = matPos[pos];
            matArray.Insert(i, ingredient);
            matArray[i].transform.position = matPos[pos].position + new Vector3(0, i * 0.2f, 0);
            count++;
        }

    }

    public void MakeRail()
    {
        photonView.RPC("RPCMakeRail", RpcTarget.All);
    }

    [PunRPC]
    void RPCMakeRail()
    {
        if (railCount == railArray.Count && railCount > 0) return;
        if (!turn)
        {
            sound.Play();
            turn = true;
        }
            
        currentTime += Time.deltaTime;
        // �����ð��� ���� �Ŀ� 
        if (currentTime > createTime)
        {
            turn = false;
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
            currentTime = 0;
            total -= 1;
        }
        
    }

    void DeletRail()
    {
        photonView.RPC("PunDeletRail", RpcTarget.All);
    }
    [PunRPC]
    void PunDeletRail()
    {
        for (int i = 0; i < railArray.Count; i++)
        {
            Destroy(railArray[i].gameObject);
        }
        railArray.Clear();
    }

    public override void DoFire()
    {
        base.DoFire();
        print("����Ŀ ȭ��");
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
