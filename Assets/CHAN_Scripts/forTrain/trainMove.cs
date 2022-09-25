using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class trainMove :trainController,IPunObservable
{
    // 기차를 이동시키는 알고리즘
    // 기차는 선로를 따라 이동함
    // 여기서 기차의 방향은 Rail의 위치를 목표 dir 로 설정하여 이동하게 된다.
    // 게임이 클리어 되면 기차는 빠른 속도로 끝 선로까지 이동하도록 한다.
    // 기차의 상태 (일반 상태, 화재, 게임 클리어, 기차 die
    // 속성
    // 기차의 속도, 방향, 기차 오브젝트, 기차 생성 위치, 기차 출발 타이머
    // 게임이 시작될 대, 메인 기차는 start 블럭에, 나머지 짐칸일 경우, 메인기차의 왼쪽으로 위치시킨다. 
     Vector3[] dir;
    
    [SerializeField] Transform[] rayPos;
    [SerializeField] int gap;
    [SerializeField] float RotScale;

    [SerializeField] Text SpeedText;
    float trainTimer;
    public bool depart;
    bool[] isDie;
    [SerializeField] int[] railCount;
    public bool isEnding;
    public float trainSpeed;
    public float departTime;
    float time;
    float setTime = 2;
    bool[] isturn;
    [Header("포톤 UDP 정보 모음")]
    [SerializeField] GameObject[] trains;
    Vector3[] recievePos;
    Quaternion[] recieveRot;


    [SerializeField] float LerpSpeed;



    void Start()
    {

        // 여기서 기차는 시각선로에서 시작되도록 설정 한다.
        transform.position = DefineBlocks.instance.StartBlocks[1].transform.position;
        for (int i = 0; i < trains.Length; i++)
        { 
            trains[i].transform.position = transform.position+(Vector3.left*i*gap);
            trains[i].transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        railCount = new int[trains.Length];
        isDie = new bool[trains.Length];
        isturn = new bool[trains.Length];
        dir = new Vector3[trains.Length];

        recievePos = new Vector3[trains.Length];
        recieveRot = new Quaternion[trains.Length];


    }

    
    void Update()
    {

        //처음에 기차는 바로 출발하지 않고 일정 시간이 지난 후 출발한다.
        if (photonView.IsMine)
        {
            if (!depart)
            {
                ReadyToDepart();
            }
            else
            {
                for (int i = 0; i < trains.Length; i++)
                {
                    if (isDie[i] && !isturn[i])
                    {
                        //DoCamShake();
                        isturn[i] = true;
                    }

                    if (trains[i].activeSelf)
                    {

                        //만약 리스트상에 선로 오브젝트가 없으면 기차는 그냥 직진만 하도록 한다.
                        if (connectRail.instance.connectedRails.Count > railCount[i])
                        {
                            //기차의 방향은 리스트에서 저장된 선로의 위치를 목표 방향으로 잡는다. 
                            if (!isDie[i])
                            { dir[i] = (connectRail.instance.connectedRails[railCount[i]].transform.position - trains[i].transform.position).normalized; }
                            CheckTrainPos(railCount[i], trains[i].transform.position, i);
                            //여기서 y좌표는 무시한다.
                            dir[i].y = trains[i].transform.position.y;
                            RotateTrain(dir[i], trains[i].transform);
                        }
                        else
                        {
                            dir[i] = trains[i].transform.forward;
                        }
                        RailChecker(rayPos[i].position, i);
                        if (connectRail.instance.stageClear)
                        {
                            trainSpeed = 5;
                            if (isEnding)
                            {
                                trainSpeed = 0;
                            }
                        }
                        trains[i].transform.position += dir[i] * trainSpeed * Time.deltaTime;
                    }
                }
            }

        }
        else
        {
            for (int i = 0; i < trains.Length; i++)
            {
                trains[i].transform.position = Vector3.Lerp(trains[i].transform.position, recievePos[i], LerpSpeed * Time.deltaTime);
                trains[i].transform.rotation = Quaternion.Lerp(trains[i].transform.rotation, recieveRot[i], LerpSpeed * Time.deltaTime);
            }
        }
       
        SpeedText.text = trainSpeed.ToString("0.00")+" "+ "m/s";


    }

    //  기차 출발 타이머
    void  ReadyToDepart()
    {
        trainTimer += Time.deltaTime;
        if (trainTimer > departTime)
        {
            for (int i = 0; i < railCount.Length; i++)
            {
                railCount[i] = DefineBlocks.instance.StartBlocks.Length;
            }
            depart = true;
        }
    }
    void RotateTrain(Vector3 dir, Transform trainPos)
    {
        //방향이 설정되면 기차는 선로쪽으로 방향을 전환한다.
        //선회 조건은 dir.rotation y 와 기차의 rotation.y 의 차이가 음수거나 양수일때
        trainPos.rotation = Quaternion.Lerp(trainPos.rotation, Quaternion.LookRotation(dir), trainSpeed *RotScale*Time.deltaTime);
        //이때 일정시간 간격으로 선회를 한다. (선회와 이동은 독립적으로 작용한다.)
        //회전속도는 어떻게 정의할 것인가?
        // 이동속도와 비례해서 회전하도록 만든다.
    }
    void CheckTrainPos(int count, Vector3 trainPosition,int i)
    {
        Vector3 trainPos = new Vector3(trainPosition.x ,0, trainPosition.z);
        Vector3 railPos = new Vector3(connectRail.instance.connectedRails[count].transform.position.x, 0, connectRail.instance.connectedRails[count].transform.position.z);

        float distance = Vector3.Distance(trainPos, railPos);
        //만약 기차와 rail 사이 거리가 일정거리 이하 좁혀졌을 때
        //다음 레일을 결정시킴
        if (distance < 0.1f)
        {
            //만약 기차가 선로쪽으로 이동을 완료했다면, 다음 선로쪽으로방향을 설정한다.
            railCount[i]++;
        }
    }
    //기차 탈선됐을 때 죽는 처리하는 기능
     void RailChecker(Vector3 rayPosition,int i)
    {
        photonView.RPC("RpcRailChecker", RpcTarget.All, rayPosition, i);
    }
    [PunRPC]
    void RpcRailChecker(Vector3 rayPosition, int i)
    {
        Ray ray = new Ray(rayPosition, -transform.up);
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("Bridge");
        if (Physics.Raycast(ray, out hit, 3, ~layerMask))
        {
            //만약 지금 기차가 선로가 아닌곳에 있다면 
            if (hit.transform.GetComponent<ItemGOD>().items == ItemGOD.Items.Idle)
            {
                //기차가 터진다
                isDie[i] = true;
                GameObject explosion = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/train_explosion"));
                explosion.transform.position = trains[i].transform.position;
                Destroy(explosion,1);
                trains[i].transform.gameObject.SetActive(false);
                if (isDie[0] && isDie[1] && isDie[2] && isDie[3])
                {
                    Invoke("BackToRoom", 2);
                }

            }
            if (hit.transform.GetComponent<ItemGOD>().items == ItemGOD.Items.EndRail)
            {
                //만약 기차가 도착 선로로 도착한다면 게임 클리어 
                isEnding = true;
                //clear씬
                time += Time.deltaTime;
                if (time > setTime)
                {
                    SceneManager.LoadScene("ClearScene");
                }

            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        for (int i = 0; i < trains.Length; i++)
        {
            //데이터 보내기
            if (stream.IsWriting)//IsMine==true;
            {
                stream.SendNext(trains[i].transform.position);
                stream.SendNext(trains[i].transform.rotation);
            }
            //데이터 받기
            else if (stream.IsReading)//IsMine==false  
            {
                recievePos[i] = (Vector3)stream.ReceiveNext();
                recieveRot[i] = (Quaternion)stream.ReceiveNext();
            }
        }
    }
    public override void TurnOffFire()
    {
        base.TurnOffFire();
    }
    void BackToRoom()
    {
        GameManager.instance.LoadWaitingRoom();
    }


}
