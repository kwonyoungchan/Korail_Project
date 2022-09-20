using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ ���� �������� ���̽�� => ���̷� ���� �� collider�� 
public class PlayerForwardRay : MonoBehaviour
{
    public bool isBranch;
    // �÷��̾� ���
    PlayerMaterial player;
    // �÷��̾� ������ ����
    PlayerItemDown playerHand;
    RiverGOD riverGOD;
    // �ð�
    float currentTime;
    public float waterTime = 4;
    // ��ä���
    public GameObject water;

    public bool isWater = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMaterial>();
        playerHand = GetComponent<PlayerItemDown>();
    }

    // Update is called once per frame
    void Update()
    {
        // �÷��̾ ������ ���̸� ���
        Ray playerRay = new Ray(transform.position, transform.forward);
        RaycastHit rayInfo;
        // ���� ���� ��ü�� �ִٸ�
        if (Physics.Raycast(playerRay, out rayInfo, 1f))
        {
            riverGOD = rayInfo.transform.GetComponentInParent<RiverGOD>();
            // �÷��̾��� �տ� ������ ������
            if (player.branchArray.Count > 0)
            {
                // ����Ű�� �����ٸ�
                if (Input.GetButtonDown("Jump"))
                {
                    if (rayInfo.transform.gameObject.layer == 8)
                    {
                        isBranch = false;
                        player.RemoveBranch();
                        // bridge�� �ٲ�
                        riverGOD.riverState = RiverGOD.River.Bridge;
                    }
                }
            }
            // ���� ��ó�� ������
            // �絿�̸� ��� �ִٸ�
            if (playerHand.holdState == PlayerItemDown.Hold.Pail)
            {
                currentTime += Time.deltaTime;
                // ���� �ð� �� �絿�̿� ���� ä������
                if(currentTime > waterTime)
                {
                    water.SetActive(true);
                    isWater = true;
                }
            }
        }
    }
}
