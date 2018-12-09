using Atmo;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public ClimateManager ClimateManager { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();

        ClimateManager = new ClimateManager();
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            SceneManager.LoadSceneAsync(0);
        }
    }
}