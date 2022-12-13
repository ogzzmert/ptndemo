using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class MenuPanel : Panel
{
    protected override void launch()
    {
        world.handle<GameEventHandler>().call(GameEventType.onMenu);
        endLaunch();
    }
    public override void discard()
    {
        world.handle<GameEventHandler>().call(GameEventType.onMenuExit);
    }
    public void startGame()
    {
        world.handle<AudioHandler>().playSoundButtonA();
        if(!onLaunch)
        {
            world.setWorld(WorldType.Game);
        }
    }

    public void showCredits()
    {
        world.handle<AudioHandler>().playSoundButtonB();
    }
}