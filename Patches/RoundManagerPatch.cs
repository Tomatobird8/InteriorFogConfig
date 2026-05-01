using HarmonyLib;
using InteriorFogConfig.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            Dictionary<string, string> fogDict = InteriorFogConfig.fogColor.Value
                .ToLower()
                .Split(',')
                .Select (part => part.Split(':'))
                .Where(part => part.Length == 2)
                .ToDictionary(sp => sp[0], sp => sp[1]);
            string planetName = new string([.. __instance.currentLevel.PlanetName.ToLower().SkipWhile(c => char.IsDigit(c) || char.IsWhiteSpace(c))]);
            InteriorFogConfig.Logger.LogInfo($"Current planet name valid for InteriorFogConfig fog color configuration: {planetName}");
            if (!fogDict.TryGetValue(planetName,out string colorString))
            {
                InteriorFogConfig.Logger.LogInfo($"Color value not found for {planetName}. Using default.");
                if (!fogDict.TryGetValue("all", out string defaultColorString))
                {
                    InteriorFogConfig.Logger.LogWarning("Couldn't get color value for specific moon nor default! Please define a color for all in the configuration to edit the fog colors.");
                    return;
                }
                if (!ColorUtility.TryParseHtmlString(defaultColorString, out Color defaultColor))
                {
                    InteriorFogConfig.Logger.LogWarning("Default color was defined but failed to be parsed. Check configuration for an example for the syntax.");
                    return;
                }
                __instance.indoorFog.parameters.albedo = defaultColor;
                return;
            }
            if (!ColorUtility.TryParseHtmlString(colorString, out Color color))
            {
                InteriorFogConfig.Logger.LogWarning($"Moon specific color for {planetName} was defined but failed to be parsed. Check configuration for an example for the syntax.");
                return;
            }
            __instance.indoorFog.parameters.albedo = color;
        }
        InteriorFogConfig.Logger.LogInfo("Indoor fog changes done.");
    }
}
