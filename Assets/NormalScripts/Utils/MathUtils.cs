using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    public static int Between(this int val,int min,int max)
    {
        val = Math.Max(min, val);
        val = Math.Min(max, val);
        return val;
    }
}
