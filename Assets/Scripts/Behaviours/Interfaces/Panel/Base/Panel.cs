using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using System;
using System.Linq;

public class Panel : MonoBehaviour
{
    [Serializable]
    public class Data 
    {
        public string name;
        public Transform item;
        public UnityEvent action;
    }
    public Data[] btn;
    public Data[] bar;
    public Data[] oth;

    public enum type
    {
        window,
        bar,
        button
    }
    protected Dictionary<type, Dictionary<string, GameInteractable>> interactables { get; private set; } = new Dictionary<type, Dictionary<string, GameInteractable>>();

    protected World world;
    protected RectTransform rect;
    protected PoolObject pooledObject;
    protected int value;
    protected bool onLaunch = false;
    public bool isActive { get; private set; }= false;

    public void initialize(World world, int value = 0)
    {
        this.world = world;
        this.value = value;
        this.rect = GetComponent<RectTransform>();
        this.rect.offsetMin = Vector2.zero;
        this.rect.offsetMax = Vector2.zero;
        this.rect.localScale = new Vector3(1, 1, 1);
        setInteractables();
        launch();
    }
    void setInteractables()
    {
        interactables.Clear();
        foreach (type item in Enum.GetValues(typeof(type)))
        {
            interactables.Add(item, new Dictionary<string, GameInteractable>());
        }
        foreach(Data data in bar) { interactables[type.bar].Add(data.name, new GameBar(world, data.item, data.action)); }
        foreach(Data data in btn) { interactables[type.button].Add(data.name, new GameButton(world, data.item, data.action)); }
    }
    protected virtual void launch()
    {
        if (!onLaunch)
        {
            onLaunch = true;
        }
    }
    public void setAsPooled(PoolObject pooledObject)
    {
        this.pooledObject = pooledObject;
    }
    public virtual void discard()
    {
        // clear gui stuff before loading override, trigger onExit for specific panel class if desired
        if(pooledObject != null && pooledObject.isAwake()) pooledObject.sendback();
    }
    public virtual void reload()
    {
        // reload gui command override
    }
    public virtual void setPanel()
    {
        // JsonUtility.FromJson<Panel.form>(response)
        // endLaunch();
    }
    public virtual void read<T>(int index, T something) where T : class
    {
        // json data = something as json;
    }
    protected void endLaunch() { onLaunch = false; }
    public PoolObject getPooledObject() { return pooledObject; }
    public Data getOther(string itemName) 
    {  
        return oth.FirstOrDefault(c => c.name == itemName);
    }
    protected T getOther<T>(string itemName) where T : GameInteractable
    {
        Data other = getOther(itemName);

        if (other != null)
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { this.world, other.item, other.action});
        }
        else return null;
    }
    public void setActive(bool condition)
    {
        isActive = condition;
    }
    public T addInteractable<T>(type interactableType, string key) where T : GameInteractable
    {
        T item = (T)Activator.CreateInstance(typeof(T), new object[] { this.world, new GameObject(key).AddComponent<RectTransform>(), null });
        item.setParent(this);

        if(!interactables[interactableType].ContainsKey(key))
        {
            interactables[interactableType].Add(key, item);
            return item;
        }
        else return null;
    }
    public T getInteractable<T>(type interactableType, string key) where T : GameInteractable
    {
        if (interactables[interactableType].ContainsKey(key))
        {
            return interactables[interactableType][key] as T;
        }
        else return null;
    }
    public void removeInteractable(type interactableType, string key)
    {
        if (interactables[interactableType].ContainsKey(key))
        {
            GameInteractable i = interactables[interactableType][key] as GameInteractable;
            interactables[interactableType].Remove(key);
            i.discard();
        }
    }
}