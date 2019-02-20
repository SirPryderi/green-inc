using System;
using Pawns;

namespace Organisations
{
    public class Organisation
    {
        protected decimal _money;
        public string Name { get; }

        public decimal Money
        {
            get => decimal.Round(_money, 2);
            protected set => _money = value;
        }

        public bool CanAfford(decimal amount) => amount <= _money;
        public bool CannotAfford(decimal amount) => !CanAfford(amount);
        public bool CanAfford(string pawn) => CanAfford(Pawn.Load(pawn).price);
        public bool CannotAfford(string pawn) => !CanAfford(pawn);

        public Organisation(string name, decimal startingMoney = 0)
        {
            Name = name;
            _money = startingMoney;
        }

        public void TransferMoney(Organisation organisation, decimal amount, bool allowNegative = false)
        {
            ConsumeMoney(amount, allowNegative);
            organisation._money += amount;
        }

        public void ConsumeMoney(decimal amount, bool allowNegative = false)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Amount must be positive", nameof(amount));
            }

            if (!allowNegative && !CanAfford(amount))
            {
                throw new ArgumentException($"Not enough balance {Money:## ###.00}$ < {amount}", nameof(amount));
            }

            _money -= amount;
        }

        public void BuyPawn(HexCell cell, string pawn)
        {
            var pawn1 = Pawn.Load(pawn);
            var price = pawn1.price;
            if (CannotAfford(price)) return;
            if (!pawn1.CanBePlacedOn(cell)) return;
            ConsumeMoney(price);
            cell.Spawn(pawn, this);
        }
    }
}