using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

[CustomEditor(typeof(MapInformation))]
[CanEditMultipleObjects]
public class MapEditor : Editor
{
    //계층에서 클릭이 되었을 때 호출되는 함수
    MapInformation map;
    //map.objs 의 이름을 담을 변수
    string[] objsName;
    //저장 파일 이름
    string saveFileName;
    //이건 시험삼아 만들어본 변수
    float m_Value;
    private void OnEnable()
    {
        map = (MapInformation)target;
        objsName = new string[map.objs.Length];
        // 오브젝트들 이름 셋팅
        for (int i = 0; i < map.objs.Length; i++)
        {
            objsName[i] = map.objs[i].name;
        }
    }

    //Inspector를 그리는 함수 
    public override void OnInspectorGUI()
    {


        //map objField
        EditorGUI.ChangeCheckScope check = new EditorGUI.ChangeCheckScope();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("objs"));
        if (check.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
        //선택한 오브젝트 Idx Field
        map.selectObjIdx = EditorGUILayout.Popup("선택 오브젝트", map.selectObjIdx, objsName);
        //바닥 Prefab Field 
        // 계층의 게임 오브젝트를 인스펙터 창에 못 넣도록 한다.(프로젝트 창에서는 가져올 수 있다.)
        //map.floor = (GameObject)EditorGUILayout.ObjectField("바닥", map.floor, typeof(GameObject), false);

        //map.blueCube = (GameObject)EditorGUILayout.ObjectField("파란색 큐브", map.blueCube, typeof(GameObject), false);
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("objs"));
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("createdObjects"));
        // 공간을 추가하자
        EditorGUILayout.Space();
        // 바닥 생성 버튼
        if (GUILayout.Button("타일 초기화"))
        {
            Initialize();
        }
        //saveFileName = EditorGUILayout.TextField("저장파일이름", saveFileName);
        // Json저장버튼
        //if (GUILayout.Button("Json 저장"))
        //{
        //    SaveJson();
        //}
        //if (GUILayout.Button("Json 불러오기"))

        //{
        //    LoadJson();
        //}
        //만약에 inspector 값이 변경되었다면
        if (GUI.changed)
        {
            // 별표 모양 표시(씬 이동시, 유니티 끌 때 저장 팝업이 뜨게)
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        //
        GUI.Label(new Rect(0, 300, 100, 30), "Rectangle Width");
        m_Value = GUI.HorizontalSlider(new Rect(100, 300, 100, 30), m_Value, 1.0f, 250.0f);
        EditorGUI.DrawRect(new Rect(50, 350, m_Value, 70), Color.green);

    }

    // Scene 창을 그리는 함수
    private void OnSceneGUI()
    {
        //Map이 선택되었을 때. Scene에서 다른 오브젝트를 클릭해도 선택이 되지 않게 하기 
        int id = GUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(id);
        DrawGrid();
        CreateObject();
        //DeleteObject();

    }
    //void LoadJson()
    //{
    //    //만약에 saveFileName 의 길이가 0이라면
    //    if (saveFileName.Length <= 0)
    //    {
    //        Debug.LogError("파일 이름을 입력 하세요.");
    //        return;
    //    }
    //    // 파일 이를을 입력하세요

    //    //이전 맵 데이터를 삭제한다.
    //    Initialize();
    //    //mapData.txt를 불러오기
    //    string jsonData = File.ReadAllText(Application.dataPath + "/" + saveFileName + ".txt");
    //    //ArratJson 형태로 Json을 변환
    //    ArrayJson arrayJson = JsonUtility.FromJson<ArrayJson>(jsonData);
    //    for (int i = 0; i < arrayJson.datas.Count; i++)
    //    {
    //        SaveJsonInfo info = arrayJson.datas[i];
    //        LoadObject(info.idx, info.pos, info.eulerAngle, info.localScale);
    //    }
    //    //ArrayJson의 데이터를 가지고 오브젝트 생성

    //}
    //void SaveJson()
    //{
    //    //만약에 saveFileName 의 길이가 0이라면
    //    if (saveFileName.Length <= 0)
    //    {
    //        Debug.LogError("파일 이름을 입력 하세요.");
    //        return;
    //    }
    //    //map.createdObjects 에 있는 정보를 json으로 변환
    //    //idx, position, eulerAngle, localScale
    //    //map.createdObjects 갯수만큼 saveJsonInfo를 셋팅
    //    //ArrayJson 하나 만든다.
    //    ArrayJson arrayJson = new ArrayJson();
    //    arrayJson.datas = new List<SaveJsonInfo>();
    //    SaveJsonInfo info;
    //    for (int i = 0; i < map.createdObjects.Count; i++)
    //    {

    //        CreatedInfo createdInfo = map.createdObjects[i];

    //        info = new SaveJsonInfo();
    //        info.idx = createdInfo.idx;
    //        info.pos = createdInfo.go.transform.position;
    //        info.eulerAngle = createdInfo.go.transform.eulerAngles;
    //        info.localScale = createdInfo.go.transform.localScale;
    //        //ArrayJson  datas에 하나씩 추가
    //        arrayJson.datas.Add(info);
    //    }
    //    string jsonData = JsonUtility.ToJson(arrayJson, true);
    //    Debug.Log("json :" + jsonData);
    //    //jsonData를 파일로 저장
    //    File.WriteAllText(Application.dataPath + "/" + saveFileName + ".txt", jsonData);


    //}
    void DrawGrid()
    {
        Vector3 start;
        Vector3 end;
        for (int i = 0; i < map.tileX+1; i++)
        {
            start = new Vector3(i-0.5f, 0, -0.5f);
            end = new Vector3(i-0.5f, 0, map.tileZ-0.5f);
            Handles.color = Color.red;
            Handles.DrawLine(start, end);
        }
        for (int i = 0; i < map.tileZ+1; i++)
        {
            start = new Vector3(-0.5f, 0, i-0.5f);
            end = new Vector3(map.tileX-0.5f, 0, i-0.5f);
            Handles.color = Color.blue;
            Handles.DrawLine(start, end);
        }

    }

    void Initialize()
    {
        
        //땅 오브젝트를 넣을 빈 오브젝트를 만든다.
        GameObject empty = GameObject.Find("obj_parent");
        if (empty != null)
        {
            DestroyImmediate(empty);
        }
        // 새로운 게임 오브젝트를 만든다.
        empty = new GameObject();
        empty.name = "obj_parent";
        // 기본 바닥 생성 (가로X세로만큼 반복해야 한다.)   
        for (int i = 0; i < map.tileZ; i++)
        {
            for (int j = 0; j < map.tileX; j++)
            {
                GameObject floor = (GameObject)PrefabUtility.InstantiatePrefab(map.objs[0], empty.transform);
                floor.transform.position = new Vector3(j, 0, i);
            }
        }
        map.MapObjects.Clear();
    }
    void LoadObject(int idx, Vector3 position, Vector3 eulerAngle, Vector3 localScale)
    {
        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(
    map.objs[idx]);

        // 만약 맞은 오브젝트가 Obj 이면  맞은 객체의 위,아래 ,오른쪽으로 
        //dir.Normalize();
        obj.transform.position = position;
        Debug.Log("created :"+obj.transform.position);
        obj.transform.eulerAngles = eulerAngle;
        obj.transform.localScale = localScale;
        obj.transform.parent = GameObject.Find("obj_parent").transform;
        CreatedInfo info = new CreatedInfo();
        //만들어진 오브젝트를 리스트에 추가
        info.go = obj;
        info.idx = idx;
        map.MapObjects.Add(info);
    }
    void CreateObject()
    {
        // 현재 마우스 이벤트
        Event e = Event.current;
        //마우스가 드래그됐다면
        if (e.button == 0 && !e.control && e.type == EventType.MouseDrag)
        {
            //레이를 쏴서 마우스 포인터가 위치한 지점에 오브젝트를 설치한다.
            
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;
            int layer = 1 << 11;
            if (Physics.Raycast(ray, out hit,900,~layer))
            {
                Debug.Log("ray :"+hit.transform.position);
                Vector3 p = new Vector3(Mathf.RoundToInt(hit.point.x), 0, Mathf.RoundToInt(hit.point.z));
                if (hit.transform.name != map.objs[map.selectObjIdx].transform.name)
                {
                    Vector3 pos = hit.transform.position;
                    pos.y = 0;
                    LoadObject(map.selectObjIdx, pos, Vector3.zero, Vector3.one);
                    DestroyImmediate(hit.transform.gameObject);
                }
            }
        }
    }
        //void DeleteObject()
        //{
        //    Event e = Event.current;
        //    //마우스 왼쪽 버튼을 누리면&컨트롤 키를 누르고 있으면
        //    if (e.button == 0 && e.control && e.type == EventType.MouseDown)
        //    {
        //        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        //        //마우스 포인터에서 Ray를 만들고
        //        RaycastHit hit;
        //        if (Physics.Raycast(ray, out hit))
        //        {
        //            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Obj"))
        //            {
        //                for (int i = 0; i < map.createdObjects.Count; i++)
        //                {

        //                    if (map.createdObjects[i].go == hit.transform.gameObject)
        //                    {
        //                        map.createdObjects.RemoveAt(i);
        //                        break;
        //                    }
        //                }
        //                DestroyImmediate(hit.transform.gameObject);
        //            }

        //        }
        //        //만든 Ray를 발사해서 부딪친 놈이 있다면
        //        // 해당 오브젝트를 파괴하겠다.
        //    }

        //}
    }