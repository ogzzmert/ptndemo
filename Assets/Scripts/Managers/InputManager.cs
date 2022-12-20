// Mert Oguz - 2022 demo project

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public static InputType platform { get; private set; }
    public enum InputType
    {
        Desktop = 0,
        Mobile = 1
    }
    private InputManager() { }
    public static void load()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) platform = InputType.Mobile;
        else platform = InputType.Desktop;
    }
    public static Vector3 getPrimaryPosition()
    {
        switch(platform)
        {
            case InputType.Desktop:
                return Input.mousePosition;
            case InputType.Mobile:
                if (Input.touchCount > 0) return Input.touches[0].position;
                else break;
        }
        return Vector3.zero;
    }
}