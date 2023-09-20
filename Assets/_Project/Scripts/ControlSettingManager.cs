using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace ThePatient
{
    public class ControlSettingManager : SingletonBehaviour<ControlSettingManager>
    {
        public const string MOUSE_SENSIVITY = "MouseSensivity";

        public float Mouse_Sensivity {  get; private set; }

        protected override void Awake()
        {
            base.Awake();
            UpdateMouseSensivity();
        }
        public float UpdateMouseSensivity()
        {
            if (PlayerPrefs.HasKey(MOUSE_SENSIVITY))
            {
                Mouse_Sensivity = PlayerPrefs.GetFloat(MOUSE_SENSIVITY);
            }
            return Mouse_Sensivity;
        }
        public void SaveSetting(float value)
        {
            PlayerPrefs.SetFloat(MOUSE_SENSIVITY, value);
            PlayerPrefs.Save();
        }
    }
}
