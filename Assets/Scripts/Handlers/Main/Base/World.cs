using UnityEngine;
using System;
using System.Collections;

public class World : MonoBehaviour 
{
    [field: SerializeField] public InterfaceHandler gui { get; private set; }
    [field: SerializeField] public PoolHandler pool { get; private set; }

    public enum type
    {
        menu = 0,
        game = 1
    }
    private bool ready = false;
    private void Awake() {

        gui.initialize(this);
        pool.initialize(this);
    }
    private void Start() {
        
    }
    private void Update() {
        
    }
    public void setWorld(type worldType, int worldIndex = 0)
    {
        StartCoroutine(loadWorld(worldType, worldIndex));
    }
    private IEnumerator loadWorld(type worldType, int worldIndex)
    {
        if (isReady()) StartCoroutine(clearWorld());
        yield return new WaitUntil(() => !isReady());
        ready = true;
    }
    private IEnumerator clearWorld()
    {
        yield return null;
        ready = false;
    }
    public bool isReady() { return ready; }
    public GameObject spawn(GameObject g) { return Instantiate(g); }
    public void destroy(GameObject g) { Destroy(g); }
}