using UnityEngine;

namespace UI
{
    public class MainUIController : MonoBehaviour
    {
        public bool isPaused = false;

        public void TogglePause()
        {
            isPaused = !isPaused;

            GetComponent<MainUIUpdater>().UpdateClock();
        }

        public void Advance()
        {
            GameManager.Instance.MapManager.Observer.AdvanceTime(12);
        }
    }
}