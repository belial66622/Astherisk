using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace ThePatient
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [Header("Menu Button")]
        [SerializeField] Button _menu, _setting, _start, _backMenu;
        [SerializeField] bool _done = true;
        SceneLoader _sceneLoader;
        [SerializeField] GameObject _menuCanvas, _settingCanvas;

        public bool done => _done;
        // Start is called before the first frame update
        void Start()
        {
            _sceneLoader = new SceneLoader();
            _backMenu.onClick.AddListener(Menu);
            _menu.onClick.AddListener(Exit);
            _setting.onClick.AddListener(Setting);
            _start.onClick.AddListener(delegate { _sceneLoader.ChangeScene(ESceneName.ThePatient); });
            AudioManager.Instance.PlayBGM("MainMenuBGM");
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Menu()
        {
            if (done)
            {
                _animator.SetTrigger("Menu");
            }
        }


        public void Exit()
        {
            if (done)
            {
                Application.Quit();
            }
        }

        public void Setting()
        {
            if (done)
            {
                _animator.SetTrigger("Setting");
            }
        }


        void OpenMainMenu()
        {
            _menuCanvas.SetActive(true);
            ClearHUD();
        }

        void OpenSetting()
        {
            _settingCanvas.SetActive(true);
            ClearHUD();
        }

        void ClearHUD()
        {
            _settingCanvas.SetActive(false);
            _menuCanvas.SetActive(false);
        }

        public void Done()
        {
            _done = true;
        }

        public void NotDone()
        {
           _done = false;
        }

    }
}
