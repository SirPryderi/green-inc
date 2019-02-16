using System.Linq;
using Evaluators;
using Items;
using Logistics;
using Organisations;
using UnityEngine;

namespace AI
{
    public class FoodCompanyController : CompanyController
    {
        private readonly Item _food;
        private readonly string _crop;
        private readonly Evaluator _eval;

        public FoodCompanyController(Company company) : base(company)
        {
            _food = Resources.Load<Item>("Items/Food");
            _eval = new SoilFertilityEvaluator();
            _crop = "Pawns/Crop";
        }

        public override void Control()
        {
            Pawns = OwnedPawns();

            if (Requester.ItemSatisfaction(_food) < 0.95f)
            {
                WantToBuild(_crop, _eval);
            }
        }
    }
}