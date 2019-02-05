using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMapSpawner : MonoBehaviour
{
    public HexGrid prefab;

    private void Awake()
    {
        var grid = FindObjectOfType<HexGrid>();
        if (grid != null)
        {
            // Moves the grid to the current scene so that it won't be kept on load
            SceneManager.MoveGameObjectToScene(grid.gameObject, SceneManager.GetActiveScene());
        }
        else
        {
            prefab.width = 50;
            prefab.height = 30;

            grid = Instantiate(prefab);

            var gen = new HexGridGenerator(grid);

            GameManager.Instance.MapManager.Randomise();
            gen.GenerateFromPerlin();
        }

        Destroy(gameObject);
    }
}