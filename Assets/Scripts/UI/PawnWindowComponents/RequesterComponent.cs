using Logistics;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PawnWindowComponents
{
    public class RequesterComponent : PawnWindowComponent
    {
        [HideInInspector] public Requester component;

        public Image icon;
        public Slider slider;
        public Image sliderImage;

        public Color bad = Color.red;
        public Color warn = Color.yellow;
        public Color good = Color.green;

        public override void Refresh()
        {
            var perc = component.SatisfiedPerc;
            icon.sprite = component.item.sprite;
            sliderImage.color = perc < 1f ? Color.Lerp(bad, warn, perc) : good;
            slider.value = perc;
        }
    }
}