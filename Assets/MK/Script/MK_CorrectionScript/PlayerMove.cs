using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// �÷��̾� �̵�
// 1. �⺻���� �̵�
// 2. �÷��̾ �ٶ󺸴� �κ�
// 3. �뽬
public class PlayerMove : MonoBehaviourPun, IPunObservable
{
    // �ʿ� �Ӽ� : �ӵ�
    public float speed = 3;
    // �뽬 �ӵ�
    public float a = 2;

    // ���� �ӵ�
    public float finSpeed;

    // �뽬 �ð�
    public float dashTime = 0.3f;

    // �Է°� �޾ƿ��� ����
    float h;
    float v;

    // ������ٵ�
    Rigidbody rigid;

    // ��Ʈ��ũ
    // ��ġ
    Vector3 receivePos;
    Vector3 receiveNPos;
    // ȸ��
    Quaternion receiveRot;
    // ���� �ӷ�

    public Transform NicknameUI;
    public Text nName;
    public float lerpSpeed = 10;
    float curTime;
    float SetTime=5;

    string s;

    // Start is called before the first frame update
    void Start()
    {
        // ������ٵ�
        rigid = GetComponentInChildren<Rigidbody>();
        // �ӵ� �ʱ�ȭ
        finSpeed = speed;
        nName.text = photonView.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        s = SceneManager.GetActiveScene().name;
        if (s == "WaitingRoom")
        {
            NicknameUI.transform.gameObject.SetActive(true);
        }
        else
        {
            NicknameUI.transform.gameObject.SetActive(false);
        }
        // ������ ���� : ������ �ƴϸ� ��ȯ
        if (photonView.IsMine)
        {
            // 1. ����� �Է°� �ޱ�
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");

            // ���� ��ȯ�� ���� ĳ���� �ٶ󺸴� �κ�
            LookPlayer();

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Dashing();
            }

            // 2. ���� �����ϱ�
            Vector3 dir = h * Vector3.right + v * Vector3.forward;
            dir.Normalize();

            // 3. �÷��̾� �����̱�
            transform.position += dir * finSpeed * Time.deltaTime;
            NicknameUI.transform.position = transform.position + new Vector3(0, 3, 0);
            
        }
        else
        {
            // Lerp�� �̿��ؼ� ������, ����������� �̵� �� ȸ��
            transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
            NicknameUI.position = Vector3.Lerp(NicknameUI.transform.position, receiveNPos, lerpSpeed * Time.deltaTime);
            
        }
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
        finSpeed = speed * a;
        yield return new WaitForSeconds(dashTime);
        finSpeed = speed;
    }

    #region ��Ʈ��ũ
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // ������ ������� �ޱ�� �Ѱ����� ture�� ��
        // ������ ������
        if (stream.IsWriting)
        {
            // position, rotation => class ���ѱ� value�� ����, value Ÿ�� �迭�̳� ����Ʈ ����
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(NicknameUI.transform.position);

        }
        // ������ �ޱ�
        else if (stream.IsReading) // = if (stream.IsReading)
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
            receiveNPos = (Vector3)stream.ReceiveNext();
        }
    }
    #endregion
    //void ShowNickName()
    //{
    //    photonView.RPC("RpcshowNickName", RpcTarget.All);
    //}
    //[PunRPC]
    //void RpcshowNickName()
    //{
    //    nName.text = GameInfo.instance.nickName;
    //}
}
