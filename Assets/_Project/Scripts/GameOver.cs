using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

namespace ThePatient
{
    public class GameOver : MonoBehaviour
    {
        SceneLoader _loader;
        [SerializeField]Button _tryAgain, _mainMenu;
        [SerializeField] GameObject save;
        // Start is called before the first frame update

        private void OnEnable()
        {
            Cursor.visible = true;
        }

        void Start()
        {
            _loader = new SceneLoader();
            _tryAgain.onClick.AddListener(delegate 
            { 
                save.GetComponent<SavingWrapper>().Save();
                StartCoroutine(_loader.ChangeScene(ESceneName.ThePatient)); 
            });
            _mainMenu.onClick.AddListener(delegate 
            {
                save.GetComponent<SavingWrapper>().Save();
                StartCoroutine(_loader.ChangeScene(ESceneName.MainMenu)); 
            });
            save.GetComponent<SavingWrapper>().Load();
        }

        // Update is called once per frame
        void Update()
        {
        
        }



    }
}
