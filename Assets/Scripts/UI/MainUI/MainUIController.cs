using System.Collections;
using Pawns;
using UI.DynamicMenus;
using UI.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class MainUIController : MonoBehaviour
    {
        public bool isPaused;

        public Canvas canvas;
        public PawnWindow pawnWindowPrefab;
        public Window resourcesWindowPrefab;

        public DynamicMenu menu;
        public GameObject menuContainer;

        public Pawn Focused { get; private set; }

        private ToolsController tools;

        private void Start()
        {
            tools = GetComponent<ToolsController>();
            menu.Generate(menuContainer);
        }

        public void TogglePause()
        {
            isPaused = !isPaused;

            GetComponent<MainUIUpdater>().UpdateClock();
        }

        public void Advance()
        {
            G.O.AdvanceTime(12);
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
                StartCoroutine(nameof(DispatchToolController));
            }
        }

        private IEnumerator DispatchToolController()
        {
            return tools.HandleInput();
        }

        private void HandleKeyInput()
        {
            if (Input.GetKeyDown("escape"))
            {
                HandleEscapeButton();
            }

            StartCoroutine(nameof(HandleAdvanceTime));
        }

        private void HandleAdvanceTime()
        {
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
            if (tools.IsToolSet())
            {
                // Unset the brush and absorb the event
                tools.Reset();
                return;
            }
            
            var window = FindObjectOfType<Window>();
            if (window != null)
            {
                // Close one window and absorb the event
                Destroy(window.gameObject);
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

        public void ToggleResourceWindow()
        {
            Instantiate(resourcesWindowPrefab, canvas.transform);
        }

        private static void CloseAllPawnWindows()
        {
            foreach (var components in FindObjectsOfType<PawnWindow>())
            {
                Destroy(components.gameObject);
            }
        }
    }
}