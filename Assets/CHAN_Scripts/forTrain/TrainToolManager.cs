using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainToolManager : MonoBehaviour
{
    // �̰��� ���� �������� �Ѱ��ϴ� �θ� Ŭ���� �̴�. 
    // ���⼭ �� �������� �Ӽ��� �����ϰ�
    // ȭ��
    // ��ȭ
    // ������ ���׷��̵带 ������ ���̴�. 
    //�Ӽ�: ������ ����, ���׷��̵� ���� ������ ����Ʈ, ���׷��̵� ����Ʈ, ��� ��������, ���� ���� ����, �� ����뷮
    public static int UpgradePoint;

    GameObject fireEffect;

    float waterVolume;
    public float maxWaterVolume;
    public float drainSpeed;
    protected bool isFire;

    void Start()
    {
        //�ʱ⿡ �� �뷮�� �ִ� �� �뷮���� �����Ѵ�. 
        waterVolume = maxWaterVolume;
    }

    // Update is called once per frame
    void Update()
    {
        //���⼭ ���� ȭ�� �߻��� ���õ� �Լ��� ������ �ȴ�.
        //�ð����� ���� �뷮�� �����Ѵ�.
        waterVolume -= drainSpeed;
        if (waterVolume < 0)
        {
            // �� �뷮�� 0���ϰ� �Ǹ� 
            // ȭ��߻��� ���۵ȴ�.
            isFire = true;
        }
        else if (waterVolume > 0)
        {
            isFire = false;
        }

    }
    // ������ ���� ��
    public virtual void ToolLevelUP()
    {
       
     }
    // ȭ�� �߻�
    public void FireFire(Transform pos)
    {
        // �� �Լ��� �� ���� ������ ���� ���� �Լ��̴�. 
        // ���� ���� 
        // ���� Ȱ��ȭ �Ѵ�.
        GameObject fireParticle = Instantiate(fireEffect, pos); 
        // ����Ʈ�� ��ġ�� ��ũ��Ʈ�� ������ �ִ� ������Ʈ�� �Ѵ�. 
        fireParticle.transform.position = pos.position;

    }

    public void FireOff()
    { 
        //�� �Լ��� ���� ���� �Լ��̴�.
        // ���� ���� particle �� �ִ� ������Ʈ �� ��ġ�� ã�Ƽ� ��� ���ֵ��� �����.
        //�Ӽ�: �� ������Ʈ�� �ִ� transform, 
    }
}
