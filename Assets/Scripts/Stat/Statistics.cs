using System;
using System.Collections.Generic;
using System.Linq;

namespace Stat
{
    public class Statistics
    {
        public readonly LinkedList<DataFrame> Data;

        public Statistics()
        {
            Data = new LinkedList<DataFrame>();
        }

        public void TakeSnapshot()
        {
            Data.AddLast(DataFrame.TakeSnapshot());
        }

        public static long Population =>
            Convert.ToInt64(G.MP.Orgs.Cities.Sum(city => city.Population));

        public static float AverageTemperature =>
            G.MP.Grid.cells.Average(cell => cell.Temperature);
    }
}