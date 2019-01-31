using UnityEngine;

namespace UI
{
    public class MainUIController : MonoBehaviour
    {
        public void Advance()
        {
            GameManager.Instance.MapManager.Observer.AdvanceTime(1);
        }
    }
}