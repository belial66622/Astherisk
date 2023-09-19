using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace ThePatient
{
    public class BakeLightmapSwapper : SingletonBehaviour<BakeLightmapSwapper>, ISaveable
    {
        [SerializeField] LightmapData[] brightLMData;
        [SerializeField] LightmapData[] darkLMData;

        [SerializeField] List<BakeLightmapSwapperSO> lightmapSO;

        bool isDarkness;
        private void Start()
        {
            List<LightmapData> brightLMlist = new List<LightmapData>();

            for (int i = 0; i < lightmapSO[0].lightMapDir.Length; i++)
            {
                LightmapData lmData = new LightmapData();

                lmData.lightmapDir = lightmapSO[0].lightMapDir[i];
                lmData.lightmapColor = lightmapSO[0].lightMapColor[i];
                lmData.shadowMask = lightmapSO[0].shadowMaskColor[i];
                brightLMlist.Add(lmData);
            }

            brightLMData = brightLMlist.ToArray();

            List<LightmapData> darkLMlist = new List<LightmapData>();

            for (int i = 0; i < lightmapSO[1].lightMapDir.Length; i++)
            {
                LightmapData lmData = new LightmapData();

                lmData.lightmapDir = lightmapSO[1].lightMapDir[i];
                lmData.lightmapColor = lightmapSO[1].lightMapColor[i];
                lmData.shadowMask = lightmapSO[0].shadowMaskColor[i];

                darkLMlist.Add(lmData);
            }

            darkLMData = darkLMlist.ToArray();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                isDarkness = SetBrightLM();
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                isDarkness = SetDarkLM();
            }
        }

        bool SetBrightLM()
        {
            LightmapSettings.lightmaps = brightLMData;
            LightmapSettings.lightProbes.bakedProbes = lightmapSO[0].lightProbesData;
            return false;
        }
        bool SetDarkLM()
        {
            LightmapSettings.lightmaps = darkLMData;
            LightmapSettings.lightProbes.bakedProbes = lightmapSO[1].lightProbesData;
            return true;
        }


        public object CaptureState()
        {
            return isDarkness;
        }

        public void RestoreState(object state)
        {
            bool isDark = (bool)state;
            if (isDark)
            {
                SetDarkLM();
            }
            else
            {
                SetBrightLM();
            }
        }
    }
}
