using System;
using System.Collections.Generic;
using UnityEngine;
public class PoolHandler : MonoBehaviour
{
    [field: SerializeField]
    public int size { get; private set; }

    List<Pool> pool_list;
    Transform parent;
    World world;

    public void initialize(World world)
    {
        this.world = world;
        parent = this.transform;
        pool_list = new List<Pool>();
        size = size > 0 ? size : 64;
    }
    public Pool generate(GameObject baseObject, int count = -1)
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
}