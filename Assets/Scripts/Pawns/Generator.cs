using System;
using Items;
using Mechanics;
using Organisations;
using UnityEditor;
using UnityEngine;

namespace Pawns
{
    public class Generator : Pawn, IObservable
    {
        [Header("Generator")]
        public Item item;

        public uint itemsPerHour;
        public float pricePerItem;
        public float co2PerItem;

        [HideInInspector] public uint ProvidedInPastFrame;

        private uint provided;

        public void StartFrame()
        {
            owner.ConsumeMoney(upkeep * G.DeltaTime);
        }

        public void EndFrame()
        {
            ProvidedInPastFrame = provided;
            provided = 0;
        }

        public uint ProduceItemsFor(uint amount, Organisation org)
        {
            provided += amount;

            var providedLimit = Convert.ToUInt32(itemsPerHour * G.DeltaTime);

            if (provided > providedLimit)
            {
                amount = amount - (provided - providedLimit);
                provided = providedLimit;
            }

            if (amount == 0)
            {
                return 0;
            }

            var total = Convert.ToInt32(amount * pricePerItem);
            org.TransferMoney(owner, total);

            G.CM.Atmosphere.ReleaseGas("Carbon Dioxide", amount * co2PerItem);

            return amount;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (G.DeltaTime != 0)
                Handles.Label(transform.position, $"{ProvidedInPastFrame / 1000 / G.DeltaTime} kW");
        }
#endif
    }
}