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
    public Data[] wndw;
    public Data[] btn;
    public Data[] bar;
    public Data[] oth;

    protected enum type
    {
        window,
        bar,
        button
    }
    protected Dictionary<type, Dictionary<string, GameInteractable>> items { get; private set; } = new Dictionary<type, Dictionary<string, GameInteractable>>();
    protected World world;
    protected RectTransform rect;
    protected int value;
    protected bool onLaunch = false;
    public bool isActive { get; private set; }= false;

    public void initialize(World world, int value = 0)
    {
        this.world = world;
        this.value = value;
        this.rect = GetComponent<RectTransform>();
        this.rect.offsetMax = Vector2.zero;
        this.rect.offsetMin = Vector2.zero;
        setInteractables();
        launch();
    }
    void setInteractables()
    {
        foreach (type item in Enum.GetValues(typeof(type)))
        {
            items.Add(item, new Dictionary<string, GameInteractable>());
        }
        foreach(Data data in wndw) { items[type.window].Add(data.name, new GameWindow(world, data.item, data.action)); }
        foreach(Data data in bar) { items[type.bar].Add(data.name, new GameBar(world, data.item, data.action)); }
        foreach(Data data in btn) { items[type.button].Add(data.name, new GameButton(world, data.item, data.action)); }
    }
    protected virtual void launch()
    {
        if (!onLaunch)
        {
            onLaunch = true;
        }
    }
    public virtual void discard()
    {
        // clear gui stuff before loading override, trigger onExit for specific panel class if desired
    }
    public virtual void reload()
    {
        // reload gui command override
    }
    public virtual void setPanel()
    {
        // JsonUtility.FromJson<Panel.form>(response)
        endLaunch();
    }
    public virtual void read<T>(int index, T something) where T : class
    {
        // json data = something as json;
    }
    public virtual WorldType getCategory() { return WorldType.Menu; }
    protected void endLaunch() { onLaunch = false; }
    protected GameWindow getWindow(string itemName) { return items[type.window][itemName] as GameWindow; }
    protected GameBar getBar(string itemName) { return items[type.bar][itemName] as GameBar; }
    protected GameButton getButton(string itemName) { return items[type.button][itemName] as GameButton; }
    public Transform getOther(string itemName) 
    {  
        return oth.FirstOrDefault(c => c.name == itemName).item;
    }
    protected T getSomething<T>(string itemName) where T : GameInteractable
    {
        return (T)Activator.CreateInstance(typeof(T), new object[] { this.world, getOther(itemName) });
    }
    public void setActive(bool condition)
    {
        isActive = condition;
    }
}