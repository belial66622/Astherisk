using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ThePatient
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] Button _menu, _setting;
        [SerializeField]bool _done = true;
        public bool done => _done;
        // Start is called before the first frame update
        void Start()
        {
            _menu.onClick.AddListener(Menu);
            _setting.onClick.AddListener(Setting);
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
