using Evaluators;
using UnityEngine;

namespace UI
{
    public class OverlayController : MonoBehaviour
    {
        public void SetOverlay(int overlay)
        {
            SetOverlay((Overlay) overlay);
        }

        public void SetOverlay(Overlay overlay)
        {
            var hexGrid = FindObjectOfType<HexGrid>();

            hexGrid.overlayType = overlay;

            switch (hexGrid.overlayType)
            {
                case Overlay.CITY_ATTRACTIVENESS:
                    hexGrid.evaluator = new CityAttractivenessEvaluator();
                    break;
                case Overlay.SOIL_FERTILITY:
                    hexGrid.evaluator = new SoilFertilityEvaluator();
                    break;
                default:
                    hexGrid.evaluator = new CellColourEvaluator();
                    break;
            }

            if (hexGrid.cells == null) return;

            hexGrid.Refresh();
        }
    }
}