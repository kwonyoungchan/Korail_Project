using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trainMove : MonoBehaviour
{
    // ������ �̵���Ű�� �˰���
    // ������ ���θ� ���� �̵���
    // ���⼭ ������ ������ Rail�� ��ġ�� ��ǥ dir �� �����Ͽ� �̵��ϰ� �ȴ�.
    // ������ Ŭ���� �Ǹ� ������ ���� �ӵ��� �� ���α��� �̵��ϵ��� �Ѵ�.
    // ������ ���� (�Ϲ� ����, ȭ��, ���� Ŭ����, ���� die
    // �Ӽ�
    // ������ �ӵ�, ����, ���� ������Ʈ, ���� ���� ��ġ, ���� ��� Ÿ�̸�
    // ������ ���۵� ��, ���� ������ start ����, ������ ��ĭ�� ���, ���α����� �������� ��ġ��Ų��. 
     Vector3[] dir;
    [SerializeField] Transform[] rayPos;
    [SerializeField] int gap;
    [SerializeField] public Transform TrainSpawn;
    [SerializeField] GameObject[] trains;
    float trainTimer;
    bool depart;
    bool isDie;
    int[] railCount;
    public bool isEnding;
    public float trainSpeed;
    public float departTime;

    void Start()
    {
        // ���⼭ ������ �ð����ο��� ���۵ǵ��� ���� �Ѵ�.
        transform.position = DefineBlocks.instance.StartBlocks[0].transform.position;
        for (int i = 0; i < trains.Length; i++)
        { 
            trains[i].transform.position = transform.position+(Vector3.left*i*gap);
        }
        railCount = new int[trains.Length];
        dir = new Vector3[trains.Length];
        
    }

    
    void Update()
    {
        //ó���� ������ �ٷ� ������� �ʰ� ���� �ð��� ���� �� ����Ѵ�.

        if (!depart)
        {
            ReadyToDepart();
        }
        else
        {
            for (int i = 0; i < trains.Length; i++)
            {
                //���� ����Ʈ�� ���� ������Ʈ�� ������ ������ �׳� ������ �ϵ��� �Ѵ�.
                if (connectRail.instance.connectedRails.Count > railCount[i])
                {
                    //������ ������ ����Ʈ���� ����� ������ ��ġ�� ��ǥ �������� ��´�. 
                    CheckTrainPos(railCount[i],trains[i].transform.position,i);
                    if (!isDie)
                    { dir[i] = (connectRail.instance.connectedRails[railCount[i]].transform.position - trains[i].transform.position).normalized; }
                    //���⼭ y��ǥ�� �����Ѵ�.
                    dir[i].y = trains[i].transform.position.y;
                    RotateTrain(dir[i], trains[i].transform);
                }
                else
                {
                    dir[i] = trains[i].transform.forward;
                }
                RailChecker(rayPos[i].position);
                if (connectRail.instance.stageClear)
                {
                    trainSpeed = 10;
                    if (isEnding)
                    {
                        trainSpeed = 0;
                    }
                }
                trains[i].transform.position += dir[i] * trainSpeed * Time.deltaTime;
            }
        }
        
       
    }

    //  ���� ��� Ÿ�̸�
    void  ReadyToDepart()
    {
        trainTimer += Time.deltaTime;
        if (trainTimer > departTime)
        {
            for (int i = 0; i < railCount.Length; i++)
            {
                railCount[i]++;
            }
            depart = true;
        }
    }
    void RotateTrain(Vector3 dir, Transform trainPos)
    {
        //������ �����Ǹ� ������ ���������� ������ ��ȯ�Ѵ�.
        //��ȸ ������ dir.rotation y �� ������ rotation.y �� ���̰� �����ų� ����϶�
        trainPos.rotation = Quaternion.Lerp(trainPos.rotation, Quaternion.LookRotation(dir), trainSpeed*5 * Time.deltaTime);
        //�̶� �����ð� �������� ��ȸ�� �Ѵ�. (��ȸ�� �̵��� ���������� �ۿ��Ѵ�.)
        //ȸ���ӵ��� ��� ������ ���ΰ�?
        // �̵��ӵ��� ����ؼ� ȸ���ϵ��� �����.
    }
    void CheckTrainPos(int count, Vector3 trainPosition,int i)
    {
        Vector3 trainPos = new Vector3(trainPosition.x ,0, trainPosition.z);
        Vector3 railPos = new Vector3(connectRail.instance.connectedRails[count].transform.position.x, 0, connectRail.instance.connectedRails[count].transform.position.z);

        float distance = Vector3.Distance(trainPos, railPos);
        //���� ������ rail ���� �Ÿ��� �����Ÿ� ���� �������� ��
        //���� ������ ������Ŵ
        if (distance < 0.1f)
        {
            //���� ������ ���������� �̵��� �Ϸ��ߴٸ�, ���� ���������ι����� �����Ѵ�.
            railCount[i]++;
        }
    }
    //���� Ż������ �� �״� ó���ϴ� ���
     void RailChecker(Vector3 rayPosition)
    {
        Ray ray = new Ray(rayPosition, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //���� ���� ������ ���ΰ� �ƴѰ��� �ִٸ� 
            if (hit.transform.GetComponent<ItemGOD>().items == ItemGOD.Items.Idle)
            {
                //������ ������
                isDie = true;
                Destroy(gameObject, 1);
            }
            if (hit.transform.GetComponent<ItemGOD>().items == ItemGOD.Items.EndRail)
            {
                //���� ������ ���� ���η� �����Ѵٸ� ���� Ŭ���� 
                isEnding = true;
            }
        }
    }



}
