using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitEntity : Entity
{
    [field: SerializeField] public EntityUnitType unitType { get; private set; }
    [field: SerializeField] public Operation[] operation { get; private set; }
    [field: SerializeField] public int moveLimit { get; private set; }

    [Serializable]
    public class Operation
    {
        [field: SerializeField] public EntityUnitOperationType type { get; private set; }
        [field: SerializeField] public Belonging[] required { get; private set; }
    }

    public class Job
    {
        public EntityUnitOperationType type;
        public Stack<Node> path;
        public int tick;
    }

    Job job;
    public override void initialize(World world, int id)
    {
        base.initialize(world, id);
    }
    public void tryOperation(EntityUnitOperationType operationType, Stack<Node> nodes)
    {
        int frequencyTick = 1;

        if (operationType == EntityUnitOperationType.Move) frequencyTick = 2;

        job = new Job() { type = operationType, path = nodes, tick = frequencyTick };

        setBusy(true);
    }
    protected override void doJob(int frequency)
    {
        if (job != null)
        {
            if (job.type == EntityUnitOperationType.Move || job.type == EntityUnitOperationType.Charge)
            {
                if (job.path.Count > 0) 
                { 
                    if (frequency % job.tick == 0) move(job.path.Pop()); 
                }
                else setBusy(false);
            }
            else if (job.type == EntityUnitOperationType.Disband) 
            { 
                world.handle<UserHandler>().unitDiscard(this);
                setBusy(false);
            }
        }
    }
    void move(Node node)
    {
        MapHandler map = world.handle<MapHandler>();

        BoundsInt b = bounds;
        b.position = this.position;

        TileBase[] t = EntityManager.GetUnit(unitType).tiles;
        
        map.clearTiles(MapLayer.Settlement, b);

        b.position = node.position;

        map.setTiles(MapLayer.Settlement, b, t);

        map.replaceEntityOnMap(this, b.position);
    }
}