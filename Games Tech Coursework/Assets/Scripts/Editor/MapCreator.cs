using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class MapCreator : MonoBehaviour
{
    public List<MapFiller> instances;
    private Dictionary<Color, GameObject> MapTileDic;

    public Texture2D map;
    public float spaceBetweenTiles;

    private void Start()
    {
        StartDictionary();
        CreateMap();
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
                if (MapTileDic.TryGetValue(map.GetPixel(i,j), out tile))
                {
                    GameObject.Instantiate(tile, this.transform.position + tilePos, Quaternion.identity);
                }
                else
                {
                    Debug.Log("No texture for that colour");
                }
                
            }
        }
    }
}
/*[Serializable]
public class MapFiller
{
    public Color color;
    public GameObject instance;
}*/
