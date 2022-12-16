using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GamePanel : Panel
{
    [field: SerializeField] ScrollSubPanel leftScroll;
    [field: SerializeField] PadSubPanel middlePad;
    [field: SerializeField] InfoSubPanel rightWindow;
    protected override void launch()
    {
        
        leftScroll.initialize<GamePanel>(world, this);
        middlePad.initialize<GamePanel>(world, this);
        rightWindow.initialize<GamePanel>(world, this);

        rightWindow.showCraftableInfo(EntityProductType.Barracks);

        endLaunch();

        world.handle<GameEventHandler>().call(GameEventType.onGame);
    }
    public override void discard()
    {
        world.handle<GameEventHandler>().call(GameEventType.onGameExit);
    }

    public void showCraftableInfo(EntityProductType productType)
    {
        rightWindow.showCraftableInfo(productType);

        world.handle<AudioHandler>().playSoundButtonB();
    }

}