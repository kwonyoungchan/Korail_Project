using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 생성되는 Material 개수 확인 후, 줍거나 내려놓기
public class PlayerGetMaterial : MonoBehaviour
{
    // 생성되는 Material 개수 확인할 GameObject 배열
    public GameObject[] branch;
    public GameObject[] steel;

    // 생성되는 Material의 true false 확인 용
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
        // 오브젝트 개수 확인
        ingredient = GameObject.FindGameObjectsWithTag(name);
        for (int i = 0; i < ingredient.Length; i++)
        {
            isIngredient = new bool[ingredient.Length];
            isIngredient[i] = ingredient[i].GetComponent<Material>().isIngredient[n];
        }

    }
}
