using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class MenuPanel : Panel
{
    protected override void launch()
    {
        
    }

    public void startGame()
    {
        Debug.Log("start");
    }

    public void showCredits()
    {
        Debug.Log("credits");
    }
}