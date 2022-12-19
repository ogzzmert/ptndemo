using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProductEntity : Entity
{
    [field: SerializeField] public EntityProductType productType { get; private set; }
    [field: SerializeField] public EntityProductType[] required { get; private set; }
    [field: SerializeField] private Craftable[] craftable;

    private Vector3Int[] spawnPositions;

    [Serializable]
    public class Craftable
    {
        [field: SerializeField] public EntityUnitType type { get; private set; }
        [field: SerializeField] public Belonging[] required { get; private set; }
    }
    public override void initialize(World world, int id)
    {
        base.initialize(world, id);

        setSpawnPositions();
        setLabel<GameLabel>("durability", buildDurability);
    }
    private void buildDurability(GameLabel label)
    {
        // set health bar over objects when camera is nearby
    }
    public Craftable[] GetCraftables() { return craftable; }
    public Vector3Int[] GetSpawnPositions() 
    { 
        return spawnPositions;
    }
    private void setSpawnPositions()
    {
        int index = 0;
        int count = bounds.size.x * 2 + bounds.size.y * 2;
        spawnPositions = new Vector3Int[count];

        for(int x = 0; x < bounds.size.x; x++) { spawnPositions[index] = new Vector3Int(position.x + x, position.y - 1, 0); index++; }
        for(int x = 0; x < bounds.size.x; x++) { spawnPositions[index] = new Vector3Int(position.x + x, position.y + bounds.size.y, 0); index++; }
        for(int y = 0; y < bounds.size.y; y++) { spawnPositions[index] = new Vector3Int(position.x - 1, position.y + y, 0); index++; }
        for(int y = 0; y < bounds.size.y; y++) { spawnPositions[index] = new Vector3Int(position.x + bounds.size.x, position.y + y, 0); index++; }
    }
}