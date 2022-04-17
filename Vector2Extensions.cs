using UnityEngine;

public static  class Vector2Extensions
{
    public static Vector2 Rotate(this Vector2 vector, float degrees)
    {
        return Quaternion.Euler(0, 0, degrees) * vector;
    }
}