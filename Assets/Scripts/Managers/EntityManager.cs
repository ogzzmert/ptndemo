using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class EntityManager
{
    static Dictionary<EntityProductType, ProductEntity> product = new Dictionary<EntityProductType, ProductEntity>();
    static Dictionary<EntityUnitType, UnitEntity> unit = new Dictionary<EntityUnitType, UnitEntity>();

    private EntityManager() { }
    public static void load()
    {
        foreach(EntityProductType ept in Enum.GetValues(typeof(EntityProductType)))
        {
            GameObject obj = ResourceManager.load<GameObject>("Prefab/Map/Objects/Product/" + ept.ToString());
            if (obj != null)
            {
                ProductEntity entity = obj.GetComponent<ProductEntity>();
                entity.setTiles(entity.GetComponent<Tilemap>());
                product.Add(ept, entity);
            }
        }
        foreach(EntityUnitType eut in Enum.GetValues(typeof(EntityUnitType)))
        {
            GameObject obj = ResourceManager.load<GameObject>("Prefab/Map/Objects/Unit/" + eut.ToString());
            if (obj != null)
            {
                UnitEntity entity = obj.GetComponent<UnitEntity>();
                entity.setTiles(entity.GetComponent<Tilemap>());
                unit.Add(eut, entity);
            }
        }
    }
    public static ProductEntity GetProduct(EntityProductType ept) { return product[ept]; }
    public static UnitEntity GetUnit(EntityUnitType eut) { return unit[eut]; }
}