// Mert Oguz - 2022 demo project

using UnityEngine;

public class PoolObject
{
    Transform h;
    GameObject g;
    int i = 0;
    bool awake = false;

    public PoolObject(GameObject _g, int _i, Transform _h)
    {
        h = _h;
        g = _g;
        i = _i;
        g.transform.parent = h;
        sendback();
    }
    public GameObject getObject()
    {
        return g;
    }
    public GameObject wakeObject()
    {
        g.SetActive(awake);
        return g;
    }
    public void woke() { awake = true; }
    public bool isAwake() { return awake; }
    public int getIndex() { return i; }
    public void sendback()
    {
        awake = false; 
        g.SetActive(false);
        g.transform.SetParent(h);
    }
}