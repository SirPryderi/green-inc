using Evaluators;
using Items;
using Logistics;
using Organisations;
using UnityEngine;

namespace AI
{
    public class ElectricalCompanyController : CompanyController
    {
        private readonly Item _electricity;
        private readonly Evaluator _eval;
        private readonly string _pawn;

        public ElectricalCompanyController(Company company) : base(company)
        {
            _electricity = Resources.Load<Item>("Items/Electricity");
            _eval = new BuildingEvaluator();
            _pawn = "Pawns/Electrical/ElectricalPlant";
        }

        public override void Control()
        {
            Pawns = OwnedPawns();

            if (LogisticNetwork.ItemSatisfaction(_electricity) < 0.95f)
            {
                WantToBuild(_pawn, _eval);
            }
        }
    }
}