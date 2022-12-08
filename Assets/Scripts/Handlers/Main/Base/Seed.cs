using UnityEngine;

public class Seed : MonoBehaviour 
{
    private bool ready = false;
    private void Awake() {
        
    }
    private void Start() {
        
    }
    private void Update() {
        
    }
    public bool isReady() { return ready; }
    public void destroy(GameObject g) { Destroy(g); }
}