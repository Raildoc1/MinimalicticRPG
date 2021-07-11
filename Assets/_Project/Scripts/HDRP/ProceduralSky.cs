using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Rendering.HighDefinition
{
    [VolumeComponentMenu("Sky/Procedural Skybox")]
    [SkyUniqueID(9999)]
    public class ProceduralSky : SkySettings
    {

        public override int GetHashCode()
        {
            return base.GetHashCode();

        }

        public override Type GetSkyRendererType()
        {
            //return typeof(ProceduralSkyRenderer);
            return typeof(ProceduralSkyRenderer);
        }
    }
}

