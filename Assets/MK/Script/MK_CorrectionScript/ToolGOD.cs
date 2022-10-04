using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
// 플레이어의 명령에 따라 cube(맵)의 상태를 전환함
public class ToolGOD : MonoBehaviourPun
{
    // 도구 상태 enum
    public enum Tools
    {
        Idle,
        Ax,
        Pick,
        Pail,
        
    }
    public Tools toolsState;

    // 생성되는 y 위치
    public float y = 0.55f;

    // 아이템 생성 확인
    GameObject toolItem;
    public bool isWater;


    // Update is called once per frame
    void Update()
    {
        ToolFSM();
    }
    
    // 재료에 따른 상태머신
    void ToolFSM()
    {
        switch (toolsState)
        {
            case Tools.Idle:
                if (toolItem != null)
                {
                    Destroy(toolItem);
                    break;
                }
                else break;
            case Tools.Ax:
                if(toolItem)
                {
                    if (toolItem.name.Contains("Ax"))
                    {
                        return;
                    }
                    else
                    {
                        Destroy(toolItem);
                        return;
                    }
                }
                toolItem = Instantiate(Resources.Load<GameObject>("MK_Prefab/Ax"));
                toolItem.transform.GetChild(1).gameObject.SetActive(true);
                toolItem.transform.position = transform.position + new Vector3(0, y, 0);
                break;
            case Tools.Pick:
                if (toolItem)
                {
                    if (toolItem.name.Contains("Pick"))
                    {
                        return;
                    }
                    else
                    {
                        Destroy(toolItem);
                        return;
                    }
                }
                toolItem = Instantiate(Resources.Load<GameObject>("MK_Prefab/Pick"));
                toolItem.transform.GetChild(1).gameObject.SetActive(true);
                toolItem.transform.position = transform.position + new Vector3(0, y, 0);
                break;
            case Tools.Pail:
                if (toolItem)
                {
                    if (toolItem.name.Contains("Pail"))
                    {
                        return;
                    }
                    else
                    {
                        Destroy(toolItem);
                        return;
                    }
                }
                toolItem = Instantiate(Resources.Load<GameObject>("MK_Prefab/Pail"));
                // toolItem.transform.GetChild(1).gameObject.SetActive(true);
                toolItem.transform.position = transform.position + new Vector3(0, y + 0.2f, 0);
                if (toolItem.transform.GetComponentInChildren<Collider>() != null)
                {
                    isWater = true;
                }
                else
                {
                    isWater = false;
                    
                }
                break;
        }
    }


    public void ChangeState(Tools s)
    {
        photonView.RPC("RpcChangeState", RpcTarget.All, s);
    }

    [PunRPC]
    void RpcChangeState(Tools s)
    {
        toolsState = s;
    }
}
