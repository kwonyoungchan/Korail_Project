using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾�� ��������� ȹ�氡���ϰ� �����
public class ToolItem : MonoBehaviour
{
    // �÷��̾���� �Ÿ�
    public float itemDis = 0.2f;
    // ȹ�� ����
    public bool isAx = false;
    public bool isPick = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            if (gameObject.name.Equals("Ax"))
            {
                isAx = true;
            }
            if (gameObject.name.Equals("Pick"))
            {
                isPick = true;
            }
        }
    }
}
