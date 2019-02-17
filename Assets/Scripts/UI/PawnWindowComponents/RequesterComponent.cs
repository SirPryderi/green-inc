using Logistics;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PawnWindowComponents
{
    public class RequesterComponent : PawnWindowComponent
    {
        [HideInInspector] public Requester component;

        public Image icon;
        public BarIndicator bar;

        public override void Refresh()
        {
            icon.sprite = component.item.sprite;
            bar.Value = component.SatisfiedPerc;
        }
    }
}