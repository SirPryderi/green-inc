using System;
using Mechanics;
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

        public MainUIController MainUIController;

        private void Awake()
        {
            UpdateAll();
        }

        public void StartFrame()
        {
            // nothing to do just yet
        }

        public void EndFrame()
        {
            UpdateAll();
        }

        private void UpdateAll()
        {
            UpdateClock();
            UpdateCO2();
        }

        private void UpdateCO2()
        {
            var valueF = GameManager.Instance.MapManager.ClimateManager.Atmosphere
                             .GetGasPercentage("Carbon Dioxide") * 1_000_000f;
            var value = Convert.ToInt32(valueF);
            CO2Budget.text = $"CO₂ {value:##,###} <size=10>ppm</size>";
        }

        public void UpdateClock()
        {
            PlayPause.text = MainUIController.isPaused ? "▶" : "||";

            var start = DateTime.Parse("1 January 2020");

            var date = start.Add(TimeSpan.FromHours(GameManager.Instance.MapManager.Observer.Time));

            DateDay.text = date.Day.ToString();
            DateYear.text = date.Year.ToString();
            DateMonth.text = date.ToString("MMM").ToUpper();
        }
    }
}