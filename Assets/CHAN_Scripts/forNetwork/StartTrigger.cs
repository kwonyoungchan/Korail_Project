using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]Collider[] detect;
    Vector3 boxSize = new Vector3(2, 2, 2);
    void Start()
    {
        
    }

    //���⼭ ����� �÷��̾ 4���� �Ǹ� �������� �ö󰡰� �������� 100% �Ǹ� ���� ������ ���� �Ѵ�.
    //�׸��� ���� ������ Ŭ�����ϰ� �ٽ� �ش� ������ ���� �� ��, ���� ��ҵ� �ٽ� �ʱ�ȭ �ؾ��ϴ°� ���� ����.

    void Update()
    {
        detect = Physics.OverlapBox(transform.position,boxSize*0.5f,default,1<<6);
    }
    private void OnTriggerEnter(Collider other)
    {
     
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}
