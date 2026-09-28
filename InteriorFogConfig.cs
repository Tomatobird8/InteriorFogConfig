using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;

namespace InteriorFogConfig;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class InteriorFogConfig : BaseUnityPlugin
{
    public static InteriorFogConfig Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;
    internal static Harmony? Harmony { get; set; }

    public static List<Configuration> configurations = [];

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        configurations.Add(GenerateConfiguration("default", true));

        Patch();

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }

    internal static Configuration GenerateConfiguration(string category, bool forceEnabled)
    {
        return new Configuration(
            category,
            forceEnabled || Instance.Config.Bind(category, "ConfigEnabled", false, "Should this configuration be used for the moon? Uses defaults if false.").Value,
            Instance.Config.Bind(category, "FogEnabled", true, "Should interior fog be enabled? Interior fog will spawn randomly as determined by the game."),
            Instance.Config.Bind(category, "ForceFog", false, "Should interior fog be forced to spawn?"),
            Instance.Config.Bind(category, "FogThinness", 5, "Control the density of the fog. Higher values make fog thinner. Ignored if RandomFogThinness is used."),
            Instance.Config.Bind(category, "RandomFogThinness", false, "Make the density of the fog random per each round. Note that this isn't network synchronized and in multiplayer players will see different fog density."),
            Instance.Config.Bind(category, "MinThinness", 3, "Minimum thinness of the fog. The densest the fog will get. Inclusive."),
            Instance.Config.Bind(category, "MaxThinness", 8, "Maximum thinness of the fog. The least dense the fog will get. Exclusive. Set this higher than MinThinness"),
            Instance.Config.Bind(category, "ChangeFogColor", false, "Should the fog color be changed?"),
            Instance.Config.Bind(category, "FogColor", "#261710", "Color of the interior fog."),
            Instance.Config.Bind(category, "FogAlpha", 1.0f, new BepInEx.Configuration.ConfigDescription("Alpha value of the fog. 0 is fully transparent, 1 is fully opaque. Vanilla uses value of 1.", new BepInEx.Configuration.AcceptableValueRange<float>(0.0f, 1.0f))),
            Instance.Config.Bind(category, "FogOnFoggy", false, "Should interior fog be always enabled if the weather is foggy?"),
            Instance.Config.Bind(category, "AnimateFogThinness", false, "Should interior fog density change over time?"),
            Instance.Config.Bind(category, "FogThinnessAnimation", "", "Define an animation curve that multiplies the set fog thinness with a value from the curve depending on normalize time of day. First value is the normalized time, second value is the fog multiplier. Example: 0.0,2.0 ; 0.2,1.5 ; 0.4,0.8 ; 0.6,0.5 ; 0.8,1.0 ; 1.0,3.0 (You can also define in/out tangets and weights for each key like so: 0.4,0.2,1.1,2,0.4,0.6 (Check Keyframe declaration in unity documentation!))")
            );
    }

    internal static void Patch()
    {
        Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

        Logger.LogDebug("Patching...");

        Harmony.PatchAll();

        Logger.LogDebug("Finished patching!");
    }

    internal static void Unpatch()
    {
        Logger.LogDebug("Unpatching...");

        Harmony?.UnpatchSelf();

        Logger.LogDebug("Finished unpatching!");
    }
}
