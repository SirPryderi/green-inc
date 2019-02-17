using UnityEngine;

namespace UI.PawnWindowComponents
{
    public abstract class PawnWindowComponent : MonoBehaviour
    {
        [HideInInspector] public PawnWindow parent;

        private void Awake()
        {
            Refresh();
        }

        public abstract void Refresh();

        protected static string FormatEmissions(float emissions)
        {
            return $"{emissions:0.00} kg/h";
        }
    }
}