using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Window : MonoBehaviour
    {
        public Text title;
        public RectTransform body;

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}