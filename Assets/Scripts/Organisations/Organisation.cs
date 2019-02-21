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

        [Obsolete]
        public bool BuildPawn(HexCell cell, string pawn)
        {
            return BuildPawn(cell, Pawn.Load(pawn));
        }

        public bool BuildPawn(HexCell cell, Pawn pawn)
        {
            var price = pawn.price;
            if (CannotAfford(price)) return false;
            if (!pawn.CanBePlacedOn(cell)) return false;
            ConsumeMoney(price);
            cell.Spawn(pawn.gameObject, this);
            return true;
        }
    }
}