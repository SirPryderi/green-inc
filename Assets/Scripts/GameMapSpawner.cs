using UnityEngine;
using UnityEngine.SceneManagement;


public class GameMapSpawner : MonoBehaviour
{
    public HexGrid prefab;

    [Range(1, 50)] public int chunkSizeX = 15;
    [Range(1, 50)] public int chunkSizeY = 10;

    private void Awake()
    {
        G.MP.Statistics.TakeSnapshot();

        var grid = FindObjectOfType<HexGrid>();
        if (grid != null)
        {
            // Moves the grid to the current scene so that it won't be kept on load
            SceneManager.MoveGameObjectToScene(grid.gameObject, SceneManager.GetActiveScene());
        }
        else
        {
            prefab.chunkCountX = chunkSizeX;
            prefab.chunkCountZ = chunkSizeY;

            grid = Instantiate(prefab);

            G.GM.ResetMap(grid);
        }

        Destroy(gameObject);
    }
}