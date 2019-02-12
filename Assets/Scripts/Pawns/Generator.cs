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

        [HideInInspector] public ulong ProvidedInPastFrame;

        private ulong provided;

        public override void StartFrame()
        {
            owner.ConsumeMoney(upkeep * G.DeltaTime);
        }

        public override void EndFrame()
        {
            ProvidedInPastFrame = provided;
            provided = 0;
        }

        public ulong ProduceItemsFor(ulong amount, Organisation org)
        {
            provided += amount;

            var providedLimit = Convert.ToUInt64(itemsPerHour * G.DeltaTime);

            if (provided > providedLimit)
            {
                amount = amount - (provided - providedLimit);
                provided = providedLimit;
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
                Handles.Label(transform.position, $"{ProvidedInPastFrame / 1000ul / (ulong) G.DeltaTime} kW");
        }
#endif
    }
}