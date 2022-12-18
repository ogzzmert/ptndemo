using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class StatSubPanel : SubPanel
{
    [field: SerializeField] Text txt;
    public override void initialize<T>(World world, T parentPanel)
    {
        base.initialize(world, parentPanel);
        updateValues();
    }

    public void updateValues()
    {
        string result = "";

        string[] resourceNames = TextManager.bring(TextManager.Content.Currency).Split('*');

        foreach(ResourceType rt in Enum.GetValues(typeof(ResourceType)))
        {
            int amount = world.handle<UserHandler>().getResource(rt);
            result += " | " + resourceNames[(int)rt] + " x" + amount.ToString();
        }

        txt.text = result + " | ";
    }
}