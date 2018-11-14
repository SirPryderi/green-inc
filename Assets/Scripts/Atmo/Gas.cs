using System;

namespace Atmo
{
    [Serializable]
    public class Gas
    {
        public string Name { get; }
        public double HalfLife { get; }

        public Gas(string name)
        {
            Name = name;
        }
    }
}