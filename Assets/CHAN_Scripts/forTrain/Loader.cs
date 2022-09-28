using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : trainController
{
    void Start()
    {
        MakeFire();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFire&&!turn)
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

    }
    public override void DoFire()
    {
        base.DoFire();
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
