using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    private static Dictionary<Type, Dictionary<string, UnityEngine.Object>> cache = new Dictionary<Type, Dictionary<string, UnityEngine.Object>>();

    private ResourceManager() { }
    public static bool save<T>(string path, string name) where T : UnityEngine.Object
    {
        if (!cache.ContainsKey(typeof(T))) cache.Add(typeof(T), new Dictionary<string, UnityEngine.Object>());

        switch(cache[typeof(T)].ContainsKey(name))
        {
            case true:
                return true;

            case false:
                cache[typeof(T)].Add(name, load<T>(path + name));
                return false;
        }
    }
    public static T load<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }
    public static T loadFromCache<T>(string name) where T : UnityEngine.Object
    {
        return cache[typeof(T)].ContainsKey(name) ? cache[typeof(T)][name] as T : null;
    }
}