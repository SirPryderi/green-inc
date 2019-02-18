using System;
using Items;
using Organisations;
using UnityEditor;
using UnityEngine;

namespace Pawns
{
    public class Generator : Pawn
    {
        [Header("Generator")] public Item item;

        public uint itemsPerHour;
        public float pricePerItem;
        public float co2PerItem;

        public float ProductivityPercentage => ProducedInPastHour / itemsPerHour;
        public float ProducedInPastHour => (float) _provided / G.DeltaTime;

        private ulong _provided;
        private int _lastTime;

        private void CheckNewTick()
        {
            if (_lastTime == G.O.Time) return;
            
            _lastTime = G.O.Time;
            _provided = 0;
        }

        public ulong ProduceItemsFor(ulong amount, Organisation org)
        {
            CheckNewTick();

            _provided += amount;

            var providedLimit = Convert.ToUInt64(itemsPerHour * G.DeltaTime);

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
            org.TransferMoney(owner, total);

            G.CM.Atmosphere.ReleaseGas("Carbon Dioxide", amount * co2PerItem);

            return amount;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (G.DeltaTime != 0)
                Handles.Label(transform.position, $"{_provided / 1000ul / (ulong) G.DeltaTime} kW");
        }
#endif
    }
}