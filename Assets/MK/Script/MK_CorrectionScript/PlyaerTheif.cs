using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���̸� ��ٰ� Theif�� ������ Thief�� state�� Run���� ����
public class PlyaerTheif : MonoBehaviour
{
    // ���� ��� ��ġ 
    public GameObject rPos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // �÷��̾ ������ ���̸� ���
        Ray playerRay = new Ray(rPos.transform.position, transform.forward);
        RaycastHit rayInfo;
        // ���� ���� ��ü�� �ִٸ�
        if (Physics.Raycast(playerRay, out rayInfo))
        {
            // ���� ��ü�� �����̶�� state�� Run���� �ٲ�
            Theif theif = rayInfo.transform.gameObject.GetComponent<Theif>();
            if (theif)
            {
                theif.state = Theif.TState.Run;
            }
        }
    }
}
