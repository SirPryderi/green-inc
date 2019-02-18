using System;
using Items;
using Organisations;
using Pawns;
using UnityEngine;

namespace Logistics
{
    public class Provider : MonoBehaviour
    {
        // Pawn Component Properties
        public Pawn parent;

        // Provider Properties
        public Item item;
        public float pricePerItem;

        // Generator Properties
        public float co2PerItem;
        public ulong itemsPerHour;

        // Productivity Limitation
        private ulong _provided;
        private int _lastTime;

        #region Stats

        public float ProductivityPercentage => ProducedInPastHour / itemsPerHour;
        public float ProducedInPastHour => (float) _provided / G.DeltaTime;

        #endregion

        public ulong ProduceItemsFor(ulong amount, Organisation org)
        {
            CheckNewTick();

            _provided += amount;

            var providedLimit = Convert.ToUInt64(itemsPerHour * (ulong) G.DeltaTime);

            if (_provided > providedLimit)
            {
                amount = amount - (_provided - providedLimit);
                _provided = providedLimit;
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

        private void CheckNewTick()
        {
            if (_lastTime == G.O.Time) return;

            _lastTime = G.O.Time;
            _provided = 0;
        }
    }
}