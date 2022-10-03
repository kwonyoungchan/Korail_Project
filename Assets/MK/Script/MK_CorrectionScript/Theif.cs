using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 플레이어가 안 보면 물건을 훔쳐서 달아남
public class Theif : MonoBehaviourPun, IPunObservable
{
    public enum TState
    {
        Idle,
        Move,
        Hold,
        Rot,
        Run,
        Throw,
        Die
    }
    [SerializeField]
    public TState state;
    // 속도
    public float speed = 2;
    // 랜덤 위치 거리
    public float range = 3;
    // 도둑 체력
    public int hp = 6;
    // 반지름
    public float r = 5;
    // 손에 물건드는 부분
    public GameObject iPos;
    public bool isRun;

    Rigidbody rigid;
    // 레이어
    int layer = 0;
    int layer1 = 0;
    // material 배열
    Collider[] material;
    Collider[] movement;
    List<Collider> mat = new List<Collider>();

    MaterialGOD matGod;

    public Vector3 pForward;

    // 랜덤 위치
    Vector3 rndPos;
    // 시간
    float currentTime;
    // 손에 들고 있는 리스트
    List<GameObject> matArray = new List<GameObject>();

    public Animator anim;
    public GameObject lArm;
    public GameObject rArm;
    bool isCollision = false;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        TheifState(TState.Rot);
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            // 스위치 문
            switch (state)
            {
                case TState.Idle:
                    Idle();
                    break;
                // 근처에 재료가 생성된다면 그곳으로 움직임
                case TState.Move:
                    Move();
                    break;
                // 물건 들기
                case TState.Hold:
                    lArm.transform.localEulerAngles = new Vector3(-80, 0, 90);
                    rArm.transform.localEulerAngles = new Vector3(-85, 0, -90);
                    Hold();
                    break;
                // 랜덤 위치로 움직이기
                case TState.Rot:
                    Rot();
                    mat.Clear();
                    break;
                // 밖으로 물건 던지기
                case TState.Throw:
                    Throw();
                    break;
                // 플레이어가 보면 물건 놓고 도망가기
                case TState.Run:
                    Run();
                    break;
            }
            if (isRun)
            {
                TheifState(TState.Run);
            }
            else
            {
                if (state != TState.Hold)
                {
                    if (matArray.Count <= 0 && matArray.Count <= 0)
                    {
                        layer = 1 << 10;
                        // 도둑 주변에 물체가 있는지 여부 확인
                        material = Physics.OverlapSphere(transform.position, r, layer);
                        if (material.Length > 0)
                        {
                            TheifState(TState.Move);
                        }
                        else
                        {
                            TheifState(TState.Rot);
                        }
                    }
                }
            }
        }
        else
        {
            // Lerp를 이용해서 목적지, 목적방향까지 이동 및 회전
            transform.position = Vector3.Lerp(transform.position, receivePos, 10 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, 10 * Time.deltaTime);
        }
    }

    public void TheifState(TState s)
    {
        photonView.RPC("RPCTheifState", RpcTarget.All, s);
    }
    [PunRPC]
    void RPCTheifState(TState s)
    {
        state = s;
    }
    void Idle()
    {
        TheifState(TState.Rot);
    }
    // 근처에 재료가 생성된다면 그곳으로 움직임
    void Move()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit rayInfo;
        if (Physics.Raycast(ray, out rayInfo, 3))
        {
            if (rayInfo.transform.gameObject.name.Contains("train"))
            {
                TheifState(TState.Rot);
            }
        }
        // 물체가 있다면
        if (material.Length > 0)
        {
            if (material[0] == null) TheifState(TState.Rot);
            if (matArray.Count > 0) TheifState(TState.Throw);
            for (int i = 0; i < material.Length; i++)
            {
                if (material[i].transform.parent != null)
                {
                    continue;
                }
                else if (material[i].transform.parent == null)
                {
                    layer = 1 << 10;
                    // 도둑 주변에 물체가 있는지 여부 확인
                    material = Physics.OverlapSphere(transform.position, r, layer);
                }
                if (material.Length > 0) mat.Add(material[i]);
                if (mat.Count > 0)
                {
                    if (mat[0] == null)
                    {
                        mat.Clear();
                        return;
                    }
                    Vector3 pos = new Vector3(mat[0].gameObject.transform.position.x, 1.5f, mat[0].gameObject.transform.position.z);
                    pos.y = 1.5f;
                    // if (pos == transform.position) TheifState(TState.Hold);
                    // 방향이 정해지고 
                    Vector3 dir = pos - transform.position;
                    dir.Normalize();
                    float dis = Vector3.Distance(pos, transform.position);
                    // 거리 확인
                    if (dis < 0.3f)
                    {
                        transform.position = pos;
                        anim.SetTrigger("Idle");
                        TheifState(TState.Hold);
                    }
                    else
                    {
                        transform.LookAt(pos);
                        // 움직인다
                        transform.position += dir * speed * Time.deltaTime;
                        //cc.Move(dir * speed * Time.deltaTime);
                    }
                }

            }
        }

    }
    void Hold()
    {
        layer = 1 << 10;
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit rayInfo;
        if (Physics.Raycast(ray, out rayInfo, 10, ~layer))
        {
            matGod = rayInfo.transform.gameObject.GetComponent<MaterialGOD>();
            // 나무나 철이라면 줍기
            if (matGod.matState == MaterialGOD.Materials.Branch || matGod.matState == MaterialGOD.Materials.Steel)
            {
                //      손 로테이션 시키기
                /*                arm.transform.localEulerAngles = new Vector3(rotX, 0, rotAngle);
                                RotArm(rArm, -85, 90);
                                RotArm(lArm, -80, -90);*/
                // IPos에 물체 생성
                if (matGod.matState == MaterialGOD.Materials.Branch)
                {
                    CreateMat(0);
                }
                else if (matGod.matState == MaterialGOD.Materials.Steel)
                {
                    CreateMat(1);
                }

                // RPC사용
                //      바닥 상태 변화 시키기
                matGod.ChangeMaterial(MaterialGOD.Materials.None, 1);
                // 손에 무언가 들리면 state 변경
                if (matArray.Count > 0)
                {
                    TheifState(TState.Throw);
                }
            }
        }
    }
    void Throw()
    {
        if (movement == null || movement.Length <= 0)
        {
            // 맵 가장자리로 가기
            layer1 = 1 << 12;
            // 도둑 주변에 물체가 있는지 여부 확인
            movement = Physics.OverlapSphere(transform.position, 5, layer1);
        }
        if (movement.Length > 0)
        {
            Vector3 pos = new Vector3
                (movement[movement.Length - 1].transform.position.x, transform.position.y, movement[movement.Length - 1].transform.position.z) + new Vector3(0, 0, -1);
            pos.y = 1.5f;
            // 방향이 정해지고 
            Vector3 dir = pos - transform.position;

            dir.Normalize();
            float dis = Vector3.Distance(pos, transform.position);
            // 거리 확인
            if (dis < 0.3f)
            {
                transform.position = pos;
                if (transform.position == pos)
                {

                    movement = null;
                    // 물건을 버리고 Rot상태로 변경
                    Destroy(matArray[0].gameObject, 3);
                    matArray.Clear();
                    if (matArray.Count <= 0)
                    {
                        anim.SetTrigger("Move");
                        TheifState(TState.Rot);
                    }
                }
            }
            else
            {
                transform.LookAt(pos);
                // 움직인다
                //cc.Move(dir * speed * Time.deltaTime);
                transform.position += dir * speed * Time.deltaTime;
            }
        }
        if (isCollision)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit info;
            int layer = 1 << 11;
            if (Physics.Raycast(ray, out info, 1.2f, layer))
            {
                transform.Rotate(0, 360, 0);
                if (info.transform == null)
                {
                    transform.position += transform.forward.normalized * speed * Time.deltaTime;
                }
            }
            movement = null;
        }

    }
    // 랜덤 위치로 움직이기
    void Rot()
    {
        if (matArray.Count <= 0 || isCollision || (isCollision != true && matArray.Count > 0))
        {
            anim.SetTrigger("Move");
            if (rndPos == Vector3.zero || isCollision)
            {
                // 랜덤 위치 정하기
                rndPos = UnityEngine.Random.insideUnitSphere * range;
                rndPos.y = 1.5f;
            }
            Vector3 rndDir = rndPos - transform.position;
            transform.LookAt(rndPos);
            if (Vector3.Distance(transform.position, rndPos) < 0.3f)
            {
                transform.position = rndPos;
                rndPos = Vector3.zero;
                TheifState(TState.Idle);
            }
            else
            {
                //cc.Move(rndDir.normalized * speed * Time.deltaTime);
                transform.position += rndDir.normalized * speed * Time.deltaTime;
            }

        }
        else
        {
            if (isCollision != true)
            {
                if (movement == null || movement.Length <= 0)
                {
                    // 맵 가장자리로 가기
                    layer1 = 1 << 12;
                    // 도둑 주변에 물체가 있는지 여부 확인
                    movement = Physics.OverlapSphere(transform.position, 5, layer1);
                }
                if (movement.Length > 0) TheifState(TState.Throw);

            }
            else
            {
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit info;
                int layer = 1 << 11;
                if (Physics.Raycast(ray, out info, 1.2f, layer))
                {
                    transform.Rotate(0, 360, 0);
                    if (info.transform == null)
                    {
                        transform.position += transform.forward.normalized * speed * Time.deltaTime;
                    }
                }
            }
        }
    }
    Vector3 rPos;
    // 플레이어가 보면 도망가기
    public void Run()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit rayInfo;
        // 바닥에 레이를 쏴서 바닥 상태 확인
        if (Physics.Raycast(ray, out rayInfo))
        {
            matGod = rayInfo.transform.gameObject.GetComponent<MaterialGOD>();
            // 손에 무언가 있다면
            if (matArray.Count > 0)
            {
                // 바닥이 idle 상태라면
                if (matGod.matState == MaterialGOD.Materials.Idle)
                {
                    // 손에 들고 있는 오브젝트에 따라 바닥 상태 변경
                    if (matArray[0].gameObject.name == "Branch(Clone)")
                    {
                        matGod.ChangeMaterial(MaterialGOD.Materials.Branch, 1);
                        // 손에 있는 오브젝트를 지우고
                        Destroy(matArray[0].gameObject);
                        matArray.Clear();
                    }
                    else if (matArray[0].gameObject.name == "Steel(Clone)")
                    {
                        matGod.ChangeMaterial(MaterialGOD.Materials.Steel, 1);
                        // 손에 있는 오브젝트를 지우고
                        Destroy(matArray[0].gameObject);
                        matArray.Clear();
                    }
                    if (matArray.Count <= 0)
                    {
                        // 스테이트를 변경한다
                        TheifState(TState.Rot);
                    }
                }
                // 아니라면
                else
                {
                    // transform.LookAt(pForward);
                    transform.position += pForward.normalized * speed * Time.deltaTime;
                }

            }
            else
            {
                // transform.LookAt(pForward);
                transform.position += pForward.normalized * speed * Time.deltaTime;
            }
        }
    }
    // 데미지 입으면
    public void Damage()
    {
        currentTime += Time.deltaTime;
        if (currentTime > 1)
        {
            hp--;
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
            currentTime = 0;
        }
    }

    // 손에 물건생성
    void CreateMat(int index)
    {
        photonView.RPC("PunCreateMat", RpcTarget.All, index);
    }
    [PunRPC]
    void PunCreateMat(int index)
    {
        string s;
        if (index == 0)
        {
            s = "MK_Prefab/Branch";
        }
        else
        {
            s = "MK_Prefab/Steel";
        }
        GameObject mat = Instantiate(Resources.Load<GameObject>(s));
        mat.gameObject.transform.parent = iPos.transform;
        matArray.Add(mat);
        for (int i = 0; i < matArray.Count; i++)
        {
            matArray[i].transform.position = iPos.transform.position + new Vector3(0, i * 0.2f, 0);
        }
    }
    // 위치
    Vector3 receivePos;
    // 회전
    Quaternion receiveRot;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 데이터 보내기와 받기는 한가지만 ture가 됨
        // 데이터 보내기
        if (stream.IsWriting)
        {
            // position, rotation => class 못넘김 value만 가능, value 타입 배열이나 리스트 가능
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

        }
        // 데이터 받기
        else if (stream.IsReading) // = if (stream.IsReading)
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Gather") || collision.gameObject.layer == LayerMask.NameToLayer("Prevent")
            || collision.gameObject.layer == LayerMask.NameToLayer("Movement") || collision.gameObject.layer == LayerMask.NameToLayer("Train"))
        {
            isCollision = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        isCollision = false;
    }
}
