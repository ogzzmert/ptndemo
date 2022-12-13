using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour 
{
    Dictionary<Type, Handler> handlers = new Dictionary<Type, Handler>();

    bool worldLoaded = false;

    private void Awake() 
    {
        foreach(Handler handler in GetComponentsInChildren<Handler>())
        {
            handler.initialize(this);
            handlers.Add(handler.GetType(), handler);
        }
    }
    private void Start() 
    {
        handle<GameEventHandler>().call(GameEventType.onStart);   
    }
    public void launch()
    {
        setWorld(WorldType.Menu);
    }
    public void setWorld(WorldType worldWorldType, int worldIndex = 0)
    {
        StartCoroutine(loadWorld(worldWorldType, worldIndex));
    }
    private IEnumerator loadWorld(WorldType worldType, int worldIndex)
    {
        if (worldLoaded)
        {
            worldLoaded = false;
            foreach(Handler handler in handlers.Values) if (handler is IHandlerGenerator) (handler as IHandlerGenerator).degenerate();
        }

        yield return new WaitForSeconds(0.1f);

        foreach(Handler handler in handlers.Values) if (handler is IHandlerGenerator) (handler as IHandlerGenerator).generate(worldType, worldIndex);

        worldLoaded = true;
    }
    public bool isReady() { return true; }
    public GameObject spawn(GameObject g) { return Instantiate(g); }
    public void destroy(GameObject g) { Destroy(g); }
    public T handle<T>() where T : Handler 
    { 
        return handlers[typeof(T)] as T; 
    }
}