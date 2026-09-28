using InteriorFogConfig.Patches;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace InteriorFogConfig.MonoBehaviours;
internal class FogDensityAnimator : MonoBehaviour
{
    public AnimationCurve? fogAnimation;
    public float fogDensity = 5;
    LocalVolumetricFog? fog;
    void Awake()
    {
        if (TimeOfDay.Instance == null || !gameObject.activeInHierarchy)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        fog = GetComponent<LocalVolumetricFog>();
    }

    public void DestroySelf()
    {
        Destroy(this);
    }

    void Update()
    {
        if (!InteriorFogConfig.configurations[InteriorFogPatch.currentIndex].animateFogThinness.Value)
        {
            return;
        }
        if (fogAnimation == null)
        {
            return;
        }
        fog.parameters.meanFreePath = fogDensity * fogAnimation.Evaluate(TimeOfDay.Instance.normalizedTimeOfDay);
    }
}
