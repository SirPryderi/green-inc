namespace Items.Storages.Inventories
{
    public class SingleItemInventory : BaseInventory
    {
        public ulong amount;
        public Item type;

        public override ulong CountItem(Item item)
        {
            return type != item ? 0 : amount;
        }

        public override bool Has(Item item, ulong quantity = 1)
        {
            return CountItem(item) >= quantity;
        }

        public override bool Remove(Item item, ulong quantity = 1)
        {
            if (!Has(item, quantity)) return false;

            amount -= quantity;

            return true;
        }

        public override bool Add(Item item, ulong quantity = 1)
        {
            if (CanAdd(item, quantity)) return false;

            amount += quantity;

            return true;
        }

        public override bool CanAdd(Item item, ulong quantity = 1)
        {
            if (item != type) return false;

            return amount + quantity <= capacity;
        }

        public override bool IsFull()
        {
            return amount >= capacity;
        }
    }
}