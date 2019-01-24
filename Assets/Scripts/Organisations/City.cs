using System;
using UnityEngine;

namespace Organisations
{
    public class City : Organisation
    {
        public uint Inhabitants { get; private set; }
        public uint AverageInhabitantWage { get; private set; }

        private float taxPercentage;

        public float TaxPercentage
        {
            get => taxPercentage;
            private set
            {
                if (value < 0f || value > 1f)
                {
                    throw new ArgumentException("Tax must be between 0..1", nameof(value));
                }

                taxPercentage = value;
            }
        }

        public City(string name) : base(name)
        {
        }

        public override void EvaluateWeek()
        {
            // Evaluates tax revenue
            Money += Mathf.RoundToInt(Inhabitants * AverageInhabitantWage * taxPercentage);

            base.EvaluateWeek();
        }
    }
}