namespace Stat
{
    public class DataFrame
    {
        public int time;
        public int money;
        public double carbonDioxideConcentration;

        public static DataFrame TakeSnapshot()
        {
            return new DataFrame
            {
                time = G.O.Time,
                money = G.PC.Money,
                carbonDioxideConcentration = G.CM.Atmosphere.GetGasPercentage("Carbon Dioxide") * 1_000_000
            };
        }
    }
}