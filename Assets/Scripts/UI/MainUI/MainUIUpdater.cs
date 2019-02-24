using System;
using Mechanics;
using Stat;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainUIUpdater : MonoBehaviour, IObservable
    {
        public Text PlayPause;
        public Text Speed;

        public Text DateDay;
        public Text DateMonth;
        public Text DateYear;

        public Text MoneyBudget;
        public Text MoneyIncome;

        public Text CO2Budget;
        public Text CO2Income;

        public Text Population;
        public Text AverageTemperature;

        public MainUIController MainUIController;

        private void Awake()
        {
            UpdateAll();
        }

        public void PreTick()
        {
            // Nope!
        }

        public void Tick()
        {
            // nothing to do just yet
        }

        public void PostTick()
        {
            // Nothing!
        }

        public void LateTick()
        {
            UpdateAll();
        }

        public void UpdateAll()
        {
            UpdateClock();
            UpdateCO2();
            UpdateBalance();
            UpdatePopulation();
        }

        private void UpdatePopulation()
        {
            Population.text = $"{Statistics.Population:##,###0}";
            AverageTemperature.text = $"{Statistics.AverageTemperature:##,###0.00 °C}";
        }

        public void UpdateBalance()
        {
            // Amount
            var budget = G.PC.Money;
            MoneyBudget.text = $"$ {budget:##,###0}";

            // Variation
            var current = G.MP.Statistics.Data.Last;

            if (current?.Previous == null)
            {
                MoneyIncome.text = "-/DAY";

                return;
            }

            var previous = current.Previous;

            var deltaTime = current.Value.time - previous.Value.time;
            var deltaMoney = current.Value.money - previous.Value.money;

            var increase = deltaMoney / deltaTime * 24;

            MoneyIncome.text = $"{increase:+##,###0}/DAY";
        }

        private void UpdateCO2()
        {
            // Amount
            var valueF = G.CM.Atmosphere.GetGasPercentage("Carbon Dioxide") * 1_000_000f;
            var value = Convert.ToInt32(valueF);
            CO2Budget.text = $"CO₂ {value:##,###} <size=10>ppm</size>";

            // Variation
            var current = G.MP.Statistics.Data.Last;

            if (current?.Previous == null)
            {
                CO2Income.text = "-/DAY";

                return;
            }

            var previous = current.Previous;

            var deltaTime = current.Value.time - previous.Value.time;
            var deltaMoney = current.Value.carbonDioxideConcentration - previous.Value.carbonDioxideConcentration;

            var increase = deltaMoney / deltaTime * 24 * 365 * 1_000_000;

            CO2Income.text = $"{increase:+##,###0.00}/YEAR";
        }

        public void UpdateClock()
        {
            PlayPause.text = MainUIController.isPaused ? "▶" : "||";

            var start = DateTime.Parse("1 January 2020");

            var date = start.Add(TimeSpan.FromHours(G.O.Time));

            DateDay.text = date.Day.ToString();
            DateYear.text = date.Year.ToString();
            DateMonth.text = date.ToString("MMM").ToUpper();
        }
    }
}