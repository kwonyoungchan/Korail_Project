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
    [SerializeField]GameObject StartRail;
    [SerializeField]GameObject EndRail;

    

    void Start()
    {
        
        connectedRails.Add(StartRail);
    }

    // Update is called once per frame
    void Update()
    {
        //����Ʈ�� �������κп� �� ���ӿ�����Ʈ�� �ش� �Լ��� �۵���Ű�� �ʹ�.
        Detect();
        CheckConnect();
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
    }

    
}
