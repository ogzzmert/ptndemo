using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GamePanel : Panel
{
    [field: SerializeField] ScrollSubPanel leftScroll;
    [field: SerializeField] PadSubPanel middlePad;
    protected override void launch()
    {
        
        leftScroll.initialize<GamePanel>(world, this);
        middlePad.initialize<GamePanel>(world, this);

        endLaunch();

        world.handle<GameEventHandler>().call(GameEventType.onGame);
    }
    public override void discard()
    {
        world.handle<GameEventHandler>().call(GameEventType.onGameExit);
    }

}