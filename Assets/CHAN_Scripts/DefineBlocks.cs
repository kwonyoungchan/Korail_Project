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

    public GameObject StartStation;
    public GameObject EndStation;
    void Start()
    {
        GameObject createStartStation = Instantiate(StartStation);
        GameObject createEndStation = Instantiate(EndStation);
        createStartStation.transform.position = StartBlocks[1].transform.position + new Vector3(0,1.5f,1);
        createEndStation.transform.position = EndBlocks[0].transform.position + new Vector3(0, 1.5f, 1); ;
    }
}
