// Mert Oguz - 2022 demo project

using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Entity : MonoBehaviour
{
    [field: SerializeField] public EntityType type { get; private set; }
    [field: SerializeField] public int durability { get; private set; }
    [field: SerializeField] public Belonging[] cost { get; private set; }
    [field: SerializeField] public BoundsInt bounds { get; private set; }
    [field: SerializeField] public MapTile[] ground { get; private set; }
    public TileBase[] tiles { get; private set; }

    protected World world { get; private set; }
    protected Dictionary<string, GameLabel> labels {get; private set;}
    public Transform baseObject { get; private set; }
    public Transform baseModel { get; private set; }
    protected int ID { get; private set; }
    public string lastActive { get; private set; } = null;
    public bool isCore {get; private set;} = false;
    public bool isActive { get; private set; } = false;
    public bool isBusy { get; private set; } = false;
    public Vector3Int position { get; private set; }

    public virtual void initialize(World world, int id)
    {
        this.world = world;
        setID(id);
        setBaseObject(this.transform);
        setBaseModel(this.transform);
        labels = new Dictionary<string, GameLabel>();
    }
    public virtual void discard()
    {
        // override for self deletion
        removeAllLabels();
        setActive(false);
        setBusy(false);
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
        ID = index;
    }
    public void setName(string data)
    {
        name = data;
    }
    public int getID()
    {
        return ID;
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
    protected void setBusy(bool condition)
    {
        isBusy = condition;
    }
    public virtual void setPosition(Vector3Int position)
    {
        this.position = position;
        this.baseObject.position = position;
        // baseObject.position = new Vector3(position.x * 0.1f, baseObject.position.y, position.y * -0.1f);
    }
    public float getDistanceTo(Vector3Int position)
    {
        return Calculator.getDistance(this.position, position);
    }
    public void tryJob(int frequency)
    {
        if (isBusy) doJob(frequency);
    }
    protected virtual void doJob(int frequency)
    {
        // override for operations or jobs
    }
    public void setTiles(Tilemap map)
    {
        tiles = map.GetTilesBlock(bounds);
    }
}