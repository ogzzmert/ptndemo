using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class MenuPanel : Panel
{
    SubPanel credits;
    protected override void launch()
    {
        getInteractable<GameButton>(type.button, "start").setText(TextManager.bring(TextManager.Content.MenuStart));
        getInteractable<GameButton>(type.button, "credits").setText(TextManager.bring(TextManager.Content.MenuCredits));
        
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

        if (credits == null || !credits.getPooledObject().isAwake())
        {
            credits = world.handle<InterfaceHandler>().bringMessage(TextManager.bring(TextManager.Content.Credits));
        }
    }
}