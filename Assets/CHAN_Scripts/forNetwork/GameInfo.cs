using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
    public static GameInfo instance;
    private void Awake()
    {
        instance = this;
        curCharacter = charaters[0];
        DontDestroyOnLoad(gameObject);
    }
    // ���� �̵��ӵ�
    public float trainSpeed;
    public float distance;

    // ĳ���� ����
    public GameObject[] charaters;
    public GameObject curCharacter;
    public int CharacterCount=0;
    public Vector3 curPos;
}

