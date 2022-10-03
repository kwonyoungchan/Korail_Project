using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class mainTrain : trainController
{
    // Start is called before the first frame update
    void Start()
    {
        MakeFire();
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
