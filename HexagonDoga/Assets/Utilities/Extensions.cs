using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static bool IsEqualColor(this Color myColor, Color otherColor)
    {
        Color32 myCol = myColor;
        Color32 otherCol = otherColor;

        return myCol.r == otherCol.r && myCol.g == otherCol.g && myCol.b == otherCol.b && myCol.a == otherCol.a;
    }
}
