namespace Items.Storages
{
    public interface IStorage
    {
        ulong CountItem(Item item);
        bool Has(Item item, ulong quantity = 1ul);
        bool Remove(Item item, ulong quantity = 1ul);
        bool Add(Item item, ulong quantity = 1ul);
        bool CanAdd(Item item, ulong quantity = 1ul);
        bool IsFull();

        bool Transfer(IStorage storage, Item item, ulong quantity = 1ul);
    }
}