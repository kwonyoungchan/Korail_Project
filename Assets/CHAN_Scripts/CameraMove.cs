using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // ī�޶� �̵��ϴ� ��ũ��Ʈ
    // ī�޶� �����̴� ���� : ����
    // ������ x�� �������� �̵��ϴ°͸� ī�޶� �Ǻ��ؼ� �̵��Ѵ�.
    // �� ī�޶��� ������ ���� ������ �ʴ´�. 
    // �ʿ� �Ӽ�
    // ������ ��ġ 
    public Transform TrainPos;
    [SerializeField] Vector3 setPos;
    void Start()
    {
        transform.position = TrainPos.position + setPos; 
    }

    void Update()
    {
        //ī�޶��̵��� ������ x����� �̵����� �����Ѵ�.
        Vector3 pos = Vector3.zero;
        if (!TrainPos.gameObject.activeSelf)
        {
            pos.x = TrainPos.position.x;
        }
        transform.position = pos + setPos;
    }
}
