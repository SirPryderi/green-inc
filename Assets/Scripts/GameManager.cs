using Atmo;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public ClimateManager ClimateManager { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();

        ClimateManager = new ClimateManager();
    }
}