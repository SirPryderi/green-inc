using System;
using System.Collections.Generic;
using Items.Storages;
using UnityEngine;

namespace Items
{
    [Serializable]
    public struct ItemAmount
    {
        public Item item;
        [Range(1, 999)] public ushort amount;
    }

    [CreateAssetMenu]
    public class Recipe : ScriptableObject
    {
        public List<ItemAmount> ingredients;
        public List<ItemAmount> results;

        public bool CanProduce(IStorage storage, ushort count = 1)
        {
            foreach (var ingredient in ingredients)
            {
                if (storage.ItemCount(ingredient.item) * count < ingredient.amount * count)
                {
                    return false;
                }
            }

            return true;
        }

        public void Produce(IStorage storage, IStorage targetStorage = null, ushort count = 1)
        {
            if (targetStorage == null) targetStorage = storage;

            if (!CanProduce(storage, count)) return;

            foreach (var ingredient in ingredients)
            {
                for (var i = 0; i < ingredient.amount * count; i++)
                {
                    storage.RemoveItem(ingredient.item);
                }
            }

            foreach (var result in results)
            {
                for (var i = 0; i < result.amount * count; i++)
                {
                    targetStorage.AddItem(result.item);
                }
            }
        }
    }
}