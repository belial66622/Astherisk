using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace ThePatient
{
    public class ControlSettingManager : SingletonBehaviour<ControlSettingManager>
    {
        public const string MOUSE_SENSIVITY = "MouseSensivity";

        [field: SerializeField]public float Mouse_Sensivity {  get; private set; }

        public event Action<float> OnSettingChanged;

        protected override void Awake()
        {
            base.Awake();
            Mouse_Sensivity = UpdateMouseSensivity();
        }
        public float UpdateMouseSensivity()
        {
            if (PlayerPrefs.HasKey(MOUSE_SENSIVITY))
            {
                Mouse_Sensivity = PlayerPrefs.GetFloat(MOUSE_SENSIVITY);
            }
            Debug.Log("Load setting");
            OnSettingChanged?.Invoke(Mouse_Sensivity);
            return Mouse_Sensivity;
        }
        public void SaveSetting(float value)
        {
            PlayerPrefs.SetFloat(MOUSE_SENSIVITY, value);
            PlayerPrefs.Save();
            Debug.Log("Save setting");
        }
    }
}
