using System.Linq;
using UnityEngine;

namespace Evaluators
{
    public class MineEvaluator : Evaluator
    {
        public override Color CellToColour(HexCell value)
        {
            return HeatMap(Evaluate(value));
        }

        public override float Evaluate(HexCell cell)
        {
            if (cell.HasWater) return 0f;

            if (cell.transform.childCount > 0)
            {
                var child = cell.transform.GetChild(0);

                if (child.GetComponent<Tree>() == null)
                {
                    return 0f;
                }
            }

            var score = 0f;

            score += MathExtension.GaussianProbability(cell.Temperature, 17, 20) * 35;

            score += MathExtension.GaussianProbability(cell.Elevation, 3, 2) * 35;

            var numberOfTilesAtSameHeight = cell.neighbors.Count
            (
                neighbor => (neighbor != null && neighbor.Elevation == cell.Elevation)
            );

            score += MathExtension.GaussianProbability(numberOfTilesAtSameHeight, 5, 2) * 30;

            return Mathf.Clamp01(score / 100f);
        }
    }
}