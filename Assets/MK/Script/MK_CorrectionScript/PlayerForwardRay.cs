using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 플레이어가 보는 방향으로 레이쏘기 => 레이로 재료와 강 collider판 
// 플레이어가 보는 방향에 나무나 철이 있다면 캐기
public class PlayerForwardRay : MonoBehaviourPun //, IPunObservable
{
    // 레이 위치
    public GameObject rPos;
    public GameObject iPos;
    public bool isBranch;
    // 플레이어 재료
    PlayerMaterial player;
    // 플레이어 아이템 상태
    PlayerItemDown playerHand;
    RiverGOD riverGOD; 
    // 시간
    float currentTime;
    public float waterTime = 4;
    // 물채우기
    public GameObject water;
    Animal animal;

    public bool isWater = false;
    public bool isItemDown = false;


    // 채집을 위한 변수들
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
        if (playerHand.holdState == PlayerItemDown.Hold.Animal)
        {
            if (Input.GetButtonDown("Jump"))
            {
                isItemDown = false;
                iPos.transform.GetChild(1).GetComponent<Animal>().animalState = Animal.Animals.Idle;
                playerHand.holdState = PlayerItemDown.Hold.ChangeIdle;
                iPos.transform.GetChild(1).gameObject.transform.position = iPos.transform.position + new Vector3(0, 0.5f, 0.8f);
                iPos.transform.GetChild(1).gameObject.transform.parent = null;
            }
        }
        // 플레이어가 앞으로 레이를 쏜다
        Ray playerRay = new Ray(rPos.transform.position, transform.forward);
        RaycastHit rayInfo;
        // 만약 맞은 물체가 있다면
        if (Physics.Raycast(playerRay, out rayInfo, 1.5f))
        {
            Debug.DrawRay(rPos.transform.position, transform.forward, Color.blue);
            // 맞은 곳이 강
            riverGOD = rayInfo.transform.GetComponentInParent<RiverGOD>();
            // 맞은 곳이 재료 수집하는 곳
            item = rayInfo.transform.GetComponentInParent<IngredientItem>();
            animal = rayInfo.transform.GetComponent<Animal>();
            if (riverGOD)
            {
                // 플레이어의 손에 나무가 있을때
                if (player.branchArray.Count > 0)
                {
                    // 점프키를 누른다면
                    if (Input.GetButtonDown("Jump"))
                    {
                        if (rayInfo.transform.gameObject.layer == 8)
                        {
                            isBranch = false;
                            player.RemoveBranch();
                            // bridge로 바뀜
                            riverGOD.ChangeRiver(RiverGOD.River.Bridge);
                        }
                    }
                }
                // 물가 근처에 있을때
                // 양동이를 들고 있다면
                if (playerHand.holdState == PlayerItemDown.Hold.Pail)
                {
                    currentTime += Time.deltaTime;
                    // 일정 시간 후 양동이에 물이 채워진다
                    if (currentTime > waterTime)
                    {
                        water.SetActive(true);
                        Water(true);
                    }
                }
            }
            // 맞은 곳이 나무나 철이라면
            if (item)
            {
                isGathering = true;
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

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    throw new System.NotImplementedException();
    //}
}
