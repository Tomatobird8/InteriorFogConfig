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

        public static ConfigEntry<int> fogThinness;
        public static ConfigEntry<bool> randomFogThinness;
        public static ConfigEntry<int> minThinness;
        public static ConfigEntry<int> maxThinness;
        public static ConfigEntry<bool> changeFogColor;
        public static ConfigEntry<string> fogColor;

        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            fogThinness = Config.Bind<int>("General","FogThinness",5,"Control the density of the fog. Higher values make fog thinner. Ignored if RandomFogThinness is used.");
            randomFogThinness = Config.Bind<bool>("General", "RandomFogThinness", false, "Make the density of the fog random per each round. Note that this isn't network synchronized and in multiplayer players will see different fog density."); // fix if bored
            minThinness = Config.Bind<int>("General", "MinThinness", 3, "Minimum thinness of the fog. The densest the fog will get. Inclusive.");
            maxThinness = Config.Bind<int>("General", "MaxThinness", 8, "Maximum thinness of the fog. The least dense the fog will get. Exclusive. Set this higher than MinThinness");
            changeFogColor = Config.Bind<bool>("General","ChangeFogColor",false,"Should the mod also change the color of the fog?");
            fogColor = Config.Bind<string>("General","FogColor", "#261710FF", "Color of the interior fog.");

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
