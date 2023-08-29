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

    private void OnEnable()
    {
        EventAggregate<InteractionTextEventArgs>.Instance.StartListening(UpdateInteractionText);
        EventAggregate<InteractionLockUIEventArgs>.Instance.StartListening(UpdateLockSliderUI);
    }
    private void OnDisable()
    {
        EventAggregate<InteractionTextEventArgs>.Instance.StopListening(UpdateInteractionText);
        EventAggregate<InteractionLockUIEventArgs>.Instance.StopListening(UpdateLockSliderUI);
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
