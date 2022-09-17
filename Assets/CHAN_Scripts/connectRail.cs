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
    //����� ���ε��� �����ϴ� ����Ʈ�� �����Ѵ�.
    [SerializeField]public List<GameObject> connectedRails = new List<GameObject>();
    //������ ������ ��, start rail�� ����Ʈ�� �߰��Ѵ�.
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
        //����Ʈ�� �������κп� �� ���ӿ�����Ʈ�� �ش� �Լ��� �۵���Ű�� �ʹ�.
        Detect();
        if (connectedRails.Count > 2)
        { 
            CheckConnect();
        }
    }
    //4���� ���� �����س��� �迭
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
    // Ȥ�� �𸣴� ������������ rail�� connection�� �˻��ϴ� �Լ�
    void CheckConnect()
    {
        for (int i=0; i<connectedRails.Count-1;i++)
        {
            if (!connectedRails[i].GetComponent<ItemGOD>().isConnected)
            {
                connectedRails[i].GetComponent<ItemGOD>().isConnected = true;
            }
        }

        // ���⼭ ������ ������ �����ϴ� ������ ���۵ȴ�.
        //���⼭ ������ ���� ������ �˱����� ����2���� ���� ������ ����� ����.
        Vector3 basePos = connectedRails[connectedRails.Count - 2].transform.position;
        //detect1= ����Ʈ�� ������ ������Ʈ
        //detect2= ����Ʈ�� �������� 2���� ������Ʈ
        Vector3[] detect=new Vector3[2];

        // 1. 2�� �ݺ��Ѵ�.
        for (int i = 0; i < 2; i++)
        {
            Vector3 detectPos = connectedRails[connectedRails.Count - 1 - (2 * i)].transform.position;
            //������ ����
            if (basePos.x - detectPos.x < 0 && basePos.z - detectPos.z == 0)
            {
                detect[i] = Vector3.right;
            }
            //���� ����
            else if (basePos.x - detectPos.x > 0 && basePos.z - detectPos.z == 0)
            {
                detect[i] = Vector3.left;
            }
            //���� ����
            else if (basePos.x - detectPos.x ==0 && basePos.z - detectPos.z < 0)
            {
                detect[i] = Vector3.forward;
            }
            //�Ʒ�����
            else if (basePos.x - detectPos.x == 0 && basePos.z - detectPos.z > 0)
            {
                detect[i] = Vector3.back;
            }
        }
        // ���⼭���� �տ��� ���� �ΰ��� ���͸� ���Ѵ�.

        //��, ��
        if ((detect[0] == Vector3.left && detect[1] == Vector3.forward) ||
            (detect[1] == Vector3.left && detect[0] == Vector3.forward))
        {
            // _| ����
            // �Ʒ��� �ڳʷ��Ϸ� �ٲ�
            if (detect[0] == Vector3.forward)
            {
                connectedRails[connectedRails.Count - 2].GetComponent<ItemGOD>().
                ChangeState(ItemGOD.Items.CornerRail, Quaternion.Euler(0, 90, 0));
                //�Ʒ��� ������ ������ �ٲ�
                connectedRails[connectedRails.Count - 1].GetComponent<ItemGOD>().
                ChangeState(ItemGOD.Items.Rail, Quaternion.Euler(0, 90, 0));
            }
            else
            {
                connectedRails[connectedRails.Count - 2].GetComponent<ItemGOD>().
                ChangeState(ItemGOD.Items.CornerRail, Quaternion.Euler(0, 90, 0));
            }
            
        }
        //��, �Ʒ�
        else if ((detect[0] == Vector3.left && detect[1] == Vector3.back) ||
                (detect[1] == Vector3.left && detect[0] == Vector3.back))
        {
            // �� ����
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
            // �Ʒ��� �ڳʷ��Ϸ� �ٲ�
           

        }
        //��, �Ʒ�
        else if ((detect[0] == Vector3.right && detect[1] == Vector3.back) ||
                (detect[1] == Vector3.right && detect[0] == Vector3.back))
        {
            // |-- ����
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
            // �Ʒ��� �ڳʷ��Ϸ� �ٲ�

        }
        // ��, �� 
        else if ((detect[0] == Vector3.right && detect[1] == Vector3.forward) ||
                (detect[1] == Vector3.right && detect[0] == Vector3.forward))
        {
            if (detect[0] == Vector3.forward)
            {
                connectedRails[connectedRails.Count - 2].GetComponent<ItemGOD>().
                ChangeState(ItemGOD.Items.CornerRail, Quaternion.Euler(0, 180, 0));
                //�Ʒ��� ������ ������ �ٲ�
                connectedRails[connectedRails.Count - 1].GetComponent<ItemGOD>().
                    ChangeState(ItemGOD.Items.Rail, Quaternion.Euler(0, 90, 0));
            }
            else
            {
                connectedRails[connectedRails.Count - 2].GetComponent<ItemGOD>().
                ChangeState(ItemGOD.Items.CornerRail, Quaternion.Euler(0, 180, 0));
            }
            // �Ʒ��� �ڳʷ��Ϸ� �ٲ�

            // �� ����
        }
        else if ((detect[0] == Vector3.forward && detect[1] == Vector3.back) ||
                (detect[1] == Vector3.forward && detect[0] == Vector3.back))
        {
            // | ����
            connectedRails[connectedRails.Count - 1].GetComponent<ItemGOD>().
                ChangeState(ItemGOD.Items.Rail, Quaternion.Euler(0, 90, 0));

        }
    }


}
