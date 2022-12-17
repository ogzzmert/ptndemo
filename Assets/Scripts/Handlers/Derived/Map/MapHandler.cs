using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapHandler : Handler, IHandlerGenerator
{
    [field: SerializeField] private Grid grid;

    public enum Layer
    {
        Ground = 0,
        Settlement = 1,
        Interactions = 2,
        Select = 3,
        Hover = 4
    }
    Dictionary<string, GameObject> maps = new Dictionary<string, GameObject>();
    Dictionary<Layer, Tilemap> layers = new Dictionary<Layer, Tilemap>();

    protected override void initialize()
    {
        foreach(WorldType wt in Enum.GetValues(typeof(WorldType)))
        {
            int i = 0;

            while(true)
            {
                string mapname = getMapName(wt, i);
                GameObject map = ResourceManager.load<GameObject>("Prefab/Map/Levels/" + mapname);
                if (map != null)
                {
                    maps.Add(mapname, map);
                    i++;
                }
                else break;
            }
        }
    }
    public void generate(WorldType worldType, int worldIndex)
    {
        string mapname = getMapName(worldType, worldIndex);

        if (maps.ContainsKey(mapname))
        {
            load(mapname);
        }
    }
    public void degenerate()
    {
        if (layers.Count > 0)
        {
            foreach(Tilemap t in layers.Values) world.destroy(t.gameObject);
        }
        layers.Clear();
    }
    private void load(string mapname)
    {
        foreach(Layer layer in Enum.GetValues(typeof(Layer)))
        {
            if (layer != Layer.Ground)
            {
                GameObject obj = new GameObject(layer.ToString());
                obj.transform.SetParent(grid.transform);
                obj.transform.localPosition = Vector2.zero;
                obj.transform.localScale = new Vector3(1, 1, 1);

                TilemapRenderer tmr = obj.AddComponent<TilemapRenderer>();
                tmr.sortingOrder = (int)layer;
                tmr.mode = TilemapRenderer.Mode.Individual;

                layers.Add(layer, obj.GetComponent<Tilemap>());
            }
            else
            {
                layers.Add(layer, world.spawn(maps[mapname]).GetComponent<Tilemap>());
                layers[layer].gameObject.name = layer.ToString();
                layers[layer].transform.SetParent(grid.transform);
                layers[layer].transform.localPosition = Vector2.zero;
                layers[layer].transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
    private string getMapName(WorldType worldType, int worldIndex)
    {
        return worldType.ToString() + "/" + worldIndex.ToString();
    }
    public void setTile(Layer layer, Vector3Int position, TileBase tilebase)
    {
        layers[layer].SetTile(position, tilebase);
    }
    public void setTiles(Layer layer, BoundsInt bounds, TileBase[] tiles)
    {
        layers[layer].SetTilesBlock(bounds, tiles);
    }
    public void clearTilemap(Layer layer)
    {
        layers[layer].ClearAllTiles();
    }
}
