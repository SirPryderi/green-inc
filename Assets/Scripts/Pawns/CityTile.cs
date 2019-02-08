using System;
using Mechanics;
using Organisations;
using UnityEditor;
using UnityEngine;

namespace Pawns
{
    public class CityTile : Pawn, IObservable
    {
        [SerializeField] [Tooltip("Total inhabitants")]
        private float population;

        [SerializeField] [Tooltip("Maximum number of inhabitants supported")]
        public int maxPopulation;

        [SerializeField] [Tooltip("Inhabitants per hour")]
        private float growth;

        [SerializeField] [Tooltip("Produced kg of CO2 per hour")]
        private float emissionsPerCapita;

        [SerializeField] [Tooltip("Consumed W per capita each hour")]
        private float WattPerCapita;

        [SerializeField] [Tooltip("Average wage per hour")]
        private float averageWage;

        [SerializeField] [Tooltip("Tax level")]
        private float taxPercentage;

        private City _city;

        public City City
        {
            get => _city;
            set
            {
                value.AddTile(this);
                _city = value;
            }
        }

        public float EmissionsPerCapita => emissionsPerCapita;
        public int Population => Mathf.FloorToInt(population);
        public float Growth => growth;

        public void StartFrame()
        {
            _city.GenerateRevenue(this);

            var releasedCo2 = G.DeltaTime * EmissionsPerCapita * Population;
            G.CM.Atmosphere.ReleaseGas("Carbon Dioxide", releasedCo2);
        }

        public int CalculateRevenue(int time)
        {
            return Convert.ToInt32(Population * taxPercentage * averageWage * time);
        }

        public void EndFrame()
        {
            if (population < maxPopulation)
            {
                population += G.DeltaTime * Growth;
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (City != null)
                Handles.Label(transform.position, $"{City.Name} - {Population}");
        }
#endif
    }
}