using System.Collections.Generic;
using Items;
using Pawns;
using UnityEngine;

namespace Logistics
{
    public class Requester : MonoBehaviour
    {
        public Item item;
        public Pawn parent;

        public ulong requested;
        public ulong satisfied;

        public bool IsSatisfied { get; private set; }

        public float SatisfiedPerc => requested == 0 ? 0 : (float) satisfied / requested;

        public Requester(Item item)
        {
            this.item = item;
        }

        public void Request(ulong amount)
        {
            IsSatisfied = false;
            requested = amount;
            satisfied = 0;

            var providers = FindProviders();

            foreach (var provider in providers)
            {
                satisfied += provider.ProduceItemsFor(requested - satisfied, parent.owner);

                if (satisfied >= requested)
                {
                    IsSatisfied = true;
                    break;
                }
            }
        }

        private static IEnumerable<Generator> FindProviders()
        {
            return FindObjectsOfType<Generator>();
        }
    }
}