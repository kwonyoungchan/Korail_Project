using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���̸� ��ٰ� Theif�� ������ Thief�� state�� Run���� ����
public class PlyaerTheif : MonoBehaviour
{
    // ���� ��� ��ġ 
    public GameObject rPos;
    int layer;

    // Update is called once per frame
    void Update()
    {
        // �÷��̾ ������ ���̸� ���
        Ray playerRay = new Ray(rPos.transform.position, transform.forward);
        RaycastHit rayInfo;
        layer = 1 << 13;
        // ���� ���� ��ü�� �ִٸ�
        if (Physics.Raycast(playerRay, out rayInfo, 30, layer))
        {
            // ���� ��ü�� �����̶�� state�� Run���� �ٲ�
            Theif theif = rayInfo.transform.gameObject.GetComponent<Theif>();
            if (theif)
            {
                theif.TheifState(Theif.TState.Run);
                theif.pForward = transform.forward;
                theif.transform.forward = transform.forward;
            }
        }
    }
}
