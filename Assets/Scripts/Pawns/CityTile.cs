using Organisations;

namespace Pawns
{
    public class CityTile : Pawn
    {
        public City City { get; set; }

        public string CityName => City.Name;

        public override void Evaluate()
        {
        }
    }
}