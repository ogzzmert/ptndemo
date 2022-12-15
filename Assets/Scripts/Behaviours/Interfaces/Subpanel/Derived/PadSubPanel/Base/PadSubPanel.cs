using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class PadSubPanel : SubPanel
{
    [field: SerializeField] private TileBase hover;
    GameHoldButton pad;
    Vector3Int beginPosition, hoverPosition;
    public override void initialize<T>(World world, T parentPanel)
    {
        base.initialize(world, parentPanel);

        pad = getOther<GameHoldButton>("pad");

        pad.onClick(moveTilePad);
        pad.onClickDown(beginMoveTilePad);
        pad.onClickUp(endMoveTilePad);
    }
    private Vector3Int getPadPosition() { return Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(InputManager.getPrimaryPosition())); }
    public void moveTilePad()
    {
        
    }
    public void beginMoveTilePad()
    {
        beginPosition = getPadPosition();
    }
    public void endMoveTilePad()
    {
        Vector3Int position = getPadPosition();

        if (beginPosition == position)
        {
            selectTile(position);
        }
    }
    private void selectTile(Vector3Int position)
    {

        if (hoverPosition != position)
        {
            world.handle<MapHandler>().clearTilemap(MapHandler.Layer.Hover);

            hoverPosition = position;
            world.handle<AudioHandler>().playSoundActionA();
        }

        world.handle<MapHandler>().setTile(MapHandler.Layer.Hover, position, hover);
    }
}