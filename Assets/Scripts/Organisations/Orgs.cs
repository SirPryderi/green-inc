using System.Collections.Generic;

namespace Organisations
{
    public class Orgs
    {
        public Company PlayerCompany = null;

        public readonly List<City> Cities = new List<City>();
        public readonly List<Company> Companies = new List<Company>();

        public void Reset()
        {
            PlayerCompany = null;
            Cities.Clear();
            Companies.Clear();
        }
    }
}