using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace ThePatient
{

    [CreateAssetMenu(fileName = "BakeLightmapSwapperSO", menuName = "Lightmap/BakeLightmapSwapperSO", order = 1)]
    public class BakeLightmapSwapperSO : ScriptableObject
    {
        public Texture2D[] lightMapDir;
        public Texture2D[] lightMapColor;
        public SphericalHarmonicsL2[] lightProbesData;

        SphericalHarmonicsL2[] currentSceneData;

        [ContextMenu("SetLightmapAndLightProbe")]
        public void SetData()
        {
            currentSceneData = LightmapSettings.lightProbes.bakedProbes;
            lightProbesData = currentSceneData;
        }
    }
}
