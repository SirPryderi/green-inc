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
}