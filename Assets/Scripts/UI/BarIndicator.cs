using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BarIndicator : MonoBehaviour
    {
        public Color bad = Color.red;
        public Color warn = Color.yellow;
        public Color good = Color.green;

        [SerializeField] private Slider slider;
        [SerializeField] private Image progressBar;

        public float Value
        {
            get => slider.value;
            set
            {
                value = Mathf.Clamp01(value);
                progressBar.color = value < 1f ? Color.Lerp(bad, warn, value) : good;
                slider.value = value;
            }
        }
    }
}