using System;
using Logistics;
using Mechanics;
using Organisations;
using UnityEditor;
using UnityEngine;

namespace Pawns
{
    public class CityTile : Pawn, IObservable
    {
        [Header("Demographics")] [SerializeField] [Tooltip("Total inhabitants")]
        private float population;

        [SerializeField] [Tooltip("Maximum number of inhabitants supported")]
        public int maxPopulation;

        [SerializeField] [Tooltip("Inhabitants per hour")]
        private float growth;

        [Header("Emissions")] [SerializeField] [Tooltip("Produced kg of CO2 per hour")]
        private float emissionsPerCapita;

        [Header("Electrical")] [SerializeField] [Tooltip("Consumed W per capita each hour")]
        private float wattPerCapita;

        [SerializeField] [Tooltip("Requester Object")]
        private Requester energyRequester;

        [Header("Financial")] [SerializeField] [Tooltip("Average wage per hour")]
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
                owner = value;
            }
        }

        public float EmissionsPerCapita => emissionsPerCapita;
        public int Population => Mathf.FloorToInt(population);
        public float Growth => growth;

        public void StartFrame()
        {
            _city.GenerateRevenue(this);

            // Emissions
            var releasedCo2 = G.DeltaTime * EmissionsPerCapita * Population;
            G.CM.Atmosphere.ReleaseGas("Carbon Dioxide", releasedCo2);

            // Electricity
            var requiredElectricity = Convert.ToUInt32(G.DeltaTime * wattPerCapita * Population);
            energyRequester.Request(requiredElectricity);

            // TODO Food
        }

        public void EndFrame()
        {
            if (!energyRequester.IsSatisfied) return;
            
            if (population < maxPopulation)
            {
                population += G.DeltaTime * Growth;
            }
        }

        public int CalculateRevenue(int time)
        {
            return Convert.ToInt32(Population * taxPercentage * averageWage * time);
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (City != null)
            {
                var text = $"{City.Name}\nPop: {Population}\nEnergy: {energyRequester.SatisfiedPerc * 100}%";

                Handles.Label(transform.position, text);
            }

            if (!energyRequester.IsSatisfied)
            {
                Handles.color = Color.red;
                Handles.DrawSolidDisc(transform.position, Vector3.up, 5);
            }
        }
#endif
    }
}