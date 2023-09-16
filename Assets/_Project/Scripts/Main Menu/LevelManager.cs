using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayBGM("MainMenuBGM");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void bukapintu()
    {
        AudioManager.Instance.PlayBGM("BukaPintu");
    }

    public void chase()
    {
        AudioManager.Instance.PlayBGM("Chase");
    }
}
