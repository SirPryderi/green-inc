using System;

namespace Organisations
{
    public class Organisation
    {
        public string Name { get; }
        public int Money { get; protected set; }
        public bool CanAfford(int amount) => amount <= Money;
        public bool CannotAfford(int amount) => !CanAfford(amount);

        public Organisation(string name, int startingMoney = 0)
        {
            Name = name;
            Money = startingMoney;
        }

        public void TransferMoney(Organisation organisation, int amount)
        {
            ConsumeMoney(amount);
            organisation.Money += amount;
        }

        public void ConsumeMoney(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Amount must be positive", nameof(amount));
            }

            if (!CanAfford(amount))
            {
                throw new ArgumentException($"Not enough balance {Money} < {amount}", nameof(amount));
            }

            Money -= amount;
        }
    }
}