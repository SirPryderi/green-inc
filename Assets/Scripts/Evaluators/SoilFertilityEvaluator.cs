using UnityEngine;

namespace Evaluators
{
    public class SoilFertilityEvaluator : Evaluator
    {
        public override float Evaluate(HexCell cell)
        {
            if (cell.HasWater) return 0f;
            var temp = cell.Temperature;
            if (temp < 10) return 0f;
            var score = 0f;

            // 60% Temperature
            // 20% Elevation
            // 20% Plain terrain

            score += MathExtension.GaussianProbability(temp, 20, 10) * 0.60f;
            score += MathExtension.GaussianProbability(cell.Elevation, 1, 2) * 0.20f;
            score += NumberOfSameHeightNeighbours(cell) / 6f * 0.20f;

            return Mathf.Clamp01(score);
        }
    }
}