using System;
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

        public override ulong ProduceItemsFor(ulong amount, Organisation org)
        {
            CheckNewTick();

            Provided += amount;

            var providedLimit = Convert.ToUInt64(itemsPerHour * (ulong) G.DeltaTime);

            if (Provided > providedLimit)
            {
                amount = amount - (Provided - providedLimit);
                Provided = providedLimit;
            }

            if (amount == 0)
            {
                return 0;
            }

            var total = Convert.ToInt64(amount * pricePerItem);
            org.TransferMoney(parent.owner, total);

            G.CM.Atmosphere.ReleaseGas("Carbon Dioxide", amount * co2PerItem);

            return amount;
        }
    }
}