using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    private ResourceManager() { }
    private static Dictionary<string, MonoBehaviour> cache = new Dictionary<string, MonoBehaviour>();

    public static bool save<T>(string name, string path) where T : MonoBehaviour
    {
        switch(cache.ContainsKey(name))
        {
            case true:
                return true;

            case false:
                cache.Add(name, load<T>(path));
                return false;
        }
    }
    public static T load<T>(string path) where T : MonoBehaviour
    {
        return Resources.Load<T>(path);
    }
    public static T loadFromCache<T>(string name) where T : MonoBehaviour
    {
        return cache.ContainsKey(name) ? cache[name] as T : null;
    }
}