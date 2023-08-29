using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities { 
    public class SceneLoader
    {

            public void ChangeScene(string scene)
            {
                SceneManager.LoadScene(scene);
                Debug.Log(scene);
            }
        }
    }
