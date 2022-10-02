using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
[Serializable]
public class CreatedInfo
{
    //������� ���� ������Ʈ
    public GameObject go;
    public string name;
    public string fileType;
    
    //���õ� ������Ʈ�� idx
    //���⼭ idx�� �ش������Ʈ�� �ε����̴�.
}
// ���� �������� JSON�������� �����Ѵ�.
[Serializable]
public class SaveJsonInfo
{
    public GameObject go;
    public string name;
    public Vector3 pos;
    public string fileType;
}
//[Serializable]
//// JSON List ���� key�� �Ҵ� ���� Ŭ����
public class ArrayJson
{
    public List<SaveJsonInfo> datas;
}
public class MyWindow : EditorWindow
{
    //��� �޴� ���κп� ��̸����� ���� ��ų���� ǥ�����ִ� �ڵ�
    int tileX;
    int tileZ;
    bool EditMap = false;
    bool EditTool = false;
    bool EditSpecial = false;
    bool initial = false;
    bool startRails = false;
    bool endRail = false;
    bool Save = false;
    bool Load = false;
    int StartRailCount;
    int EndRailCount;
    private int cellSize = 1;
    [SerializeField]
    private int paletteIndex;
    private int toolpPaletteIndex;
    private int specialPalletteIndex;
    [SerializeField]
    List<GameObject> mapPalette = new List<GameObject>();
    List<GameObject> toolPalette = new List<GameObject>();
    List<GameObject> specialPalette = new List<GameObject>();
    [SerializeField]
    private Vector2 mapScrollPos = Vector2.zero;
    private Vector2 toolTcrollPos = Vector2.zero;
    private Vector2 specialPos = Vector2.zero;
    List<CreatedInfo> createdObjects = new List<CreatedInfo>();
    string saveFileName;

    [MenuItem("Window/My Map Editor")]
   
    private static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MyWindow));
    }
    void LoadJson()
    {
        //���࿡ saveFileName �� ���̰� 0�̶��
        if (saveFileName.Length <= 0)
        {
            Debug.LogError("���� �̸��� �Է� �ϼ���.");
            return;
        }
        GameObject empty = GameObject.Find("obj_parent");
        if (empty != null)
        {
            createdObjects.Clear();
            DestroyImmediate(empty);
        }
        empty = new GameObject();
        empty.name = "obj_parent";
        // ���� �̸��� �Է��ϼ���
        //Initialize();
        //���� �� �����͸� �����Ѵ�.
        //mapData.txt�� �ҷ�����
        string jsonData = File.ReadAllText(Application.dataPath + "/" + saveFileName + ".txt");
        //ArratJson ���·� Json�� ��ȯ
        ArrayJson arrayJson = JsonUtility.FromJson<ArrayJson>(jsonData);
        for (int i = 0; i < arrayJson.datas.Count; i++)
        {
            SaveJsonInfo info = arrayJson.datas[i];
            //if (info.name != createdObjects[i].name)
            //{
            //    createdObjects.RemoveAt(i);
            //    DestroyImmediate(createdObjects[i].go);
            //}
            LoadObject(info.fileType,info.name,info.pos);
        }
        //ArrayJson�� �����͸� ������ ������Ʈ ����

    }
    void SaveJson()
    {
        //���࿡ saveFileName �� ���̰� 0�̶��
        if (saveFileName.Length <= 0)
        {
            Debug.LogError("���� �̸��� �Է� �ϼ���.");
            return;
        }
        //map.createdObjects �� �ִ� ������ json���� ��ȯ
        //idx, position, eulerAngle, localScale
        //map.createdObjects ������ŭ saveJsonInfo�� ����
        //ArrayJson �ϳ� �����.
        ArrayJson arrayJson = new ArrayJson();
        arrayJson.datas = new List<SaveJsonInfo>();
        SaveJsonInfo info;
        for (int i = 0; i < createdObjects.Count; i++)
        {

            CreatedInfo createdInfo = createdObjects[i];

            info = new SaveJsonInfo();
            info.go = createdInfo.go;
            info.pos = createdInfo.go.transform.position;
            info.name = createdInfo.name;
            info.fileType = createdInfo.fileType;
            //ArrayJson  datas�� �ϳ��� �߰�
            arrayJson.datas.Add(info);
        }
        string jsonData = JsonUtility.ToJson(arrayJson, true);
        //jsonData�� ���Ϸ� ����
        Debug.Log("json :" + jsonData);
        File.WriteAllText(Application.streamingAssetsPath + "/" + saveFileName + ".txt", jsonData);
    }

    private void OnGUI() 
    {
        
        EditorGUILayout.BeginHorizontal();
        EditMap = GUILayout.Toggle(EditMap, "�� Ÿ��", "Button", GUILayout.Height(60f));
        EditTool = GUILayout.Toggle(EditTool, "���� Ÿ��", "Button", GUILayout.Height(60f));
        EditSpecial = GUILayout.Toggle(EditSpecial, "Ư�� Ÿ��", "Button", GUILayout.Height(60f));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        initial = GUILayout.Toggle(initial, "�� �ʱ�ȭ", "Button", GUILayout.Height(60f));
        startRails = GUILayout.Toggle(startRails, "������", "Button", GUILayout.Height(60f));
        endRail = GUILayout.Toggle(endRail, "������", "Button", GUILayout.Height(60f));
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.BeginVertical();


        // ������ ���� ����
        #region ��, ����Ÿ�� ���� ��ũ�� �޴� ����
        List<GUIContent> mapPaletteIcons = new List<GUIContent>();
        List<GUIContent> toolPaletteIcons = new List<GUIContent>();
        List<GUIContent> specialPaletteIcons = new List<GUIContent>();
        foreach (GameObject prefab in mapPalette)
        {
            // Get a preview for the prefab
            Texture2D texture = AssetPreview.GetAssetPreview(prefab);
            mapPaletteIcons.Add(new GUIContent(texture));
        }

        foreach (GameObject prefab in toolPalette)
        {
            // Get a preview for the prefab
            Texture2D texture = AssetPreview.GetAssetPreview(prefab);
            toolPaletteIcons.Add(new GUIContent(texture));
        }
        foreach (GameObject prefab in specialPalette)
        {
            // Get a preview for the prefab
            Texture2D texture = AssetPreview.GetAssetPreview(prefab);
            specialPaletteIcons.Add(new GUIContent(texture));
        }
        if (EditMap)
        {
            GUILayout.Label("�� Ÿ��");
            mapScrollPos = EditorGUILayout.BeginScrollView(mapScrollPos,true,false);
            paletteIndex = GUILayout.SelectionGrid(paletteIndex, mapPaletteIcons.ToArray(), mapPaletteIcons.Count);
            EditorGUILayout.EndScrollView();
        }
        if (EditTool)
        {
            GUILayout.Label("���� Ÿ��");
            toolTcrollPos = EditorGUILayout.BeginScrollView(toolTcrollPos, true, false);
            toolpPaletteIndex = GUILayout.SelectionGrid(toolpPaletteIndex, toolPaletteIcons.ToArray(), toolPaletteIcons.Count);
            EditorGUILayout.EndScrollView();
        }
        if (EditSpecial)
        {
            GUILayout.Label("��Ÿ Ÿ��");
            specialPos = EditorGUILayout.BeginScrollView(specialPos, true, false);
            specialPalletteIndex = GUILayout.SelectionGrid(specialPalletteIndex, specialPaletteIcons.ToArray(), specialPaletteIcons.Count);
            EditorGUILayout.EndScrollView();
        }
        #endregion;
        Save = GUILayout.Toggle(Save, "���� �ϱ�", "Button",GUILayout.Height(60f));
        saveFileName = GUILayout.TextArea(saveFileName);
        GUILayout.Space(10);
        Load = GUILayout.Toggle(Load, "�ҷ� ����", "Button",GUILayout.Height(60f));
        GUILayout.Space(10);
        EditorGUILayout.EndVertical();


    }



    private void OnSceneGUI(SceneView sceneView)
    {
        sceneView.Repaint();

        if (EditMap)
        {
            DoEdit(mapPalette, "Map",paletteIndex,EventType.MouseDrag);
            EditTool = false;
            EditSpecial = false;
        }
        if (EditTool)
        {
            DoEdit(toolPalette,"Tool", toolpPaletteIndex, EventType.MouseDown);
            EditMap = false;
            EditSpecial = false;
        }
        if (EditSpecial)
        {
            DoEdit(specialPalette, "Special", specialPalletteIndex, EventType.MouseDown);
            EditMap = false;
            EditTool = false;
        }
        if (startRails)
        {
            
            SetStartRail();
        }
        if (endRail)
        {
            SetEndRail();
        }

        //�ѹ��� Ŭ����Ű�°͵�
        if (initial)
        {
            Initialize();
            StartRailCount = 0;
            EndRailCount = 0;
            initial = false;
        }
        if (Save)
        {
            SaveJson();
            Save = false;
        }
        if (Load)
        {
            LoadJson();
            Load = false;
        }
        
    }

    //������â�� ������ �� �߻��ϴ� �ڵ�?
    void OnFocus()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI; // Just in case
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
        RefreshMapPalette(); // Refresh the palette (can be called uselessly, but there is no overhead.)
        RefreshtoolPalette();
        RefreshspecialPalette();
    }

    private void RefreshMapPalette()
    {
        mapPalette.Clear();
        string[] prefabFiles = Directory.GetFiles("Assets/Editor Default Resources/prefab/Map", "*.prefab");
        foreach (string prefabFile in prefabFiles)
            mapPalette.Add(AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject);
    }
    private void RefreshtoolPalette()
    {
        toolPalette.Clear();
        string[] prefabFiles = Directory.GetFiles("Assets/Editor Default Resources/prefab/Tool", "*.prefab");
        foreach (string prefabFile in prefabFiles)
            toolPalette.Add(AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject);
    }
    private void RefreshspecialPalette()
    {
        specialPalette.Clear();
        string[] prefabFiles = Directory.GetFiles("Assets/Editor Default Resources/prefab/Special", "*.prefab");
        foreach (string prefabFile in prefabFiles)
            specialPalette.Add(AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject);
    }
    // �ش� ������ â�� �ݾ��� �� ȣ��Ǵ� �ڵ�
    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }


    private void DisplayVisualHelp(Vector3 cellCenter)
    {
        // Vertices of our square
        Vector3 topLeft = cellCenter + Vector3.left * cellSize * 0.5f + Vector3.forward * cellSize * 0.5f;
        Vector3 topRight = cellCenter - Vector3.left * cellSize * 0.5f + Vector3.forward * cellSize * 0.5f;
        Vector3 bottomLeft = cellCenter + Vector3.left * cellSize * 0.5f - Vector3.forward * cellSize * 0.5f;
        Vector3 bottomRight = cellCenter - Vector3.left * cellSize * 0.5f - Vector3.forward * cellSize * 0.5f;

        // Rendering
        Handles.color = Color.red;
        Vector3[] lines = { topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft, topLeft };
        Handles.DrawLines(lines);
    }
    private void HandleSceneViewInputs(Vector2 cellCenter)
    {
        // Filter the left click so that we can't select objects in the scene
        if (Event.current.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(0); // Consume the event
        }
    }
    void Initialize()
    {
        
        //�� ������Ʈ�� ���� �� ������Ʈ�� �����.
        GameObject empty = GameObject.Find("obj_parent");
        if (empty != null)
        {
            createdObjects.Clear();
            DestroyImmediate(empty);
        }
        // ���ο� ���� ������Ʈ�� �����.
        empty = new GameObject();
        empty.name = "obj_parent";
        tileX = 80;
        tileZ = 20;
        // �⺻ �ٴ� ���� (����X���θ�ŭ �ݺ��ؾ� �Ѵ�.)   

        for (int i = 0; i < tileZ; i++)
        {
            for (int j = 0; j < tileX; j++)
            {
                
                GameObject floor =CreateBaseTile();
                floor.transform.position = new Vector3(j, 0, i);
                CreatedInfo info = new CreatedInfo();
                info.go = floor;
                info.name = floor.transform.name;
                info.fileType = "Map";
                //ArrayJson  datas�� �ϳ��� �߰�
                createdObjects.Add(info);

            }
        }
        

    }
    void DeleteObject()
    {
        Event e = Event.current;
        //���콺 ���� ��ư�� ������&��Ʈ�� Ű�� ������ ������
        if (e.button == 0 && e.control && e.type == EventType.MouseDrag)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            //���콺 �����Ϳ��� Ray�� �����
            RaycastHit hit;
            int layer = 1 << 11;
            if (Physics.Raycast(ray, out hit, 900, ~layer))
            {

                if (hit.transform.name != "MapCube")
                {
                    GameObject floor=CreateBaseTile();
                    floor.transform.position = hit.transform.position;
                    for (int i = 0; i < createdObjects.Count; i++)
                    {

                        if (createdObjects[i].go == hit.transform.gameObject)
                        {
                            createdObjects.RemoveAt(i);
                            DestroyImmediate(hit.transform.gameObject);
                            break;
                        }
                    }
                    
                }

            }
            //���� Ray�� �߻��ؼ� �ε�ģ ���� �ִٸ�
            // �ش� ������Ʈ�� �ı��ϰڴ�.
        }

    }

    void LoadObject(string fileType,string prefabName, Vector3 position)
    {
        GameObject JH = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/prefab/"+ fileType+"\\\\" + prefabName + ".prefab", typeof(GameObject));
        GameObject obj = PrefabUtility.InstantiatePrefab(JH) as GameObject;
        // ���� ���� ������Ʈ�� Obj �̸�  ���� ��ü�� ��,�Ʒ� ,���������� 
        obj.transform.position = position;
        obj.transform.parent = GameObject.Find("obj_parent").transform;
        CreatedInfo info = new CreatedInfo();
        //������� ������Ʈ�� ����Ʈ�� �߰�
        info.go = obj;
        info.name = prefabName;
        info.fileType = fileType;
        createdObjects.Add(info);
    }
    GameObject CreateBaseTile()
    {
        GameObject JH = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/prefab/Map\\\\MapCube.prefab", typeof(GameObject));
        GameObject empty = GameObject.Find("obj_parent");
        GameObject floor = (GameObject)PrefabUtility.InstantiatePrefab(JH, empty.transform);
        //ArrayJson  datas�� �ϳ��� �߰�

        return floor;
    }
    void DoEdit(List<GameObject> pallette, string fileType,int idx,EventType Mtype)
    {
        Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;
        int layer = 1 << 11;
        if (Physics.Raycast(guiRay, out hit, 900, ~layer))
        {
            Vector3 p = new Vector3(Mathf.RoundToInt(hit.point.x), 0.5f, Mathf.RoundToInt(hit.point.z));
            DisplayVisualHelp(p);
            HandleSceneViewInputs(p);

            if (Event.current.type == Mtype && Event.current.button == 0 && !Event.current.control)
            {
                // ���⼭ ���� �ķ�Ʈ���� �� �ķ�Ʈ���� �з��ؾ���
                GameObject prefab = pallette[idx];
                if (hit.transform.name != prefab.transform.name)
                {
                    Vector3 pp = hit.transform.position;
                    if (fileType == "Map")
                    {
                        pp.y = 0;
                    }
                    else
                    {
                        pp.y = 0.5f;
                    }
                    LoadObject(fileType, prefab.transform.name, pp);
                    if (fileType == "Map")
                    {
                        for (int i = 0; i < createdObjects.Count; i++)
                        {

                            if (createdObjects[i].go == hit.transform.gameObject)
                            {
                                createdObjects.RemoveAt(i);
                                DestroyImmediate(hit.transform.gameObject);
                                break;
                            }
                        }
                    }
                    // Allow the use of Undo (Ctrl+Z, Ctrl+Y).
                }
            }
        }
        DeleteObject();
    }
    void SetStartRail()
    {
        Event e = Event.current;

        //���콺 ���� ��ư�� ������&��Ʈ�� Ű�� ������ ������
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        //���콺 �����Ϳ��� Ray�� �����
        RaycastHit hit;
        int layer = 1 << 11;
        if (Physics.Raycast(ray, out hit, 900, ~layer))
        {
            Vector3 p = new Vector3(Mathf.RoundToInt(hit.point.x), 0.5f, Mathf.RoundToInt(hit.point.z));
            DisplayVisualHelp(p);
            HandleSceneViewInputs(p);
            if (e.button == 0 && e.type == EventType.MouseDown )
            {
                if (StartRailCount <= 1)
                {
                    GameObject JH = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/prefab/StationTile\\\\startCube.prefab", typeof(GameObject));
                    GameObject empty = GameObject.Find("obj_parent");
                    GameObject floor = (GameObject)PrefabUtility.InstantiatePrefab(JH, empty.transform);
                    floor.transform.position = hit.transform.position;
                    CreatedInfo info = new CreatedInfo();
                    info.go = floor;
                    info.name = "startCube";
                    info.fileType = "StationTile";
                    createdObjects.Add(info);
                    GameObject hitObj = hit.transform.gameObject;
                    for (int i = 0; i < createdObjects.Count; i++)
                    {
                        if (hitObj == createdObjects[i].go)
                        {
                            for (int j = 0; j < 6; j++)
                            {
                                DestroyImmediate(createdObjects[i - j].go);
                                createdObjects.RemoveAt(i - j);
                            }
                            break;
                        }
                    }
                    StartRailCount++;
                    startRails = false;
                }
                else
                { Debug.LogError("�̹� ������ Ÿ���Դϴ�."); }
            }

            
        }

    }
    void SetEndRail()
    {
        Event e = Event.current;

        //���콺 ���� ��ư�� ������&��Ʈ�� Ű�� ������ ������
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        //���콺 �����Ϳ��� Ray�� �����
        RaycastHit hit;
        int layer = 1 << 11;
        if (Physics.Raycast(ray, out hit, 900, ~layer))
        {
            Vector3 p = new Vector3(Mathf.RoundToInt(hit.point.x), 0.5f, Mathf.RoundToInt(hit.point.z));
            DisplayVisualHelp(p);
            HandleSceneViewInputs(p);
            if (e.button == 0 && e.type == EventType.MouseDown)
            {
                if (EndRailCount <= 1)
                {
                    GameObject JH = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/prefab/StationTile\\\\endCube.prefab", typeof(GameObject));
                    GameObject empty = GameObject.Find("obj_parent");
                    GameObject floor = (GameObject)PrefabUtility.InstantiatePrefab(JH, empty.transform);
                    floor.transform.position = hit.transform.position;
                    CreatedInfo info = new CreatedInfo();
                    info.go = floor;
                    info.name = "endCube";
                    info.fileType = "StationTile";
                    createdObjects.Add(info);
                    GameObject hitObj = hit.transform.gameObject;
                    for (int i = 0; i < createdObjects.Count; i++)
                    {
                        if (hitObj == createdObjects[i].go)
                        {
                            DestroyImmediate(createdObjects[i].go);
                            createdObjects.RemoveAt(i);
                            break;
                        }
                    }
                    EndRailCount++;
                    endRail = false;
                }
                else
                { Debug.LogError("�̹� ������ Ÿ���Դϴ�."); }
            }


        }
    }
}
