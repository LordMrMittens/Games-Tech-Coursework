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
    public List<MapFiller> instances;
    
    private Dictionary<Color, GameObject> MapTileDic;

    public Texture2D map;
    public float spaceBetweenTiles=1;
    ScriptableObject target;
    SerializedObject so;
    SerializedProperty mapProperties;
    [MenuItem("JB Tools/Map Creator utility")]
    static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(JBMapCreator));

    }
    private void OnEnable()
    {
       mapObjects = Resources.LoadAll("GameObjects", typeof(GameObject));
        
        target = this;
        so = new SerializedObject(target);
        mapProperties = so.FindProperty("instances");
     if (mapObjects.Length > 0)
        {
            for (int i = 0; i < mapObjects.Length; i++)
            {
                mapProperties.arraySize++;
                             /* MapFiller test = new MapFiller { color = Color.black, mapTile = mapObjects[i] as GameObject };
                                Debug.Log(test.mapTile.name + " " + test.color);
                                instances[i] = test;*/
                //arraylist
                //unordered list
            }
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.PropertyField(mapProperties, true);
            if (GUILayout.Button("Add Objects to map creator"))
        {
            StartDictionary();
        }
        map = TextureField("Image to create a map", map);
        spaceBetweenTiles = EditorGUILayout.FloatField(spaceBetweenTiles);

        if (GUILayout.Button("Create Map"))
        {
            
            CreateMap();
        }

    }

    private static Texture2D TextureField(string name, Texture2D texture)
    {
        GUILayout.BeginVertical();
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.UpperCenter;
        style.fixedWidth = 70;
        GUILayout.Label(name, style);
        var result = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
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
            else
            {
                Debug.Log("Colour already present" + tile.color);
            }
        }
    }
    void CreateMap()
    {
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
                        GameObject.Instantiate(tile, new Vector3(0, 0, 0) + tilePos, Quaternion.identity);
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
