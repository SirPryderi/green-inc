using UnityEngine;

public class HexGridGenerator
{
    private readonly HexGrid _grid;

    public HexGridGenerator(HexGrid grid)
    {
        _grid = grid;
    }

    public void Flatten(int elevation)
    {
        foreach (var cell in _grid.cells)
        {
            cell.Elevation = elevation;
        }

        _grid.Refresh();
    }

    public void GenerateFromPerlin()
    {
        const float scaleFactor = 15f;

        var xOffset = GameManager.Instance.MapManager.RandomGenerator.MapOffsetX;
        var yOffset = GameManager.Instance.MapManager.RandomGenerator.MapOffsetY;

        foreach (var cell in _grid.cells)
        {
            var perlinNoise = Mathf.PerlinNoise(
                cell.coordinates.X / scaleFactor + xOffset,
                cell.coordinates.Y / scaleFactor + yOffset
            );

            perlinNoise = Mathf.Clamp(perlinNoise * 10 - 2, 0, 10);

            cell.Elevation = Mathf.FloorToInt(perlinNoise);
        }

        _grid.Refresh();
    }
}