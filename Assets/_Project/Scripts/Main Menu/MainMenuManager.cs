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
        [field: SerializeField]public HUD _hud { get; private set; }
        [SerializeField] CanvasUI canvas;
        [SerializeField] Animator _animator;
        
        bool _muteVolume => !_volume.isOn;
        [SerializeField]Toggle _volume;
        [SerializeField] bool _done = true;
        [field: SerializeField] public SceneLoader _sceneLoader { get; private set; }

        public Action OpenSetting, OpenMenu;



        private void OnEnable()
        {
            _hud.StartGame += delegate
            {
                StartCoroutine(_sceneLoader.ChangeScene(ESceneName.ThePatient));
                ToggleChild(false);
                _hud.DisableMainMenu();
                canvas.gameObject.SetActive(true);
            };
            _hud.OpenMenu += Menu;
            _hud.OpenSetting += Setting;
        }

        public void ToggleChild(bool state)
        {
            foreach (Transform transform in transform)
            {
                transform.gameObject.SetActive(state);
            }
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
