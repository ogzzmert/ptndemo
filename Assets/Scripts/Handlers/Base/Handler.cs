using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler : MonoBehaviour
{
    protected World world;
    private bool isReady = false;
    public void initialize(World world)
    {
        if (!isReady && world != null)
        {
            this.world = world;
            initialize();
        }
    }
    protected virtual void initialize()
    {
        // use this for derived initialization
    }
}
