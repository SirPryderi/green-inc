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
            if (emissions > 1000)
            {
                return $"{emissions/1000:N} tonnes/h";
            } 
            
            return $"{emissions:N} kg/h";
        }
    }
}