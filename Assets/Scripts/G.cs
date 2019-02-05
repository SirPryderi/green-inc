using Atmo;

namespace DefaultNamespace
{
    public class G
    {
        public static GameManager GM => GameManager.Instance;
        public static MapManager MP => GM.MapManager;
        public static ClimateManager CM => MP.ClimateManager;
    }
}