// Mert Oguz - 2022 demo project

using UnityEngine;
public class Pool
{
    Transform object_list_handler;
    PoolObject[] object_list;
    int current;
    int length;

    /*
     * Usage
     * Creation                      : pool = new Pool(core_object, 100)
     * Step 1 - Call                 : pool_obj = pool.bring()
     * Step 2 - Get the GameObject   : obj = pool_obj.getObject()
     * Step 3 - Release              : obj.wakeObject()
     * Step 4 - Destroy              : pool_obj.sendback()
     * 
     * Note : pool_obj.isAwake() to check availability
    */
    public Pool(World world, GameObject base_object, int count, Transform parent)
    {
        length = count;
        object_list = new PoolObject[count];
        GameObject main_object_list_handler = new GameObject();
        main_object_list_handler.name = base_object.name + "_list";
        object_list_handler = main_object_list_handler.transform;
        object_list_handler.parent = parent;

        for(int i = 0; i<count; i++)
        {
            GameObject newObj = world.spawn(base_object);
            object_list[i] = new PoolObject(newObj, i, object_list_handler);
        }
        current = 0;
    }
    public PoolObject bring()
    {
        PoolObject p = object_list[current];
        while (p.isAwake())
        {
            if (current < length - 1)
            {
                current++;
                if (!object_list[current].isAwake())
                {
                    p = object_list[current];
                }
            }
            else
            {
                current = 0;
                if (object_list[current].isAwake()) object_list[current].sendback();
                p = object_list[current];
            }
        }
        p.woke();
        return p;
    }
    public void reset()
    {
        foreach(PoolObject obj in object_list) obj.sendback();
        current = 0;
    }
}
