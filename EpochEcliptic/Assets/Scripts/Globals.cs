public class Globals {
    
    public static Biome biome = Biome.Forest;
    public static int level = 1;
    public static int biomeLength = 3;
    public static float difficulty = 0.1f;
    public static float difficultyPerStage = 0.1f;

    public static StatLine playerMods = null;
    public static int playerHealth = 0;

    public static string nextScene = "MainMenu";

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

        if (biome == Biome.None) Clear();
        else nextScene = "Game";
    }

    static void Clear() {
        biome = Biome.Forest;
        level = 1;
        biomeLength = 3;
        difficulty = 0.1f;
        difficultyPerStage = 0.1f;

        playerMods = null;
        playerHealth = 0;

        nextScene = "MainMenu";
    }
}
