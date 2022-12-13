using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapHandler : Handler, IHandlerGenerator
{
    [field: SerializeField] private Grid grid;

    Dictionary<string, GameObject> maps = new Dictionary<string, GameObject>();
    Tilemap basemap, midmap, topmap;

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
            loadBaseMap(mapname);
        }
    }
    public void degenerate()
    {
        if (basemap != null) world.destroy(basemap.gameObject);
        if (midmap != null) world.destroy(midmap.gameObject);
        if (topmap != null) world.destroy(topmap.gameObject);
        basemap = null;
        midmap = null;
        topmap = null;
    }
    private void loadBaseMap(string mapname)
    {
        basemap = world.spawn(maps[mapname]).GetComponent<Tilemap>();
        basemap.transform.SetParent(grid.transform);
        basemap.transform.localPosition = Vector2.zero;
        basemap.transform.localScale = new Vector3(1, 1, 1);
    }
    private string getMapName(WorldType worldType, int worldIndex)
    {
        return worldType.ToString() + "/" + worldIndex.ToString();
    }
    private void Update()
    {

    }
}
