using System;
using Items;
using Items.Storages;
using Organisations;

namespace Logistics
{
    public class FactoryProvider : GeneratorProvider
    {
        public Recipe recipe;

        private readonly IStorage _buffer = new Storage();
        private Requester[] _requesters;

        private void Start()
        {
            SetUpRequesters();
        }

        private void OnDestroy()
        {
            foreach (var requester in _requesters)
            {
                if (requester != null) Destroy(requester);
            }
        }

        private void SetUpRequesters()
        {
            if (recipe.results.Count != 1)
            {
                throw new NotSupportedException("Only recipes with one result are supported.");
            }

            item = recipe.results[0].item;

            _requesters = new Requester[recipe.ingredients.Count];

            for (var index = 0; index < recipe.ingredients.Count; index++)
            {
                var itemAmount = recipe.ingredients[index];
                _requesters[index] = gameObject.AddComponent<Requester>();
                _requesters[index].item = itemAmount.item;
                _requesters[index].parent = parent;
            }
        }

        public override ulong ProduceItemsFor(ulong amount, Organisation org, IStorage storage = null)
        {
            CheckNewTick();

            amount = ClapProduction(amount);

            if (amount == 0ul) return 0ul;

            var produced = ProduceFromRecipe(amount);

            if (produced == 0ul) return 0ul;

            CalculateEmissions(produced);
            Bill(produced, org);
            Provided -= amount - produced;

            if (storage != null)
            {
                _buffer.Transfer(storage, item, produced);
            }
            else
            {
                _buffer.Remove(item, produced);
            }

            return produced;
        }

        private ulong ProduceFromRecipe(ulong amount)
        {
            for (var index = 0; index < recipe.ingredients.Count; index++)
            {
                var requestedAmount = recipe.ingredients[index].amount * amount;
                _requesters[index].Request(requestedAmount, _buffer);
            }

            var count = amount / recipe.results[0].amount;
            var produced = recipe.ProduceAtMost(_buffer, _buffer, count);
            return produced;
        }
    }
}