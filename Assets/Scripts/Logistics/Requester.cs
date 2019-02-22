using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using Items.Storages;
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

        public ulong Request(ulong amount, IStorage storage = null)
        {
            IsSatisfied = false;
            requested = amount;
            satisfied = 0;

            var providers = LogisticNetwork.FindProviders(item);
            providers.Sort(new DistanceFrom(gameObject));

            foreach (var provider in providers)
            {
                try
                {
                    satisfied += provider.ProduceItemsFor(requested - satisfied, parent.owner, storage);

                    if (satisfied < requested) continue;

                    IsSatisfied = true;
                    break;
                }
                catch (ArgumentException e)
                {
                    Debug.LogWarning(e.Message);
                }
            }

            return satisfied;
        }
    }
}