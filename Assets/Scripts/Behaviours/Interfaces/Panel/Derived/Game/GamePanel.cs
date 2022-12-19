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

        rightWindow.showProductInfo(EntityProductType.Barracks);

        endLaunch();

        world.handle<GameEventHandler>().call(GameEventType.onGame);
    }
    public override void discard()
    {
        world.handle<GameEventHandler>().call(GameEventType.onGameExit);
    }

    public void showProductInfo(EntityProductType productType)
    {
        rightWindow.showProductInfo(productType);

        world.handle<AudioHandler>().playSoundButtonB();
    }
    public void showCraftableInfo(ProductEntity entity)
    {
        rightWindow.showCraftableInfo(entity);
    }
    public void showUnitInfo(UnitEntity entity)
    {
        rightWindow.showUnitInfo(entity);
    }
    public void clearInfo() { rightWindow.clear(); }
    public void setSelectedPosition(Vector3Int position) { middlePad.setSelectedPosition(position); }
    public Vector3Int getSelectedPosition() { return middlePad.getSelectedPosition(); }
    public void setHoverEntity(Entity entity) { middlePad.setHover(entity.bounds, entity.tiles); }
    public void clearHover() { middlePad.clearHover(); }
    public void setSelectAction(UnityAction action) { middlePad.setSelectAction(action); }
    public void updateStatusValues() { statsInfo.updateValues(); }
}