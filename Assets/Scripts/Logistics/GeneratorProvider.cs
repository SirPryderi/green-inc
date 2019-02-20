using Items.Storages;
using Organisations;

namespace Logistics
{
    public class GeneratorProvider : Provider
    {
        public float co2PerItem;
        public ulong itemsPerHour;

        protected ulong Provided;
        protected int LastTime;

        #region Stats

        public override float ProductivityPercentage => ProducedInPastHour / itemsPerHour;
        public override float ProducedInPastHour => (float) Provided / G.DeltaTime;
        public override float CO2InPastHour => ProducedInPastHour * co2PerItem;
        public override float ItemsPerHour => itemsPerHour;

        protected void CheckNewTick()
        {
            if (LastTime == G.O.Time) return;

            LastTime = G.O.Time;
            Provided = 0;
        }

        #endregion

        public override ulong ProduceItemsFor(ulong amount, Organisation org, IStorage storage = null)
        {
            CheckNewTick();

            amount = ClapProduction(amount);

            if (amount == 0)
            {
                return 0;
            }

            CalculateEmissions(amount);
            Bill(amount, org);

            // WARNING: does not check if the inventory is full
            storage?.Add(item, amount);

            return amount;
        }

        protected ulong ClapProduction(ulong amount)
        {
            Provided += amount;

            var providedLimit = itemsPerHour * (ulong) G.DeltaTime;

            if (Provided > providedLimit)
            {
                amount = amount - (Provided - providedLimit);
                Provided = providedLimit;
            }

            return amount;
        }

        protected void CalculateEmissions(ulong amount)
        {
            G.CM.Atmosphere.ReleaseGas("Carbon Dioxide", amount * co2PerItem);
        }
    }
}