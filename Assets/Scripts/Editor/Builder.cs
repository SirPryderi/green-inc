namespace Editor
{
    using UnityEditor;

    public static class Builder
    {
        private static readonly string[] Levels =
        {
            "Assets/Scenes/MainMenu.unity",
            "Assets/Scenes/Game.unity",
            "Assets/Tutorials/HexGrid/Scene.unity"
        };

        [MenuItem("Build/Build All")]
        public static void BuildAll()
        {
            BuildLinux();
            BuildWindows();
        }

        [MenuItem("Build/Build for Windows", false, 1)]
        public static void BuildWindows()
        {
            const string path = "Builds/Windows_Build";

            BuildPipeline.BuildPlayer(
                Levels,
                path + "/Green Inc.exe",
                BuildTarget.StandaloneWindows64,
                BuildOptions.Development);
        }

        [MenuItem("Build/Build for Linux", false, 2)]
        public static void BuildLinux()
        {
            const string path = "Builds/Linux_Build";

            BuildPipeline.BuildPlayer(
                Levels,
                path + "/Green Inc",
                BuildTarget.StandaloneLinux64,
                BuildOptions.Development);
        }
    }
}