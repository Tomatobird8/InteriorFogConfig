using HarmonyLib;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace InteriorFogConfig.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    public class RoundManagerPatch
    {
        [HarmonyPatch("RefreshEnemiesList")]
        [HarmonyPostfix]
        private static void ApplyNewFogValues(RoundManager __instance)
        {
            InteriorFogConfig.Logger.LogInfo("Running Indoor Fog changes.");
            if (__instance.indoorFog != null)
            {
                if (InteriorFogConfig.randomFogThinness.Value)
                {
                    float randomThinness = Random.Range(InteriorFogConfig.minThinness.Value, InteriorFogConfig.maxThinness.Value);
                    InteriorFogConfig.Logger.LogInfo("Random fog thinness value: " + randomThinness);
                    __instance.indoorFog.parameters.meanFreePath = randomThinness;
                }
                else
                {
                    __instance.indoorFog.parameters.meanFreePath = InteriorFogConfig.fogThinness.Value;
                }
                
                if (InteriorFogConfig.changeFogColor.Value)
                {
                    ColorUtility.TryParseHtmlString(InteriorFogConfig.fogColor.Value, out Color color);
                    __instance.indoorFog.parameters.albedo = color;
                }
                InteriorFogConfig.Logger.LogInfo("Indoor fog changes done.");
            }
            else
            {
                InteriorFogConfig.Logger.LogWarning("Indoor fog changes were not done. indoorFog object is null.");
            }
        }
    }
}
