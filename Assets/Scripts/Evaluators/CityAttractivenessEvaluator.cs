using System.Linq;
using UnityEngine;

namespace Evaluators
{
    public class CityAttractivenessEvaluator : Evaluator
    {
        public override Color CellToColour(HexCell value)
        {
            return HeatMap(Evaluate(value));
        }

        public override float Evaluate(HexCell cell)
        {
            if (cell.HasWater) return 0f;

            var score = 0f;

            score += MathExtension.GaussianProbability(cell.Temperature, 15, 10) * 60;

            score += MathExtension.GaussianProbability(cell.Elevation, 1, 2) * 10;

            var numberOfCoastalTiles = cell.neighbors.Count(neighbor => neighbor != null && neighbor.HasWater);

            if (numberOfCoastalTiles == 6)
            {
                score -= 20;
            }
            else
            {
                score += numberOfCoastalTiles * 5f;
            }

            var numberOfTilesAtSameHeight = cell.neighbors.Count
            (
                neighbor => (neighbor != null && neighbor.Elevation == cell.Elevation)
            );

            score += numberOfTilesAtSameHeight * 5f;

            return Mathf.Clamp(score / 100f, 0f, 1f);
        }
    }
}