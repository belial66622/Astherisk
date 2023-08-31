using System;
using System.Collections;
using System.Collections.Generic;
using ThePatient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUI : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] TextMeshProUGUI interactPopupText;
    [SerializeField] Transform lockUI;
    [SerializeField] Image lockUISlider;
    [SerializeField] Transform inspectUI;
    [SerializeField] Transform inspectLockPuzzleUI;

    [Header("Pause Menu")]
    [SerializeField] Transform pauseMenu;
    [SerializeField] Button resume;
    [SerializeField] Button setting;
    [SerializeField] Button exit;

    [Header("!!! TEMP, DELETE LATER !!!")]
    [SerializeField] InputReader input;

    private void Awake()
    {
        resume.onClick.AddListener(Resume);
        setting.onClick.AddListener(Setting);
        exit.onClick.AddListener(Exit);
    }

    private void Exit()
    {
        Application.Quit();
    }

    private void Setting()
    {

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
        EventAggregate<InteractionTextEventArgs>.Instance.StartListening(UpdateInteractionText);
        EventAggregate<InteractionLockUIEventArgs>.Instance.StartListening(UpdateLockSliderUI);
        EventAggregate<InteractionInspectEventArgs>.Instance.StartListening(UpdateInspectUI);
        EventAggregate<InteractionLockPuzzleEventArgs>.Instance.StartListening(UpdateInspectLockPuzzleUI);
        input.Pause += Input_Pause;
    }

    private void Input_Pause(bool state)
    {
        if (!state && pauseMenu.gameObject.activeSelf)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            input.EnablePlayerControll();
        }
        else
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            input.EnableUIControl();
            input.DisablePlayerControll();
        }
        pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
    }

    private void OnDisable()
    {
        EventAggregate<InteractionTextEventArgs>.Instance.StopListening(UpdateInteractionText);
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

    void UpdateInteractionText(InteractionTextEventArgs e)
    {
        if (e.isActive)
        {
            interactPopupText.gameObject.SetActive(true);
        }
        else
        {
            interactPopupText.gameObject.SetActive(false);
        }
        interactPopupText.text = e.message;
    }

    void UpdateLockSliderUI(InteractionLockUIEventArgs e)
    {
        if(e.isActive)
        {
            lockUI.gameObject.SetActive(true);
        }
        else
        {
            lockUI.gameObject.SetActive(false);
        }
        lockUISlider.fillAmount = e.valueFraction;
    }
}
