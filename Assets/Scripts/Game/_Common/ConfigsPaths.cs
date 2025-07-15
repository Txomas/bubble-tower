namespace Game
{
    public class ConfigsPaths
    {
        private const string Slash = "/";
        
        public const string AssetExtension = ".asset";
        
        public const string Assets = nameof(Assets) + Slash;
        public const string Configs = nameof(Configs) + Slash;
        
        public const string AssetsRefs = Configs + nameof(AssetsRefs) + Slash;
        public const string Gameplay = Configs + nameof(Gameplay) + Slash;
        public const string Levels = Gameplay + nameof(Levels) + Slash;
        public const string Features = Gameplay + nameof(Features) + Slash;
        
        public const string LevelEditor = Gameplay + nameof(LevelEditor) + Slash;  
        
        public const string LevelsFolder = Assets + Levels;
    }
}