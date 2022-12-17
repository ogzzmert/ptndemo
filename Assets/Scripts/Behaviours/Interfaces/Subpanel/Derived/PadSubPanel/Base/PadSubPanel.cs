using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class PadSubPanel : SubPanel
{
    [field: SerializeField] private TileBase hover;
    [field: SerializeField] private TileBase select;
    GameHoldButton pad;
    Vector3 holdPosition;
    Vector3Int beginPosition, selectPosition, hoverPosition;
    DateTime pressTreshold, holdTreshold;
    public override void initialize<T>(World world, T parentPanel)
    {
        base.initialize(world, parentPanel);

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
        Vector3Int position = getPadPositionInt();

        if (hoverPosition != position)
        {
            world.handle<MapHandler>().clearTilemap(MapHandler.Layer.Hover);
            hoverPosition = position;
            world.handle<MapHandler>().setTile(MapHandler.Layer.Hover, hoverPosition, hover);
        }
    }
    void endHoverTilePad()
    {
        world.handle<MapHandler>().clearTilemap(MapHandler.Layer.Hover);
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
            selectTile(position);
        }
    }
    void selectTile(Vector3Int position)
    {
        if (selectPosition != position)
        {
            world.handle<MapHandler>().clearTilemap(MapHandler.Layer.Select);

            selectPosition = position;
            world.handle<AudioHandler>().playSoundActionA();
        }

        world.handle<MapHandler>().setTile(MapHandler.Layer.Select, position, select);
    }
    public Vector3Int getSelectedPosition() { return selectPosition; }
}