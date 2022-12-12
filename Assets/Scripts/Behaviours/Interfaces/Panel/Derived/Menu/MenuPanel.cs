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
        world.handle<AudioHandler>().playSoundButtonA();
        Debug.Log("start");
    }

    public void showCredits()
    {
        world.handle<AudioHandler>().playSoundButtonB();
        Debug.Log("credits");
    }
}