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
        [SerializeField] Button _menu, _setting,_start;
        [SerializeField]bool _done = true;
        SceneLoader _sceneLoader;
        public bool done => _done;
        // Start is called before the first frame update
        void Start()
        {
            _sceneLoader = new SceneLoader();
            _menu.onClick.AddListener(Menu);
            _setting.onClick.AddListener(Setting);
            _start.onClick.AddListener(delegate { _sceneLoader.ChangeScene("MovementScene"); });
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

        public void Setting()
        {
            if (done)
            {
                _animator.SetTrigger("Setting");
            }
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
