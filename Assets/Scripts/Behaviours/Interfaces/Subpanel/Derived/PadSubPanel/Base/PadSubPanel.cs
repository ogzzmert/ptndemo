using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class PadSubPanel : SubPanel
{
    [field: SerializeField] private TileBase[] hover;
    [field: SerializeField] private BoundsInt hoverBounds;
    [field: SerializeField] private TileBase select;

    private TileBase[] hoverTiles;
    private BoundsInt hoverBoundsInt;
    GameHoldButton pad;
    Vector3 holdPosition;
    Vector3Int beginPosition, selectPosition, hoverPosition;
    DateTime pressTreshold, holdTreshold;
    UnityAction selectAction;
    public override void initialize<T>(World world, T parentPanel)
    {
        base.initialize(world, parentPanel);

        clearHover();

        pad = getOther<GameHoldButton>("pad");

        pad.onClickHover(hoverTilePad, endHoverTilePad);
        pad.onClick(moveTilePad);
        pad.onClickDown(beginMoveTilePad);
        pad.onClickUp(endMoveTilePad);
    }
    private Vector3 getPadPositionFloat() { return Camera.main.ScreenToWorldPoint(InputManager.getPrimaryPosition()); }
    private Vector3Int getPadPositionInt() { return Vector3Int.FloorToInt(getPadPositionFloat()); }

    void hoverTilePad()
    {
        // set hovering cursor while pointer is over the game pad
        Vector3Int position = getPadPositionInt();

        if (hoverPosition != position)
        {
            world.handle<MapHandler>().clearTilemap(MapLayer.Hover);
            hoverPosition = position;

            BoundsInt hoverBoundsTemp = hoverBoundsInt;
            hoverBoundsTemp.position = hoverPosition;

            world.handle<MapHandler>().setTiles(MapLayer.Hover, hoverBoundsTemp, hoverTiles);
        }
    }
    void endHoverTilePad()
    {
        // exit hovering, clear hover layer
        world.handle<MapHandler>().clearTilemap(MapLayer.Hover);
    }
    void moveTilePad()
    {
        Vector3 direction = (holdPosition - getPadPositionFloat());

        if (direction.magnitude > 0.5f)
        {
            float magnitude = direction.magnitude < 3 ? direction.magnitude : 3;

            world.handle<CameraHandler>().pushCamera(direction.normalized * magnitude);

            holdPosition -= direction.normalized * magnitude * 0.01f;
        }
    }
    void beginMoveTilePad()
    {
        holdPosition = getPadPositionFloat();
        beginPosition = getPadPositionInt();

        pressTreshold = DateTime.Now;
        holdTreshold = DateTime.Now;
    }
    void endMoveTilePad()
    {
        Vector3Int position = getPadPositionInt();

        if (beginPosition == position && (DateTime.Now - pressTreshold).TotalMilliseconds < 250)
        {
            setSelectedPosition(position);
        }
    }
    public void setSelectedPosition(Vector3Int position)
    {
        if (selectPosition != position)
        {
            world.handle<MapHandler>().clearTilemap(MapLayer.Select);

            selectPosition = position;
            
            world.handle<AudioHandler>().playSoundActionA();
        }

        if (selectAction != null)
        {
            selectAction.Invoke();
            clearHover();
        }

        tryHighlightSelected();

        world.handle<MapHandler>().setTile(MapLayer.Select, position, select);
    }
    private void tryHighlightSelected()
    {
        Entity entity = world.handle<UserHandler>().withinEntityBounds(selectPosition);

        if (entity != null)
        {
            if (entity.type == EntityType.Product) getParentPanel<GamePanel>().showCraftableInfo(entity as ProductEntity);
            else if (entity.type == EntityType.Unit) getParentPanel<GamePanel>().showUnitInfo(entity as UnitEntity);
        }
        else getParentPanel<GamePanel>().clearInfo();
    }
    public Vector3Int getSelectedPosition() { return selectPosition; }
    public void setHover(BoundsInt bounds, TileBase[] tiles)
    {
        hoverTiles = tiles.Clone() as TileBase[];
        hoverBoundsInt = bounds;
    }
    public void clearHover()
    {
        hoverTiles = hover.Clone() as TileBase[];
        hoverBoundsInt = hoverBounds;
        selectAction = null;
    }
    public void setSelectAction(UnityAction action)
    {
        this.selectAction = action;
    }
}