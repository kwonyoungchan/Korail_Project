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
    //�������� Ŭ���� �Ǿ��� �� ȣ��Ǵ� �Լ�
    MapInformation map;
    //map.objs �� �̸��� ���� ����
    string[] mapObjsName;
    string[] toolObjsName;
    string[] specialObjsName;
    //���� ���� �̸�
    string saveFileName;
    //�̰� ������ ���� ����
    float m_Value;
    private void OnEnable()
    {
        map = (MapInformation)target;
        mapObjsName = new string[map.mapObjs.Length];
        toolObjsName = new string[map.toolObjs.Length];
        specialObjsName = new string[map.specialObjs.Length];

        SetObjNames(mapObjsName, map.mapObjs);
        SetObjNames(toolObjsName, map.toolObjs);
        SetObjNames(specialObjsName, map.specialObjs);

        // �ʿ�����Ʈ�� �̸� ����

        //mapObjsName = new string[map.mapObjs.Length];
        //for (int i = 0; i < map.mapObjs.Length; i++)
        //{
        //    mapObjsName[i] = map.mapObjs[i].name;
        //}
        //SetObjNames(toolObjsName, map.ToolObjs);
        //SetObjNames(specialObjsName, map.SpacailObjs);
    }

    //Inspector�� �׸��� �Լ� 
    public override void OnInspectorGUI()
    {

        //map objField
        EditorGUI.ChangeCheckScope check = new EditorGUI.ChangeCheckScope();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("mapObjs")); 
        if (check.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
        //������ ������Ʈ Idx Field
        map.selectMapIdx = EditorGUILayout.Popup("�� Ÿ��", map.selectMapIdx, mapObjsName);
        map.selectToolIdx = EditorGUILayout.Popup("���� ������Ʈ", map.selectToolIdx, toolObjsName);
        map.selectSpecialIdx = EditorGUILayout.Popup("Ư�� ������Ʈ", map.selectSpecialIdx, specialObjsName);


        //�ٴ� Prefab Field 
        // ������ ���� ������Ʈ�� �ν����� â�� �� �ֵ��� �Ѵ�.(������Ʈ â������ ������ �� �ִ�.)
        //map.floor = (GameObject)EditorGUILayout.ObjectField("�ٴ�", map.floor, typeof(GameObject), false);

        //map.blueCube = (GameObject)EditorGUILayout.ObjectField("�Ķ��� ť��", map.blueCube, typeof(GameObject), false);
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("objs"));
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("createdObjects"));
        // ������ �߰�����
        EditorGUILayout.Space(10);
        // �ٴ� ���� ��ư
        if (GUILayout.Button("Ÿ�� �ʱ�ȭ"))
        {
            Initialize();
        }
        //saveFileName = EditorGUILayout.TextField("���������̸�", saveFileName);
        // Json�����ư
        //if (GUILayout.Button("Json ����"))
        //{
        //    SaveJson();
        //}
        //if (GUILayout.Button("Json �ҷ�����"))

        //{
        //    LoadJson();
        //}
        //���࿡ inspector ���� ����Ǿ��ٸ�
        if (GUI.changed)
        {
            // ��ǥ ��� ǥ��(�� �̵���, ����Ƽ �� �� ���� �˾��� �߰�)
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        //
        GUI.Label(new Rect(0, 300, 100, 30), "Rectangle Width");
        m_Value = GUI.HorizontalSlider(new Rect(100, 300, 100, 30), m_Value, 1.0f, 250.0f);
        EditorGUI.DrawRect(new Rect(50, 350, m_Value, 70), Color.green);

    }

    // Scene â�� �׸��� �Լ�
    private void OnSceneGUI()
    {
        //Map�� ���õǾ��� ��. Scene���� �ٸ� ������Ʈ�� Ŭ���ص� ������ ���� �ʰ� �ϱ� 
        int id = GUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(id);
        DrawGrid();
        CreateObject(map.selectMapIdx,EventType.MouseDrag);
        CreateObject(map.selectToolIdx,EventType.MouseDown);
        CreateObject(map.selectSpecialIdx,EventType.MouseDown);
        //DeleteObject();

    }
    //void LoadJson()
    //{
    //    //���࿡ saveFileName �� ���̰� 0�̶��
    //    if (saveFileName.Length <= 0)
    //    {
    //        Debug.LogError("���� �̸��� �Է� �ϼ���.");
    //        return;
    //    }
    //    // ���� �̸��� �Է��ϼ���

    //    //���� �� �����͸� �����Ѵ�.
    //    Initialize();
    //    //mapData.txt�� �ҷ�����
    //    string jsonData = File.ReadAllText(Application.dataPath + "/" + saveFileName + ".txt");
    //    //ArratJson ���·� Json�� ��ȯ
    //    ArrayJson arrayJson = JsonUtility.FromJson<ArrayJson>(jsonData);
    //    for (int i = 0; i < arrayJson.datas.Count; i++)
    //    {
    //        SaveJsonInfo info = arrayJson.datas[i];
    //        LoadObject(info.idx, info.pos, info.eulerAngle, info.localScale);
    //    }
    //    //ArrayJson�� �����͸� ������ ������Ʈ ����

    //}
    //void SaveJson()
    //{
    //    //���࿡ saveFileName �� ���̰� 0�̶��
    //    if (saveFileName.Length <= 0)
    //    {
    //        Debug.LogError("���� �̸��� �Է� �ϼ���.");
    //        return;
    //    }
    //    //map.createdObjects �� �ִ� ������ json���� ��ȯ
    //    //idx, position, eulerAngle, localScale
    //    //map.createdObjects ������ŭ saveJsonInfo�� ����
    //    //ArrayJson �ϳ� �����.
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
    //        //ArrayJson  datas�� �ϳ��� �߰�
    //        arrayJson.datas.Add(info);
    //    }
    //    string jsonData = JsonUtility.ToJson(arrayJson, true);
    //    Debug.Log("json :" + jsonData);
    //    //jsonData�� ���Ϸ� ����
    //    File.WriteAllText(Application.dataPath + "/" + saveFileName + ".txt", jsonData);


    //}
    void DrawGrid()
    {
        Vector3 start;
        Vector3 end;
        for (int i = 0; i < map.tileX+1; i++)
        {
            start = new Vector3(i-0.5f, 0.5f, -0.5f);
            end = new Vector3(i-0.5f, 0.5f, map.tileZ-0.5f);
            Handles.color = Color.blue;
            Handles.DrawLine(start, end);
        }
        for (int i = 0; i < map.tileZ+1; i++)
        {
            start = new Vector3(-0.5f, 0.5f, i-0.5f);
            end = new Vector3(map.tileX-0.5f, 0.5f, i-0.5f);
            Handles.color = Color.blue;
            Handles.DrawLine(start, end);
        }

    }

    void Initialize()
    {
        
        //�� ������Ʈ�� ���� �� ������Ʈ�� �����.
        GameObject empty = GameObject.Find("obj_parent");
        if (empty != null)
        {
            DestroyImmediate(empty);
        }
        // ���ο� ���� ������Ʈ�� �����.
        empty = new GameObject();
        empty.name = "obj_parent";
        // �⺻ �ٴ� ���� (����X���θ�ŭ �ݺ��ؾ� �Ѵ�.)   
        for (int i = 0; i < map.tileZ; i++)
        {
            for (int j = 0; j < map.tileX; j++)
            {
                GameObject floor = (GameObject)PrefabUtility.InstantiatePrefab(map.mapObjs[0], empty.transform);
                floor.transform.position = new Vector3(j, 0, i);
            }
        }
        map.MapObjects.Clear();
    }
    void LoadObject(int idx, Vector3 position, Vector3 eulerAngle, Vector3 localScale)
    {
        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(
    map.mapObjs[idx]);

        // ���� ���� ������Ʈ�� Obj �̸�  ���� ��ü�� ��,�Ʒ� ,���������� 
        //dir.Normalize();
        obj.transform.position = position;
        Debug.Log("created :"+obj.transform.position);
        obj.transform.eulerAngles = eulerAngle;
        obj.transform.localScale = localScale;
        obj.transform.parent = GameObject.Find("obj_parent").transform;
        CreatedInfo info = new CreatedInfo();
        //������� ������Ʈ�� ����Ʈ�� �߰�
        info.go = obj;
        map.MapObjects.Add(info);
    }
    void CreateObject(int idx ,EventType mouseType)
    {
        // ���� ���콺 �̺�Ʈ
        Event e = Event.current;
        //���콺�� �巡�׵ƴٸ�
        if (e.button == 0 && !e.control && e.type == mouseType)
        {
            //���̸� ���� ���콺 �����Ͱ� ��ġ�� ������ ������Ʈ�� ��ġ�Ѵ�.
            
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;
            int layer = 1 << 11;
            if (Physics.Raycast(ray, out hit,900,~layer))
            {
                //Debug.Log("ray :"+hit.transform.position);
                Vector3 p = new Vector3(Mathf.RoundToInt(hit.point.x), 0, Mathf.RoundToInt(hit.point.z));
                if (hit.transform.name != map.mapObjs[idx].transform.name)
                {
                    Vector3 pos = hit.transform.position;
                    pos.y = 0;
                    LoadObject(idx, pos, Vector3.zero, Vector3.one);
                    DestroyImmediate(hit.transform.gameObject);
                }
            }
        }
    }
    //void DeleteObject()
    //{
    //    Event e = Event.current;
    //    //���콺 ���� ��ư�� ������&��Ʈ�� Ű�� ������ ������
    //    if (e.button == 0 && e.control && e.type == EventType.MouseDown)
    //    {
    //        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
    //        //���콺 �����Ϳ��� Ray�� �����
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
    //        //���� Ray�� �߻��ؼ� �ε�ģ ���� �ִٸ�
    //        // �ش� ������Ʈ�� �ı��ϰڴ�.
    //    }

    //}
    void SetObjNames(string[] listName,GameObject[] objs)
    {
        for (int i = 0; i < objs.Length; i++)
        {
            listName[i] = objs[i].name;
        }
    }
    }