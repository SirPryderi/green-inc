using Atmo;
using Mechanics;
using Organisations;

public class MapManager
{
    public ClimateManager ClimateManager { get; }

    public RandomGenerator RandomGenerator { get; private set; }

    public Observer Observer { get; }

    public HexGrid Grid;

    public readonly Orgs Orgs = new Orgs();

    public MapManager()
    {
        ClimateManager = new ClimateManager();
        RandomGenerator = new RandomGenerator(0);

        Observer = new Observer();
        Orgs.PlayerCompany = new Company("Green Inc.", CompanyType.GENERIC);
        Orgs.Companies.Add(new Company("Black Inc.", CompanyType.ELECTRICAL));
    }

    public void Randomise()
    {
        RandomGenerator = new RandomGenerator(System.Environment.TickCount);
    }
}