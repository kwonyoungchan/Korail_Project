using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefineBlocks : MonoBehaviour
{
    public static DefineBlocks instance;
    private void Awake()
    {
        instance = this;
    }
    public GameObject[] StartBlocks;
    public GameObject[] EndBlocks;

}
