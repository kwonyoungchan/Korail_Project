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
        //����  G�� H������ ĳ���Ͱ� ����ǵ��ϸ��� ���̴�.
        //H������ �迭�� ���������� ĳ���� ����
        //G ������ �迭�� �������� ĳ���� ����
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
        //ĳ���� �迭�� �ε����� ���Ѵ�.
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
        //�÷��̾��� ������Ʈ�� ��ü�Ѵ�.
        info.curPos = transform.position;
        Destroy(gameObject);
        GameObject player =Instantiate(info.curCharacter);
        player.transform.position = info.curPos;
    }
}
