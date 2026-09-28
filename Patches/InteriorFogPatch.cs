using HarmonyLib;
using InteriorFogConfig.Extensions;
using InteriorFogConfig.MonoBehaviours;
using InteriorFogConfig.Utils;
using UnityEngine;

namespace InteriorFogConfig.Patches;
[HarmonyPatch]
public class InteriorFogPatch
{
    public static int currentIndex = 0;

    [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.RefreshEnemiesList))]
    [HarmonyPostfix]
    private static void ApplyNewFogValues(RoundManager __instance)
    {
        if (__instance.indoorFog == null)
        {
            return;
        }
        bool configFound = false;
        for (int i = 0;i < InteriorFogConfig.configurations.Count; i++)
        {
            if (InteriorFogConfig.configurations[i].name != $"Moon: {RoundManager.Instance.currentLevel.PlanetName}") continue;

            if (InteriorFogConfig.configurations[i].configEnabled) currentIndex = i;
            else currentIndex = 0;
            configFound = true;
            InteriorFogConfig.Logger.LogInfo($"Configuration found for {RoundManager.Instance.currentLevel.PlanetName}! {(InteriorFogConfig.configurations[i].configEnabled ? "Using moon specific configuration." : "Using default configuration.")}");
            break;
        }
        if (!configFound)
        {
            currentIndex = 0;
            InteriorFogConfig.Logger.LogWarning($"Configuration for {RoundManager.Instance.currentLevel.PlanetName} was not found! Using defaults...");
        }

        if (!InteriorFogConfig.configurations[currentIndex].fogEnabled.Value)
        {
            __instance.indoorFog.gameObject.SetActive(false);
            InteriorFogConfig.Logger.LogDebug("Set interior fog to not active.");
            return;
        }

        if (InteriorFogConfig.configurations[currentIndex].forceFog.Value || (InteriorFogConfig.configurations[currentIndex].fogOnFoggy.Value && RoundManager.Instance.currentLevel.currentWeather == LevelWeatherType.Foggy))
        {
            __instance.indoorFog.gameObject.SetActive(true);
            InteriorFogConfig.Logger.LogDebug("Set interior fog to active.");
        }

        if (InteriorFogConfig.configurations[currentIndex].randomFogThinness.Value)
        {
            System.Random fogRandom = new(StartOfRound.Instance.randomMapSeed + 69);
            float randomThinness = fogRandom.NextFloat(InteriorFogConfig.configurations[currentIndex].minThinness.Value, InteriorFogConfig.configurations[currentIndex].maxThinness.Value);
            InteriorFogConfig.Logger.LogDebug("Random fog thinness value: " + randomThinness);
            __instance.indoorFog.parameters.meanFreePath = randomThinness;
        }
        else
        {
            InteriorFogConfig.Logger.LogDebug("Set fog thinness to:" + InteriorFogConfig.configurations[currentIndex].fogThinness.Value);
            __instance.indoorFog.parameters.meanFreePath = InteriorFogConfig.configurations[currentIndex].fogThinness.Value;
        }
        
        if (InteriorFogConfig.configurations[currentIndex].changeFogColor.Value)
        {
            InteriorFogConfig.Logger.LogDebug("Changing interior fog color.");
            Color? indoorFogColor = ParseColor(InteriorFogConfig.configurations[currentIndex].fogColor.Value);
            if (indoorFogColor != null)
            {
                __instance.indoorFog.parameters.albedo = (Color)indoorFogColor;
                __instance.indoorFog.parameters.albedo.a = InteriorFogConfig.configurations[currentIndex].fogAlpha.Value;
            }
        }

        if (InteriorFogConfig.configurations[currentIndex].animateFogThinness.Value)
        {
            if (InteriorFogConfig.configurations[currentIndex].fogThinnessAnimation.Value == "")
            {
                InteriorFogConfig.Logger.LogWarning("Animation was enabled but no animation was defined!");
                if (__instance.indoorFog.gameObject.TryGetComponent(out FogDensityAnimator animator))
                {
                    InteriorFogConfig.Logger.LogDebug("Destroying fog animator.");
                    animator.DestroySelf();
                }
            }
            else
            {
                if (!__instance.indoorFog.gameObject.TryGetComponent(out FogDensityAnimator animator))
                {
                    animator = __instance.indoorFog.gameObject.AddComponent<FogDensityAnimator>();
                }
                InteriorFogConfig.Logger.LogDebug("Adding fog animator.");
                animator.fogAnimation = CurveUtil.CreateCurve(InteriorFogConfig.configurations[currentIndex].fogThinnessAnimation.Value);
                animator.fogDensity = __instance.indoorFog.parameters.meanFreePath;
            }
        }
        else
        {
            if (__instance.indoorFog.gameObject.TryGetComponent(out FogDensityAnimator animator))
            {
                InteriorFogConfig.Logger.LogDebug("Destroying fog animator.");
                animator.DestroySelf();
            }
        }
        InteriorFogConfig.Logger.LogInfo("Indoor fog changes done.");
    }

    [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndOfGame))]
    [HarmonyPostfix]
    private static void KillAnimator()
    {
        if (RoundManager.Instance.indoorFog.gameObject.TryGetComponent(out FogDensityAnimator animator))
        {
            InteriorFogConfig.Logger.LogDebug("Destroying fog animator.");
            animator.DestroySelf();
        }
    }

    private static Color? ParseColor(string colorString)
    {
        if (ColorUtility.TryParseHtmlString(colorString, out Color fogColor))
        {
            return fogColor;
        }

        InteriorFogConfig.Logger.LogWarning("Couldn't parse color string! Make sure the formatting is correct.");
        return null;
    }
}
