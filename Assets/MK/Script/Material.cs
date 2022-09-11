using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 선로 재료로 사용되는 나무 or 철과 플레이어 간의 교류
public class Material : MonoBehaviour
{
    // 플레이어가 인식되면 true값을 반환
    public bool[] isIngredient = new bool[2] { false, false};

    // 플레이어 찾기
    PlayerGetItem player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerGetItem>();
    }
    // trigger에 플레이어 인식
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            if (gameObject.name.Contains("Branch"))
            {
                isIngredient[0] = true;
                if(player.curArm > 1)
                {
                    player.curArm = 0;
                    isIngredient[0] = false;
                    Destroy(gameObject);
                }
            }
            if (gameObject.name.Contains("Steel"))
            {
                isIngredient[1] = true;
                if (player.curArm > 1)
                {
                    player.curArm = 0;
                    isIngredient[1] = false;
                    Destroy(gameObject);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            for(int i = 0; i<isIngredient.Length;i++)
            {
                isIngredient[i] = false;
            }
        }
    }
    private void OnDestroy()
    {
        isIngredient[0] = false;
        isIngredient[1] = false;    
    }
}
