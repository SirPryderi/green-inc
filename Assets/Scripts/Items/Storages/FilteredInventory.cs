using System;

namespace Items.Storages
{
    public class FilteredInventory : Storage
    {
        public ushort amount;
        public Item type;

        public override ushort ItemCount(Item item)
        {
            return type != item ? (ushort) 0 : amount;
        }

        public override bool HasItem(Item item)
        {
            return ItemCount(item) > 0;
        }

        public override bool RemoveItem(Item item)
        {
            if (!HasItem(item)) return false;

            amount--;
            return true;
        }

        public override bool AddItem(Item item)
        {
            if (IsFull(item)) return false;

            amount++;
            return true;
        }

        public override bool IsFull(Item item)
        {
            return amount >= capacity;
        }
    }
}