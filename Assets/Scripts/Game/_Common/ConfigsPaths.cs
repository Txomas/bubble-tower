namespace Game
{
    public class ConfigsPaths
    {
        private const string Slash = "/";
        
        public const string AssetExtension = ".asset";
        
        public const string Assets = nameof(Assets) + Slash;
        public const string Configs = nameof(Configs) + Slash;
        public const string Game = Configs + nameof(Game) + Slash;
        public const string Levels = Game + nameof(Levels) + Slash;
        
        public const string LevelsFolder = Assets + Levels;
    }
}