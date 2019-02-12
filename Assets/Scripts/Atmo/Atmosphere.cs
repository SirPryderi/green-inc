using System;
using System.Collections.Generic;

namespace Atmo
{
    [Serializable]
    public class Atmosphere
    {
        /// <summary>
        /// Realistic Earth atmosphere mass
        /// </summary>
        private const double EARTH_ATMOSPHERE_MASS = 5.1480e18;

        /// <summary>
        /// Deliberately smaller mass to magnify the effects of pollution
        /// </summary>
        private const double BALANCED_ATMOSPHERE_MASS = 5e14;

        private const double CO2c = 0.000407;
        private const double Nc = 0.78084;
        private const double O2c = 0.20946;

        public Dictionary<string, GasComposition> Gasses { get; }

        public Atmosphere()
        {
            Gasses = new Dictionary<string, GasComposition>();

            var nitrogen = new Gas("Nitrogen");
            var oxygen = new Gas("Oxygen");
            var carbonDioxide = new Gas("Carbon Dioxide");
            var other = new Gas("Other");

            const double initialAtmosphere = BALANCED_ATMOSPHERE_MASS;

            var nitrogenComposition = new GasComposition(nitrogen, Nc * initialAtmosphere);
            var oxygenComposition = new GasComposition(oxygen, O2c * initialAtmosphere);

            var carbonDioxideComposition = new GasComposition(carbonDioxide, CO2c * initialAtmosphere);
            var otherComposition = new GasComposition(other, (1 - (Nc + O2c + CO2c)) * initialAtmosphere);

            Gasses.Add(nitrogen.Name, nitrogenComposition);
            Gasses.Add(oxygen.Name, oxygenComposition);
            Gasses.Add(carbonDioxide.Name, carbonDioxideComposition);
            Gasses.Add(other.Name, otherComposition);
        }

        public double Mass
        {
            get
            {
                double total = 0;

                foreach (var gasComposition in Gasses.Values)
                {
                    total += gasComposition.Mass;
                }

                return total;
            }
        }

        public double DeltaTemperature => (GetGasPercentage("Carbon Dioxide") * 1_000_000d - 325d) / 100d;

        public double GetGasPercentage(string gas)
        {
            return Gasses[gas].Mass / Mass;
        }

        public void SetCO2Concentration(double c)
        {
            Gasses["Carbon Dioxide"].Mass = c * Mass;
            Gasses["Other"].Mass = (1 - (Nc + O2c + c)) * Mass;
        }

        public void ReleaseGas(string gas, double mass)
        {
            Gasses[gas].Mass += mass;
        }

        public void Absorb(string gas, double mass)
        {
            Gasses[gas].Mass -= mass;

            if (Gasses[gas].Mass < 0)
            {
                Gasses[gas].Mass = 0;
            }
        }
    }

    [Serializable]
    public class GasComposition
    {
        public Gas Gas { get; }

        public double Mass { get; set; }

        public GasComposition(Gas gas, double mass)
        {
            Gas = gas ?? throw new ArgumentNullException(nameof(gas));
            Mass = mass;
        }
    }
}