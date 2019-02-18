using System.Collections.Generic;

namespace Items.Storages
{
    public class Storage : IStorage
    {
        private readonly Dictionary<Item, ulong> _storageMap;

        public Storage()
        {
            _storageMap = new Dictionary<Item, ulong>();
        }

        public ulong CountItem(Item item)
        {
            return _storageMap.ContainsKey(item) ? _storageMap[item] : 0ul;
        }

        public bool Has(Item item, ulong quantity = 1)
        {
            return CountItem(item) >= quantity;
        }

        public bool Remove(Item item, ulong quantity = 1)
        {
            if (!Has(item, quantity)) return false;

            _storageMap[item] -= quantity;

            return true;
        }

        public bool Add(Item item, ulong quantity = 1)
        {
            if (!CanAdd(item, quantity)) return false;

            if (_storageMap.ContainsKey(item))
                _storageMap[item] += quantity;
            else
                _storageMap[item] = quantity;

            return true;
        }

        public bool CanAdd(Item item, ulong quantity = 1)
        {
            return true;
        }

        public bool IsFull()
        {
            return false;
        }
    }
}