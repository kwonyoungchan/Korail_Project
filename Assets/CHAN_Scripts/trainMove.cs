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
    Vector3 dir;
    [SerializeField] Transform TrainSpawn;
    [SerializeField] Transform rayPos;
    [SerializeField] float departTime;
    [SerializeField] float trainSpeed;
    [SerializeField] int num;  
    float trainTimer;
    bool depart;
    bool isDie;
    int railCount;
    void Start()
    {
        // ���⼭ ������ �ð����ο��� ���۵ǵ��� ���� �Ѵ�.
        //train = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Train"));
        transform.position = TrainSpawn.position+(Vector3.left*num);
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
            //���� ����Ʈ�� ���� ������Ʈ�� ������ ������ �׳� ������ �ϵ��� �Ѵ�.
            if (connectRail.instance.connectedRails.Count > railCount)
            {
                //������ ������ ����Ʈ���� ����� ������ ��ġ�� ��ǥ �������� ��´�. 
                CheckTrainPos();
                if (!isDie)
                { dir = (connectRail.instance.connectedRails[railCount].transform.position - transform.position).normalized; }
                //���⼭ y��ǥ�� �����Ѵ�.
                dir.y = transform.position.y;
                RotateTrain(dir);
            }
            else
            {
                dir = transform.forward;
            }
            if (connectRail.instance.stageClear)
            {
                trainSpeed = 10;
            }
            transform.position += dir * trainSpeed * Time.deltaTime;
            //���� ������ ���� ���η� �����Ѵٸ� ���� Ŭ���� 
            OutOfRail();
        }   
    }

    //  ���� ��� Ÿ�̸�
    void  ReadyToDepart()
    {
        trainTimer += Time.deltaTime;
        if (trainTimer > departTime)
        {
            railCount++;
            depart = true;
        }
    }
    void RotateTrain(Vector3 dir)
    {
        //������ �����Ǹ� ������ ���������� ������ ��ȯ�Ѵ�.
        //��ȸ ������ dir.rotation y �� ������ rotation.y �� ���̰� �����ų� ����϶�
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), trainSpeed*5 * Time.deltaTime);
        //�̶� �����ð� �������� ��ȸ�� �Ѵ�. (��ȸ�� �̵��� ���������� �ۿ��Ѵ�.)
        //ȸ���ӵ��� ��� ������ ���ΰ�?
        // �̵��ӵ��� ����ؼ� ȸ���ϵ��� �����.
    }
    void CheckTrainPos()
    {
        Vector3 trainPos = new Vector3(transform.position.x ,0, transform.position.z);
        Vector3 railPos = new Vector3(connectRail.instance.connectedRails[railCount].transform.position.x, 0, connectRail.instance.connectedRails[railCount].transform.position.z);

        float distance = Vector3.Distance(trainPos, railPos);
        //���� ������ rail ���� �Ÿ��� �����Ÿ� ���� �������� ��
        //���� ������ ������Ŵ
        if (distance < 0.1f)
        {
            //���� ������ ���������� �̵��� �Ϸ��ߴٸ�, ���� ���������ι����� �����Ѵ�.
            railCount++;
        }
    }
    //���� Ż������ �� �״� ó���ϴ� ���
    void OutOfRail()
    {
        Ray ray = new Ray(rayPos.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            print(hit.transform.name);
            //���� ���� ������ ���ΰ� �ƴѰ��� �ִٸ� 
            if (hit.transform.GetComponent<ItemGOD>().items == ItemGOD.Items.Idle)
            {
                //������ ������
                isDie = true;
                Destroy(gameObject, 1);
            }
        }
    }

}
