using System.Collections.Generic;
using UnityEngine;

namespace InteriorFogConfig.Utils;
internal class CurveUtil
{
    internal static AnimationCurve CreateCurve(string s)
    {
        string[] keys = s.Split(';');
        if (keys.Length == 1)
        {
            if (float.TryParse(keys[0], out float value))
            {
                return AnimationCurve.Constant(0f, 1f, value);
            }
        }
        List<Keyframe> ks = [];
        foreach (string key in keys)
        {
            string[] parts = key.Split(",");
            if (parts.Length == 2)
            {
                string timeStr = parts[0].Trim();
                string valueStr = parts[1].Trim();

                if (float.TryParse(timeStr, out float time) && float.TryParse(valueStr, out float value))
                {
                    ks.Add(new Keyframe(time, value));
                }
            }
            else if (parts.Length == 6)
            {
                string timeStr = parts[0].Trim();
                string valueStr = parts[1].Trim();
                string inTangentStr = parts[2].Trim();
                string outTangentStr = parts[3].Trim();
                string inWeightStr = parts[4].Trim();
                string outWeightStr = parts[5].Trim();

                if (float.TryParse(timeStr, out float time) && float.TryParse(valueStr, out float value) && float.TryParse(inTangentStr, out float inTangent) && float.TryParse(outTangentStr, out float outTangent) && float.TryParse(inWeightStr, out float inWeight) && float.TryParse(outWeightStr, out float outWeight))
                {
                    ks.Add(new Keyframe(time, value, inTangent, outTangent, inWeight, outWeight));
                }
            }
        }
        return new AnimationCurve([.. ks]);
    }
}
