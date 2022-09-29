using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreatedInfo
{
    //만들어진 게임 오브젝트
    public GameObject go;
    //선택된 오브젝트의 idx
    //여기서 idx는 해당오브젝트의 인덱스이다.
    public int idx;
}
// 블럭의 정보들을 JSON형식으로 저장한다.
//[Serializable]
//public class SaveJsonInfo
//{
//    public int idx;
//    public Vector3 pos;
//    public Vector3 eulerAngle;
//    public Vector3 localScale;
//}
//[Serializable]
//// JSON List 파일 key값 할당 위한 클래스
//public class ArrayJson
//{
//    public List<SaveJsonInfo> datas;
//}

public class MapInformation : MonoBehaviour
{
    // 맵의 가로 크기
    public int tileX=80;
    // 맵으 세로 크기
    public int tileZ=20;
    //바닥 Prefab
    public GameObject defBlock;
    //파란색 큐브 prefab
    //public GameObject blueCube;

    //생성하고싶은 게임오브젝트를 담을 변수를 만들자
    public GameObject[] objs;
    //선택한 오브젝트 index
    public int selectObjIdx;

    //맵 오브젝트 정보 담을 리스트
    public List<CreatedInfo> MapObjects = new List<CreatedInfo>();
    //도구 오브젝트 정보 담을 리스트
    public List<CreatedInfo> ToolObjects = new List<CreatedInfo>();
    //그외 기타 오브젝트 정보 담을 리스트
    public List<CreatedInfo> SpecialObjects = new List<CreatedInfo>();


}
