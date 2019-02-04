using UnityEngine;

public class GameMapSpawner : MonoBehaviour
{
    public HexGrid prefab;

    private void Awake()
    {
        var grid = FindObjectOfType<HexGrid>();
        if (grid != null)
        {
            // Make sure everything is bound to it
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

        Destroy(this);
    }
}