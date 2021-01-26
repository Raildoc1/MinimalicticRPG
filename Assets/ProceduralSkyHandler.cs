using KG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralSkyHandler : MonoBehaviour
{

    private static ProceduralSkyHandler _instance;
    public static ProceduralSkyHandler instance
    {
        get {
            if (!_instance)
            {
                _instance = FindObjectOfType<ProceduralSkyHandler>();
            }

            return _instance;
        }
    }

    [Range(0.2f, 3f)]
    public float GradientPower = 1f;

    public float GetMinutes()
    {
        return GameTime.instance.minutes % 1440;
    }

}
