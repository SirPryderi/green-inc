using System;
using System.Collections.Generic;

namespace Atmo
{
    [Serializable]
    public class Atmosphere
    {
        private const double EARTH_ATMOSPHERE_MASS = 5.1480e18;
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

            var nitrogenComposition = new GasComposition(nitrogen, Nc * EARTH_ATMOSPHERE_MASS);
            var oxygenComposition = new GasComposition(oxygen, O2c * EARTH_ATMOSPHERE_MASS);

            var carbonDioxideComposition = new GasComposition(carbonDioxide, CO2c * EARTH_ATMOSPHERE_MASS);
            var otherComposition = new GasComposition(other, (1 - (Nc + O2c + CO2c)) * EARTH_ATMOSPHERE_MASS);

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
            Gasses["Carbon Dioxide"].Mass = c * EARTH_ATMOSPHERE_MASS;
            Gasses["Other"].Mass = (1 - (Nc + O2c + c)) * EARTH_ATMOSPHERE_MASS;
        }

        public void ReleaseGas(string gas, double mass)
        {
            Gasses[gas].Mass += mass;
        }

        public void Absorb(string gas, double mass)
        {
            Gasses[gas].Mass -= mass;
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