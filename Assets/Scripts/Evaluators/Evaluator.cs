using System;
using System.Linq;
using UnityEngine;

namespace Evaluators
{
    public abstract class Evaluator
    {
        public virtual Color CellToColour(HexCell value)
        {
            return HeatMap(Evaluate(value));
        }

        public abstract float Evaluate(HexCell cell);

        public static Color HeatMap(float value, float min = 0, float max = 1)
        {
            if (Mathf.Abs(value) < 0.001f) return Color.black;

            var val = (value - min) / (max - min);

            return new Color(val, 1 - val, 0);
        }

        public Tuple<float, HexCell>[] EvaluateAll(HexGrid grid)
        {
            var dictionary = new Tuple<float, HexCell>[grid.NumberOfCells];

            for (var i = 0; i < grid.cells.Length; i++)
            {
                var cell = grid.cells[i];

                dictionary[i] = new Tuple<float, HexCell>(Evaluate(cell), cell);
            }

            SortAll(dictionary);

            return dictionary;
        }

        public static void SortAll(Tuple<float, HexCell>[] dictionary)
        {
            Array.Sort(dictionary, (a, b) => (int) ((b.Item1 - a.Item1) * 1000));
        }

        protected static int NumberOfCoastalNeighbours(HexCell cell)
        {
            return cell.neighbors.Count(neighbor => neighbor != null && neighbor.HasWater);
        }

        protected static int NumberOfSameHeightNeighbours(HexCell cell)
        {
            var numberOfTilesAtSameHeight = cell.neighbors.Count
            (
                neighbor => (neighbor != null && neighbor.Elevation == cell.Elevation)
            );
            return numberOfTilesAtSameHeight;
        }
    }
}