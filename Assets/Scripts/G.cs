using Atmo;
using Mechanics;
using Organisations;

public static class G
{
    public static GameManager GM => GameManager.Instance;
    public static MapManager MP => GM.MapManager;
    public static ClimateManager CM => MP.ClimateManager;
    public static Company PC => MP.Orgs.PlayerCompany;
    public static Observer O => GM.MapManager.Observer;
    public static int DeltaTime => O.DeltaTime;
}