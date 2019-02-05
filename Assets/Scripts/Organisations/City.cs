using System.Collections.Generic;
using Pawns;

namespace Organisations
{
    public class City : Organisation
    {
        private readonly List<CityTile> _tiles = new List<CityTile>();

        public City(string name) : base(name)
        {
        }

        public int Population
        {
            get
            {
                var sum = 0;

                foreach (var tile in _tiles) sum += tile.Population;

                return sum;
            }
        }

        public void AddTile(CityTile tile)
        {
            _tiles.Add(tile);
        }

        public void GenerateRevenue(CityTile tile)
        {
            Money += tile.CalculateRevenue(GameManager.Instance.MapManager.Observer.DeltaTime);
        }
    }
}