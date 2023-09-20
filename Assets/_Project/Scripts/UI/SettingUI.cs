using ThePatient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Transform _menuTransform;

    [Header("Setting Menu")]
    [SerializeField] Button _backMenu;
    [SerializeField] Button _saveButton;
    [SerializeField] Button _control;
    [SerializeField] Button _sound;
    [SerializeField] Button _graphic;
    [SerializeField] GameObject _controlDisplay;
    [SerializeField] GameObject _soundDisplay;
    [SerializeField] GameObject _graphicDisplay;


    [Header("Control Display")]
    [SerializeField] Slider _mouseSensitivity;
    [SerializeField] TextMeshProUGUI _sensValue;


    [Header("Sound Display")]
    [SerializeField] Slider _bgmVolume;
    [SerializeField] Slider _sfxVolume;
    [SerializeField] TextMeshProUGUI _bgmValue;
    [SerializeField] TextMeshProUGUI _sfxValue;

    private void OnEnable()
    {
        OptionLoad();
    }
    private void Start()
    {
        _control.onClick.AddListener(() => { EnableDisplay(_controlDisplay); });
        _sound.onClick.AddListener(() => { EnableDisplay(_soundDisplay); });
        _graphic.onClick.AddListener(() => { EnableDisplay(_graphicDisplay); });
        _bgmVolume.onValueChanged.AddListener(delegate
        {
            AudioManager.Instance.BGMVolume(_bgmVolume.value);
            _bgmValue.text = _bgmVolume.value.ToString();
        });

        _sfxVolume.onValueChanged.AddListener(delegate
        {
            AudioManager.Instance.SFXVolume(_sfxVolume.value);
            _sfxValue.text = _sfxVolume.value.ToString();
        });
        _mouseSensitivity.onValueChanged.AddListener((float value) =>
        {
            _sensValue.text = value.ToString("#.#");
        });
        _backMenu.onClick.AddListener(delegate
        {
            _menuTransform.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
        _saveButton.onClick.AddListener(() =>
        {
            ControlSettingManager.Instance.SaveSetting(_mouseSensitivity.value);
        });
    }
    void EnableDisplay(GameObject transform)
    {
        SettingClear();
        transform.SetActive(true);
    }

    void SettingClear()
    {
        _controlDisplay.SetActive(false);
        _graphicDisplay.SetActive(false);
        _soundDisplay.SetActive(false);
    }

    void OptionLoad()
    {
        //mouse sensivity
        var sensValue = ControlSettingManager.Instance.UpdateMouseSensivity();
        _sensValue.text = sensValue.ToString("#.#");
        _mouseSensitivity.value = sensValue;

        //Sound
        _sfxValue.text = PlayerPrefs.GetFloat("_SFXVolume").ToString();
        _bgmValue.text = PlayerPrefs.GetFloat("_BGMVolume").ToString();
        _sfxVolume.value = PlayerPrefs.GetFloat("_SFXVolume");
        _bgmVolume.value = PlayerPrefs.GetFloat("_BGMVolume");
    }
}
