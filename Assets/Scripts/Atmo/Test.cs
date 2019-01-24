using System;

namespace Atmo
{
    public class Test
    {
       
        public void TestMe()
        {
            Atmosphere atmosphere = new Atmosphere();

            var mass = atmosphere.Mass;

            Console.WriteLine($"Atmosphere mass: {mass} kg");

            foreach (var composition in atmosphere.Gasses.Values)
            {
                Console.WriteLine($"{composition.Gas.Name}: {composition.Mass / mass * 100}%");
            }

            Console.WriteLine($"dT: {atmosphere.DeltaTemperature} °C");
        }


        public void TestTemperatureDelta()
        {
            ClimateManager manager = new ClimateManager();
            printTemps(manager);
        }


        public void TestTemperatureDelta2()
        {
            ClimateManager manager = new ClimateManager();

            var c = 0.000507;

            manager.Atmosphere.SetCO2Concentration(c);

            printTemps(manager);
        }

        private static void printTemps(ClimateManager manager)
        {
            Console.WriteLine($"== Annual Average Temperature ({manager.Atmosphere.GetGasPercentage("Carbon Dioxide")*1_000_000} CO₂ ppm) ==" );
            Console.WriteLine($"Equator:\t{manager.GetBaseTemperature(0):0.## °C}");
            Console.WriteLine($"Catania:\t{manager.GetBaseTemperature(37):0.## °C}");
            Console.WriteLine($"Temperate:\t{manager.GetBaseTemperature(45):0.## °C}");
            Console.WriteLine($"Stirling:\t{manager.GetBaseTemperature(56):0.## °C}");
            Console.WriteLine($"North Pole:\t{manager.GetBaseTemperature(90):0.## °C}");
        }
    }
}