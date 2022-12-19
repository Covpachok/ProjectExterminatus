using System;
using UnityEngine;

public static class Utils
{
    public static Quaternion LookAt2D(Vector3 direction)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);
    }

    public static Quaternion LookAt2D(Vector3 from, Vector3 to) => LookAt2D(to - from);

    public static Vector3 Vector3Lerp(Vector3 start, Vector3 end, float delta)
    {
        return new Vector3
        {
            x = Mathf.Lerp(start.x, end.x, delta),
            y = Mathf.Lerp(start.y, end.y, delta),
            z = Mathf.Lerp(start.z, end.z, delta)
        };
    }
}