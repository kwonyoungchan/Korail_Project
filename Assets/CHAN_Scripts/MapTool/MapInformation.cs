using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreatedInfo
{
    //������� ���� ������Ʈ
    public GameObject go;
    //���õ� ������Ʈ�� idx
    //���⼭ idx�� �ش������Ʈ�� �ε����̴�.
    public int idx;
}
// ���� �������� JSON�������� �����Ѵ�.
//[Serializable]
//public class SaveJsonInfo
//{
//    public int idx;
//    public Vector3 pos;
//    public Vector3 eulerAngle;
//    public Vector3 localScale;
//}
//[Serializable]
//// JSON List ���� key�� �Ҵ� ���� Ŭ����
//public class ArrayJson
//{
//    public List<SaveJsonInfo> datas;
//}

public class MapInformation : MonoBehaviour
{
    // ���� ���� ũ��
    public int tileX=80;
    // ���� ���� ũ��
    public int tileZ=20;
    //�ٴ� Prefab
    public GameObject defBlock;
    //�Ķ��� ť�� prefab
    //public GameObject blueCube;

    //�����ϰ���� ���ӿ�����Ʈ�� ���� ������ ������
    public GameObject[] objs;
    //������ ������Ʈ index
    public int selectObjIdx;

    //�� ������Ʈ ���� ���� ����Ʈ
    public List<CreatedInfo> MapObjects = new List<CreatedInfo>();
    //���� ������Ʈ ���� ���� ����Ʈ
    public List<CreatedInfo> ToolObjects = new List<CreatedInfo>();
    //�׿� ��Ÿ ������Ʈ ���� ���� ����Ʈ
    public List<CreatedInfo> SpecialObjects = new List<CreatedInfo>();


}
