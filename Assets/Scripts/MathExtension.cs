using UnityEngine;

public static class MathExtension
{
    public static float Remap(this float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }

    public static float Remap(this int from, float fromMin, float fromMax, float toMin, float toMax)
    {
        return ((float) from).Remap(fromMin, fromMax, toMin, toMax);
    }

    public static float GaussianDistribution(float x, float mean, float sigma)
    {
        var a = Mathf.Sqrt(2f * Mathf.PI * sigma * sigma);
        return 1f / a * GaussianProbability(x, mean, sigma);
    }

    public static float GaussianProbability(float x, float mean, float sigma)
    {
        var b = -Mathf.Pow(x - mean, 2);
        var c = 2f * sigma * sigma;

        return Mathf.Exp(b / c);
    }
}