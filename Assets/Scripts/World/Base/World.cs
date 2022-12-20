// Mert Oguz - 2022 demo project

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour 
{
    [field: SerializeField] private float tickDelta;
    Dictionary<Type, Handler> handlers = new Dictionary<Type, Handler>();

    private void Awake() 
    {
        foreach(Handler handler in GetComponentsInChildren<Handler>())
        {
            handler.initialize(this);
            handlers.Add(handler.GetType(), handler);
        }
        loadManagers();
    }
    private void Start() 
    {
        tickDelta = tickDelta <= 0.0015f ? 0.015f : tickDelta;

        StartCoroutine(TickLoop());

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

        foreach(Handler handler in handlers.Values) if (handler is IHandlerGenerator) (handler as IHandlerGenerator).degenerate();
        
        yield return new WaitForSeconds(0.1f);

        foreach(Handler handler in handlers.Values) if (handler is IHandlerGenerator) (handler as IHandlerGenerator).generate(worldType, worldIndex);

    }
    IEnumerator TickLoop()
    {
        int frequency = 0;

        while(true)
        {
            foreach(Handler handler in handlers.Values) if (handler is IHandlerTicker) (handler as IHandlerTicker).Tick(frequency);

            yield return new WaitForSeconds(tickDelta);

            frequency = frequency < 100 ? frequency + 1 : 0;
        }
    }
    public bool isReady() { return true; }
    public GameObject spawn(GameObject g) { return Instantiate(g); }
    public void destroy<T>(T g) where T : UnityEngine.Object
    { 
        Destroy(g); 
    }
    public T handle<T>() where T : Handler 
    { 
        return handlers[typeof(T)] as T; 
    }
    private void loadManagers()
    {
        // Managers needs to be loaded within a certain order
        
        InputManager.load();
        TextManager.load();
        TextureManager.load();
        EntityManager.load();
    }
}