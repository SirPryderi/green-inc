using UnityEngine;

namespace Mechanics
{
    public class ObservableAutoRegister : MonoBehaviour
    {
        private void Awake()
        {
            var component = GetComponent<IObservable>();

            GameManager.Instance.MapManager.Observer.Register(component);
        }

        private void OnDestroy()
        {
            var component = GetComponent<IObservable>();

            GameManager.Instance.MapManager.Observer.UnRegister(component);
        }
    }
}