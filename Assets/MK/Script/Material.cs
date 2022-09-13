using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� ���Ǵ� ���� or ö�� �÷��̾� ���� ����
public class Material : MonoBehaviour
{
    // �÷��̾ �νĵǸ� true���� ��ȯ
    public bool[] isIngredient = new bool[2] { false, false};

    // �÷��̾� ã��
    PlayerItemDown player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerItemDown>();
    }
    // trigger�� �÷��̾� �ν�
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            if (gameObject.name.Contains("Branch"))
            {
                isIngredient[0] = true;
                if(player.armState > 0)
                {
                    player.armState = 0;
                    isIngredient[0] = false;
                    
                }
            }
            if (gameObject.name.Contains("Steel"))
            {
                isIngredient[1] = true;
                if (player.armState > 0)
                {
                    player.armState = 0;
                    isIngredient[1] = false;
                   
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            isIngredient[0] = false;
            isIngredient[1] = false;
        }
    }
    private void OnDestroy()
    {
        isIngredient[0] = false;
        isIngredient[1] = false;    
    }
}
