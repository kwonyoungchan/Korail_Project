using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInfo : MonoBehaviour
{
    public static GameInfo instance;
    private void Awake()
    {
        instance = this;
        curCharacter = charaters_Lobby[0];
        type = "lobby";
        DontDestroyOnLoad(gameObject);
    }
    // 기차 이동속도
    public float trainSpeed;
    public float distance;

    // 캐릭터 정보
    public GameObject[] charaters_Lobby;
    public GameObject[] charaters_Main;
    public GameObject curCharacter;
    public int CharacterCount=0;
    public Vector3 curPos;
    public string type;
    public string nickName;

    public string CallBackName()
    {
        type = SceneManager.GetActiveScene().name == "WaitingRoom" ? "main" : "lobby";
        if (type == "main")
        {
            curCharacter = charaters_Main[CharacterCount];
        }
        else if(type == "lobby")
        {
            curCharacter = charaters_Lobby[CharacterCount];
        }
        string s = curCharacter.ToString();
        int idx = s.IndexOf(" ");
        s = s.Substring(0, idx);
        return s;
    }
}

