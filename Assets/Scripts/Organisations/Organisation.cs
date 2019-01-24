using System;

namespace Organisations
{
    public class Organisation
    {
        public string Name { get; }
        public int Money { get; protected set; }

        public Organisation(string name)
        {
            Name = name;
            Money = 0;
        }

        /// <summary>
        /// Calculates the expenses and revenues of the organisation for a week time.
        /// </summary>
        public virtual void EvaluateWeek()
        {
            // TODO

            // Calculate all expenses/revenues from objects
        }

        public void TransferMoney(Organisation organisation, int amount)
        {
            // TODO should this be thread safe?

            if (amount < 0)
            {
                throw new ArgumentException("Amount must be positive", nameof(amount));
            }

            if (Money < amount)
            {
                throw new ArgumentException("Not enough balance", nameof(amount));
            }

            Money -= amount;
            organisation.Money += amount;
        }
    }
}