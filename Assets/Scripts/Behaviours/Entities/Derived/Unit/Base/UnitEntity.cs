using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitEntity : Entity
{
    [field: SerializeField] private EntityUnitType unitType;
    [field: SerializeField] private Operation[] operation;

    [Serializable]
    public class Operation
    {
        [field: SerializeField] public EntityUnitType type { get; private set; }
        [field: SerializeField] public int cost { get; private set; }
        [field: SerializeField] public EntityProductType[] required { get; private set; }
    }
    public override void initialize(World world)
    {
        base.initialize(world);
    }
}