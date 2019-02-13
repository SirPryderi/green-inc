using System.Linq;
using UnityEngine;

namespace Evaluators
{
    public class CityAttractivenessEvaluator : Evaluator
    {
        public override float Evaluate(HexCell cell)
        {
            if (cell.HasWater) return 0f;

            var score = 0f;

            score += MathExtension.GaussianProbability(cell.Temperature, 17, 15) * 60;

            score += MathExtension.GaussianProbability(cell.Elevation, 1, 2) * 10;

            var numberOfCoastalTiles = NumberOfCoastalNeighbours(cell);

            if (numberOfCoastalTiles == 6)
            {
                score -= 20;
            }
            else
            {
                score += numberOfCoastalTiles * 5f;
            }

            score += NumberOfSameHeightNeighbours(cell) * 5f;

            return Mathf.Clamp(score / 100f, 0f, 1f);
        }
    }
}