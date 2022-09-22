using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (isFire&&!turn)
        {
            DoActive += DoFire;

        }
        if (isBoom)
        {
            DoActive += Boom;
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
}
