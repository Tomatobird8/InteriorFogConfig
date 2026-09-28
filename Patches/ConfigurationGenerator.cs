using HarmonyLib;

namespace InteriorFogConfig.Patches;
[HarmonyPatch(typeof(StartOfRound))]
internal class ConfigurationGenerator
{
    [HarmonyPatch(nameof(StartOfRound.Start))]
    [HarmonyPostfix]
    internal static void GenerateConfigurations(StartOfRound __instance)
    {
        InteriorFogConfig.Logger.LogDebug("Generating configurations for each moon.");
        SelectableLevel[] levels = __instance.levels;

        for (int i = 0;i < levels.Length; i++)
        {
            if (!levels[i].spawnEnemiesAndScrap) continue;
            InteriorFogConfig.configurations.Add(InteriorFogConfig.GenerateConfiguration($"Moon: {levels[i].PlanetName}", false));
        }
    }
}
