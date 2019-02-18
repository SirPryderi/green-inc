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
        [Range(1, 999)] public ulong amount;
    }

    [CreateAssetMenu(menuName = "Items/Recipe")]
    public class Recipe : ScriptableObject
    {
        public List<ItemAmount> ingredients;
        public List<ItemAmount> results;

        public bool CanProduce(IStorage storage, ulong count = 1)
        {
            foreach (var ingredient in ingredients)
            {
                if (storage.CountItem(ingredient.item) * count < ingredient.amount * count)
                {
                    return false;
                }
            }

            return true;
        }

        public void Produce(IStorage storage, IStorage targetStorage = null, ulong count = 1)
        {
            if (targetStorage == null) targetStorage = storage;

            if (!CanProduce(storage, count)) return;

            foreach (var ingredient in ingredients)
            {
                for (var i = 0ul; i < ingredient.amount * count; i++)
                {
                    storage.Remove(ingredient.item);
                }
            }

            foreach (var result in results)
            {
                for (var i = 0ul; i < result.amount * count; i++)
                {
                    targetStorage.Add(result.item);
                }
            }
        }
    }
}