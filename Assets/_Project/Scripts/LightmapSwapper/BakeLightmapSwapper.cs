using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ThePatient
{
    public class BakeLightmapSwapper : SingletonMonoBehaviour<BakeLightmapSwapper>
    {
        [SerializeField] LightmapData[] brightLMData;
        [SerializeField] LightmapData[] darkLMData;

        [SerializeField] BakeLightmapSwapperSO brightLMSO;
        [SerializeField] BakeLightmapSwapperSO darkLMSO;


        private void Start()
        {
            List<LightmapData> brightLMlist = new List<LightmapData>();

            for (int i = 0; i < brightLMSO.lightMapDir.Length; i++)
            {
                LightmapData lmData = new LightmapData();

                lmData.lightmapDir = brightLMSO.lightMapDir[i];
                lmData.lightmapColor = brightLMSO.lightMapColor[i];

                brightLMlist.Add(lmData);
            }

            brightLMData = brightLMlist.ToArray();

            List<LightmapData> darkLMlist = new List<LightmapData>();

            for (int i = 0; i < darkLMSO.lightMapDir.Length; i++)
            {
                LightmapData lmData = new LightmapData();

                lmData.lightmapDir = darkLMSO.lightMapDir[i];
                lmData.lightmapColor = darkLMSO.lightMapColor[i];

                darkLMlist.Add(lmData);
            }

            darkLMData = darkLMlist.ToArray();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.B))
            {
                SetBrightLM();
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                SetDarkLM();
            }   
        }

        void SetBrightLM()
        {
            LightmapSettings.lightmaps = brightLMData;
            LightmapSettings.lightProbes.bakedProbes = brightLMSO.lightProbesData;
        }
        void SetDarkLM()
        {
            LightmapSettings.lightmaps = darkLMData;
            LightmapSettings.lightProbes.bakedProbes = darkLMSO.lightProbesData;
        }


    }
}
