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
    public Transform NicknameUI;
    public Transform IntroUI;
    public Text nName;

    // �뽬 �ð�
    public float dashTime = 0.3f;

    // �Է°� �޾ƿ��� ����
    float h;
    float v;




    // ���� �ӷ�
    public float lerpSpeed = 10;
    float curTime;
    float SetTime=5;

    // Start is called before the first frame update
    void Start()
    {
        // �ӵ� �ʱ�ȭ
        finSpeed = speed;
        IntroUI.transform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime > SetTime)
        {
            IntroUI.transform.gameObject.SetActive(true);
        }
        if (curTime > 2 * SetTime)
        {
            IntroUI.transform.gameObject.SetActive(false);
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
        NicknameUI.transform.position = transform.position+new Vector3(0,3,0);
        nName.text = GameInfo.instance.nickName;


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

   