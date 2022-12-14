// Mert Oguz - 2022 demo project

using System;
using UnityEngine;

public class Calculator
{
    public static System.Random random {get; private set;} = new System.Random();
    static Vector2 normVector = new Vector2(0, -1);
    private Calculator() { }
    public static float getDistance(float x, float z, float x_, float z_)
    {
        float result = (x - x_) * (x - x_) + (z - z_) * (z - z_);
        if (result < 0) result *= -1;
        return result;
    }
    public static float getDistance(Vector3 one, Vector3 another)
    {
        return getDistance(one.x, one.z, another.x, another.z);
    }
    public static float getAngle(Vector2 position)
    {
        return -Vector2.SignedAngle(normVector, position);
    }
    public static float getAngle(Vector2 one, Vector2 another)
    {
        return getAngle(one - another);
    }
    public static GameObject getChild(Transform transform, int[] arr)
    {
        // get the child recursively from a game object without failure
        Transform child = transform;
        
        if (arr != null && arr.Length > 0)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (child.childCount > arr[i])
                {
                    child = child.GetChild(arr[i]);
                }
                else break;
            }
        }
        return child.gameObject;
    }
    public static bool withinBounds(Vector3Int position, Vector3Int targetPosition, BoundsInt targetBounds)
    {
        for(int i = targetPosition.x; i < targetPosition.x + targetBounds.size.x; i++)
        {
            for(int j = targetPosition.y; j < targetPosition.y + targetBounds.size.y; j++)
            {
                Vector3Int boundPosition = new Vector3Int(i, j, 0);

                if (boundPosition == position) return true;
            }
        }
        return false;
    }
}