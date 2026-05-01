using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Drawing;

namespace InteriorFogConfig
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class InteriorFogConfig : BaseUnityPlugin
    {
        public static InteriorFogConfig Instance { get; private set; } = null!;
        internal new static ManualLogSource Logger { get; private set; } = null!;
        internal static Harmony? Harmony { get; set; }

        public static ConfigEntry<int> fogThinness = null!;
        public static ConfigEntry<bool> randomFogThinness = null!;
        public static ConfigEntry<int> minThinness = null!;
        public static ConfigEntry<int> maxThinness = null!;
        public static ConfigEntry<bool> changeFogColor = null!;
        public static ConfigEntry<string> fogColor = null!;

        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            fogThinness = Config.Bind<int>("General","FogThinness",5,"Control the density of the fog. Higher values make fog thinner. Ignored if RandomFogThinness is used.");
            randomFogThinness = Config.Bind<bool>("General", "RandomFogThinness", false, "Make the density of the fog random per each round. Note that this isn't network synchronized and in multiplayer players will see different fog density.");
            minThinness = Config.Bind<int>("General", "MinThinness", 3, "Minimum thinness of the fog. The densest the fog will get. Inclusive.");
            maxThinness = Config.Bind<int>("General", "MaxThinness", 8, "Maximum thinness of the fog. The least dense the fog will get. Exclusive. Set this higher than MinThinness");
            changeFogColor = Config.Bind<bool>("General","ChangeFogColor",false,"Should the mod also change the color of the fog?");
            fogColor = Config.Bind<string>("General","FogColor", "All:#261710FF", "Color of the interior fog. Set color to all moons example: All:#261710FF. Set color to specific moons by appending moon names to the config: All:#261710FF,Experimentation:#110502FF,Assurance:#21110BFF");

            Patch();

            Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
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
}
