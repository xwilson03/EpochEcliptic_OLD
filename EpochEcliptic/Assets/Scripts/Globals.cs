public class Globals {
    
    public static Biome biome;
    public static int level;
    public static int biomeLength;
    public static float difficulty;
    public static float difficultyPerStage;

    public static StatLine playerMods;
    public static int playerHealth;

    public static string nextScene;

    public static void Init() {
        biome = Biome.Forest;
        level = 1;
        biomeLength = 3;
        difficulty = 0.1f;
        difficultyPerStage = 0.1f;

        playerMods = null;
        playerHealth = 0;

        nextScene = "MainMenu";
    }

    public static void PrepareNextFloor() {
        playerMods = Refs.player.mods;
        playerHealth = Refs.player.health;
        level++;
        difficulty += difficultyPerStage;

        if (level > biomeLength) {
            biome++;
            level = 1;
        }

        //TODO: import mountain assets and remove this
        if (biome == Biome.Mountain) biome++;
        if (biome == Biome.None) nextScene = "MainMenu";
    }
}
