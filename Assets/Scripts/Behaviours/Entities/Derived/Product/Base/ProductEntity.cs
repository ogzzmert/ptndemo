using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProductEntity : Entity
{
    [field: SerializeField] public EntityProductType productType { get; private set; }
    [field: SerializeField] public EntityProductType[] required { get; private set; }
    [field: SerializeField] private Craftable[] craftable;

    [Serializable]
    public class Craftable
    {
        [field: SerializeField] public EntityUnitType type { get; private set; }
        [field: SerializeField] public Belonging[] required { get; private set; }
    }
    public override void initialize(World world, int id)
    {
        base.initialize(world, id);

        setLabel<GameLabel>("durability", buildDurability);
    }
    private void buildDurability(GameLabel label)
    {
        // set health bar over objects when camera is nearby
    }
    public Craftable[] GetCraftables() { return craftable; }
}