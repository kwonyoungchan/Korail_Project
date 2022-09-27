using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        //만약  G나 H누르면 캐릭터가 변경되도록만들 것이다.
        //H누르면 배열의 오른쪽으로 캐릭터 변경
        //G 누르면 배열의 왼쪽으로 캐릭터 변경
        if (Input.GetKeyDown(KeyCode.H))
        {
            Change(true);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            Change(false);
        }

    }
    void Change(bool isRight)
    {
        GameInfo info = GameInfo.instance;
        //캐릭터 배열의 인덱스를 구한다.
        if (isRight)
        {
            info.CharacterCount++;
            if (info.CharacterCount > info.charaters.Length-1)
            {
                info.CharacterCount = 0;
            }
            info.curCharacter = info.charaters[info.CharacterCount];
        }
        else
        {
            info.CharacterCount--;
            if (info.CharacterCount < 0)
            {
                info.CharacterCount = info.charaters.Length-1;
            }
            
            info.curCharacter = info.charaters[info.CharacterCount];
        }
        //플레이어의 오브젝트를 교체한다.
        info.curPos = transform.position;
        Destroy(gameObject);
        GameObject player =Instantiate(info.curCharacter);
        player.transform.position = info.curPos;
    }
}
