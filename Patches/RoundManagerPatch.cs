using HarmonyLib;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace InteriorFogConfig.Patches;
[HarmonyPatch(typeof(RoundManager))]
public class RoundManagerPatch
{
    [HarmonyPatch("RefreshEnemiesList")]
    [HarmonyPostfix]
    private static void ApplyNewFogValues(RoundManager __instance)
    {
        if (__instance.indoorFog == null)
        {
            return;
        }
        if (InteriorFogConfig.randomFogThinness.Value)
        {
            System.Random fogRandom = new(StartOfRound.Instance.randomMapSeed + 69);
            float randomThinness = fogRandom.NextFloat(InteriorFogConfig.minThinness.Value, InteriorFogConfig.maxThinness.Value);
            InteriorFogConfig.Logger.LogInfo("Random fog thinness value: " + randomThinness);
            __instance.indoorFog.parameters.meanFreePath = randomThinness;
        }
        else
        {
            __instance.indoorFog.parameters.meanFreePath = InteriorFogConfig.fogThinness.Value;
        }
        
        if (InteriorFogConfig.changeFogColor.Value)
        {
            if (!ColorUtility.TryParseHtmlString(InteriorFogConfig.fogColor.Value, out Color color))
            {
                return;
            }
            __instance.indoorFog.parameters.albedo = color;
        }
        InteriorFogConfig.Logger.LogInfo("Indoor fog changes done.");
    }
}
