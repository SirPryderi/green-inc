using UnityEngine;
using UnityEngine.UI;

namespace UI.PawnWindowComponents
{
    public class TreeComponent : PawnWindowComponent
    {
        [HideInInspector] public Pawns.Trees.Tree component;

        public Text co2;

        public override void Refresh()
        {
            co2.text = FormatEmissions(-component.absorbedCo2);
        }
    }
}