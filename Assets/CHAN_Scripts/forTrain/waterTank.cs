using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterTank : TrainToolManager
{
    // Start is called before the first frame update
    [SerializeField] Transform firePos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFire)
        {
            FireFire(firePos);
        }
    }

}
