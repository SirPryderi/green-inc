using UnityEngine;

public class RandomGenerator
{
    public int Seed { get; }

    public readonly float MapOffsetX;
    public readonly float MapOffsetY;

    public readonly float ClimateNoiseX;
    public readonly float ClimateNoiseY;

    public RandomGenerator(int seed)
    {
        Seed = seed;

        Random.InitState(seed);

        MapOffsetX = Random.Range(0f, 1024f);
        MapOffsetY = Random.Range(0f, 1024f);
        
        ClimateNoiseX = Random.Range(0f, 1024f);
        ClimateNoiseY = Random.Range(0f, 1024f);
    }

    public RandomGenerator(string seed)
    {
        Seed = System.Convert.ToInt32(seed);
    }
}