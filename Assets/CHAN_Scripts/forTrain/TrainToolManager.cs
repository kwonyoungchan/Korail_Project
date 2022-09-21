using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainToolManager : MonoBehaviour
{
    // 이것은 기차 아이템을 총괄하는 부모 클래스 이다. 
    // 여기서 각 아이템의 속성을 관리하고
    // 화재
    // 소화
    // 아이템 업그레이드를 제어할 것이다. 
    //속성: 아이템 레벨, 업그레이드 위한 아이템 포인트, 업그레이드 포인트, 재료 보관갯수, 선로 보관 갯수, 물 저장용량
    public static int UpgradePoint;

    GameObject fireEffect;

    float waterVolume;
    public float maxWaterVolume;
    public float drainSpeed;
    protected bool isFire;

    void Start()
    {
        //초기에 물 용량을 최대 물 용량으로 설정한다. 
        waterVolume = maxWaterVolume;
    }

    // Update is called once per frame
    void Update()
    {
        //여기서 기차 화재 발생과 관련된 함수가 실행이 된다.
        //시간마다 물의 용량이 감소한다.
        waterVolume -= drainSpeed;
        if (waterVolume < 0)
        {
            // 물 용량이 0이하가 되면 
            // 화재발생이 시작된다.
            isFire = true;
        }
        else if (waterVolume > 0)
        {
            isFire = false;
        }

    }
    // 아이템 레벨 업
    public virtual void ToolLevelUP()
    {
       
     }
    // 화재 발생
    public void FireFire(Transform pos)
    {
        // 이 함수는 각 기차 파츠에 불을 내는 함수이다. 
        // 불이 나면 
        // 불을 활성화 한다.
        GameObject fireParticle = Instantiate(fireEffect, pos); 
        // 이펙트의 위치를 스크립트를 가지고 있는 오브젝트로 한다. 
        fireParticle.transform.position = pos.position;

    }

    public void FireOff()
    { 
        //이 함수는 불을 끄는 함수이다.
        // 불을 끄는 particle 이 있는 오브젝트 의 위치를 찾아서 모두 없애도록 만든다.
        //속성: 불 오브젝트가 있는 transform, 
    }
}
