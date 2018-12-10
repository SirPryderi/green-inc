using UnityEngine;

public class HexGridGenerator
{
    private readonly HexGrid grid;

    public HexGridGenerator(HexGrid grid)
    {
        this.grid = grid;
    }

    public void Flatten(int elevation)
    {
        foreach (var cell in grid.cells)
        {
            cell.Elevation = elevation;
        }

        grid.Refresh();
    }

    public void GenerateFromPerlin()
    {
        var scaleFactor = 15f;
        var xOffset = Random.Range(0f, 1024f);
        var yOffset = Random.Range(0f, 1024f);

        foreach (var cell in grid.cells)
        {
            var perlinNoise = Mathf.PerlinNoise(cell.coordinates.X / scaleFactor + xOffset,
                cell.coordinates.Y / scaleFactor + yOffset);

            perlinNoise = Mathf.Clamp(perlinNoise * 10 - 2, 0, 10);

            cell.Elevation = Mathf.FloorToInt(perlinNoise);
        }

        grid.Refresh();
    }
}