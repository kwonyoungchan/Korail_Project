using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ ���� �������� ���̽�� => ���̷� ���� �� collider�� 
// �÷��̾ ���� ���⿡ ������ ö�� �ִٸ� ĳ��
public class PlayerForwardRay : MonoBehaviour
{
    // ���� ��ġ
    public GameObject rPos;
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

    // ä���� ���� ������
    // ingredientItem
    IngredientItem item;

    public bool isGathering = false;

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
        Ray playerRay = new Ray(rPos.transform.position, transform.forward);
        RaycastHit rayInfo;
        // ���� ���� ��ü�� �ִٸ�
        if (Physics.Raycast(playerRay, out rayInfo, 1f))
        {
            Debug.DrawRay(rPos.transform.position, transform.forward, Color.blue);
            // ���� ���� ��
            riverGOD = rayInfo.transform.GetComponentInParent<RiverGOD>();
            // ���� ���� ��� �����ϴ� ��
            item = rayInfo.transform.GetComponentInParent<IngredientItem>();
            if (riverGOD)
            {
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
                    if (currentTime > waterTime)
                    {
                        water.SetActive(true);
                        isWater = true;
                    }
                }
            }
            // ���� ���� ������ ö�̶��
            if (item)
            {
                currentTime += Time.deltaTime;
                if (currentTime > 1)
                {
                    isGathering = true;
                    currentTime = 0;
                }
            }
        }
    }
}
