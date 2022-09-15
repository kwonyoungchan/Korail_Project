using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trainMove : MonoBehaviour
{
    // 기차를 이동시키는 알고리즘
    // 기차는 선로를 따라 이동함
    // 여기서 기차의 방향은 Rail의 위치를 목표 dir 로 설정하여 이동하게 된다.
    // 게임이 클리어 되면 기차는 빠른 속도로 끝 선로까지 이동하도록 한다.
    // 기차의 상태 (일반 상태, 화재, 게임 클리어, 기차 die
    // 속성
    // 기차의 속도, 방향, 기차 오브젝트, 기차 생성 위치, 기차 출발 타이머
    // 게임이 시작될 대, 메인 기차는 start 블럭에, 나머지 짐칸일 경우, 메인기차의 왼쪽으로 위치시킨다. 
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
        // 여기서 기차는 시각선로에서 시작되도록 설정 한다.
        //train = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Train"));
        transform.position = TrainSpawn.position+(Vector3.left*num);
    }

    
    void Update()
    {
        //처음에 기차는 바로 출발하지 않고 일정 시간이 지난 후 출발한다.
        if (!depart)
        {
            ReadyToDepart();
        }
        else
        {
            //만약 리스트상에 선로 오브젝트가 없으면 기차는 그냥 직진만 하도록 한다.
            if (connectRail.instance.connectedRails.Count > railCount)
            {
                //기차의 방향은 리스트에서 저장된 선로의 위치를 목표 방향으로 잡는다. 
                CheckTrainPos();
                if (!isDie)
                { dir = (connectRail.instance.connectedRails[railCount].transform.position - transform.position).normalized; }
                //여기서 y좌표는 무시한다.
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
            //만약 기차가 도착 선로로 도착한다면 게임 클리어 
            OutOfRail();
        }   
    }

    //  기차 출발 타이머
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
        //방향이 설정되면 기차는 선로쪽으로 방향을 전환한다.
        //선회 조건은 dir.rotation y 와 기차의 rotation.y 의 차이가 음수거나 양수일때
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), trainSpeed*5 * Time.deltaTime);
        //이때 일정시간 간격으로 선회를 한다. (선회와 이동은 독립적으로 작용한다.)
        //회전속도는 어떻게 정의할 것인가?
        // 이동속도와 비례해서 회전하도록 만든다.
    }
    void CheckTrainPos()
    {
        Vector3 trainPos = new Vector3(transform.position.x ,0, transform.position.z);
        Vector3 railPos = new Vector3(connectRail.instance.connectedRails[railCount].transform.position.x, 0, connectRail.instance.connectedRails[railCount].transform.position.z);

        float distance = Vector3.Distance(trainPos, railPos);
        //만약 기차와 rail 사이 거리가 일정거리 이하 좁혀졌을 때
        //다음 레일을 결정시킴
        if (distance < 0.1f)
        {
            //만약 기차가 선로쪽으로 이동을 완료했다면, 다음 선로쪽으로방향을 설정한다.
            railCount++;
        }
    }
    //기차 탈선됐을 때 죽는 처리하는 기능
    void OutOfRail()
    {
        Ray ray = new Ray(rayPos.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            print(hit.transform.name);
            //만약 지금 기차가 선로가 아닌곳에 있다면 
            if (hit.transform.GetComponent<ItemGOD>().items == ItemGOD.Items.Idle)
            {
                //기차가 터진다
                isDie = true;
                Destroy(gameObject, 1);
            }
        }
    }

}
