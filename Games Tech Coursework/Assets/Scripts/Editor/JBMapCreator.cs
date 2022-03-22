using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[Serializable]
[CustomEditor(typeof(JBMapCreator))]
public class JBMapCreator : EditorWindow
{
    private UnityEngine.Object[] mapObjects;
    public List<MapFiller> instances = new List<MapFiller>();
    private Dictionary<Color, GameObject> MapTileDic;
    public Texture2D map;
    public float spaceBetweenTiles = 1;
    ScriptableObject target;
    SerializedObject so;
    SerializedProperty mapProperties;
    bool isGameplay;
    GameObject mapPivotPoint;
    GameObject levelContainer;

    bool explanation;
    [MenuItem("JB Tools/Map Creator utility %#m")]
    static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(JBMapCreator));
    }
    private void OnEnable()
    {
        mapObjects = Resources.LoadAll("MapObjects", typeof(GameObject));

        target = this;
        so = new SerializedObject(target);
        mapProperties = so.FindProperty("instances");
    }

    private void OnGUI()
    {
        using (var horizontal = new GUILayout.HorizontalScope())
        {
            using (var vertical = new GUILayout.VerticalScope())
            {
                EditorGUILayout.PropertyField(mapProperties, true);
            }
            if (instances.Count > 0)
            {
                StartDictionary();
            }
        }
        using (var horizontal = new GUILayout.HorizontalScope())
        {

            using (var vertical = new GUILayout.VerticalScope())
            {
                isGameplay = EditorGUILayout.Toggle("Gameplay Object Map: ", isGameplay);
                spaceBetweenTiles = EditorGUILayout.FloatField("Distance between Tiles: ", spaceBetweenTiles, GUILayout.MinWidth(200), GUILayout.MaxWidth(200), GUILayout.Height(20));
            }
            map = TextureField("Texture map", map);
        }
        using (var horizontal = new GUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create Map", GUILayout.MinWidth(140), GUILayout.MaxWidth(200), GUILayout.Height(30)))
            {

                CreateMap();
            }
            GUILayout.FlexibleSpace();
        }
        GUILayout.FlexibleSpace();
        if (explanation)
        {
            GUILayout.Label("Instructions:");


            GUILayout.TextArea($"Drag and drop level piece prefabs (by default in Assets/Resources/MapObjects) into the map tile slots above. {Environment.NewLine}" +
                $"Choose a colour to associate it with that specific prefab, the alpha of any colour chosen is considered in the colour association.{Environment.NewLine}" +
                $"Using the texture picker select a texture to be used as to create the map. {Environment.NewLine}" +
                $"Textures must be set to default texture ype and set to be read/write enabled in their inspector, point filter mode is recommended{Environment.NewLine}" +
                $"Select if object is a gameplay object rather than a map object, this will determine how the object will be parented. {Environment.NewLine}" +
                $" Finally, choose the spacing between tiles.{Environment.NewLine}" +
                $"Pressing create map should create a map and parent it to a pivot point which is already controllable by the player");
        }

        if (GUILayout.Button("Show Explanation", GUILayout.MinWidth(140), GUILayout.MaxWidth(140), GUILayout.Height(20)))
        {
            if (!explanation)
            {
                explanation = true;
            }
            else
            {
                explanation = false;
            }
        }
    }
    private static Texture2D TextureField(string name, Texture2D texture)
    {
        GUILayout.BeginVertical();
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.UpperCenter;
        GUILayout.Label(name, style, GUILayout.Width(100), GUILayout.Height(20));
        var result = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(100), GUILayout.Height(100));
        GUILayout.EndVertical();
        return result;
    }
    private void StartDictionary()
    {
        MapTileDic = new Dictionary<Color, GameObject>();
        foreach (var tile in instances)
        {
            if (!MapTileDic.ContainsKey(tile.color))
            {
                MapTileDic.Add(tile.color, tile.mapTile);
            }
        }
    }
    void CreateMap()
    {
        if (GameObject.Find("Level Container"))
        {
            levelContainer = GameObject.Find("Level Container");
        }
        else
        {
            levelContainer = new GameObject();
            levelContainer.name = "Level Container";
            levelContainer.tag = "LevelRotationPoint";
            levelContainer.AddComponent<MovementController>();
            Undo.RegisterCreatedObjectUndo(levelContainer, "Created level container object");
        }
        if (GameObject.Find("Map Pivot Point"))
        {
            mapPivotPoint = GameObject.Find("Map Pivot Point");
        }
        else
        {
            mapPivotPoint = new GameObject();
            mapPivotPoint.name = "Map Pivot Point";
            Undo.RegisterCreatedObjectUndo(mapPivotPoint, "Created pivot point object");
        }
        mapPivotPoint.transform.parent = levelContainer.transform;
        for (int i = 0; i < map.width; i++)
        {
            for (int j = 0; j < map.height; j++)
            {
                Vector3 tilePos = new Vector3(i * spaceBetweenTiles, j * spaceBetweenTiles, 0);
                GameObject tile;
                if (MapTileDic.TryGetValue(map.GetPixel(i, j), out tile))
                {
                    if (tile != null)
                    {
                        GameObject mapTile = Instantiate(tile, new Vector3(levelContainer.transform.position.x - (map.width / 2), levelContainer.transform.position.y - (map.height / 2), 0) + tilePos, Quaternion.identity);
                        Undo.RegisterCreatedObjectUndo(mapTile, "Created tile object");
                        if (!isGameplay)
                        {
                            mapTile.transform.parent = mapPivotPoint.transform;
                        }
                        else
                        {
                            mapTile.transform.parent = levelContainer.transform;
                        }
                    }
                }
                else
                {
                    Debug.Log("No texture for that colour" + i + j);
                }

            }
        }
    }
}
[Serializable]
public class MapFiller
{
    public Color color = Color.white;
    public GameObject mapTile;
}
