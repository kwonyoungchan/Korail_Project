using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutDownRails : MonoBehaviour
{
    [SerializeField ]GameObject RailFac;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�����̽��ٸ� ������ ��
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //���ΰ� �����ǰ�
            
            //������ ���δ� �α��� �簢������ ��ġ�ϰ� �ȴ�.
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (!hit.transform.name.Contains("rail"))
                {

                    GameObject rail = Instantiate(RailFac);
                    // hitPoint�� �����ؾ� �� �ʿ䰡 ����
                    // ��ǥ�� ������ �Ҽ����� ��������
                    // �� ���� �����ϰ� �ٽ� 0.5�� ���Ѵ�.
                    Vector3 interpolatePos;
                    interpolatePos.x = Mathf.Floor(hit.point.x) + 0.5f;
                    interpolatePos.y = 0.5f;
                    interpolatePos.z = Mathf.Floor(hit.point.z) + 0.5f;
                    rail.transform.position = interpolatePos;
                }
                else
                {
                    print("��ħ");
                }
            }
            
            //�簢���� ��ġ�� Raycasthit���� �����Ѵ�.
        }
    }
}
