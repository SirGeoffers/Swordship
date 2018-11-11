using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordship.Utility {

    public struct Range<T> {
        public T min;
        public T max;
    }

}

public static class Vector2Extension {

    public static Vector2 Rotate(this Vector2 v, float degrees) {

        Vector2 result = new Vector2();

        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        result.x = (cos * tx) - (sin * ty);
        result.y = (sin * tx) + (cos * ty);

        return result;

    }
}