using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// �÷��̾� �ִϸ��̼� ����
public class PlayerAnim : MonoBehaviourPun
{
    public enum Anim
    {
        Idle,
        Gather,
        Move
    }
    public Anim state;

    public Animator anim;
    
    public void AnimState(Anim s)
    {
        state = s;
        photonView.RPC("RPCAnimState", RpcTarget.All, state);
    }
    [PunRPC]
    void RPCAnimState(Anim s)
    {
        switch (s)
        {
            case Anim.Idle:
                anim.SetTrigger("Idle");
                break;
            case Anim.Gather:
                anim.SetTrigger("Gathering");
                break;
            case Anim.Move:
                anim.SetTrigger("Move");
                break;
        }
    }
}
