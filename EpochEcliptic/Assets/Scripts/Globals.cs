public class Globals {
    
    public static Biome biome = Biome.Forest;
    public static int level = 1;
    public static int biomeLength = 3;
    public static float difficulty = 0.1f;
    public static float difficultyPerStage = 0.1f;

    public static StatLine playerMods;
    public static int playerHealth;

    public static string nextScene;

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

        if (biome == Biome.None) {
            biome = Biome.Forest;
            nextScene = "MainMenu";
        }
        
        else nextScene = "Game";
    }
}
