using UnityEngine;

namespace Items.Storages.Inventories
{
    public abstract class BaseInventory : MonoBehaviour, IStorage
    {
        [Range(1, 999)] [SerializeField] protected uint capacity;

        public abstract ulong CountItem(Item item);
        public abstract bool Has(Item item, ulong quantity = 1);
        public abstract bool Remove(Item item, ulong quantity = 1);
        public abstract bool Add(Item item, ulong quantity = 1);
        public abstract bool CanAdd(Item item, ulong quantity = 1);
        public abstract bool IsFull();
        public abstract bool Transfer(IStorage storage, Item item, ulong quantity = 1ul);
    }
}