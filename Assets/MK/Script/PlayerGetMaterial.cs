using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����Ǵ� Material ���� Ȯ�� ��, �ݰų� ��������
public class PlayerGetMaterial : MonoBehaviour
{
    // �����Ǵ� Material ���� Ȯ���� GameObject �迭
    public GameObject[] branch;
    public GameObject[] steel;

    // �����Ǵ� Material�� true false Ȯ�� ��
    public bool[] isBranch;
    public bool[] isSteel;

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Branch(Clone)"))
        {
            CheckTrue("Branch", branch, isBranch, 0);
        }
        if (GameObject.Find("Steel(Clone)"))
        {
            CheckTrue("Steel", steel, isSteel, 1);
        }
    }

    void CheckTrue(string name, GameObject[] ingredient, bool[] isIngredient, int n)
    {
        // ������Ʈ ���� Ȯ��
        ingredient = GameObject.FindGameObjectsWithTag(name);
        for (int i = 0; i < ingredient.Length; i++)
        {
            isIngredient = new bool[ingredient.Length];
            isIngredient[i] = ingredient[i].GetComponent<Material>().isIngredient[n];
        }

    }
}
