using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




// �÷��̾� �̵�
// 1. �⺻���� �̵�
// 2. �÷��̾ �ٶ󺸴� �κ�
// 3. �뽬
public class PlayerMove_Lobby : MonoBehaviour
{
    // �ʿ� �Ӽ� : �ӵ�
    public float speed = 3;
    // �뽬 �ӵ�
    public float a = 2;

    // ���� �ӵ�
    public float finSpeed;
    public Transform IntroUI;
    public Transform IntroUI2;
    int n = -1;
    GameObject ui;

    // �뽬 �ð�
    public float dashTime = 0.3f;

    // �Է°� �޾ƿ��� ����
    float h;
    float v;




    // ���� �ӷ�
    public float lerpSpeed = 10;
    float curTime;
    public float SetTime=5;

    // Start is called before the first frame update
    void Start()
    {
        // �ӵ� �ʱ�ȭ
        finSpeed = speed;
        IntroUI.transform.gameObject.SetActive(false);
        IntroUI2.transform.gameObject.SetActive(false);
        ui = IntroUI.transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (n == -1)
        {
            ui = IntroUI2.transform.gameObject;
        }
        else
        {
            ui = IntroUI.transform.gameObject;
        }
        curTime += Time.deltaTime;
        if (curTime > SetTime)
        {
            ui.gameObject.SetActive(true);
        }
        if (curTime > 2 * SetTime)
        {
            ui.gameObject.SetActive(false);
            n *= -1;
            curTime = 0;
        }
        // ������ ���� : ������ �ƴϸ� ��ȯ

        // 1. ����� �Է°� �ޱ�
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dashing();
        }
        LookPlayer();

        // 2. ���� �����ϱ�
        Vector3 dir = h * Vector3.right + v * Vector3.forward;
        dir.Normalize();

        // 3. �÷��̾� �����̱�
        transform.position += dir * finSpeed * Time.deltaTime;
        ui.transform.position = transform.position+new Vector3(1,3,0);
        


    }

    // �÷��̾��� �չ��� ��ȯ�ϴ� �Լ�

    // �뽬 ���
    void Dashing()
    {
        StartCoroutine(IEDash());
    }
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
        if (h > 0 && v > 0)
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

    IEnumerator IEDash()
    {
        finSpeed = speed * a;
        yield return new WaitForSeconds(dashTime);
        finSpeed = speed;
    }
}

   