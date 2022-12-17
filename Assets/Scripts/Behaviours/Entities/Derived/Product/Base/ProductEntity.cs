using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProductEntity : Entity
{
    [field: SerializeField] private EntityProductType productType;
    [field: SerializeField] public EntityProductType[] required { get; private set; }
    [field: SerializeField] private Craftable[] craftable;

    [Serializable]
    public class Craftable
    {
        [field: SerializeField] public EntityUnitType type { get; private set; }
        [field: SerializeField] public int cost { get; private set; }
        [field: SerializeField] public EntityProductType[] required { get; private set; }
    }
    public override void initialize(World world)
    {
        base.initialize(world);
    }
    public Craftable[] GetCraftables() { return craftable; }
}