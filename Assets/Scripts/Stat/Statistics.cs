using System.Collections.Generic;

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
    }
}