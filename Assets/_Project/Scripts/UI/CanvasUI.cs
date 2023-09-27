using System;
using System.Collections;
using System.Collections.Generic;
using ThePatient;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class CanvasUI : SingletonBehaviour<CanvasUI>
{
    public string[] SceneName = { "Main Menu", "the patient programmer" };

    [System.Serializable]
    class InteractionIconUI
    {
        public InteractionType interactionType;
        public Sprite icon;
    }

    [SerializeField] InteractionIconUI[] interactionIconsUI;

    [Header("Reference")]
    [SerializeField] Transform buttonEIcon;
    [SerializeField] Transform buttonHoldEIcon;
    [SerializeField] Transform lockUI;
    [SerializeField] Transform inspectUI;
    [SerializeField] Transform inspectLockPuzzleUI;
    [SerializeField] Image interactionIconBG;
    [SerializeField] Image interactionIcon;
    [SerializeField] Image lockUISlider;
    [SerializeField] TextMeshProUGUI interactPopupText;


    [Header("Pause Menu")]
    [SerializeField] Transform pauseMenu;
    [SerializeField] Transform settingPanel;
    [SerializeField] Button resume;
    [SerializeField] Button setting;
    [SerializeField] Button exit;


    [Header("Game Over")]
    [SerializeField] GameObject _gameOver;
    [SerializeField] Button _gameOverButton;

    [Header("!!! TEMP, DELETE LATER !!!")]
    [SerializeField] InputReader input;

    protected override void Awake()
    {
        base.Awake();

        resume.onClick.AddListener(Resume);
        setting.onClick.AddListener(Setting);
        exit.onClick.AddListener(Exit);
        _gameOverButton.onClick.AddListener(ExitGameOver);
    }
    public void Exit()
    {
        //Application.Quit();
        MainMenuManager mainmenu = FindObjectOfType<MainMenuManager>();
        StartCoroutine(mainmenu._sceneLoader.ChangeScene(Utilities.ESceneName.MainMenu));
        Input_Pause(false);
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode loadSceneMode) => 
        { 
            if (scene == SceneManager.GetSceneByName(SceneName[0]))
            {
                input.DisablePlayerControll();
                mainmenu.ToggleChild(true);
                mainmenu._hud.MainMenu();
                Inventory.Instance.ResetItem();
            }
        };
    }

    private void Setting()
    {
        pauseMenu.gameObject.SetActive(false);
        settingPanel.gameObject.SetActive(true);
    }

    private void Resume()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        input.EnablePlayerControll();
        pauseMenu.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        EventAggregate<InteractionIconEventArgs>.Instance.StartListening(UpdateInteractionIcon);
        EventAggregate<InteractionLockUIEventArgs>.Instance.StartListening(UpdateLockSliderUI);
        EventAggregate<InteractionInspectEventArgs>.Instance.StartListening(UpdateInspectUI);
        EventAggregate<InteractionLockPuzzleEventArgs>.Instance.StartListening(UpdateInspectLockPuzzleUI);
        input.Pause += Input_Pause;
    }

    private void Input_Pause(bool perform)
    {
        if (!perform && pauseMenu.gameObject.activeSelf)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            input.EnablePlayerControll();
        }
        else
        {
            ControlSettingManager.Instance.UpdateMouseSensivity();
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            input.EnableUIControl();
            input.DisablePlayerControll();
        }
        if(settingPanel.gameObject.activeSelf)
        {
            settingPanel.gameObject.SetActive(false);
        }
        pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
    }

    private void OnDisable()
    {
        EventAggregate<InteractionIconEventArgs>.Instance.StopListening(UpdateInteractionIcon);
        EventAggregate<InteractionLockUIEventArgs>.Instance.StopListening(UpdateLockSliderUI);
        EventAggregate<InteractionInspectEventArgs>.Instance.StopListening(UpdateInspectUI);
        EventAggregate<InteractionLockPuzzleEventArgs>.Instance.StopListening(UpdateInspectLockPuzzleUI);
        input.Pause -= Input_Pause;
    }

    private void UpdateInspectLockPuzzleUI(InteractionLockPuzzleEventArgs e)
    {
        if (e.isActive)
        {
            inspectLockPuzzleUI.gameObject.SetActive(true);
        }
        else
        {
            inspectLockPuzzleUI.gameObject.SetActive(false);
        }
    }

    private void UpdateInspectUI(InteractionInspectEventArgs e)
    {
        if (e.isActive)
        {
            inspectUI.gameObject.SetActive(true);
        }
        else
        {
            inspectUI.gameObject.SetActive(false);
        }
    }

    void UpdateInteractionIcon(InteractionIconEventArgs e)
    {
        interactionIcon.sprite = GetIcon(e.interactionType);
        if (e.isActive)
        {
            interactionIconBG.gameObject.SetActive(true);
            if(e.interactionType == InteractionType.NoKey)
            {
                buttonEIcon.gameObject.SetActive(false);
            }
            else
            {
                if (e.interactionType == InteractionType.Locked)
                {
                    buttonHoldEIcon.gameObject.SetActive(true);
                }
                else
                {
                    buttonEIcon.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            buttonEIcon.gameObject.SetActive(false);
            buttonHoldEIcon.gameObject.SetActive(false);
            interactionIconBG.gameObject.SetActive(false);
        }
    }

    Sprite GetIcon(InteractionType interactionType)
    {
        foreach (var icon in interactionIconsUI)
        {
            if (icon.interactionType == interactionType)
            {
                return icon.icon;
            }
        }
        return GetIcon(InteractionType.Default);
    }

    void UpdateLockSliderUI(InteractionLockUIEventArgs e)
    {
        //if(e.isActive)
        //{
        //    lockUI.gameObject.SetActive(true);
        //}
        //else
        //{
        //    lockUI.gameObject.SetActive(false);
        //}
        interactionIconBG.fillAmount = e.valueFraction;
    }

    public void GameoverHUD()
    {
        Debug.Log("pause"); 
        ControlSettingManager.Instance.UpdateMouseSensivity();
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        input.EnableUIControl();
        input.DisablePlayerControll();
        _gameOver.SetActive(true);

    }

    public void ExitGameOver()
    {
        //Application.Quit();
        _gameOver.SetActive(false);
        MainMenuManager mainmenu = FindObjectOfType<MainMenuManager>();
        StartCoroutine(mainmenu._sceneLoader.ChangeScene(Utilities.ESceneName.MainMenu));
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode loadSceneMode) =>
        {
            if (scene == SceneManager.GetSceneByName(SceneName[0]))
            {
                input.DisablePlayerControll();
                mainmenu.ToggleChild(true);
                mainmenu._hud.MainMenu();
                Inventory.Instance.ResetItem();
            }
        };
    }
}
