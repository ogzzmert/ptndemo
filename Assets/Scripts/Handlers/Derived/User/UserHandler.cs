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
        bool result = true;
            
        foreach(EntityProductType ept in entity.required)
        {
            if (!products.Values.Any(p => p.productType == ept))
            {
                result = false;
                break;
            }
        }
        foreach(Belonging b in entity.cost)
        {
            if(resource[b.resourceType] < b.amount)
            {
                result = false;
                break;
            }
        }

        return result;
    }
    private bool consumeProductRequired(ProductEntity entity)
    {
        if (checkProductRequired(entity))
        {
            foreach(Belonging b in entity.cost) resource[b.resourceType] -= b.amount;

            return true;
        }
        else return false;
    }
    public ProductEntity productCraft(ProductEntity entity, Vector3Int position)
    {
        if (consumeProductRequired(entity))
        {
            ProductEntity newProduct = world.spawn(entity.gameObject).GetComponent<ProductEntity>();

            int index = world.handle<MapHandler>().joinEntityToMap(newProduct, position);
            products.Add(index, newProduct);
            
            return newProduct;
        }
        else return null;
    }
    public bool checkUnitRequired(UnitEntity entity)
    {
        bool result = true;
        
        return result;
    }
}
