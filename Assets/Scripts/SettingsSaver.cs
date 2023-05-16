using UnityEngine;
using UnityEngine.UI;

public class SettingsSaver : MonoBehaviour
{
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _SFXVolumeSlider;
    [SerializeField] private Toggle _tutorialToggle;
    private static bool _isTutorialCompleete = false;//static so we dont need an instance of the class in every scene

    public static bool IsTutorialCompleete
    {
        get => _isTutorialCompleete;
        set
        {
            _isTutorialCompleete = value;
            if (_isTutorialCompleete)//cause PlayerPrefs dosent have bool
            {
                PlayerPrefs.SetInt("isTutorialCompleete", 1);
            }
            else
            {
                PlayerPrefs.SetInt("isTutorialCompleete", 0);
            }
        }
    }

    private void Awake()
    {
        _isTutorialCompleete = PlayerPrefs.GetInt("isTutorialCompleete") == 0 ? false : true ;
        _tutorialToggle.isOn = _isTutorialCompleete;

        _musicVolumeSlider.value = PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume") : 1;
        _SFXVolumeSlider.value = PlayerPrefs.HasKey("SFXVolume") ? PlayerPrefs.GetFloat("SFXVolume") : 1;
    }
    private void OnDestroy()
    {
        if (_isTutorialCompleete)//cause PlayerPrefs dosent have bool
        {
            PlayerPrefs.SetInt("isTutorialCompleete", 1);
        }
        else
        {
            PlayerPrefs.SetInt("isTutorialCompleete", 0);
        }
        PlayerPrefs.SetFloat("MusicVolume", _musicVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", _SFXVolumeSlider.value);
    }

}
