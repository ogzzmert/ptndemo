using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Linq;

public class UserHandler : Handler
{
    [field: SerializeField] private Belonging[] belongings;
    Dictionary<ResourceType, int> resource = new Dictionary<ResourceType, int>();
    Dictionary<int, ProductEntity> products = new Dictionary<int, ProductEntity>();
    Dictionary<int, UnitEntity> units = new Dictionary<int, UnitEntity>();
    protected override void initialize()
    {
        foreach(ResourceType resourceType in Enum.GetValues(typeof(ResourceType))) resource.Add(resourceType, 0);
        foreach(Belonging belonging in belongings) resource[belonging.resourceType] += belonging.amount;
        belongings = null;
    }
    public int getResource(ResourceType resourceType)
    {
        return resource[resourceType];
    }
    public bool checkProductRequired(ProductEntity entity)
    {
        return !doesMatchRequirements(entity.required) || !canConsumeCost(entity.cost) ? false : true;
    }
    public bool checkCraftableRequired(ProductEntity.Craftable craftable)
    {
        return !canConsumeCost(craftable.required) ? false : true;
    } 
    private bool doesMatchRequirements(EntityProductType[] productTypes)
    {
        foreach(EntityProductType ept in productTypes)
        {
            if (!products.Values.Any(p => p.productType == ept))
            {
                return false;
            }
        }
        return true;
    }
    private bool consumeProductCost(ProductEntity entity)
    {
        if (checkProductRequired(entity))
        {
            consumeCost(entity.cost);

            return true;
        }
        else return false;
    }
    private bool consumeCraftableCost(ProductEntity.Craftable craftable)
    {
        if (checkCraftableRequired(craftable))
        {
            consumeCost(craftable.required);

            return true;
        }
        else return false;
    }
    private bool canConsumeCost(Belonging[] cost)
    {
        foreach(Belonging b in cost)
        {
            if(resource[b.resourceType] < b.amount)
            {
                return false;
            }
        }
        return true;
    }
    private void consumeCost(Belonging[] cost)
    {
        foreach(Belonging b in cost) resource[b.resourceType] -= b.amount;
    }
    public void productCraft(ProductEntity entity, Vector3Int position)
    {
        if (consumeProductCost(entity))
        {
            ProductEntity newProduct = craft(entity, position);

            products.Add(newProduct.getID(), newProduct);
        }
    }
    public void unitCraft(ProductEntity.Craftable craftable, UnitEntity entity, Vector3Int position)
    {
        if (consumeCraftableCost(craftable))
        {
            UnitEntity newUnit = craft(entity, position);

            units.Add(newUnit.getID(), newUnit);
        }
    }
    private T craft<T>(T entity, Vector3Int position) where T : Entity
    {
        T newEntity = world.spawn(entity.gameObject).GetComponent<T>();

        world.handle<MapHandler>().joinEntityToMap(newEntity, position);

        BoundsInt bounds = entity.bounds;
        bounds.position = position;

        world.handle<MapHandler>().setTiles(MapLayer.Settlement, bounds, entity.tiles);
        
        return newEntity;
    }
    public bool checkUnitRequired(UnitEntity entity)
    {
        bool result = true;
        
        return result;
    }
    public Entity withinEntityBounds(Vector3Int position)
    {
        foreach(ProductEntity entity in products.Values)
        {
            if(Calculator.withinBounds(position, entity.position, entity.bounds)) return entity;
        }
        foreach(UnitEntity entity in units.Values)
        {
            if(Calculator.withinBounds(position, entity.position, entity.bounds)) return entity;
        }
        return null;
    }
}
