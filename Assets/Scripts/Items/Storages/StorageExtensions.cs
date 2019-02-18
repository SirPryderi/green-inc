namespace Items.Storages
{
    public static class StorageExtensions
    {
        public static bool Transfer(this IStorage from, IStorage to, Item item, ulong quantity = 1)
        {
            if (!from.Has(item, quantity)) return false;
            if (!to.CanAdd(item, quantity)) return false;

            from.Remove(item, quantity);
            to.Add(item, quantity);

            return true;
        }
    }
}