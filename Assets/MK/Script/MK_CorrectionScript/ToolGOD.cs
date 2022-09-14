using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 명령에 따라 cube(맵)의 상태를 전환함
public class ToolGOD : MonoBehaviour
{
    // 도구 상태 enum
    public enum Tools
    {
        Idle,
        Ax,
        Pick,
        Pail,
        
    }
    public Tools toolsState;

    // 생성되는 y 위치
    public float y = 0.5f;

    // 아이템 생성 확인
    GameObject toolItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ToolFSM();
    }
    
    // 재료에 따른 상태머신
    void ToolFSM()
    {
        switch (toolsState)
        {
            case Tools.Idle:
                if (toolItem != null)
                {
                    Destroy(toolItem);
                    break;
                }
                else break;
            case Tools.Ax:
                if(toolItem)
                {
                    if (toolItem.name.Contains("Ax"))
                    {
                        return;
                    }
                    else
                    {
                        Destroy(toolItem);
                        return;
                    }
                }
                toolItem = Instantiate(Resources.Load<GameObject>("MK_Prefab/Ax"));
                toolItem.transform.position = transform.position + new Vector3(0, y, 0);
                break;
            case Tools.Pick:
                if (toolItem)
                {
                    if (toolItem.name.Contains("Pick"))
                    {
                        return;
                    }
                    else
                    {
                        Destroy(toolItem);
                        return;
                    }
                }
                toolItem = Instantiate(Resources.Load<GameObject>("MK_Prefab/Pick"));
                toolItem.transform.position = transform.position + new Vector3(0, y, 0);
                break;
            case Tools.Pail:
                if (toolItem)
                {
                    if (toolItem.name.Contains("Pail"))
                    {
                        return;
                    }
                    else
                    {
                        Destroy(toolItem);
                        return;
                    }
                }
                toolItem = Instantiate(Resources.Load<GameObject>("MK_Prefab/Pail"));
                toolItem.transform.position = transform.position + new Vector3(0, y, 0);
                break;
        }
    }

}
