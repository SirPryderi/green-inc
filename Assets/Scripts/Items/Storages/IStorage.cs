namespace Items.Storages
{
    public interface IStorage
    {
        ushort ItemCount(Item item);
        bool HasItem(Item item);
        bool RemoveItem(Item item);
        bool AddItem(Item item);
        bool IsFull(Item item);
    }
}