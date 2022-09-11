using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutDownItems : MonoBehaviour
{

    // �÷��̾ ���� ��� �ִ� �������� ������ �˾ƾ� �Ѵ�.
    int n = -1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        { 
            n*=-1;
        }
        //�����̽��ٸ� ������ ��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //�簢���� ��ġ�� Raycasthit���� �����Ѵ�.
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                #region �̺κ��� �÷��̾ �ٷ� �������� �����ϴ� �ڵ�
                //if (!hit.transform.name.Contains("rail"))
                //{
                //    //���ΰ� �����ǰ�
                //    //������ ���δ� �α��� �簢������ ��ġ�ϰ� �ȴ�.
                //    // ���߿� ��ġ�� �Ǹ� �Ʒ��� ���� ��� ��ġ��Ű�� ��ɸ� �ֵ��� ����.
                //    GameObject rail = Instantiate(RailFac);
                //    // hitPoint�� �����ؾ� �� �ʿ䰡 ����
                //    // ��ǥ�� ������ �Ҽ����� ��������
                //    // �� ���� �����ϰ� �ٽ� 0.5�� ���Ѵ�.
                //    Vector3 interpolatePos;
                //    interpolatePos.x = Mathf.Floor(hit.point.x) + 0.5f;
                //    interpolatePos.y = 0.5f;
                //    interpolatePos.z = Mathf.Floor(hit.point.z) + 0.5f;
                //    rail.transform.position = interpolatePos;
                //}
                //else
                //{
                //    print("��ħ");
                //}
                #endregion
                //�ٷ� rail�� ��ġ�Ѵٰ� ����
                if (n == -1)
                {
                    if (HasItems.instance.rails.Count > 0
                        &&hit.transform.gameObject.GetComponent<ItemGOD>().items!=ItemGOD.Items.Rail)
                    {
                        hit.transform.gameObject.GetComponent<ItemGOD>()
                       .ChangeState(ItemGOD.Items.Rail);
                        HasItems.instance.RemoveRail();
                    }
                }
                else
                {
                    if (hit.transform.gameObject.GetComponent<ItemGOD>().items == ItemGOD.Items.Rail)
                    {
                        hit.transform.gameObject.GetComponent<ItemGOD>()
                           .ChangeState(ItemGOD.Items.Idle);
                        HasItems.instance.AddRail();
                    }
                }
                
            }


        }
    }


    
}
