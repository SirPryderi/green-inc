using System;
using Items.Storages;
using Logistics;
using Organisations;
using UnityEditor;
using UnityEngine;

namespace Pawns
{
    public class CityTile : Pawn
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

        [Header("Food")] [SerializeField] [Tooltip("Consumed units of food each hour")]
        private float foodPerCapita;

        [SerializeField] [Tooltip("Requester Object")]
        private Requester foodRequester;

        [Header("Financial")] [SerializeField] [Tooltip("Average wage per hour")]
        private float averageWage;

        [SerializeField] [Tooltip("Tax level")]
        private float taxPercentage;

        [Header("Garbage")] [SerializeField] [Tooltip("Produced kg of garbage each hour")]
        private float garbagePerCapita;

        [SerializeField] [Tooltip("Garbage generator")]
        private GeneratorProvider garbageGenerator;

        private readonly Storage _garbageBufferStorage = new Storage();

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

        public override void Tick()
        {
            // Calculates money from taxes
            _city.GenerateRevenue(this);

            // Emissions
            var releasedCo2 = G.DeltaTime * EmissionsPerCapita * Population;
            G.CM.Atmosphere.ReleaseGas("Carbon Dioxide", releasedCo2);

            // Electricity
            var requiredElectricity = Convert.ToUInt64(G.DeltaTime * wattPerCapita * Population);
            energyRequester.Request(requiredElectricity);

            // Food
            var requiredFood = Convert.ToUInt64(G.DeltaTime * foodPerCapita * Population);
            foodRequester.Request(requiredFood);

            // Garbage
            var rate = Convert.ToUInt64(garbagePerCapita * Population);
            garbageGenerator.itemsPerHour = rate;
            garbageGenerator.ProduceItemsFor(rate * (ulong) G.DeltaTime, _city, _garbageBufferStorage);
        }

        public override void PostTick()
        {
            Grow();
        }

        private void Grow()
        {
            population += ComputeGrowth() * G.DeltaTime;
            population = Mathf.Clamp(population, 0f, maxPopulation);
        }

        public float ComputeGrowth()
        {
            // if there isn't enough food cities will starve...
            // ...actually, no scratch that, they just die too easily right now.

            // Let's just prevent growth.
            if (!foodRequester.IsSatisfied) return 0f;

            // a city without energy won't grow either (???)
            if (!energyRequester.IsSatisfied) return 0f;

            // prevent cities from overgrowing
            if (population >= maxPopulation) return 0f;

            // Otherwise expect normal growth
            return Growth;
        }

        public decimal CalculateRevenue(int time)
        {
            return (decimal) (Population * taxPercentage * averageWage * time);
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (City != null)
            {
                var text = $"{City.Name}\nPop: {Population}\n" +
                           $"Energy: {energyRequester.SatisfiedPerc * 100:0.##}%\n" +
                           $"Garbage: {_garbageBufferStorage.CountItem(garbageGenerator.item):N} kg\n" +
                           $"Food: {foodRequester.SatisfiedPerc * 100:0.##}%";

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