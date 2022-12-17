using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserHandler : Handler
{
    public enum ResourceType
    {
        Gold = 0,
        Wood = 1,
        Brick = 2,
        Iron = 3,
        Gem = 4
    }
    [field: SerializeField] private Belonging[] belongings;
    
    [Serializable]
    protected class Belonging
    {
        public ResourceType resourceType;
        public int amount;
    }
    Dictionary<ResourceType, int> resource = new Dictionary<ResourceType, int>();
    protected override void initialize()
    {
        foreach(ResourceType resourceType in Enum.GetValues(typeof(ResourceType))) resource.Add(resourceType, 0);
        foreach(Belonging belonging in belongings) resource[belonging.resourceType] += belonging.amount;
    }
}
