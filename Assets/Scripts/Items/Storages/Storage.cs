using UnityEngine;

namespace Items.Storages
{
    public abstract class Storage : MonoBehaviour, IStorage
    {
        [Range(1, 999)] [SerializeField] protected uint capacity;

        public abstract ushort ItemCount(Item item);
        public abstract bool HasItem(Item item);
        public abstract bool RemoveItem(Item item);
        public abstract bool AddItem(Item item);
        public abstract bool IsFull(Item item);
    }
}