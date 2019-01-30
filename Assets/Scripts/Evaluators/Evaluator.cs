using System;
using UnityEngine;

namespace Evaluators
{
    public abstract class Evaluator
    {
        public abstract Color CellToColour(HexCell value);

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
    }
}