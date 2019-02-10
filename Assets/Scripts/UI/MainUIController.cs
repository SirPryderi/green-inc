using System.Collections;
using Pawns;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class MainUIController : MonoBehaviour
    {
        public int bulldozeCost;
        public bool isPaused = false;

        private BrushType _brush = BrushType.NONE;

        public void TogglePause()
        {
            isPaused = !isPaused;

            GetComponent<MainUIUpdater>().UpdateClock();
        }

        public void Advance()
        {
            GameManager.Instance.MapManager.Observer.AdvanceTime(12);
        }

        public void SetBrush(BrushType brush)
        {
            _brush = brush;
        }

        public void SetBrush(int brush)
        {
            SetBrush((BrushType) brush);
        }

        private void Update()
        {
            if (_brush == BrushType.NONE)
                return;

            if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
            {
                StartCoroutine(nameof(HandleInput));
            }
        }

        private IEnumerator HandleInput()
        {
            var inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(inputRay, out var hit)) yield break;

            var cell = G.MP.Grid.GetCell(hit.point);

            if (_brush == BrushType.BULLDOZER || cell.IsClear())
            {
                switch (_brush)
                {
                    case BrushType.BULLDOZER:
                        BulldozeCell(cell);
                        break;
                    case BrushType.WIND_TURBINE:
                        BuyPawn(cell, "WindTurbine");
                        break;
                    case BrushType.COAL_PLANT:
                        BuyPawn(cell, "ElectricalPlant");
                        break;
                    case BrushType.TREE:
                        BuyPawn(cell, "PineTree");
                        break;
                    case BrushType.NONE:
                    default:
                        break;
                }

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    _brush = BrushType.NONE;
                }

                GetComponent<MainUIUpdater>().UpdateBalance();
            }

            yield return null;
        }

        private void BuyPawn(HexCell cell, string pawn)
        {
            var price = Pawn.Load(pawn).price;
            if (G.PC.CannotAfford(price)) return;
            G.PC.ConsumeMoney(price);
            cell.Spawn(pawn, G.PC);
        }

        private void BulldozeCell(HexCell cell)
        {
            // The cell has nothing to destroy
            if (cell.IsClear()) return;

            // Makes sure you can only bulldoze nobody's or your pawns
            if (cell.Pawn.owner != null && cell.Pawn.owner != G.PC) return;

            // Checks if the price can be paid
            if (G.PC.CannotAfford(bulldozeCost)) return;

            G.PC.ConsumeMoney(bulldozeCost);
            cell.Clear();
        }
    }
}