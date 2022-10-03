using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class trainMove :trainController,IPunObservable
{
    // ������ �̵���Ű�� �˰�����
    // ������ ���θ� ���� �̵���
    // ���⼭ ������ ������ Rail�� ��ġ�� ��ǥ dir �� �����Ͽ� �̵��ϰ� �ȴ�.
    // ������ Ŭ���� �Ǹ� ������ ���� �ӵ��� �� ���α��� �̵��ϵ��� �Ѵ�.
    // ������ ���� (�Ϲ� ����, ȭ��, ���� Ŭ����, ���� die
    // �Ӽ�
    // ������ �ӵ�, ����, ���� ������Ʈ, ���� ���� ��ġ, ���� ��� Ÿ�̸�
    // ������ ���۵� ��, ���� ������ start ������, ������ ��ĭ�� ���, ���α����� �������� ��ġ��Ų��. 
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
    float delay;
    [SerializeField] float delayTime;
    public float departTime;
    float time;
    float setTime = 2;
    bool[] isturn;
    [Header("���� UDP ���� ����")]
    [SerializeField] GameObject[] trains;
    Vector3[] recievePos;
    Quaternion[] recieveRot;
    float recieveSpeed;
    [SerializeField] float LerpSpeed;
    float tSpeed;

    void Start()
    {

        // ���⼭ ������ �ð����ο��� ���۵ǵ��� ���� �Ѵ�.
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

        tSpeed = GameInfo.instance.trainSpeed;
    }

    
    void Update()
    {
        
        //ó���� ������ �ٷ� ������� �ʰ� ���� �ð��� ���� �� ����Ѵ�.
        if (photonView.IsMine)
        {
            delay += Time.deltaTime;
            if (delay > delayTime)
            {
                tSpeed += 0.01f;
                delay = 0;
            }
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
                        GameManager.instance.DoCamShake();
                        isturn[i] = true;
                    }

                    if (trains[i].activeSelf)
                    {

                        //���� ����Ʈ�� ���� ������Ʈ�� ������ ������ �׳� ������ �ϵ��� �Ѵ�.
                        if (connectRail.instance.connectedRails.Count > railCount[i])
                        {
                            //������ ������ ����Ʈ���� ����� ������ ��ġ�� ��ǥ �������� ��´�. 
                            if (!isDie[i])
                            { dir[i] = (connectRail.instance.connectedRails[railCount[i]].transform.position - trains[i].transform.position).normalized; }
                            CheckTrainPos(railCount[i], trains[i].transform.position, i);
                            //���⼭ y��ǥ�� �����Ѵ�.
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
                            tSpeed = 5;
                            if (isEnding)
                            {
                                tSpeed = 0;
                            }
                        }
                        trains[i].transform.position += dir[i] * tSpeed * Time.deltaTime;
                    }
                }
            }
            if (!trains[0].activeSelf && !trains[1].activeSelf && !trains[2].activeSelf && !trains[3].activeSelf)
            {
                Invoke("BackToRoom", 2);
            }
        }
        else
        {
            for (int i = 0; i < trains.Length; i++)
            {
                trains[i].transform.position = Vector3.Lerp(trains[i].transform.position, recievePos[i], LerpSpeed * Time.deltaTime);
                trains[i].transform.rotation = Quaternion.Lerp(trains[i].transform.rotation, recieveRot[i], LerpSpeed * Time.deltaTime);
            }
            tSpeed = recieveSpeed;
        }
       
        SpeedText.text = tSpeed.ToString("0.00")+" "+ "m/s";


    }

    //  ���� ��� Ÿ�̸�
    void  ReadyToDepart()
    {
        trainTimer += Time.deltaTime;
        if (trainTimer > departTime)
        {
            for (int i = 0; i < railCount.Length; i++)
            {
                railCount[i] = DefineBlocks.instance.StartBlocks.Length;
                GetComponent<AudioSource>().PlayOneShot(soundClips[0]);
            }
            depart = true;
        }
    }
    void RotateTrain(Vector3 dir, Transform trainPos)
    {
        //������ �����Ǹ� ������ ���������� ������ ��ȯ�Ѵ�.
        //��ȸ ������ dir.rotation y �� ������ rotation.y �� ���̰� �����ų� ����϶�
        trainPos.rotation = Quaternion.Lerp(trainPos.rotation, Quaternion.LookRotation(dir), tSpeed * RotScale*Time.deltaTime);
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
            //���� ���� ������ ���ΰ� �ƴѰ��� �ִٸ� 
            if (hit.transform.GetComponent<ItemGOD>().items == ItemGOD.Items.Idle)
            {
                //������ ������
                isDie[i] = true;
                GameObject explosion = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/train_explosion"));
                explosion.transform.position = trains[i].transform.position;
                GetComponent<AudioSource>().PlayOneShot(soundClips[2]);
                Destroy(explosion,1);
                trains[i].transform.gameObject.SetActive(false);


            }
            if (hit.transform.GetComponent<ItemGOD>().items == ItemGOD.Items.EndRail)
            {
                //���� ������ ���� ���η� �����Ѵٸ� ���� Ŭ���� 
                isEnding = true;
                //clear��
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
            //������ ������
            if (stream.IsWriting)//IsMine==true;
            {
                stream.SendNext(trains[i].transform.position);
                stream.SendNext(trains[i].transform.rotation);
            }
            //������ �ޱ�
            else if (stream.IsReading)//IsMine==false  
            {
                recievePos[i] = (Vector3)stream.ReceiveNext();
                recieveRot[i] = (Quaternion)stream.ReceiveNext();
            }
        }
        if (stream.IsWriting)
        {
            stream.SendNext(tSpeed);
        }
        else
        {
            recieveSpeed = (float)stream.ReceiveNext();
        }
    }
    public override void TurnOffFire()
    {
        base.TurnOffFire();
    }
    bool b;
    void BackToRoom()
    {
        if (b) return;
        b = true;
        GameManager.instance.LoadWaitingRoom();
    }


}
