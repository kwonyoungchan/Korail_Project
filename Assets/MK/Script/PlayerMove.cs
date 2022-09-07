using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾� �̵�
public class PlayerMove : MonoBehaviour
{
    // �ʿ� �Ӽ� : �ӵ�
    public float speed = 3;

    // ĳ���� ��Ʈ�ѷ�
    CharacterController playerCc;
    // Start is called before the first frame update
    void Start()
    {
        // ĳ���� ��Ʈ�ѷ� �Ӽ� ��������
        playerCc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 1. ����� �Է°� �ޱ�
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // ���� ��ȯ�� ���� ĳ���� ��?
        if(h <= -1)
        {
            transform.forward = Vector3.left;
        }
        else if(h >= 1)
        {
            transform.forward = Vector3.right;
        }   
        if(v >= 1)
        {
            transform.forward = Vector3.forward;
        }
        if(v <= -1)
        {
            transform.forward = Vector3.back;
        }

        // 2. ���� �����ϱ�
        Vector3 dir = h * Vector3.right + v * Vector3.forward;
        dir.Normalize();


        // 3. �÷��̾� �����̱�
        playerCc.Move(dir * speed * Time.deltaTime);
    }
}
