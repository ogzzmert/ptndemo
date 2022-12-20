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
    public override void initialize(World world, int id)
    {
        base.initialize(world, id);
    }
    public void tryOperation(EntityUnitOperationType operationType, Stack<Node> nodes)
    {
        if (operationType == EntityUnitOperationType.Move) StartCoroutine(moveIterator(nodes));
        else if (operationType == EntityUnitOperationType.Charge) StartCoroutine(moveIterator(nodes, 2f));
        else if (operationType == EntityUnitOperationType.Disband) world.handle<UserHandler>().unitDiscard(this);
    }
    IEnumerator moveIterator(Stack<Node> nodes, float speed = 1f)
    {
        MapHandler map = world.handle<MapHandler>();

        BoundsInt b = bounds;
        b.position = this.position;

        TileBase[] t = EntityManager.GetUnit(unitType).tiles;

        while(nodes.Count > 0)
        {
            map.clearTiles(MapLayer.Settlement, b);

            b.position = nodes.Pop().position;

            map.setTiles(MapLayer.Settlement, b, t);

            yield return new WaitForSeconds(1f / speed);
        }
    }
}