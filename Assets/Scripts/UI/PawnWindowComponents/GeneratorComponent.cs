using Logistics;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PawnWindowComponents
{
    public class GeneratorComponent : PawnWindowComponent
    {
        [HideInInspector] public Provider component;

        public Image icon;
        public BarIndicator bar;
        public Text co2;

        public override void Refresh()
        {
            icon.sprite = component.item.sprite;

            bar.Value = component.ProductivityPercentage;
            bar.Text = $"{component.ProducedInPastHour:N}/{component.ItemsPerHour:N}";

            co2.text = FormatEmissions(component.CO2InPastHour);
        }
    }
}