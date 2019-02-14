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

        public Organisation(string name, decimal startingMoney = 0)
        {
            Name = name;
            _money = startingMoney;
        }

        public void TransferMoney(Organisation organisation, decimal amount)
        {
            ConsumeMoney(amount);
            organisation._money += amount;
        }

        public void ConsumeMoney(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Amount must be positive", nameof(amount));
            }

            if (!CanAfford(amount))
            {
                throw new ArgumentException($"Not enough balance {Money:## ###.00}$ < {amount}", nameof(amount));
            }

            _money -= amount;
        }

        public void BuyPawn(HexCell cell, string pawn)
        {
            var price = Pawn.Load(pawn).price;
            if (CannotAfford(price)) return;
            ConsumeMoney(price);
            cell.Spawn(pawn, this);
        }
    }
}