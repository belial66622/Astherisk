using System.Collections;
using System.Collections.Generic;
using ThePatient;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities {
    public class SceneLoader
    {
        public string[] SceneName = {"Main Menu","the patient programmer" };


        public IEnumerator ChangeScene(ESceneName name)
        {
            Fader fader = GameObject.FindObjectOfType<Fader>();

            Debug.Log("fade out");
            yield return fader.FadeOut(.2f);


            Debug.Log("load the scene");
            yield return SceneManager.LoadSceneAsync(SceneName[((byte)name)]);
            Debug.Log("start bgm");
            AudioManager.Instance.PlayBGM("LevelBGM");
            Debug.Log("done load the scene");


            Debug.Log("wait before fader");
            yield return new WaitForSeconds(1);

            Debug.Log("finish wait do fader in");
            yield return fader.FadeIn(.2f);
        }

    }


    public enum ESceneName
    { 
        MainMenu,
        ThePatient
    }
}


