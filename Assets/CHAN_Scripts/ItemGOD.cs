using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 블럭 주변의 rail 정보 클래스
public class RailInfo
{
    public string info;
    public float rotaion;
}


public class ItemGOD : MonoBehaviour
{
    List<RailInfo> railInfo = new List<RailInfo>();
    
    public enum Items
    {
        Idle,
        StartRail,
        Rail,
        EndRail

    }
    public Items items;
    public bool turn;
    public bool isConnected;
    GameObject createItem;
    Quaternion setRot;
    Renderer rd;
    Color defaultColor;
    [SerializeField] connectRail cr;
    
    void Start()
    {
        rd = GetComponent<Renderer>();
        defaultColor = Color.green;
    }

    
    void Update()
    {
        StateMachine();
        //오브젝트가 레일리스트 제일 마지막에 있을 경우  제거 가능하도록 isconnected =false
        if (gameObject == connectRail.instance.connectedRails[connectRail.instance.connectedRails.Count - 1])
        {
            isConnected = false;
        }
        if (isConnected)
        {
            rd.material.color = Color.red;
        }
        else if(isConnected==false&&items==Items.Idle)
        {
            rd.material.color = defaultColor;
        }
    }

    void StateMachine()
    {
        switch (items)
        {
            case Items.Idle:
                if (createItem != null)
                {
                    Destroy(createItem);
                    break;
                }
                else
                {
                    break;
                }
            case Items.StartRail:
                if (!turn)
                {
                    //createItem = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/startRail"));
                    //createItem.transform.position = transform.position + new Vector3(0, 0.5f, 0);
                    //createItem.transform.rotation = setRot;
                    rd.material.color = Color.blue;
                    turn = true;
                }
                break;

            case Items.Rail:
                if (!turn)
                {
                    createItem = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail"));
                    createItem.transform.position = transform.position + new Vector3(0, 0.5f, 0);
                    createItem.transform.rotation = setRot;
                    turn = true;
                }
                break;
            case Items.EndRail:
                if (!turn)
                {
                    rd.material.color = Color.black;
                    turn = true;
                }
                break;

        }


    }


    public void ChangeState(Items item)
    {
        turn = false;
        items = item;
    }

    
    //Vector3[] dir = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
    //[SerializeField]List<int> count = new List<int>();
//    void DoSelect()
//    {
//        //여기서 4방향에서 선로를 찾는다. 
//        //Ray를 쏜다.
//        //총 4번 쏜다. 
//        //4번 반복해서 얻은 정보를 토대로 선로의 방향을 결정
//        float[] railRot = { 90, 0, 180, 0 };
//        //float[] railRot_R = { 90, 180, -90, 0 };
//        //float[] railRot_L = { -90, 0, 90, 180 };
//        //주변 선로 파악 
        
//        for (int i = 0; i < 4; i++)
//        {
//            Ray ray = new Ray(transform.position, dir[i]);
//            RaycastHit hit;
//            RailInfo RI = new RailInfo();
//            //여기서 기차 선회를 위한 코드가 작성된다.
//            if (Physics.Raycast(ray, out hit))
//            {   
//                if (hit.transform.gameObject.GetComponent<ItemGOD>().items ==ItemGOD.Items.Rail)
//                {
//                    RI.info = "Rail";
//                    RI.rotaion = hit.transform.eulerAngles.y;  
//                }
//                else if (hit.transform.gameObject.GetComponent<ItemGOD>().items == ItemGOD.Items.Rail_L)
//                {
//                    RI.info = "Rail_L";
//                    RI.rotaion = hit.transform.eulerAngles.y;
//                }
//                else if (hit.transform.gameObject.GetComponent<ItemGOD>().items == ItemGOD.Items.Rail_R)
//                {
//                    RI.info = "Rail_R";
//                    RI.rotaion = hit.transform.eulerAngles.y;
//                }
//                else
//                {
//                    RI.info = null;
//                    RI.rotaion = hit.transform.eulerAngles.y;
//                }
//            }
//            railInfo.Add(RI);
//        }
//        //여기서 선로를 결정하게 됨
//        for (int i = 0; i < 4; i++)
//        {
//            if (railInfo[i].info == "Rail")
//            {
//                //if (railInfo[i].rotaion == railRot[i])
//                //블럭 주변에 선로가 1개 존재하면 
//                //코너 선로 설치를 시작한다.
//                if (count.Count == 1)
//                {
//                    // 북 남 , 발견선로 : 동
//                    if (i % 2 == 0 && count[0] == 1)
//                    {
//                        Destroy(createItem);
//                        //만약 북쪽 선로
//                        if (i == 0)
//                        {
//                            ChangeState(Items.Rail_L);
//                            setRot = Quaternion.Euler(0, 180, 0);
//                        }
//                        //만약 남쪽 선로
//                        else
//                        {
//                            ChangeState(Items.Rail_R);
//                            setRot = Quaternion.Euler(0, 0, 0);
//                        }
//                    }
//                    // 북 남 , 발견선로 : 서
//                    else if (i % 2 == 0 && count[0] == 3)
//                    {
//                        Destroy(createItem);
//                        //만약 북
//                        if (i == 0)
//                        {
//                            ChangeState(Items.Rail_R);
//                            setRot = Quaternion.Euler(0, 180, 0);
//                        }
//                        //만약 남
//                        else
//                        {
//                            ChangeState(Items.Rail_L);
//                            setRot = Quaternion.Euler(0, 90, 0);
//                        }
//                    }
//                    // 동 서 , 발견선로 : 북
//                    else if (i % 2 != 0 && count[0] == 0)
//                    {
//                        Destroy(createItem);
//                        //만약 동
//                        if (i == 1)
//                        {
//                            ChangeState(Items.Rail_R);
//                            setRot = Quaternion.Euler(0, 270, 0);
//                        }
//                        //만약 서
//                        else
//                        {
//                            ChangeState(Items.Rail_L);
//                            setRot = Quaternion.Euler(0, 180, 0);
//                        }
//                    }
//                    // 동 서 , 발견선로 : 남
//                    else if (i % 2 != 0 && count[0] == 2)
//                    {
//                        Destroy(createItem);
//                        //만약 동
//                        if (i == 1)
//                        {
//                            ChangeState(Items.Rail_L);
//                            setRot = Quaternion.Euler(0, 0, 0);
//                        }
//                        //만약 서
//                        else
//                        {
//                            ChangeState(Items.Rail_R);
//                            setRot = Quaternion.Euler(0, 90, 0);
//                        }
//                    }
//                }
//                else if (count.Count == 2)
//                {
//                    // 북 남 , 발견선로 : 동
//                    if (count[0] % 2 == 0 && count[1] == 1)
//                    {
//                        Destroy(createItem);
                        
//                        //만약 북쪽 선로
//                        if (i == 0)
//                        {
//                            ChangeState(Items.Rail_L);
//                            setRot = Quaternion.Euler(0, 180, 0);
//                        }
//                        //만약 남쪽 선로
//                        else
//                        {
//                            ChangeState(Items.Rail_R);
//                            setRot = Quaternion.Euler(0, 0, 0);
//                        }
//                    }
//                    // 북 남 , 발견선로 : 서
//                    else if (count[0] % 2 == 0 && count[1] == 3)
//                    {
//                        Destroy(createItem);
                        
//                        //만약 북
//                        if (i == 0)
//                        {
//                            ChangeState(Items.Rail_R);
//                            setRot = Quaternion.Euler(0, 180, 0);
//                        }
//                        //만약 남
//                        else
//                        {
//                            ChangeState(Items.Rail_L);
//                            setRot = Quaternion.Euler(0, 90, 0);
//                        }
//                    }
//                    // 동 서 , 발견선로 : 북
//                    else if (count[0] % 2 != 0 && count[1] == 0)
//                    {
//                        Destroy(createItem);
                        
//                        //만약 동
//                        if (i == 1)
//                        {
//                            ChangeState(Items.Rail_R);
//                            setRot = Quaternion.Euler(0, 270, 0);
//                        }
//                        //만약 서
//                        else
//                        {
//                            ChangeState(Items.Rail_L);
//                            setRot = Quaternion.Euler(0, 180, 0);
//                        }
//                    }
//                    // 동 서 , 발견선로 : 남
//                    else if (count[0] % 2 != 0 && count[1] == 2)
//                    {
//                        Destroy(createItem);
                        
//                        //만약 동
//                        if (i == 1)
//                        {
//                            ChangeState(Items.Rail_L);
//                            setRot = Quaternion.Euler(0, 0, 0);
//                        }
//                        //만약 서
//                        else
//                        {
//                            ChangeState(Items.Rail_R);
//                            setRot = Quaternion.Euler(0, 90, 0);
//                        }
//                    }
//                }
//                else
//                {
//                    setRot = Quaternion.Euler(0, railRot[i], 0);
//                }

//                if (!count.Contains(i))
//                { 
//                    count.Add(i);
//                }
//            }
//        }
//        railInfo.Clear();
//        print(count.Count);
//    }
}
