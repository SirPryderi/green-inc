using UI;
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
            if (FindObjectOfType<MainUIController>() == null)
            {
                ToMainMenu();
            }
        }
    }

    public static void ToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}