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
    }
}