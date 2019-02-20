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

        public bool Produce(IStorage storage, IStorage targetStorage = null, ulong count = 1)
        {
            if (!CanProduce(storage, count)) return false;

            _produce(storage, targetStorage, count);

            return true;
        }

        private void _produce(IStorage storage, IStorage targetStorage, ulong count)
        {
            if (count == 0ul) return;

            if (targetStorage == null) targetStorage = storage;

            foreach (var ingredient in ingredients)
            {
                storage.Remove(ingredient.item, ingredient.amount * count);
            }

            foreach (var result in results)
            {
                targetStorage.Add(result.item, result.amount * count);
            }
        }

        public ulong ProduceAtMost(IStorage storage, IStorage targetStorage = null, ulong count = 1)
        {
            var maxCanProduce = count;

            foreach (var ingredient in ingredients)
            {
                var tmp = storage.CountItem(ingredient.item) / ingredient.amount;

                if (tmp < maxCanProduce) maxCanProduce = tmp;
            }

            _produce(storage, targetStorage, maxCanProduce);

            return maxCanProduce;
        }
    }
}