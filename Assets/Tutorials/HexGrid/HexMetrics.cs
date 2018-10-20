using UnityEngine;

public static class HexMetrics
{
    private const float RadiusRatio = 0.866025404f;

    public const float outerRadius = 10f;

    public const float innerRadius = outerRadius * RadiusRatio;

    private static readonly Vector3[] corners =
    {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(0f, 0f, outerRadius)
    };

    public static Vector3 GetFirstCorner(HexDirection direction)
    {
        return corners[(int) direction];
    }

    public static Vector3 GetSecondCorner(HexDirection direction)
    {
        return corners[(int) direction + 1];
    }
}