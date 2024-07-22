using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PositionConverter
{
    public static float ConvertToTransform(int arrayNumber)
    {
        return 1.25f * arrayNumber - 4.375f;
    }

    public static int ConvertToArrayNumber(float transformPosition)
    {
        return Mathf.RoundToInt(transformPosition * 0.8f + 3.5f);
    }
}
