using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct XLocation
{
    public const float A = -4.2f;
    public const float B = -3.0f;
    public const float C = -1.8f;
    public const float D = -0.6f;
    public const float E = 0.6f;
    public const float F = 1.8f;
    public const float G = 3.0f;
    public const float H = 4.2f;

    public static float GetLocation(int xPos)
    {
        switch (xPos)
        {
            case 1: return A;case 2: return B;case 3: return C;case 4: return D;case 5: return E;case 6: return F;case 7: return G;case 8: return H;
            default: return 0;
        }
    }

    public static int GetPosition(float xLocation)
    {
        return (int)((xLocation + 5) / 1.25)+1;
    }
}

public struct YLocation
{
    public const float Y1 = -4.2f;
    public const float Y2 = -3.0f;
    public const float Y3 = -1.8f;
    public const float Y4 = -0.6f;
    public const float Y5 = 0.6f;
    public const float Y6 = 1.8f;
    public const float Y7 = 3.0f;
    public const float Y8 = 4.2f;

    public static float GetLocation(int yPos)
    {
        switch (yPos) { case 1:return Y1;case 2:return Y2;case 3:return Y3;case 4:return Y4;case 5:return Y5;case 6:return Y6;case 7:return Y7;case 8:return Y8; default: return 0; }
    }

    public static int GetPosition(float yLocation)
    {
        return (int)((yLocation + 5) / 1.25)+1;
    }
}
