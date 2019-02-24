using System.Collections.Generic;
using Items;
using Logistics;
using Mechanics;
using UnityEngine;

namespace UI.ResourcesWindow
{
    public class ResourcesList : MonoBehaviour, IObservable
    {
        public ResourceEntry rowPrefab;
        public List<Item> items;

        public Color good = Color.green;
        public Color bad = Color.red;

        private void Awake()
        {
            G.O.Register(this);
            Clear();
            Populate();
        }

        private void OnDestroy()
        {
            G.O.UnRegister(this);
        }

        private void Clear()
        {
            foreach (var component in GetComponentsInChildren<ResourceEntry>())
            {
                Destroy(component.gameObject);
            }
        }

        private void Populate()
        {
            foreach (var item in items)
            {
                LogisticNetwork.ItemSatisfaction(item, out float requested, out float satisfied);
                var capacity = LogisticNetwork.ItemCapacity(item).Maximum;

                rowPrefab.icon.sprite = item.sprite;
                rowPrefab.icon.SetNativeSize();
                rowPrefab.name.text = item.name;

                requested /= G.DeltaTime;
                satisfied /= G.DeltaTime;

                rowPrefab.produced.color = satisfied < requested ? bad : good;

                rowPrefab.requested.text = requested.ToString("N");
                rowPrefab.produced.text = satisfied.ToString("N");
                rowPrefab.capacity.text = capacity.ToString("N");

                Instantiate(rowPrefab, transform);
            }
        }

        public void PreTick()
        {
        }

        public void Tick()
        {
        }

        public void PostTick()
        {
        }

        public void LateTick()
        {
            Clear();
            Populate();
        }
    }
}