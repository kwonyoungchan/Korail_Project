using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 평소에는 플레이어 이동을 막다가 플레이어가 나무를 들고 가까이오면 나무 다리를 생성, 나무 다리가 있는 상태에서 레일을 들고 있다면 레일로 변경
public class RiverGOD : MonoBehaviourPun
{
    public enum River
    {
        Idle,
        Bridge,
        PutRail
    }
    public River riverState = River.Idle;

    GameObject riverMat;
    GameObject prevent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RiverSwitch();
    }

    // FSM
    void RiverSwitch()
    {
        switch (riverState)
        {
            case River.Idle: 
                break;
            case River.Bridge:
                if (riverMat)
                {
                    return;
                }
                Destroy(gameObject.transform.GetChild(0).gameObject);
                riverMat = Instantiate(Resources.Load<GameObject>("MK_Prefab/Bridge"));
                riverMat.transform.position = transform.position + new Vector3(0, 0.7f, 0);
                break;
            case River.PutRail:
                if (riverMat)
                {
                    Destroy(riverMat);
                    return;
                }
                GameObject rail = Instantiate(Resources.Load<GameObject>("MK_Prefab/BridgeRail"));
                rail.transform.position = transform.position + new Vector3(0, 0.7f, 0);
                break;
        }
    }

    public void ChangeRiver(River s)
    {
        photonView.RPC("PUNChangeRiver", RpcTarget.All, s);
    }

    [PunRPC]
    void PUNChangeRiver(River s)
    {
        riverState = s;
    }
}
