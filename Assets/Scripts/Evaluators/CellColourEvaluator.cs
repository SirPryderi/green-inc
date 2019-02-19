using UnityEngine;

namespace Evaluators
{
    public class CellColourEvaluator : Evaluator
    {
        public override Color CellToColour(HexCell cell)
        {
            var temp = cell.Temperature;
            var seaColor = new Color(0, 0.3117442f, 1);
            var sandColor = new Color(0.94f, 0.73f, 0.38f);

            // TODO improve code

            const int desert = 35;
            const int temperate = 25;
            const int tundra = 10;
            const int ice = 0;

            Color color;

            if (cell.Elevation == 0)
            {
                color = seaColor;
            }
            else if (temp > desert)
            {
                color = sandColor;
            }
            else if (temp > temperate)
            {
                var f = temp.Remap(temperate, desert, 0, 1);
                color = Color.Lerp(new Color(0.074f, 0.427f, 0.082f), sandColor, f);
            }
            else if (temp > tundra)
            {
                var f = temp.Remap(tundra, temperate, 0, 1);
                color = Color.Lerp(new Color(0.254f, 0.596f, 0.039f), new Color(0.074f, 0.427f, 0.082f), f);
            }
            else if (temp > ice)
            {
                var f = temp.Remap(ice, tundra, 0, 1);
                color = Color.Lerp(Color.white, new Color(0.254f, 0.596f, 0.039f), f);
            }
            else
            {
                color = Color.white;
            }


            return color;
        }

        public override float Evaluate(HexCell cell)
        {
            return cell.Temperature;
        }
    }
}