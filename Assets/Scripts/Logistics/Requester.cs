using System.Collections.Generic;
using System.Linq;
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

        private IEnumerable<Generator> FindProviders()
        {
            var result = FindObjectsOfType<Generator>();
            var filter = new List<Generator>(result.Length);

            filter.AddRange(result.Where(gen => gen.item == item));
            filter.TrimExcess();

            // TODO this could be done more efficiently
            filter.Sort((generator, generator1) =>
            {
                var transformPosition = parent.transform.position;

                var distance1 = (generator1.transform.position - transformPosition).sqrMagnitude;
                var distance2 = (generator.transform.position - transformPosition).sqrMagnitude;

                return (int) (distance2 - distance1);
            });

            return filter;
        }
    }
}