using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;

namespace Logistics
{
    public static class LogisticNetwork
    {
        public static List<Provider> FindProviders(Item item)
        {
            var result = Object.FindObjectsOfType<Provider>();
            var filter = new List<Provider>(result.Length);

            filter.AddRange(result.Where(gen => gen.item == item));
            filter.TrimExcess();

            return filter;
        }

        public static List<Requester> FindRequesters(Item item)
        {
            var result = Object.FindObjectsOfType<Requester>();
            var filter = new List<Requester>(result.Length);

            filter.AddRange(result.Where(requester => requester.item == item));
            filter.TrimExcess();

            return filter;
        }

        public static float ItemSatisfaction(Item item, out float requested, out float satisfied)
        {
            var requesters = FindRequesters(item);

            requested = 0f;
            satisfied = 0f;

            foreach (var requester in requesters)
            {
                requested += requester.requested;
                satisfied += requester.satisfied;
            }

            var percent = satisfied / requested;

            return Mathf.Clamp01(percent);
        }

        public struct Capacity
        {
            public float Maximum;
            public float Current;
            public float Percentage;
        }

        public static Capacity ItemCapacity(Item item)
        {
            var providers = FindProviders(item);

            var capacity = new Capacity();

            foreach (var provider in providers)
            {
                capacity.Maximum += provider.ItemsPerHour;
                capacity.Current += provider.ProducedInPastHour;
            }

            capacity.Percentage = capacity.Current / capacity.Maximum;

            return capacity;
        }

        public static float ItemSatisfaction(Item item)
        {
            return ItemSatisfaction(item, out _, out _);
        }
    }
}