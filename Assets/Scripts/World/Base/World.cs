using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour 
{
    [field: SerializeField] private Handler[] handlerList;

    Dictionary<Type, Handler> handlers = new Dictionary<Type, Handler>();

    private void Awake() 
    {
        foreach(Handler handler in handlerList)
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
        foreach(Handler handler in handlers.Values) if (handler is IHandlerGenerator) (handler as IHandlerGenerator).generate(worldType, worldIndex);
        yield return null; // load resources and wait for them to be loaded/instantiated
    }
    public bool isReady() { return true; }
    public GameObject spawn(GameObject g) { return Instantiate(g); }
    public void destroy(GameObject g) { Destroy(g); }
    public T handle<T>() where T : Handler 
    { 
        return handlers[typeof(T)] as T; 
    }
}