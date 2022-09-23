using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
// �÷��̾��� ��ɿ� ���� cube(��)�� ���¸� ��ȯ��
public class ToolGOD : MonoBehaviourPun
{
    // ���� ���� enum
    public enum Tools
    {
        Idle,
        Ax,
        Pick,
        Pail,
        
    }
    public Tools toolsState;

    // �����Ǵ� y ��ġ
    float y = 0.5f;

    // ������ ���� Ȯ��
    GameObject toolItem;


    // Update is called once per frame
    void Update()
    {
        ToolFSM();
    }
    
    // ��ῡ ���� ���¸ӽ�
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
                toolItem.transform.position = transform.position + new Vector3(0, 0.55f, 0);
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
                toolItem.transform.position = transform.position + new Vector3(0, 0.75f, 0);
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
