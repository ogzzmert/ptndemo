using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitEntity : Entity
{
    [field: SerializeField] public EntityUnitType unitType { get; private set; }
    [field: SerializeField] public Operation[] operation { get; private set; }

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
}