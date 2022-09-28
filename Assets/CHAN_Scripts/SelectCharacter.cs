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
        //����  G�� H������ ĳ���Ͱ� ����ǵ��ϸ��� ���̴�.
        //H������ �迭�� ���������� ĳ���� ����
        //G ������ �迭�� �������� ĳ���� ����
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
        //ĳ���� �迭�� �ε����� ���Ѵ�.
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
        //�÷��̾��� ������Ʈ�� ��ü�Ѵ�.
        info.curPos = transform.position;
        GameObject player =Instantiate(info.curCharacter);
        player.transform.position = info.curPos;
        Destroy(parent);
    }
}
