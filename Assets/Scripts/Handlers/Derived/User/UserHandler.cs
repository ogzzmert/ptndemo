using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserHandler : Handler
{
    [field: SerializeField] private Belonging[] belongings;
    Dictionary<ResourceType, int> resource = new Dictionary<ResourceType, int>();
    protected override void initialize()
    {
        foreach(ResourceType resourceType in Enum.GetValues(typeof(ResourceType))) resource.Add(resourceType, 0);
        foreach(Belonging belonging in belongings) resource[belonging.resourceType] += belonging.amount;
    }
}
