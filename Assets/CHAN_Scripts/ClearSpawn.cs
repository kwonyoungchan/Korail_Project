using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSpawn : MonoBehaviour
{
    void Start()
    {
        GameManager.instance.RandomSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
