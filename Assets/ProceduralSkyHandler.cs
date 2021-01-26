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
    public float MaxSunIntensity = 100_000f;
    public Transform Sun;
    public Vector3 InitForward = new Vector3(-0.3f, -1.0f, 0.0f);

    private UnityEngine.Rendering.HighDefinition.HDAdditionalLightData lightData;

    public float GetMinutes()
    {
        return GameTime.instance.minutes % 1440;
    }

    private void Awake()
    {
        if (!Sun)
        {
            Debug.LogError("No Sun Assigned!");
            return;
        }

        lightData = Sun.GetComponent<UnityEngine.Rendering.HighDefinition.HDAdditionalLightData>();

        Sun.forward = InitForward;
    }

    private void Update()
    {
        UpdateSunIntensity();
        UpdateSunRotation();
    }

    private void UpdateSunRotation()
    {
        var time = GetMinutes();
        var halfTime = 12f * 60f;

        var k = 1f - Mathf.Abs(time - halfTime) / halfTime;

        var phi = k * 2f * Mathf.PI;

        var forward = Vector3.up * Mathf.Cos(phi) + Vector3.right * Mathf.Sin(phi);

    }

    private void UpdateSunIntensity()
    {
        var time = GetMinutes();
        var halfTime = 12f * 60f;

        var k = 1f - Mathf.Abs(time - halfTime) / halfTime;

        lightData.intensity = MaxSunIntensity * k;
    }
}
