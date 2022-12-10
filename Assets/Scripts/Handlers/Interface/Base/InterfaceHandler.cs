using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceHandler : MonoBehaviour
{
    [field: SerializeField] private Canvas canvas;

    World world;

    // Start is called before the first frame update
    public void initialize(World world)
    {
        this.world = world;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
