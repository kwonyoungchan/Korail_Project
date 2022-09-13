using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �� �ֺ��� rail ���� Ŭ����
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
    Quaternion setRot;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        StateMachine();
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
            case Items.Rail:
                if (!turn)
                {
                    createItem = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail"));
                    createItem.transform.position = transform.position + new Vector3(0, 0.5f, 0);
                    createItem.transform.rotation = setRot;
                    turn = true;
                }
                DoSelect();
                break;
            case Items.Rail_L:
                if (!turn)
                {
                    createItem = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/cornerRail"));
                    createItem.transform.position = transform.position + new Vector3(0, 0.5f, 0);
                    createItem.transform.rotation = setRot;
                    turn = true;
                }
                DoSelect();
                break;
            case Items.Rail_R:
                if (!turn)
                {
                    createItem = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/cornerRail"));
                    createItem.transform.position = transform.position + new Vector3(0, 0.5f, 0);
                    createItem.transform.rotation = setRot;
                    turn = true;
                }
                DoSelect();
                break;
        }


    }


    public void ChangeState(Items item)
    {
        turn = false;
        items = item;
    }

    
    Vector3[] dir = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
    [SerializeField]List<int> count = new List<int>();
    void DoSelect()
    {
        //���⼭ 4���⿡�� ���θ� ã�´�. 
        //Ray�� ���.
        //�� 4�� ���. 
        //4�� �ݺ��ؼ� ���� ������ ���� ������ ������ ����
        float[] railRot = { 90, 0, 180, 0 };
        //float[] railRot_R = { 90, 180, -90, 0 };
        //float[] railRot_L = { -90, 0, 90, 180 };
        //�ֺ� ���� �ľ� 
        
        for (int i = 0; i < 4; i++)
        {
            Ray ray = new Ray(transform.position, dir[i]);
            RaycastHit hit;
            RailInfo RI = new RailInfo();
            //���⼭ ���� ��ȸ�� ���� �ڵ尡 �ۼ��ȴ�.
            if (Physics.Raycast(ray, out hit))
            {   
                if (hit.transform.gameObject.GetComponent<ItemGOD>().items ==ItemGOD.Items.Rail)
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
        }
        //���⼭ ���θ� �����ϰ� ��
        for (int i = 0; i < 4; i++)
        {
            if (railInfo[i].info == "Rail")
            {
                //if (railInfo[i].rotaion == railRot[i])
                //�� �ֺ��� ���ΰ� 1�� �����ϸ� 
                //�ڳ� ���� ��ġ�� �����Ѵ�.
                if (count.Count == 1)
                {
                    // �� �� , �߰߼��� : ��
                    if (i % 2 == 0 && count[0] == 1)
                    {
                        Destroy(createItem);
                        //���� ���� ����
                        if (i == 0)
                        {
                            ChangeState(Items.Rail_L);
                            setRot = Quaternion.Euler(0, 180, 0);
                        }
                        //���� ���� ����
                        else
                        {
                            ChangeState(Items.Rail_R);
                            setRot = Quaternion.Euler(0, 0, 0);
                        }
                    }
                    // �� �� , �߰߼��� : ��
                    else if (i % 2 == 0 && count[0] == 3)
                    {
                        Destroy(createItem);
                        //���� ��
                        if (i == 0)
                        {
                            ChangeState(Items.Rail_R);
                            setRot = Quaternion.Euler(0, 180, 0);
                        }
                        //���� ��
                        else
                        {
                            ChangeState(Items.Rail_L);
                            setRot = Quaternion.Euler(0, 90, 0);
                        }
                    }
                    // �� �� , �߰߼��� : ��
                    else if (i % 2 != 0 && count[0] == 0)
                    {
                        Destroy(createItem);
                        //���� ��
                        if (i == 1)
                        {
                            ChangeState(Items.Rail_R);
                            setRot = Quaternion.Euler(0, 270, 0);
                        }
                        //���� ��
                        else
                        {
                            ChangeState(Items.Rail_L);
                            setRot = Quaternion.Euler(0, 180, 0);
                        }
                    }
                    // �� �� , �߰߼��� : ��
                    else if (i % 2 != 0 && count[0] == 2)
                    {
                        Destroy(createItem);
                        //���� ��
                        if (i == 1)
                        {
                            ChangeState(Items.Rail_L);
                            setRot = Quaternion.Euler(0, 0, 0);
                        }
                        //���� ��
                        else
                        {
                            ChangeState(Items.Rail_R);
                            setRot = Quaternion.Euler(0, 90, 0);
                        }
                    }
                }
                else if (count.Count == 2)
                {
                    // �� �� , �߰߼��� : ��
                    if (count[0] % 2 == 0 && count[1] == 1)
                    {
                        Destroy(createItem);
                        
                        //���� ���� ����
                        if (i == 0)
                        {
                            ChangeState(Items.Rail_L);
                            setRot = Quaternion.Euler(0, 180, 0);
                        }
                        //���� ���� ����
                        else
                        {
                            ChangeState(Items.Rail_R);
                            setRot = Quaternion.Euler(0, 0, 0);
                        }
                    }
                    // �� �� , �߰߼��� : ��
                    else if (count[0] % 2 == 0 && count[1] == 3)
                    {
                        Destroy(createItem);
                        
                        //���� ��
                        if (i == 0)
                        {
                            ChangeState(Items.Rail_R);
                            setRot = Quaternion.Euler(0, 180, 0);
                        }
                        //���� ��
                        else
                        {
                            ChangeState(Items.Rail_L);
                            setRot = Quaternion.Euler(0, 90, 0);
                        }
                    }
                    // �� �� , �߰߼��� : ��
                    else if (count[0] % 2 != 0 && count[1] == 0)
                    {
                        Destroy(createItem);
                        
                        //���� ��
                        if (i == 1)
                        {
                            ChangeState(Items.Rail_R);
                            setRot = Quaternion.Euler(0, 270, 0);
                        }
                        //���� ��
                        else
                        {
                            ChangeState(Items.Rail_L);
                            setRot = Quaternion.Euler(0, 180, 0);
                        }
                    }
                    // �� �� , �߰߼��� : ��
                    else if (count[0] % 2 != 0 && count[1] == 2)
                    {
                        Destroy(createItem);
                        
                        //���� ��
                        if (i == 1)
                        {
                            ChangeState(Items.Rail_L);
                            setRot = Quaternion.Euler(0, 0, 0);
                        }
                        //���� ��
                        else
                        {
                            ChangeState(Items.Rail_R);
                            setRot = Quaternion.Euler(0, 90, 0);
                        }
                    }
                }
                else
                {
                    setRot = Quaternion.Euler(0, railRot[i], 0);
                }

                if (!count.Contains(i))
                { 
                    count.Add(i);
                }
            }
        }
        railInfo.Clear();
        print(count.Count);
    }
}
