using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities {
    public class SceneLoader
    {
        public string[] SceneName = {"Main Menu","the patient" };


        public void ChangeScene(ESceneName name)
        {
            AudioManager.Instance.PlayBGM("LevelBGM");
            SceneManager.LoadScene(SceneName[((byte)name)]);

        }

    }


    public enum ESceneName
    { 
        MainMenu,
        ThePatient
    }
}


