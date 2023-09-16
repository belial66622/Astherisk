using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class MainMenuBg : MonoBehaviour
    {
        [SerializeField]GameObject Artist1;
        bool _change =false;
        public bool Change => _change;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Flicker()
        {
            if (Change == true)
            { 
                _change= false;
                Artist1.SetActive(false);
            }
            else 
            {
                _change = true;
                Artist1.SetActive(true);
            }


        }
    }
}
