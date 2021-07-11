using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnityEngine.Rendering.HighDefinition
{
    public class ProceduralSkyRenderer : SkyRenderer
    {

        public Color dayColor = new Color(0.2f, 0.92f, 0.92f, 1f);
        public Color nightColor = new Color(0.066f, 0.145f, 0.435f, 1f);

        Material skyMaterial;
        MaterialPropertyBlock m_PropertyBlock = new MaterialPropertyBlock();

        public ProceduralSkyRenderer()
        { 
        
        }

        public override void Build()
        {
            if (skyMaterial == null)
            {
                skyMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("HDRP/Minimalistic RPG/Sky"));
            }
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(skyMaterial);
        }

        public override void RenderSky(BuiltinSkyParameters builtinParams, bool renderForCubemap, bool renderSunDisk)
        {
            if (skyMaterial == null)
            {
                Build();
            }

            m_PropertyBlock.SetMatrix("_PixelCoordToViewDirWS", builtinParams.pixelCoordToViewDirMatrix);
            m_PropertyBlock.SetColor("_DayColor", dayColor);
            m_PropertyBlock.SetColor("_NightColor", nightColor);
            m_PropertyBlock.SetFloat("_GradientOffset", ProceduralSkyHandler.instance.GradientPower);
            m_PropertyBlock.SetFloat("_DayTime", ProceduralSkyHandler.instance.GetMinutes());

            var skySettings = builtinParams.skySettings as ProceduralSky;

            m_PropertyBlock.SetFloat("_SkyIntensity", GetSkyIntensity(skySettings, builtinParams.debugSettings));

            CoreUtils.DrawFullScreen(builtinParams.commandBuffer, skyMaterial, m_PropertyBlock, renderForCubemap ? 0 : 1);

        }
    }
}
