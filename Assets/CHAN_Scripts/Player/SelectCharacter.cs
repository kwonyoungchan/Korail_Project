using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    public GameObject parent;
    private void Awake()
    {
        DontDestroyOnLoad(parent);
    }
    void Update()
    {
        //만약  G나 H누르면 캐릭터가 변경되도록만들 것이다.
        //H누르면 배열의 오른쪽으로 캐릭터 변경
        //G 누르면 배열의 왼쪽으로 캐릭터 변경
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "WaitingRoom" || scene.name == "GameScene")
            return;
        else
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                string t = GameInfo.instance.type;
                Change(true, t);
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                string t = GameInfo.instance.type;
                Change(false, t);
            }
        }
    }
    void Change(bool isRight,string type)
    {
        GameInfo info = GameInfo.instance;
        //캐릭터 배열의 인덱스를 구한다.
        if (isRight)
        {
            info.CharacterCount++;
            if (info.CharacterCount > info.charaters_Lobby.Length-1)
            {
                info.CharacterCount = 0;
            }
            if (type == "lobby")
            { info.curCharacter = info.charaters_Lobby[info.CharacterCount];}
            else
            { info.curCharacter = info.charaters_Main[info.CharacterCount];}
            
        }
        else
        {
            info.CharacterCount--;
            if (info.CharacterCount < 0)
            {
                info.CharacterCount = info.charaters_Lobby.Length-1;
            }
            if (type == "lobby")
            { info.curCharacter = info.charaters_Lobby[info.CharacterCount]; }
            else
            { info.curCharacter = info.charaters_Main[info.CharacterCount]; }
        }
        //플레이어의 오브젝트를 교체한다.
        info.curPos = transform.position;
        GameObject player =Instantiate(info.curCharacter);
        player.transform.position = info.curPos;
        Destroy(parent);
    }
}
