using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class connectRail : MonoBehaviour
{
    public static connectRail instance;
    private void Awake()
    {
        instance = this;
    }
    //연결된 선로들을 저장하는 리스트를 생성한다.
    [SerializeField]public List<GameObject> connectedRails = new List<GameObject>();
    //게임을 시작할 때, start rail를 리스트에 추가한다.
    [SerializeField]GameObject[] StartRail;
    [SerializeField] GameObject EndRail;
    Vector3 defaultdir;
    public bool stageClear;

    void Start()
    {
        connectedRails.Add(StartRail[0]);
        defaultdir = Vector3.right;
    }

    // Update is called once per frame
    void Update()
    {
        //리스트의 마지막부분에 들어간 게임오브젝트만 해당 함수를 작동시키고 싶다.
        Detect();
        if (connectedRails.Count > 2)
        { 
            CheckConnect();
        }
    }
    //4방향 벡터 저장해놓은 배열
    Vector3[] dir = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
    void Detect()
    {
        if (connectedRails.Count > 0)
        {
            for (int i = 0; i < 4; i++)
            {
                Ray ray = new Ray(connectedRails[connectedRails.Count-1].transform.position, dir[i]);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                if (hit.transform.GetComponent<ItemGOD>().items == ItemGOD.Items.Rail && !hit.transform.GetComponent<ItemGOD>().isConnected)
                {
                    hit.transform.GetComponent<ItemGOD>().isConnected = true;
                    if (!connectedRails.Contains(hit.transform.gameObject))
                    {
                        connectedRails.Add(hit.transform.gameObject);
                    }
                    break;
                }
                else if (hit.transform.GetComponent<ItemGOD>().items == ItemGOD.Items.EndRail)
                {
                    print("Clear!!");
                    if (!stageClear)
                    { 
                        connectedRails.Add(hit.transform.gameObject);
                    }
                    stageClear = true; 


                }
            }
        }
    }
    // 혹시 모르니 예방차원에서 rail의 connection을 검사하는 함수
    void CheckConnect()
    {
        for (int i=0; i<connectedRails.Count-1;i++)
        {
            if (!connectedRails[i].GetComponent<ItemGOD>().isConnected)
            {
                connectedRails[i].GetComponent<ItemGOD>().isConnected = true;
            }
        }

        // 여기서 선로의 방향을 결정하는 구문이 시작된다.
        //여기서 양쪽의 선로 방향을 알기위한 방향2개를 담을 변수가 만들어 진다.
        Vector3 basePos = connectedRails[connectedRails.Count - 2].transform.position;
        //detect1= 리스트의 마지막 오브젝트
        //detect2= 리스트의 마지막의 2번쨰 오브젝트
        Vector3[] detect=new Vector3[2];

        // 1. 2번 반복한다.
        for (int i = 0; i < 2; i++)
        {
            Vector3 detectPos = connectedRails[connectedRails.Count - 1 - (2 * i)].transform.position;
            //오른쪽 조건
            if (basePos.x - detectPos.x < 0 && basePos.z - detectPos.z == 0)
            {
                detect[i] = Vector3.right;
            }
            //왼쪽 조건
            else if (basePos.x - detectPos.x > 0 && basePos.z - detectPos.z == 0)
            {
                detect[i] = Vector3.left;
            }
            //위쪽 조건
            else if (basePos.x - detectPos.x ==0 && basePos.z - detectPos.z < 0)
            {
                detect[i] = Vector3.forward;
            }
            //아래조건
            else if (basePos.x - detectPos.x == 0 && basePos.z - detectPos.z > 0)
            {
                detect[i] = Vector3.back;
            }
        }
        // 여기서부터 앞에서 구한 두개의 벡터를 비교한다.

        //위, 좌
        if ((detect[0] == Vector3.left && detect[1] == Vector3.forward) ||
            (detect[1] == Vector3.left && detect[0] == Vector3.forward))
        {
            // _| 레일
            // 아래는 코너레일로 바꿈
            if (detect[0] == Vector3.forward)
            {
                connectedRails[connectedRails.Count - 2].GetComponent<ItemGOD>().
                ChangeState(ItemGOD.Items.CornerRail, Quaternion.Euler(0, 90, 0));
                //아래는 레일의 방향을 바꿈
                connectedRails[connectedRails.Count - 1].GetComponent<ItemGOD>().
                ChangeState(ItemGOD.Items.Rail, Quaternion.Euler(0, 90, 0));
            }
            else
            {
                connectedRails[connectedRails.Count - 2].GetComponent<ItemGOD>().
                ChangeState(ItemGOD.Items.CornerRail, Quaternion.Euler(0, 90, 0));
            }
            
        }
        //좌, 아래
        else if ((detect[0] == Vector3.left && detect[1] == Vector3.back) ||
                (detect[1] == Vector3.left && detect[0] == Vector3.back))
        {
            // ㄱ 레일
            if (detect[0] == Vector3.back)
            {
                connectedRails[connectedRails.Count - 2].GetComponent<ItemGOD>().
                ChangeState(ItemGOD.Items.CornerRail, Quaternion.Euler(0, 0, 0));

                connectedRails[connectedRails.Count - 1].GetComponent<ItemGOD>().
                ChangeState(ItemGOD.Items.Rail, Quaternion.Euler(0, 90, 0));
            }
            else
            {
                connectedRails[connectedRails.Count - 2].GetComponent<ItemGOD>().
                ChangeState(ItemGOD.Items.CornerRail, Quaternion.Euler(0, 0, 0));
            }
            // 아래는 코너레일로 바꿈
           

        }
        //우, 아래
        else if ((detect[0] == Vector3.right && detect[1] == Vector3.back) ||
                (detect[1] == Vector3.right && detect[0] == Vector3.back))
        {
            // |-- 레일
            if (detect[0] == Vector3.back)
            {
                connectedRails[connectedRails.Count - 2].GetComponent<ItemGOD>().
                ChangeState(ItemGOD.Items.CornerRail, Quaternion.Euler(0, 270, 0));
                connectedRails[connectedRails.Count - 1].GetComponent<ItemGOD>().
                ChangeState(ItemGOD.Items.Rail, Quaternion.Euler(0, 90, 0));
            }
            else
            {
                connectedRails[connectedRails.Count - 2].GetComponent<ItemGOD>().
                ChangeState(ItemGOD.Items.CornerRail, Quaternion.Euler(0, 270, 0));
            }
            // 아래는 코너레일로 바꿈

        }
        // 우, 위 
        else if ((detect[0] == Vector3.right && detect[1] == Vector3.forward) ||
                (detect[1] == Vector3.right && detect[0] == Vector3.forward))
        {
            if (detect[0] == Vector3.forward)
            {
                connectedRails[connectedRails.Count - 2].GetComponent<ItemGOD>().
                ChangeState(ItemGOD.Items.CornerRail, Quaternion.Euler(0, 180, 0));
                //아래는 레일의 방향을 바꿈
                connectedRails[connectedRails.Count - 1].GetComponent<ItemGOD>().
                    ChangeState(ItemGOD.Items.Rail, Quaternion.Euler(0, 90, 0));
            }
            else
            {
                connectedRails[connectedRails.Count - 2].GetComponent<ItemGOD>().
                ChangeState(ItemGOD.Items.CornerRail, Quaternion.Euler(0, 180, 0));
            }
            // 아래는 코너레일로 바꿈

            // ㄴ 레일
        }
        else if ((detect[0] == Vector3.forward && detect[1] == Vector3.back) ||
                (detect[1] == Vector3.forward && detect[0] == Vector3.back))
        {
            // | 레일
            connectedRails[connectedRails.Count - 1].GetComponent<ItemGOD>().
                ChangeState(ItemGOD.Items.Rail, Quaternion.Euler(0, 90, 0));

        }
    }


}
