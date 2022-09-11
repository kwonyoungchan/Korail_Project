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
                    break;
                case Items.Rail_R:
                    createItem = Instantiate(Resources.Load<GameObject>("CHAN_Prefab/Rail_R"));
                    createItem.transform.position = transform.position + new Vector3(0, 0.5f, 0);
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
        //���⼭ 4���⿡�� ���θ� ã�´�. 
        //Ray�� ���.
        //�� 4�� ���. 
        //4�� �ݺ��ؼ� ���� ������ ���� ������ ������ ����
        float[] railRot = { 90, 0, 180, 0 };
        float[] railRot_R = { 90, 180, -90, 0 };
        float[] railRot_L = { -90, 0, 90, 180 };
        List<int> count = new List<int>();
        
        Vector3[] dir = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
        for (int i = 0; i < 4; i++)
        {
            Ray ray = new Ray(transform.position, dir[i]);
            RaycastHit hit;
            RailInfo RI = new RailInfo();
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
            print(railInfo[i].info);
            print(railInfo[i].rotaion);
        }
        //���⼭ ���θ� �����ϰ� ��
        for (int i = 0; i < 4; i++)
        {
            if (railInfo[i].info == "Rail")
            {
                if (railInfo[i].rotaion == railRot[i])
                {
                    //���ι��� ����

                    //�ϰ� count ���Ƹ���
                    if (count.Count == 1)
                    {
                        if (i % 2 == 0)
                        {
                            if (count[0] == 1)
                            {
                                Destroy(createItem);
                                ChangeState(Items.Rail_L);
                                if (i == 0)
                                {
                                    createItem.transform.Rotate(0, 180, 0);
                                    transform.rotation = Quaternion.Euler(0, 180, 0);
                                }

                            }

                            else if (count[0] == 3)
                            {
                                Destroy(createItem);
                                ChangeState(Items.Rail_R);
                                if (i == 2)
                                {
                                    createItem.transform.Rotate(0, 180, 0);
                                    transform.rotation = Quaternion.Euler(0, 180, 0);
                                }

                            }
                        }
                        else
                        {
                            if (count[0] == 0)
                            {
                                Destroy(createItem);
                                ChangeState(Items.Rail_L);
                                if (i == 3)
                                {
                                    createItem.transform.rotation= Quaternion.Euler(0, 90, 0);
                                    transform.rotation = Quaternion.Euler(0, 90, 0);
                                }
                                

                            }
                            else if (count[0] == 2)
                            {
                                Destroy(createItem);
                                ChangeState(Items.Rail_R);
                                if (i == 1)
                                {
                                    createItem.transform.Rotate(0, 90, 0);
                                    transform.rotation = Quaternion.Euler(0, 90, 0);
                                }
                            }
                        }
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(0, railRot[i], 0);
                    }    
                }
                count.Add(i);
            }
            else if (railInfo[i].info == "Rail_R")
            {
                if (railInfo[i].rotaion == railRot_R[i])
                {
                    //���ι��� ����
                    transform.rotation = Quaternion.EulerAngles(0, railRot[i], 0);
                    //�ϰ� count ���Ƹ���
                    if (count.Count == 1)
                    {
                        if (i % 2 == 0)
                        {
                            if (count[0] == 1)
                            {
                                Destroy(createItem);
                                ChangeState(Items.Rail_L);
                                createItem.transform.Rotate(0, 180, 0);
                                //-180
                            }
                                
                            else if (count[0] == 3) 
                            {
                                Destroy(createItem);
                                ChangeState(Items.Rail_R);
                                createItem.transform.Rotate(0, 180, 0);
                                //=180
                            }
                        }
                        else
                        {
                            if (count[0] == 0)
                            {
                                Destroy(createItem);
                                ChangeState(Items.Rail_L);
                                createItem.transform.Rotate(0, 180, 0);
                                //180
                            }
                            else if (count[0] == 2)
                            {
                                Destroy(createItem);
                                ChangeState(Items.Rail_R);
                                createItem.transform.Rotate(0, 180, 0);
                                //180
                            }
                                
                        }
                    }
                    
                }
                count.Add(i);
            }
            else if (railInfo[i].info == "Rail_L")
            {
                if (railInfo[i].rotaion == railRot_L[i])
                {
                    //���ι��� ����
                    transform.rotation = Quaternion.EulerAngles(0, railRot[i], 0);
                    //�ϰ� count ���Ƹ���
                    if (count.Count == 1)
                    {
                        if (i % 2 == 0)
                        {
                            if (count[0] == 1)
                            {
                                Destroy(createItem);
                                ChangeState(Items.Rail_L);
                                createItem.transform.rotation = Quaternion.EulerAngles(0, -180, 0);
                            }

                            else if (count[0] == 3)
                            {
                                Destroy(createItem);
                                ChangeState(Items.Rail_R);
                                createItem.transform.rotation = Quaternion.EulerAngles(0, -180, 0);
                            }
                        }
                        else
                        {
                            if (count[0] == 0)
                            {
                                Destroy(createItem);
                                ChangeState(Items.Rail_L);
                                createItem.transform.rotation = Quaternion.EulerAngles(0, 180, 0);
                            }
                            else if (count[0] == 2)
                            {
                                Destroy(createItem);
                                ChangeState(Items.Rail_R);
                                createItem.transform.rotation = Quaternion.EulerAngles(0, 180, 0);
                            }

                        }
                    }
                    
                }
                count.Add(i);
            }
            print(count.Count);
        }
        

    }
}
