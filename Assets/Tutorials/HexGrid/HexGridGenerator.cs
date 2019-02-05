using System;
using Evaluators;
using Organisations;
using Pawns;
using UnityEngine;
using Random = UnityEngine.Random;

public class HexGridGenerator
{
    private const int NumberOfCities = 5;
    private const int NumberOfMines = 10;
    private readonly HexGrid _grid;

    public HexGridGenerator(HexGrid grid)
    {
        _grid = grid;
    }

    public void Flatten(int elevation)
    {
        foreach (var cell in _grid.cells) cell.Elevation = elevation;

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

        GenerateTrees();
        GenerateCities();
        GenerateMines();
    }

    private void GenerateMines()
    {
        var results = new MineEvaluator().EvaluateAll(_grid);

        for (var i = 0; i < NumberOfMines; i++)
        {
            var tile = results[i].Item2;

            tile.Clear();
            tile.Spawn("CoalMine");
        }
    }

    private void GenerateCities()
    {
        var evaluator = new CityAttractivenessEvaluator();
        var results = evaluator.EvaluateAll(_grid);

        for (var i = 0; i < NumberOfCities; i++)
        {
            if (Math.Abs(results[0].Item1) < 0.001)
            {
                throw new Exception("Unable to find suitable tile for city.");
            }

            var city = new City($"City {i}");

            var tile = results[0].Item2;
            tile.Clear();
            var cityTile = tile.Spawn("City");
            cityTile.GetComponent<CityTile>().City = city;

            // Makes nearby cells less attractive
            UpdateResults(results, tile);
        }
    }

    private void UpdateResults(Tuple<float, HexCell>[] results, HexCell tile)
    {
        // Makes the current tile not unusable
        results[0] = new Tuple<float, HexCell>(0, tile);

        // Weights the each cell's score based on the new added city
        for (var j = 0; j < results.Length; j++)
        {
            var (score, tCell) = results[j];
            var distance = (tile.transform.position - tCell.transform.position).magnitude;
            var newScore = score - 1f / distance;

            newScore = Mathf.Clamp01(newScore);

            results[j] = new Tuple<float, HexCell>(newScore, tCell);
        }

        // Sorts the results again so that the best cell will be on top of the queue
        Evaluator.SortAll(results);
    }

    public void GenerateTrees()
    {
        Random.InitState(GameManager.Instance.MapManager.RandomGenerator.Seed + "trees".GetHashCode());

        foreach (var cell in _grid.cells)
        {
            if (cell.Elevation <= 0) continue;

            if (Random.value < MathExtension.GaussianProbability(cell.Temperature, 20, 5))
            {
                var tree = cell.Spawn("BroadLeafTree");

                var scale = Random.Range(0.7f, 1.5f);

                tree.transform.localScale = new Vector3(scale, scale, scale);

                tree.transform.Rotate(Vector3.up, Random.Range(0f, 360f), Space.Self);
            }
            else if (Random.value < MathExtension.GaussianProbability(cell.Temperature, 7, 5))
            {
                var tree = cell.Spawn("PineTree");

                var scale = Random.Range(0.7f, 1.5f);

                tree.transform.localScale = new Vector3(scale, scale, scale);

                tree.transform.Rotate(Vector3.up, Random.Range(0f, 360f), Space.Self);
            }
            else if (Random.value + 0.5f < MathExtension.GaussianProbability(cell.Temperature, 35, 10))
            {
                var tree = cell.Spawn("Cactus");

                var scale = Random.Range(1f, 1.5f);

                tree.transform.localScale = new Vector3(scale, scale, scale);

                tree.transform.Rotate(Vector3.up, Random.Range(0f, 360f), Space.Self);
            }
        }
    }
}