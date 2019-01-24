using Atmo;
using UnityEngine;

public class MapManager
{
    public ClimateManager ClimateManager { get; }

    public RandomGenerator RandomGenerator { get; private set; }

    public MapManager()
    {
        ClimateManager = new ClimateManager();
        RandomGenerator = new RandomGenerator(0);
    }

    public void Randomise()
    {
        RandomGenerator = new RandomGenerator(System.Environment.TickCount);
    }
}