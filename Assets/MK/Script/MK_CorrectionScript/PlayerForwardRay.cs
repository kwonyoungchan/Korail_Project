using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// �÷��̾ ���� �������� ���̽��� => ���̷� ������ �� collider��
// �÷��̾ ���� ���⿡ ������ ö�� �ִٸ� ĳ��
public class PlayerForwardRay : MonoBehaviourPun
{
    // ���� ��ġ
    public GameObject rPos;
    public GameObject iPos;
    public bool isBranch;
    // �÷��̾� ����
    PlayerMaterial player;
    // �÷��̾� ������ ����
    PlayerItemDown playerHand;
    PlayerAnim anim;
    RiverGOD riverGOD;
    // �ð�
    float currentTime;
    public float waterTime = 4;
    // ��ä����
    public GameObject water;
    Animal animal;

    public bool isWater = false;
    public bool isItemDown = false;
    public bool isMat = false;


    // ä���� ���� ������
    // ingredientItem
    IngredientItem item;

    public bool isGathering = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMaterial>();
        playerHand = GetComponent<PlayerItemDown>();
        anim = GetComponent<PlayerAnim>();
    }

    // Update is called once per frame
    void Update()
    {

        if (playerHand.holdState == PlayerItemDown.Hold.Animal)
        {
            if (Input.GetButtonDown("Jump"))
            {
                anim.AnimState(PlayerAnim.Anim.Idle);
                isItemDown = false;
                iPos.transform.GetChild(1).GetComponent<Animal>().animalState = Animal.Animals.Idle;
                playerHand.holdState = PlayerItemDown.Hold.ChangeIdle;
                iPos.transform.GetChild(1).gameObject.transform.position = iPos.transform.position + new Vector3(0, 0.5f, 0.8f);
                iPos.transform.GetChild(1).gameObject.transform.parent = null;
            }
        }
        // �÷��̾ ������ ���̸� ����
        Ray playerRay = new Ray(rPos.transform.position, transform.forward);
        RaycastHit rayInfo;
        // ���� ���� ��ü�� �ִٸ�
        if (Physics.Raycast(playerRay, out rayInfo, 1.5f))
        {
            Debug.DrawRay(rPos.transform.position, transform.forward, Color.blue);
            // ���� ���� ��
            riverGOD = rayInfo.transform.GetComponentInParent<RiverGOD>();
            // ���� ���� ���� �����ϴ� ��
            item = rayInfo.transform.GetComponentInParent<IngredientItem>();
            animal = rayInfo.transform.GetComponent<Animal>();

            if (riverGOD)
            {
                isBranch = true;
                // �÷��̾��� �տ� ������ ������
                if (player.branchArray.Count > 0)
                {
                    // ����Ű�� �����ٸ�
                    if (Input.GetButtonDown("Jump"))
                    {
                        if (rayInfo.transform.gameObject.layer == 8)
                        {

                            player.RemoveBranch();
                            // bridge�� �ٲ�
                            riverGOD.ChangeRiver(RiverGOD.River.Bridge);
                        }
                    }
                }
                // ���� ��ó�� ������
                // �絿�̸� ���� �ִٸ�
                if (playerHand.holdState == PlayerItemDown.Hold.Pail)
                {
                    currentTime += Time.deltaTime;
                    // ���� �ð� �� �絿�̿� ���� ä������
                    if (currentTime > waterTime)
                    {
                        water.SetActive(true);
                        Water(true);
                    }
                }
            }
            else
            {
                isBranch = false;   
            }
            // ���� ���� ������ ö�̶���
            if (item)
            {
                item.isGathering = true;
                if (playerHand.holdState == PlayerItemDown.Hold.Ax)
                {
                    item.isAx = true;
                    item.isPick = false;
                }
                else if (playerHand.holdState == PlayerItemDown.Hold.Pick)
                {
                    item.isPick = true;
                    item.isAx = false;
                }
                else
                {
                    item.isAx = false;
                    item.isPick = false;
                }

            }

            if(playerHand.holdState == PlayerItemDown.Hold.Mat || rayInfo.transform.gameObject.layer != 11)
            {
                anim.AnimState(PlayerAnim.Anim.Idle);
            }
            if (rayInfo.transform.gameObject.layer == 11 && playerHand.holdState != PlayerItemDown.Hold.Mat)
            {
                anim.AnimState(PlayerAnim.Anim.Gather);
            }
            if (animal)
            {

                if (Input.GetButtonDown("Jump"))
                {
                    if (playerHand.holdState == PlayerItemDown.Hold.Idle)
                    {
                        isItemDown = true;
                        animal.animalState = Animal.Animals.Stop;
                        playerHand.holdState = PlayerItemDown.Hold.Animal;
                        animal.gameObject.transform.parent = iPos.transform;
                        animal.gameObject.transform.localPosition = new Vector3(0, 0.5f, 0.6f);
                    }

                }
                if (playerHand.holdState == PlayerItemDown.Hold.Ax || playerHand.holdState == PlayerItemDown.Hold.Pick)
                {
                    animal.Damage();
                }
            }
        }
        else
        {
            if(playerHand.holdState != PlayerItemDown.Hold.Mat)
                anim.AnimState(PlayerAnim.Anim.Move);
            else
                anim.AnimState(PlayerAnim.Anim.Idle);
        }

    }

    public void Water(bool s)
    {
        photonView.RPC("RPCWater", RpcTarget.All, s);
    }

    [PunRPC]
    void RPCWater(bool s)
    {
        isWater = s;
    }

}
