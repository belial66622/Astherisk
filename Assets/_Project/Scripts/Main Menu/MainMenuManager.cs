using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace ThePatient
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] HUD _hud;
        [SerializeField] Animator _animator;
        
        bool _muteVolume => !_volume.isOn;
        [SerializeField]Toggle _volume;
        [SerializeField] bool _done = true;
        SceneLoader _sceneLoader;

        public Action OpenSetting, OpenMenu;



        private void OnEnable()
        {
            _hud.StartGame += delegate { StartCoroutine(_sceneLoader.ChangeScene(ESceneName.ThePatient)); };
            _hud.OpenMenu += Menu;
            _hud.OpenSetting += Setting;
        }

        private void OnDisable()
        {
            
        }


        public bool done => _done;
        // Start is called before the first frame update
        void Start()
        {
            _sceneLoader = new SceneLoader();

            AudioManager.Instance.PlayBGM("MainMenuBGM");
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Menu()
        {
                _animator.CrossFade("Menu",0f,-1,0f);
                PlayerPrefs.Save();
        }




        public void Setting()
        {
                _animator.CrossFade("Setting",0f,-1,0f);
        }


      

        public void Settingsend()
        {
            OpenSetting?.Invoke();
        }

        public void Menusend()
        {
            OpenMenu?.Invoke();
        }

    }
}
