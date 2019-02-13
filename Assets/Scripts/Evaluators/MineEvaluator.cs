using UnityEngine;
using Tree = Pawns.Trees.Tree;

namespace Evaluators
{
    public class MineEvaluator : Evaluator
    {
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

            score += MathExtension.GaussianProbability(NumberOfSameHeightNeighbours(cell), 5, 2) * 30;

            return Mathf.Clamp01(score / 100f);
        }
    }
}