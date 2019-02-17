using Pawns;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PawnWindowComponents
{
    public class CityTileComponent : PawnWindowComponent
    {
        [HideInInspector] public CityTile component;

        public Text population;
        public Text co2;

        public override void Refresh()
        {
            population.text = $"{component.Population:##,###}/{component.maxPopulation:##,###}";
            co2.text = FormatEmissions(component.Population * component.EmissionsPerCapita);
        }
    }
}