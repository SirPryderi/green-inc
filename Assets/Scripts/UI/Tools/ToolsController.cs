using System.Collections;
using Pawns;
using UnityEngine;

namespace UI.Tools
{
    public class ToolsController : MonoBehaviour
    {
        private ToolType _tool = ToolType.NONE;
        private Pawn _pawnBrush;

        public int bulldozeCost;

        public Texture2D destroyCursor;
        public Texture2D buildCursor;
        public Texture2D buyCursor;

        public void Reset()
        {
            SetTool(ToolType.NONE);
            _pawnBrush = null;
        }

        public void SetTool(ToolType tool, Pawn pawn = null)
        {
            _tool = tool;
            _pawnBrush = pawn;

            switch (tool)
            {
                case ToolType.BULLDOZER:
                    Cursor.SetCursor(destroyCursor, Vector2.zero, CursorMode.Auto);
                    break;
                case ToolType.BUY:
                    Cursor.SetCursor(buyCursor, Vector2.zero, CursorMode.Auto);
                    break;
                case ToolType.BUILD:
                    Cursor.SetCursor(buildCursor, Vector2.zero, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    break;
            }
        }

        public bool IsToolSet()
        {
            return _tool != ToolType.NONE;
        }

        public IEnumerator HandleInput()
        {
            if (Input.GetMouseButton(1))
            {
                Reset();
                yield return null;
            }

            var inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(inputRay, out var hit)) yield break;

            var cell = G.MP.Grid.GetCell(hit.point);

            var success = false;

            switch (_tool)
            {
                case ToolType.BULLDOZER:
                    if (Input.GetMouseButton(0))
                        success = BulldozeCell(cell);
                    break;
                case ToolType.BUY:
                    if (Input.GetMouseButton(0))
                        success = BuyPawn(cell);
                    break;
                case ToolType.BUILD:
                    if (Input.GetMouseButtonDown(0))
                        success = BuildPawn(cell, _pawnBrush);
                    break;
                case ToolType.NONE:
                    if (Input.GetMouseButtonDown(0))
                        GetComponent<MainUIController>().Focus(cell.Pawn);
                    break;
            }

            if (success && !Input.GetKey(KeyCode.LeftShift))
            {
                Reset();
            }

            GetComponent<MainUIUpdater>().UpdateBalance();

            yield return null;
        }

        private static bool BuyPawn(HexCell cell)
        {
            if (cell.IsClear()) return false;

            var pawn = cell.Pawn;

            if (pawn.owner == G.PC) return false;

            var price = pawn.price * 10;

            if (G.PC.CannotAfford(price)) return false;

            G.PC.TransferMoney(pawn.owner, price);

            pawn.owner = G.PC;
            return true;
        }

        private static bool BuildPawn(HexCell cell, Pawn pawn)
        {
            if (!cell.IsClear()) return false;

            return G.PC.BuildPawn(cell, pawn);
        }

        private bool BulldozeCell(HexCell cell)
        {
            // The cell has nothing to destroy
            if (cell.IsClear()) return false;

            // Makes sure you can only bulldoze nobody's or your pawns
            if (cell.Pawn.owner != null && cell.Pawn.owner != G.PC) return false;

            // Checks if the price can be paid
            if (G.PC.CannotAfford(bulldozeCost)) return false;

            G.PC.ConsumeMoney(bulldozeCost);
            cell.Clear();
            return true;
        }
    }
}