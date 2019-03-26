using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public MapManager MapManager { get; private set; }
    private bool _uiVisible = true;

    protected override void OnAwake()
    {
        base.OnAwake();
        Reset();
    }

    public void Reset()
    {
        MapManager = new MapManager();
    }

    public void ResetMap(HexGrid grid)
    {
        Reset();
        MapManager.Grid = grid;
        Instance.MapManager.Randomise();
        new HexGridGenerator(grid).GenerateFromPerlin();
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

        if (Input.GetKeyDown("h"))
        {
            ToggleUI();
        }
    }

    private void ToggleUI()
    {
        var sceneName = SceneManager.GetActiveScene().name;

        if (sceneName != "Game" && sceneName != "Scene")
        {
            return;
        }

        _uiVisible = !_uiVisible;

        var objects = FindGameObjectsInLayer(LayerMask.NameToLayer("UI"));

        foreach (var obj in objects)
        {
            obj.transform.localScale = _uiVisible ? Vector3.one : Vector3.zero;
        }
    }

    public static IEnumerable<GameObject> FindGameObjectsInLayer(int layer)
    {
        var goList = new List<GameObject>();

        if (FindObjectsOfType(typeof(GameObject)) is GameObject[] goArray)
        {
            goList.AddRange(goArray.Where(obj => obj.layer == layer));
        }

        return goList.ToArray();
    }

    public static void ToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}