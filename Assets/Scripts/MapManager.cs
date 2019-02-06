using Atmo;
using Mechanics;
using Organisations;
using Stat;

public class MapManager
{
    public ClimateManager ClimateManager { get; }

    public RandomGenerator RandomGenerator { get; private set; }

    public Observer Observer { get; }

    public HexGrid Grid;

    public readonly Statistics Statistics;

    public readonly Orgs Orgs = new Orgs();

    public MapManager()
    {
        ClimateManager = new ClimateManager();
        RandomGenerator = new RandomGenerator(0);
        Statistics = new Statistics();

        Observer = new Observer();
        Orgs.PlayerCompany = new Company("Green Inc.", CompanyType.GENERIC, 5_000);
        Orgs.Companies.Add(new Company("Energy Inc.", CompanyType.ELECTRICAL, 10_000));
        Orgs.Companies.Add(new Company("Food Inc.", CompanyType.FOOD, 10_000));
    }

    public void Randomise()
    {
        RandomGenerator = new RandomGenerator(System.Environment.TickCount);
    }
}