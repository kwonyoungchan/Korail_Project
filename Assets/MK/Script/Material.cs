using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� ���Ǵ� ���� or ö�� �÷��̾� ���� ����
public class Material : MonoBehaviour
{
    // �÷��̾ �νĵǸ� true���� ��ȯ
    public bool[] isIngredient = new bool[2] { false, false};

    // �÷��̾� ã��
    PlayerGetItem player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerGetItem>();
    }
    // trigger�� �÷��̾� �ν�
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            if (gameObject.name.Contains("Branch"))
            {
                isIngredient[0] = true;
                if(player.curArm > 0)
                {
                    player.curArm = 0;
                    isIngredient[0] = false;
                    Destroy(gameObject);
                }
            }
            if (gameObject.name.Contains("Steel"))
            {
                isIngredient[1] = true;
                if (player.curArm > 0)
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
