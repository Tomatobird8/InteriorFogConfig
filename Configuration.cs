using BepInEx.Configuration;

namespace InteriorFogConfig;
public class Configuration
{
    public string name = "";
    public bool configEnabled = false;
    public ConfigEntry<bool> fogEnabled = null!;
    public ConfigEntry<bool> forceFog = null!;
    public ConfigEntry<int> fogThinness = null!;
    public ConfigEntry<bool> randomFogThinness = null!;
    public ConfigEntry<int> minThinness = null!;
    public ConfigEntry<int> maxThinness = null!;
    public ConfigEntry<bool> changeFogColor = null!;
    public ConfigEntry<string> fogColor = null!;
    public ConfigEntry<float> fogAlpha = null!;
    public ConfigEntry<bool> fogOnFoggy = null!;
    public ConfigEntry<bool> animateFogThinness = null!;
    public ConfigEntry<string> fogThinnessAnimation = null!;

    public Configuration(
        string name,
        bool configEnabled,
        ConfigEntry<bool> fogEnabled,
        ConfigEntry<bool> forceFog,
        ConfigEntry<int> fogThinness,
        ConfigEntry<bool> randomFogThinness,
        ConfigEntry<int> minThinness,
        ConfigEntry<int> maxThinness,
        ConfigEntry<bool> changeFogColor,
        ConfigEntry<string> fogColor,
        ConfigEntry<float> fogAlpha,
        ConfigEntry<bool> fogOnFoggy,
        ConfigEntry<bool> animateFogThinness,
        ConfigEntry<string> fogThinnessAnimation
        )
    {
        this.name = name;
        this.configEnabled = configEnabled;
        this.forceFog = forceFog;
        this.fogEnabled = fogEnabled;
        this.fogThinness = fogThinness;
        this.randomFogThinness= randomFogThinness;
        this.minThinness = minThinness;
        this.maxThinness = maxThinness;
        this.changeFogColor = changeFogColor;
        this.fogColor = fogColor;
        this.fogAlpha = fogAlpha;
        this.fogOnFoggy = fogOnFoggy;
        this.animateFogThinness = animateFogThinness;
        this.fogThinnessAnimation = fogThinnessAnimation;
    }
}
