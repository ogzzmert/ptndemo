using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GamePanel : Panel
{
    [field: SerializeField] ScrollSubPanel leftScroll;
    protected override void launch()
    {
        world.handle<GameEventHandler>().call(GameEventType.onGame);
        leftScroll.initialize<GamePanel>(world, this);
        endLaunch();
    }
    public override void discard()
    {
        world.handle<GameEventHandler>().call(GameEventType.onGameExit);
    }
}