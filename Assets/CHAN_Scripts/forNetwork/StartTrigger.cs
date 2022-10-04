using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class StartTrigger : MonoBehaviourPun
{
    // Start is called before the first frame update
    [SerializeField] Collider[] detect;
    Vector3 boxSize = new Vector3(10, 2, 10);
    float curTime;
   [SerializeField] float setTime;
   [SerializeField] Scrollbar Scrollbar;
   [SerializeField] GameObject Handle;
    bool turn;
    void Start()
    {
        Handle.SetActive(false);
    }

    //���⼭ ����� �÷��̾ 4���� �Ǹ� �������� �ö󰡰� �������� 100% �Ǹ� ���� ������ ���� �Ѵ�.
    //�׸��� ���� ������ Ŭ�����ϰ� �ٽ� �ش� ������ ���� �� ��, ���� ��ҵ� �ٽ� �ʱ�ȭ �ؾ��ϴ°� ���� ����.

    void Update()
    {
        detect = Physics.OverlapBox(transform.position, boxSize * 0.5f, default, 1 << 6);
        if (detect.Length == 4)
        {
            // �ε� ����
            StartLoading();
            ShowGuageUI();
        }
        else
        {
            //�ƴϸ� ������ �ʱ�ȭ
            curTime = 0;
            Scrollbar.size = 0;
            Handle.SetActive(false);
        }
    }
    void StartLoading()
    {
        // ui �� �������� ������ �����. 
        // �ð��� ���� �������� �ص� ������ ui�� RPC�� ������ �Ѵ�. 
        curTime += Time.deltaTime;
        if (curTime > setTime&&!turn)
        {
            WaitingRoomManager.instance.LoadArena();
            turn = true;
        }
    }
    void ShowGuageUI()
    {
        Handle.SetActive(true);
        Scrollbar.size +=  Time.deltaTime/setTime;
    }
}
