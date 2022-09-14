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
    [SerializeField]GameObject StartRail;
    [SerializeField]GameObject EndRail;

    

    void Start()
    {
        
        connectedRails.Add(StartRail);
    }

    // Update is called once per frame
    void Update()
    {
        //리스트의 마지막부분에 들어간 게임오브젝트만 해당 함수를 작동시키고 싶다.
        Detect();
        CheckConnect();
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
    }

    
}
