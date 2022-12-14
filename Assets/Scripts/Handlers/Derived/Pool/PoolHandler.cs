using System;
using System.Collections.Generic;
using UnityEngine;
public class PoolHandler : Handler, IHandlerGenerator
{
    [field: SerializeField]
    public int size { get; private set; }

    List<Pool> pool_list;
    Transform parent;

    protected override void initialize()
    {
        parent = this.transform;
        pool_list = new List<Pool>();
        size = size > 0 ? size : 64;
    }
    public Pool poolify(GameObject baseObject, int count = -1)
    {
        if (count < 1) count = size; 
        Pool result = new Pool(world, baseObject, count, parent);
        pool_list.Add(result);
        return result;
    }
    public void cleanse()
    {
        foreach(Pool pool in pool_list)
        {
            pool.reset();
        }
    }

    public void generate(WorldType worldType, int worldIndex)
    {
        
    }

    public void degenerate()
    {
        cleanse();
    }
}