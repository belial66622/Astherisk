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
        [SerializeField] Button _exit;
        [SerializeField] Button _setting, _start;


        [Header("Menu Canvas")]
        [SerializeField] GameObject _menuCanvas; 
        [SerializeField] GameObject _settingCanvas;


        [Header("Setting Menu")]
        [SerializeField] Button _backMenu;
        [SerializeField] Button _control,_sound,_graphic;
        [SerializeField] GameObject _controlDisplay,_soundDisplay,_graphicDisplay;


        [Header("Control Display")]
        [SerializeField] Slider _mouseSensitivity;


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
            _menuManager.OpenMenu += MainMenu;
            _menuManager.OpenSetting += Setting;
        }

        private void OnDisable()
        {
            _menuManager.OpenMenu -= MainMenu;
            _menuManager.OpenSetting -= Setting;
        }

        private void Start()
        {
            _backMenu.onClick.AddListener(delegate 
            {   ClearHUD();
                OpenMenu?.Invoke();
            });
            _exit.onClick.AddListener(Exit);
            _setting.onClick.AddListener(delegate 
            {   ClearHUD(); 
                OpenSetting?.Invoke(); 
            });
            _start.onClick.AddListener(delegate { StartGame?.Invoke(); });
            _control.onClick.AddListener(SettingControl);
            _sound.onClick.AddListener(SettingSound);
            _graphic.onClick.AddListener(SettingGraphic);
            _bgmVolume.onValueChanged.AddListener(delegate 
            {
                AudioManager.Instance.BGMVolume((_bgmVolume.value - 100) * 0.8f);                
                _bgmValue.text = _bgmVolume.value.ToString();
            });

            _sfxVolume.onValueChanged.AddListener(delegate
            {
                AudioManager.Instance.SFXVolume((_sfxVolume.value - 100) * 0.8f);
                _sfxValue.text = _sfxVolume.value.ToString();
            });

            _bgm.onValueChanged.AddListener(delegate 
            {
                AudioManager.Instance.UpdateBGMSetting(!_bgm.isOn);
            }
            );
            _sfx.onValueChanged.AddListener(delegate 
            {
                AudioManager.Instance.UpdateSFXSetting(!_bgm.isOn);
            }
            );
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


        void MainMenu()
        {
            _menuCanvas.SetActive(true);
        }

        void Setting()
        {
            _settingCanvas.SetActive(true);

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
    }
}
