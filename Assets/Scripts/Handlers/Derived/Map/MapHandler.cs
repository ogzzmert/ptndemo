using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapHandler : Handler, IHandlerGenerator
{
    [field: SerializeField] private Grid grid;
    [field: SerializeField] private MapTileMatch[] match;

    [Serializable]
    private class MapTileMatch
    {
        public MapTile type;
        public TileBase tile;
    }
    
    Dictionary<string, GameObject> maps = new Dictionary<string, GameObject>();
    Dictionary<MapLayer, Tilemap> layers = new Dictionary<MapLayer, Tilemap>();

    Dictionary<string, MapTileMatch> mapTileMatches = new Dictionary<string, MapTileMatch>();
    Dictionary<Vector3Int, MapTile> basePathMap = new Dictionary<Vector3Int, MapTile> ();
    Dictionary<Vector3Int, MapTile> pathMap = new Dictionary<Vector3Int, MapTile> ();
    
    int indexCounter = 0;
    Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
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
        foreach(MapTileMatch m in match) mapTileMatches.Add(m.tile.name, m);
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
        entities.Clear();
        basePathMap.Clear();
        pathMap.Clear();
        indexCounter = 0;
    }
    private void load(string mapname)
    {
        foreach(MapLayer layer in Enum.GetValues(typeof(MapLayer)))
        {
            if (layer != MapLayer.Ground)
            {
                GameObject obj = new GameObject(layer.ToString());
                obj.transform.SetParent(grid.transform);
                obj.transform.localPosition = Vector2.zero;
                obj.transform.localScale = new Vector3(1, 1, 1);

                TilemapRenderer tmr = obj.AddComponent<TilemapRenderer>();
                tmr.sortingOrder = (int)layer;
                tmr.mode = TilemapRenderer.Mode.Individual;

                Tilemap tm = obj.GetComponent<Tilemap>();

                layers.Add(layer, tm);
            }
            else
            {
                Tilemap tm = world.spawn(maps[mapname]).GetComponent<Tilemap>();
                tm.gameObject.name = layer.ToString();
                tm.transform.SetParent(grid.transform);
                tm.transform.localPosition = Vector2.zero;
                tm.transform.localScale = new Vector3(1, 1, 1);
                tm.CompressBounds();
                layers.Add(layer, tm);

                TileBase[] tbs = tm.GetTilesBlock(tm.cellBounds);
                
                tm.SetTilesBlock(tm.cellBounds, tbs);

                setBaseMap(tm);
            }
        }
    }
    private string getMapName(WorldType worldType, int worldIndex)
    {
        return worldType.ToString() + "/" + worldIndex.ToString();
    }
    public void setTile(MapLayer layer, Vector3Int position, TileBase tilebase)
    {
        layers[layer].SetTile(position, tilebase);
    }
    public void setTiles(MapLayer layer, BoundsInt bounds, TileBase[] tiles)
    {
        layers[layer].SetTilesBlock(bounds, tiles);
    }
    public void clearTilemap(MapLayer layer)
    {
        layers[layer].ClearAllTiles();
    }
    public int joinEntityToMap<T>(T entity, Vector3Int position) where T : Entity
    {
        indexCounter++;
        entities.Add(indexCounter, entity);

        world.destroy(entity.GetComponent<TilemapRenderer>());
        world.destroy(entity.GetComponent<Tilemap>());

        entity.transform.SetParent(layers[MapLayer.Settlement].transform);
        entity.initialize(world, indexCounter);
        entity.setPosition(position);

        BoundsInt bounds = entity.bounds;
        bounds.position = position;

        updatePathMap(true, bounds);
        
        return indexCounter;
    }
    public void disjoinEntityFromMap<T>(T entity) where T : Entity
    {
        if(entities.ContainsKey(entity.getID()))
        {
            BoundsInt bounds = entity.bounds;
            bounds.position = entity.position;

            updatePathMap(false, bounds);

            entities.Remove(entity.getID());
            entity.discard();
        }
    }
    public bool canPlaceEntity(MapTile[] ground, BoundsInt bounds)
    {
        // check if entity can be placed on the given position

        for(int i = bounds.position.x; i < bounds.position.x + bounds.size.x; i++)
        {
            for(int j = bounds.position.y; j < bounds.position.y + bounds.size.y; j++)
            {
                Vector3Int position = new Vector3Int(i, j, 0);
                
                if (pathMap.ContainsKey(position))
                {
                    if (!ground.Any(t => t == pathMap[position])) return false;
                }
                else return false;
            }
        }

        return true;
    }
    private void updatePathMap(bool isBlock, BoundsInt bounds)
    {
        // update path map data, isBlock (true) means setting tile type to none, false means returning it back to basePathMap value
        
        for(int i = bounds.position.x; i < bounds.position.x + bounds.size.x; i++)
        {
            for(int j = bounds.position.y; j < bounds.position.y + bounds.size.y; j++)
            {
                Vector3Int position = new Vector3Int(i, j, 0);
                
                pathMap[position] = isBlock ? MapTile.None : basePathMap[position];
            }
        }
    }
    void setBaseMap(Tilemap tileMap) 
    {
        for (int n = tileMap.cellBounds.xMin; n < tileMap.cellBounds.xMax; n++)
        {
            for (int p = tileMap.cellBounds.yMin; p < tileMap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = new Vector3Int(n, p, 0);

                if (tileMap.HasTile(localPlace))
                {
                    basePathMap.Add(localPlace, mapTileMatches[tileMap.GetTile(localPlace).name].type);
                }
            }
        }
        foreach(var bpm in basePathMap) pathMap.Add(bpm.Key, bpm.Value);
    }
}
