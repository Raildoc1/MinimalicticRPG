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
    public float phi = 0;
    public Transform Sun;
    public Vector3 InitForward = new Vector3(-0.3f, -1.0f, 0.0f);

    public float testTeta = 0f;
    public Transform testTransform;

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

        //var k = 1f - Mathf.Abs(time - halfTime) / halfTime;
        var k = time / 1440;

        var teta = (k * 2f + 1.5f) * Mathf.PI;

        var forward = Vector3.up * Mathf.Cos(phi) + Vector3.right * Mathf.Sin(phi);

        Sun.position = new Vector3(Mathf.Sin(teta) * Mathf.Sin(phi), Mathf.Sin(teta) * Mathf.Cos(phi),  Mathf.Cos(teta));
        Sun.LookAt(Vector3.zero);

        testTransform.position = new Vector3(Mathf.Sin(testTeta) * Mathf.Sin(phi), Mathf.Sin(testTeta) * Mathf.Cos(phi), Mathf.Cos(testTeta));
    }

    private void UpdateSunIntensity()
    {
        var time = GetMinutes();
        var halfTime = 12f * 60f;

        var k = 1f - Mathf.Abs(time - halfTime) / halfTime;

        lightData.intensity = MaxSunIntensity * k;
    }
}
