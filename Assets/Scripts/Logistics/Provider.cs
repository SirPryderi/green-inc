using Items;
using Items.Storages;
using Organisations;
using Pawns;
using UnityEngine;

namespace Logistics
{
    public abstract class Provider : MonoBehaviour
    {
        // Pawn Component Properties
        public Pawn parent;

        // Provider Properties
        public Item item;
        public float pricePerItem;

        #region Stats

        public abstract float ProductivityPercentage { get; }
        public abstract float ProducedInPastHour { get; }

        public abstract float CO2InPastHour { get; }
        public abstract float ItemsPerHour { get; }

        #endregion

        public abstract ulong ProduceItemsFor(ulong amount, Organisation org, IStorage storage = null);

        protected void Bill(ulong amount, Organisation org)
        {
            org.TransferMoney(parent.owner, amount * (decimal) pricePerItem, true);
        }
    }
}