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

}
