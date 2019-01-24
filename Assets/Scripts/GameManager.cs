using Atmo;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public MapManager MapManager { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();

        MapManager = new MapManager();
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            SceneManager.LoadSceneAsync(0);
        }
    }
}