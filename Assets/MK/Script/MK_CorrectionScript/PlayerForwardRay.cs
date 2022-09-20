using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 보는 방향으로 레이쏘기 => 레이로 재료와 강 collider판 
public class PlayerForwardRay : MonoBehaviour
{
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
        // 플레이어가 앞으로 레이를 쏜다
        Ray playerRay = new Ray(transform.position, transform.forward);
        RaycastHit rayInfo;
        // 만약 맞은 물체가 있다면
        if (Physics.Raycast(playerRay, out rayInfo, 1f))
        {
            riverGOD = rayInfo.transform.GetComponentInParent<RiverGOD>();
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
                        riverGOD.riverState = RiverGOD.River.Bridge;
                    }
                }
            }
            // 물가 근처에 있을때
            // 양동이를 들고 있다면
            if (playerHand.holdState == PlayerItemDown.Hold.Pail)
            {
                currentTime += Time.deltaTime;
                // 일정 시간 후 양동이에 물이 채워진다
                if(currentTime > waterTime)
                {
                    water.SetActive(true);
                    isWater = true;
                }
            }
        }
    }
}
