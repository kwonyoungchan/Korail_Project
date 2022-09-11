using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        Rail,
        Rail_R,
        Rail_L
    }
    public Items items;
    public bool turn;
    GameObject createItem;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        StateMachine();
    }

    void StateMachine()
    {
        if (!turn)
        {
            turn = true;
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
                case Items.Rail:
                    createItem = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail"));
                    createItem.transform.position = transform.position + new Vector3(0, 0.5f, 0);
                    DoSelect();
                    break;
                case Items.Rail_L:
                    createItem = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail_L"));
                    createItem.transform.position = transform.position + new Vector3(0, 0.5f, 0);
                    DoSelect();
                    break;
                case Items.Rail_R:
                    createItem = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail_R"));
                    createItem.transform.position = transform.position + new Vector3(0, 0.5f, 0);
                    DoSelect();
                    break;
            }
        }
           
    }

    public void ChangeState(Items item)
    {
        turn = false;
        items = item;
    }
    void DoSelect()
    {
        //여기서 4방향에서 선로를 찾는다. 
        //Ray를 쏜다.
        //총 4번 쏜다. 
        //4번 반복해서 얻은 정보를 토대로 선로의 방향을 결정

        
        Vector3[] dir = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
        for (int i = 0; i < 4; i++)
        {
            Ray ray = new Ray(transform.position, dir[i]);
            RaycastHit hit;
            RailInfo RI = new RailInfo();
            if (Physics.Raycast(ray, out hit))
            {

                if (hit.transform.gameObject.GetComponent<ItemGOD>().items ==
                    ItemGOD.Items.Rail)
                {
                    RI.info = "Rail";
                    RI.rotaion = hit.transform.eulerAngles.y;
                }
                else if (hit.transform.gameObject.GetComponent<ItemGOD>().items == ItemGOD.Items.Rail_L)
                {
                    RI.info = "Rail_L";
                    RI.rotaion = hit.transform.eulerAngles.y;
                }
                else if (hit.transform.gameObject.GetComponent<ItemGOD>().items == ItemGOD.Items.Rail_R)
                {
                    RI.info = "Rail_R";
                    RI.rotaion = hit.transform.eulerAngles.y;
                }
                else
                {
                    RI.info = null;
                    RI.rotaion = hit.transform.eulerAngles.y;
                }
            }
            railInfo.Add(RI);
            print(railInfo[i].info);
            print(railInfo[i].rotaion);
        }
        //여기서 선로를 결정하게 됨
        //먼저 주변에 선로가 있는지 확인하고
        // 그 선로의 종류가 무엇인지 확인하고
        // 선로의 방향이 무엇인지 확인하는 순서로 진행
    }
}
