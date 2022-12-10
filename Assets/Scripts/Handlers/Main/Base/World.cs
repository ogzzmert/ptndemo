using UnityEngine;
using System;

public class World : MonoBehaviour 
{
    [field: SerializeField] public InterfaceHandler gui { get; private set; }
    [field: SerializeField] public PoolHandler pool { get; private set; }

    private bool ready = false;
    private void Awake() {
        
    }
    private void Start() {
        
    }
    private void Update() {
        
    }
    public bool isReady() { return ready; }
    public GameObject spawn(GameObject g) { return Instantiate(g); }
    public void destroy(GameObject g) { Destroy(g); }
}