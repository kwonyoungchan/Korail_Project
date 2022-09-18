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
    public Transform triggerPos;
    Vector3 pos;
    void Start()
    {
        Camera.main.transform.position = TrainPos.position + setPos;
        pos = Vector3.zero;
    }

    void Update()
    {
        //ī�޶��̵��� ������ x����� �̵����� �����Ѵ�.
        // ������ ��ũ�� ��ǥ�󿡼� Ư������ �Ѿ�� ī�޶� ���� �̵��Ѵ�. 
        
        //ī�޶� �̵��� Lerp�� �����ϸ鼭 �̵��Ѵ�.
        if (TrainPos.gameObject.activeSelf)
        {
            if (TrainPos.position.x > triggerPos.position.x)
            {
                pos.x = Mathf.Lerp(pos.x, TrainPos.position.x, 10 * Time.deltaTime);
            }
        }
        Camera.main.transform.position = pos + setPos;
        
    }
}
