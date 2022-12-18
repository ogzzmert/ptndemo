using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

public class GamePanel : Panel
{
    [field: SerializeField] ScrollSubPanel leftScroll;
    [field: SerializeField] PadSubPanel middlePad;
    [field: SerializeField] InfoSubPanel rightWindow;
    [field: SerializeField] StatSubPanel statsInfo;
    protected override void launch()
    {
        
        leftScroll.initialize<GamePanel>(world, this);
        middlePad.initialize<GamePanel>(world, this);
        rightWindow.initialize<GamePanel>(world, this);
        statsInfo.initialize<GamePanel>(world, this);

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

    public Vector3Int getSelectedPosition() { return middlePad.getSelectedPosition(); }
    public void setHoverProduct(ProductEntity entity) { middlePad.setHover(entity.bounds, entity.tiles); }
    public void clearHover() { middlePad.clearHover(); }
    public void setSelectAction(UnityAction action) { middlePad.setSelectAction(action); }
    public void updateStatusValues() { statsInfo.updateValues(); }
}