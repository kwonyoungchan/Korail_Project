using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : trainController
{
    // Start is called before the first frame update
    [SerializeField] Maker maker;
    float createTime = 3;
    float currentTime;

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

        if (maker.branchArray.Count > 0 && maker.steelArray.Count > 0)
        {
            currentTime += Time.deltaTime;
            // �����ð��� ���� �Ŀ� 
            if (currentTime > createTime)
            {
                // ������ ������ �����
                GameObject rail = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail"), maker.matPos[2]);
                maker.railArray.Add(rail);
                for (int i = 0; i < maker.railArray.Count; i++)
                {
                    rail.transform.position = maker.matPos[2].position + new Vector3(0, i * 0.2f, 0);
                }
                // railCount
                maker.railCount = maker.railArray.Count;
                // ������Ʈ ����
                Destroy(maker.branchArray[maker.branchArray.Count - 1].gameObject);
                Destroy(maker.steelArray[maker.steelArray.Count - 1].gameObject);
                // ����Ʈ ����
                maker.branchArray.RemoveAt(maker.branchArray.Count - 1);
                maker.steelArray.RemoveAt(maker.steelArray.Count - 1);
                // ī��Ʈ ����
                maker.branchCount -= 1;
                maker.steelCount -= 1;
                maker.bCount--;
                maker.sCount--;
                currentTime = 0;
            }
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
