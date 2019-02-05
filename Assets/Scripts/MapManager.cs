using Atmo;
using Mechanics;
using UnityEngine;

public class MapManager
{
    public ClimateManager ClimateManager { get; }

    public RandomGenerator RandomGenerator { get; private set; }

    public Observer Observer { get; }

    public HexGrid Grid;

    public MapManager()
    {
        ClimateManager = new ClimateManager();
        RandomGenerator = new RandomGenerator(0);
        Observer = new Observer();
    }

    public void Randomise()
    {
        RandomGenerator = new RandomGenerator(System.Environment.TickCount);
    }
}