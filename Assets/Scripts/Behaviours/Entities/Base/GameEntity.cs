using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameEntity : MonoBehaviour
{
    public class Data
    {
        public int index;
        public string code;
        public type type;
        public string info;
        public Position position;

    }
    public Data data { get; private set; }
    protected World world { get; private set; }
    protected Dictionary<string, GameLabel> labels {get; private set;}
    public Transform baseObject { get; private set; }
    public Transform baseModel { get; private set; }
    protected int ID { get; private set; }
    public string lastActive { get; private set; } = null;
    public bool isCore {get; private set;} = false;
    public bool isActive { get; private set; } = false;
    public bool isBuilt { get; private set; } = false;
    public class Position
    {
        public int x;
        public int z;
        public int r;
    }

    public class Move
    {
        public Position[] p;  // position array
        float speed;
    }

    public enum type
    {
        None = 0,
        Player = 1,
        Pet = 2,
        Npc = 3,
        Monster = 4,
        Mechanic = 5,
        Object = 6,
        Stuff = 7,
        Property = 8,
        File = 9,
        Target = 10

    }

    public GameEntity(World world, Data data)
    {
        this.world = world;
        this.data = data;
        labels = new Dictionary<string, GameLabel>();
    }
    public virtual void initialize<T>(T item) where T : class
    {
        // override for game object initialization
    }
    public virtual void discard()
    {
        // override for self deletion
        removeAllLabels();
        setActive(false);
        setBuild(false);
        Destroy(gameObject);
    }
    public virtual string calculateDamage(string[] data, bool isSkill)
    {
        // override for damage calculation
        return "0";
    }
    public virtual void Update()
    {
        // override for operations that require operating each frame
    }
    public virtual void LateUpdate()
    {
        // override for operations that require operating after each frame
    }
    public void setBaseObject(int[] arr)
    {
        baseObject = Calculator.getChild(gameObject.transform, arr).transform;
    }
    public void setBaseObject(Transform obj)
    {
        baseObject = obj;
    }
    public void setBaseModel(int[] arr)
    {
        baseModel = Calculator.getChild(gameObject.transform, arr).transform;
    }
    public void setBaseModel(Transform obj)
    {
        baseModel = obj;
    }
    protected virtual bool hasLabel()
    {
        return false;
    }
    protected virtual void setLabel<T>(string key, UnityAction<T> build) where T : GameLabel
    {
        // if (hasLabel()) { labels.Add(key, (world.game.panel as GamePlay).setLabel<T>(data.index, key)); build.Invoke((T)labels[key]); }
    }
    protected void removeLabel(string key)
    {
        // (world.game.panel as GamePlay).removeLabel(data.index, key);
    }
    protected void removeAllLabels()
    {
        foreach(string key in labels.Keys) removeLabel(key);
    }
    protected void setID(int index)
    {
        if (ID == 0) ID = index;
    }
    protected void setName(string data)
    {
        name = data;
    }
    protected void setCode(string code)
    {
        data.code = code;
    }
    public int getID()
    {
        return ID;
    }
    public string getCode()
    {
        return data.code;
    }
    public string getName()
    {
        return name;
    }
    protected void setDate(string data)
    {
        lastActive = data;
    }
    protected void setCore(bool condition)
    {
        isCore = condition;
    }
    protected void setActive(bool condition)
    {
        isActive = condition;
    }
    public void setBuild(bool condition)
    {
        isBuilt = condition;
    }
    protected virtual void setPosition(GameEntity.Position position)
    {
        this.data.position = position;
        baseObject.position = new Vector3(position.x * 0.1f, baseObject.position.y, position.z * -0.1f);
        baseModel.eulerAngles = new Vector3(0, position.r, 0);
    }
    protected virtual void setRotation(int angle)
    {
        baseModel.eulerAngles = new Vector3(0, angle, 0);
        this.data.position.r = angle;
    }
    public Vector3 getPosition()
    {
        return baseObject.position;
    }
    public virtual float getRotation()
    {
        return baseModel.eulerAngles.y;
    }
    public float getDistanceTo(Vector3 position)
    {
        return Calculator.getDistance(getPosition(), position);
    }
    public float getDistanceTo(Position position)
    {
        return Calculator.getDistance(this.data.position, position);
    }
    public virtual bool isInvisible()
    {
        // override for invisiblity check
        return false;
    }
}