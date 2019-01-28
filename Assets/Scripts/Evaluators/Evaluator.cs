using UnityEngine;

namespace Evaluators
{
    public abstract class Evaluator
    {
        public abstract Color CellToColour(HexCell value);

        public abstract float Evaluate(HexCell cell);
    }
}