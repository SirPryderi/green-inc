using System;
using System.Collections.Generic;
using System.Linq;
using Evaluators;
using Logistics;
using Organisations;
using Pawns;
using Object = UnityEngine.Object;

namespace AI
{
    public class CompanyController
    {
        protected readonly Company Company;
        protected IEnumerable<Pawn> Pawns;

        public static void ControlAll()
        {
            foreach (var company in G.MP.Orgs.Companies)
            {
                CompanyController c;

                switch (company.Type)
                {
                    case CompanyType.ELECTRICAL:
                        c = new ElectricalCompanyController(company);
                        break;
                    case CompanyType.FOOD:
                        c = new FoodCompanyController(company);
                        break;
                    default:
                        c = new CompanyController(company);
                        break;
                }

                c.Control();
            }
        }

        public CompanyController(Company company)
        {
            Company = company;
        }

        public virtual void Control()
        {
        }

        protected IEnumerable<Pawn> OwnedPawns()
        {
            var allPawns = Object.FindObjectsOfType<Pawn>();
            var ownedPawns = allPawns.Where(pawn => pawn.owner == Company);
            return ownedPawns;
        }

        protected bool AllPawnsAtFullProduction()
        {
            foreach (var pawn in Pawns)
            {
                var gen = pawn.GetComponent<Provider>();

                if (gen == null) continue;

                if (gen.ProductivityPercentage < .95f) return false;
            }

            return true;
        }

        protected void WantToBuild(string pawn, Evaluator eval)
        {
            if (Company.CannotAfford(pawn)) return;

            var cells = eval.EvaluateAll(G.MP.Grid);

            foreach (var (item1, item2) in cells)
            {
                if (Math.Abs(item1) < 0.01)
                {
                    throw new Exception("Could not find suitable tile.");
                }

                if (item2.IsClear())
                {
                    Company.BuyPawn(item2, pawn);
                    break;
                }
            }
        }
    }
}