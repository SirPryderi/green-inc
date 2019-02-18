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

        public Texture2D destroyCursor;
        public Texture2D buildCursor;
        public Texture2D buyCursor;

        public Canvas canvas;
        public PawnWindow pawnWindowPrefab;

        public Pawn Focused { get; private set; }

        public void TogglePause()
        {
            isPaused = !isPaused;

            GetComponent<MainUIUpdater>().UpdateClock();
        }

        public void Advance()
        {
            G.O.AdvanceTime(12);
        }

        public void SetBrush(BrushType brush)
        {
            _brush = brush;

            switch (brush)
            {
                case BrushType.NONE:
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    break;
                case BrushType.BULLDOZER:
                    Cursor.SetCursor(destroyCursor, Vector2.zero, CursorMode.Auto);
                    break;
                case BrushType.BUY:
                    Cursor.SetCursor(buyCursor, Vector2.zero, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(buildCursor, Vector2.zero, CursorMode.Auto);
                    break;
            }
        }

        public void SetBrush(int brush)
        {
            SetBrush((BrushType) brush);
        }

        private void Update()
        {
            if (Input.anyKey)
            {
                HandleKeyInput();
            }

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
            {
                StartCoroutine(nameof(HandleInput));
            }
        }

        private void HandleKeyInput()
        {
            if (Input.GetKeyDown("escape"))
            {
                HandleEscapeButton();
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                G.O.AdvanceTime(1);
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                G.O.AdvanceTime(24);
            }

            if (Input.GetKeyDown(KeyCode.F3))
            {
                G.O.AdvanceTime(24 * 30);
            }

            if (Input.GetKeyDown(KeyCode.F4))
            {
                G.O.AdvanceTime(24 * 365);
            }

            if (Input.GetKeyDown(KeyCode.F5))
            {
                G.O.AdvanceTime(24 * 365 * 10);
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                G.O.AdvanceTime(24 * 365 * 100);
            }
        }

        private void HandleEscapeButton()
        {
            var window = FindObjectOfType<Window>();

            if (window != null)
            {
                // Close one window and absorb the event
                Destroy(window.gameObject);
                return;
            }

            if (_brush != BrushType.NONE)
            {
                // Unset the brush and absorb the event
                SetBrush(BrushType.NONE);
                return;
            }

            if (Focused != null)
            {
                // Unset the focus and absorb the event
                Focus(null);
                return;
            }

            // Finally, if nothing has been done, quit to main menu.
            GameManager.ToMainMenu();
        }

        private IEnumerator HandleInput()
        {
            var inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(inputRay, out var hit)) yield break;

            var cell = G.MP.Grid.GetCell(hit.point);

            switch (_brush)
            {
                case BrushType.BULLDOZER:
                    if (Input.GetMouseButton(0))
                        BulldozeCell(cell);
                    break;
                case BrushType.BUY:
                    if (Input.GetMouseButton(0))
                        BuyPawn(cell);
                    break;
                case BrushType.WIND_TURBINE:
                    if (Input.GetMouseButtonDown(0))
                        BuyNewPawn(cell, "Pawns/Electrical/WindTurbine");
                    break;
                case BrushType.COAL_PLANT:
                    if (Input.GetMouseButtonDown(0))
                        BuyNewPawn(cell, "Pawns/Electrical/ElectricalPlant");
                    break;
                case BrushType.TREE:
                    if (Input.GetMouseButton(0))
                        BuyNewPawn(cell, "Pawns/Trees/PineTree");
                    break;
                case BrushType.CROP:
                    if (Input.GetMouseButtonDown(0))
                        BuyNewPawn(cell, "Pawns/Crop");
                    break;
                case BrushType.NONE:
                    if (Input.GetMouseButtonDown(0))
                        Focus(cell.Pawn);
                    break;
            }

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                SetBrush(BrushType.NONE);
            }

            GetComponent<MainUIUpdater>().UpdateBalance();

            yield return null;
        }

        public void Focus(Pawn pawn)
        {
            if (Focused != null) Focused.UnFocus();

            Focused = pawn;

            CloseAllPawnWindows();

            if (pawn != null)
            {
                OpenPawnWindow(pawn);
                Focused.Focus();
            }
        }

        private void OpenPawnWindow(Pawn pawn)
        {
            pawnWindowPrefab.pawn = pawn;
            Instantiate(pawnWindowPrefab, canvas.transform);
        }

        private static void CloseAllPawnWindows()
        {
            foreach (var components in FindObjectsOfType<PawnWindow>())
            {
                Destroy(components.gameObject);
            }
        }

        private static void BuyPawn(HexCell cell)
        {
            if (cell.IsClear()) return;

            var pawn = cell.Pawn;

            if (pawn.owner == G.PC) return;

            var price = pawn.price * 10;

            if (G.PC.CannotAfford(price)) return;

            G.PC.TransferMoney(pawn.owner, price);

            pawn.owner = G.PC;
        }

        private static void BuyNewPawn(HexCell cell, string pawn)
        {
            if (!cell.IsClear()) return;

            G.PC.BuyPawn(cell, pawn);
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