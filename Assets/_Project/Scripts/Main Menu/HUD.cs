using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace ThePatient
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] MainMenuManager _menuManager;



        [Header("Main Menu Button")]
        [SerializeField] Button _start;
        [SerializeField] Button _setting;
        [SerializeField] Button _exit;


        [Header("Menu Canvas")]
        [SerializeField] GameObject _menuCanvas; 
        [SerializeField] GameObject _settingCanvas;


        [Header("Setting Menu")]
        [SerializeField] Button _backMenu;
        [SerializeField] Button _saveButton;
        [SerializeField] Button _control,_sound,_graphic;
        [SerializeField] GameObject _controlDisplay,_soundDisplay,_graphicDisplay;


        [Header("Control Display")]
        [SerializeField] Slider _mouseSensitivity;
        [SerializeField] TextMeshProUGUI _sensValue;


        [Header("Sound Display")]
        [SerializeField] Toggle _bgm;
        [SerializeField] Toggle _sfx;
        [SerializeField] Slider _bgmVolume, _sfxVolume;
        [SerializeField] TextMeshProUGUI _bgmValue, _sfxValue;

        public Action StartGame,OpenMenu,OpenSetting;
 

        private void Awake()
        {
            
        }

        private void OnEnable()
        {
            if(_menuManager != null)
            {
                _menuManager.OpenMenu += MainMenu;
                _menuManager.OpenSetting += Setting;
            }
        }

        private void OnDisable()
        {
            if (_menuManager != null)
            {
                _menuManager.OpenMenu -= MainMenu;
                _menuManager.OpenSetting -= Setting;
            }
        }

        private void Start()
        {
            _backMenu.onClick.AddListener(delegate 
            {   ClearHUD();
                OpenMenu?.Invoke();
            });
            _saveButton.onClick.AddListener(() => 
            {
                ControlSettingManager.Instance.SaveSetting(_mouseSensitivity.value);
                OptionLoad();
            });
            _exit.onClick.AddListener(Exit);
            _setting.onClick.AddListener(delegate 
            {   ClearHUD(); 
                OpenSetting?.Invoke(); 
            });
            _start.onClick.AddListener(delegate { 
                StartGame?.Invoke(); 
                _menuCanvas.SetActive(false);
            });
            _control.onClick.AddListener(SettingControl);
            _sound.onClick.AddListener(SettingSound);
            _graphic.onClick.AddListener(SettingGraphic);
            _bgmVolume.onValueChanged.AddListener(delegate 
            {
                AudioManager.Instance.BGMVolume(_bgmVolume.value);                
                _bgmValue.text = _bgmVolume.value.ToString();
            });

            _sfxVolume.onValueChanged.AddListener(delegate
            {
                AudioManager.Instance.SFXVolume(_sfxVolume.value);
                _sfxValue.text = _sfxVolume.value.ToString();
            });

            _bgm.onValueChanged.AddListener(delegate 
            {
                AudioManager.Instance.UpdateBGMSetting(!_bgm.isOn);
                _bgmValue.text = _bgmVolume.value.ToString();
            }
            );
            _sfx.onValueChanged.AddListener(delegate 
            {
                AudioManager.Instance.UpdateSFXSetting(!_sfx.isOn);
                _sfxValue.text = _sfxVolume.value.ToString();
            }
            );
            _mouseSensitivity.onValueChanged.AddListener((float value) =>
            {
                _sensValue.text = value.ToString("#.#");
            });

            OptionLoad();
        }



        void SettingControl()
        { 
            SettingClear();
            _controlDisplay.SetActive(true);
        }

        void SettingSound()
        {
            SettingClear();
            _soundDisplay.SetActive(true);
        }


        void SettingGraphic()
        {
            SettingClear();
            _graphicDisplay.SetActive(true);
        }

        void SettingClear()
        { 
            _controlDisplay.SetActive(false);
            _graphicDisplay.SetActive(false);
            _soundDisplay.SetActive(false);
        }


        public void MainMenu()
        {
            _menuCanvas.SetActive(true);
        }
        public void DisableMainMenu()
        {
            _menuCanvas.SetActive(false);
        }
        void Setting()
        {
            var sensValue = ControlSettingManager.Instance.UpdateMouseSensivity();
            _sensValue.text = sensValue.ToString("#.#");
            _mouseSensitivity.value = sensValue;
            _settingCanvas.SetActive(true);
            _control.Select();
        }

        void ClearHUD()
        {
            _settingCanvas.SetActive(false);
            _menuCanvas.SetActive(false);

        }

        public void Exit()
        {
                Application.Quit();
        }


        void OptionLoad()
        {
            //mouse sensivity
            var sensValue = ControlSettingManager.Instance.UpdateMouseSensivity();
            _sensValue.text = sensValue.ToString("#.#");
            _mouseSensitivity.value = sensValue;

            //Sound
            _bgm.isOn = PlayerPrefs.GetInt("_isBGMMute") == 0;
            _sfx.isOn = PlayerPrefs.GetInt("_isSFXMute") == 0;
            _sfxValue.text = PlayerPrefs.GetFloat("_SFXVolume").ToString();
            _bgmValue.text = PlayerPrefs.GetFloat("_BGMVolume").ToString();
            _sfxVolume.value = PlayerPrefs.GetFloat("_SFXVolume");
            _bgmVolume.value = PlayerPrefs.GetFloat("_BGMVolume");
        }
    }
}
