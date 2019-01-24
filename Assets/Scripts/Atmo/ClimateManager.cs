using System;
using UnityEngine;

namespace Atmo
{
    public class ClimateManager
    {
        public Atmosphere Atmosphere { get; }

        public double equatorTemperature = 27d; // °C
        public double poleTemperature = -30d; // °C

        public ClimateManager()
        {
            Atmosphere = new Atmosphere();
        }

        /**
         * Average Annual Temperature
         */
        public double GetTemperature(double latitude, double altitude = 0)
        {
            const double k = 5.2d; // equatorial falloff
            const double d = 0.04d; // degrees per meter 
            latitude = Math.PI / 180 * latitude;

            // Yeah! polynomial, bitch!
            var equatorialTemperature = equatorTemperature
                                        - (equatorTemperature - poleTemperature)
                                        * Math.Pow(Math.Sin(Math.Abs(latitude)), k);

            return equatorialTemperature + Atmosphere.DeltaTemperature - d * altitude;
        }
    }
}