using System;
using UnityEngine;

namespace Atmo
{
    public class ClimateManager
    {
        public Atmosphere Atmosphere { get; }

        // Model Parameters
        private const float EquatorTemperature = 27f; // °C
        private const float PoleTemperature = -30f; // °C
        private const float K = 5.2f; // equatorial falloff factor
        private const float D = -0.04f; // degrees per meter of altitude
        
        // Noise parameters
        private const float NoiseScalingFactor = 1f / 30f; // Scaling factor, size of the noise patches
        private const float NoiseStrength = 20f; // °C - Noise max effect

        public ClimateManager()
        {
            Atmosphere = new Atmosphere();
        }

        /**
         * Average Annual Temperature
         */
        public float GetBaseTemperature(float latitude)
        {
            latitude = Mathf.PI / 180f * latitude;

            // Yeah! polynomial, bitch!
            var equatorialTemperature = EquatorTemperature
                                        - (EquatorTemperature - PoleTemperature)
                                        * Mathf.Pow(Mathf.Sin(Mathf.Abs(latitude)), K);

            return equatorialTemperature + (float) Atmosphere.DeltaTemperature;
        }

        public float GetCellTemperature(HexCell cell)
        {
            var pos = cell.coordinates.ToOffsetCoordinates();

            var randomOffset = new Vector2Int(
                (int) GameManager.Instance.MapManager.RandomGenerator.ClimateNoiseX,
                (int) GameManager.Instance.MapManager.RandomGenerator.ClimateNoiseY
            );

            pos += randomOffset;

            var baseTemp = GetBaseTemperature(cell.Latitude);

            var noise = Mathf.PerlinNoise( // returns value from 0..1
                pos.x * NoiseScalingFactor,
                pos.y * NoiseScalingFactor
            );

            var noiseDelta = noise.Remap(0f, 1f, -NoiseStrength, NoiseStrength);

            var altitudeDelta = D * cell.Elevation * 100f;

            return baseTemp + noiseDelta + altitudeDelta;
        }
    }
}