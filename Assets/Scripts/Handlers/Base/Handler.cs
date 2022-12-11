using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler : MonoBehaviour
{
    protected World world;
    public HandlerType type { get; private set; }
    private bool isReady = false;
    public void initialize(World world)
    {
        if (!isReady && world != null)
        {
            this.world = world;
            this.type = GetHandlerType();
            initialize();
        }
    }
    protected virtual void initialize()
    {
        // use this for derived initialization
    }
    protected virtual HandlerType GetHandlerType()
    {
        // override this method on derived to define handler's type
        return HandlerType.None;
    }
}
