using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾� �̵�
// 1. �⺻���� �̵�
// 2. �÷��̾ �ٶ󺸴� �κ�
// 3. �뽬
public class PlayerMove : MonoBehaviour
{
    // �ʿ� �Ӽ� : �ӵ�
    public float speed = 3;

    // ĳ���� ��Ʈ�ѷ�
    CharacterController playerCc;

    // �Է°� �޾ƿ��� ����
    float h;
    float v;

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
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        // ���� ��ȯ�� ���� ĳ���� �ٶ󺸴� �κ�
        LookPlayer();

        // 2. ���� �����ϱ�
        Vector3 dir = h * Vector3.right + v * Vector3.forward;
        dir.Normalize();

        // 3. �÷��̾� �����̱�
        playerCc.Move(dir * speed * Time.deltaTime);
    }

    // �÷��̾��� �չ��� ��ȯ�ϴ� �Լ�
    void LookPlayer()
    {
        // ����
        if (h <= -1)
        {
            transform.forward = Vector3.left;
        }
        // ������
        if (h >= 1)
        {
            transform.forward = Vector3.right;
        }
        // ��
        if (v >= 1)
        {
            transform.forward = Vector3.forward;
        }
        // �Ʒ�
        if (v <= -1)
        {
            transform.forward = Vector3.back;
        }
        // ������ �մ밢�� 
        if(h > 0 && v > 0)
        {
            transform.forward = Vector3.forward + Vector3.right;
        }
        // ������ �Ʒ� �밢��
        if (h > 0 && v < 0)
        {
            transform.forward = Vector3.back + Vector3.right;
        }
        // ���� �� �밢��
        if (h < 0 && v > 0)
        {
            transform.forward = Vector3.forward + Vector3.left;
        }
        if (h < 0 && v < 0)
        {
            transform.forward = Vector3.back + Vector3.left;
        }
    }

    // �뽬 ���
    void Dashing()
    {
        StartCoroutine(IEDash());
    }

    IEnumerator IEDash()
    {
        yield return null;
    }
}
