using Mechanizer;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;

public class WaveEditor : EditorWindow
{
    private int _prefabIndex;
    private bool _isPainting = false;
    private Wave _target;
    private List<GameObject> _prefabs;
    private List<GameObject> _graphics;
    private Vector2 _scroll;
    [MenuItem("Mechanizer/Wave Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(WaveEditor));
    }

    private void OnEnable()
    {
        // SceneView.duringSceneGui -= this.OnSceneGUI; // Just in case
        _graphics = new List<GameObject>();
        SceneView.duringSceneGui += this.OnSceneGUI;
        RefreshPalette();
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        ClearGraphics();
    }

    private void ClearGraphics()
    {
        foreach (GameObject graphic in _graphics)
        {
            DestroyImmediate(graphic);
        }
    }
    private void GetTarget()
    {
        if(Selection.activeGameObject != null)
        {
            if(Selection.activeGameObject.TryGetComponent(out Wave wave))
            {
                _target = wave;
            }
        }
    }

    private void NewWave()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Waves/wave_base.prefab");
        var scene = EditorSceneManager.GetActiveScene();
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);
        PrefabUtility.UnpackPrefabInstance(instance, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);

        string localPath = "Assets/Waves/wave.prefab";
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
        bool prefabSuccess;
        PrefabUtility.SaveAsPrefabAssetAndConnect(instance, localPath, InteractionMode.UserAction, out prefabSuccess);
        DestroyImmediate(instance);

        var asset = AssetDatabase.LoadAssetAtPath<GameObject>(localPath);
        WaveData waveData = (WaveData)AssetDatabase.LoadAssetAtPath("Assets/Waves/wave_data.asset", typeof(WaveData));
        WaveContainer waveContainer = asset.GetComponentInChildren<WaveContainer>();
        waveData.AddWaveContainer(waveContainer);
        EditorUtility.SetDirty(waveContainer);
        AssetDatabase.OpenAsset(asset);
        AssetDatabase.SaveAssets();
        scene = EditorSceneManager.GetActiveScene();
        var gameObjects = scene.GetRootGameObjects();
        for(int i = 0; i < gameObjects.Length; i++)
        {
            var gameObject = gameObjects[i];
            Wave wave = gameObject.GetComponentInChildren<Wave>();
            if(wave != null)
            {
                _target = wave;
            }
        }
    }

    private void RefreshPalette()
    {
        _prefabs = new List<GameObject>();
        string path = "Assets/Prefabs";
        string[] prefabFiles = Directory.GetFiles(path, "*.prefab", SearchOption.AllDirectories);
        foreach (string prefabFile in prefabFiles)
        {
            var asset = AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject;
            if (!asset.GetComponent<Enemy>())
                continue;
            _prefabs.Add(asset);
        }
    }

    private Vector2 GetSelectedCell()
    {
        Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.z / guiRay.direction.z);

        // Get the corresponding cell on our virtual grid
        Vector2Int cell = new Vector2Int(Mathf.FloorToInt(mousePosition.x), Mathf.FloorToInt(mousePosition.y));
        return cell;
    }
    private void DisplayVisualHelp(Vector2 cellOrigin)
    {
        // Vertices of our square
        Vector3 topLeft = cellOrigin + Vector2.up;
        Vector3 topRight = cellOrigin + Vector2.right + Vector2.up;
        Vector3 bottomLeft = cellOrigin;
        Vector3 bottomRight = cellOrigin + Vector2.right;

        // Rendering
        Handles.color = Color.green;
        Vector3[] lines = { topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft, topLeft };
        Handles.DrawLines(lines);
    }

    private bool InputPaintDown()
    {
        return Event.current.type == EventType.MouseDown && Event.current.button == 0 && _isPainting;
    }

    private bool InputEraseDown()
    {
        return Event.current.type == EventType.MouseDown && Event.current.button == 1 && _isPainting;
    }


    private void HandlePaintInputs(Vector2 cell)
    {
        if (_target == null)
            return;

        if (InputPaintDown())
        {
            Vector2 cellCenter = cell + new Vector2(0.5f, 0.5f);
            GameObject prefab = _prefabs[_prefabIndex];
            var roomArenaSpawn = new Spawn
            {
                position = cellCenter,
                prefab = prefab
            };
            _target.spawns.Add(roomArenaSpawn);
            EditorUtility.SetDirty(_target);
            CreatePreviewGraphics();
        }
        else if (InputEraseDown())
        {
            Vector2 cellCenter = cell + new Vector2(0.5f, 0.5f);
            int index = _target.GetSpawnAtPosition(cellCenter);
            if (index != -1)
            {
                _target.spawns.RemoveAt(index);
                EditorUtility.SetDirty(_target);
                CreatePreviewGraphics();
            }
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (!_isPainting)
            return;

        Vector2 cell = GetSelectedCell();
        HandlePaintInputs(cell);
        DisplayVisualHelp(cell);
        int id = GUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(id);
    }

    private void DrawPalleteGUI()
    {
        List<GUIContent> paletteIcons = new List<GUIContent>();
        foreach (GameObject prefab in _prefabs)
        {
            // Get a preview for the prefab
            Texture2D texture = AssetPreview.GetAssetPreview(prefab);
            Texture2D resizedTexture = TextureUtility.ResizeTexture(texture, 32, 32);
            paletteIcons.Add(new GUIContent(resizedTexture));
        }

        // Display the grid
        GUILayout.BeginHorizontal();
        _scroll = GUILayout.BeginScrollView(_scroll, false, true);
        _prefabIndex = GUILayout.SelectionGrid(_prefabIndex, paletteIcons.ToArray(), 4);
        GUILayout.EndScrollView();
        GUILayout.EndHorizontal();
    }

    private void CreatePreviewGraphics()
    {
        if (_target == null)
            return;

        ClearGraphics();
        foreach (var spawn in _target.spawns)
        {
            CreatePreviewGraphic(spawn);
        }
    }

    private void CreatePreviewGraphic(Spawn spawn)
    {
        var gameObject = new GameObject();
        gameObject.transform.SetParent(_target.transform);
        gameObject.transform.position = spawn.position;
        gameObject.name = spawn.prefab.name;
        var renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = spawn.prefab.GetComponentInChildren<SpriteRenderer>().sprite;
        renderer.sortingOrder = 9999;
        _graphics.Add(gameObject);
    }

    public void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(_isPainting ? "Stop Painting" : "Start Painting"))
        {
            _isPainting = !_isPainting;
            if (_isPainting)
            {
                CreatePreviewGraphics();
            }
            else
            {
                ClearGraphics();
            }
        }
        GUILayout.EndHorizontal();

        GetTarget();
        DrawPalleteGUI();
        if(GUILayout.Button("New Wave"))
        {
            NewWave();
        }
    }
}