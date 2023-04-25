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
            if (!_isTutorialCompleete)//cause PlayerPrefs dosent have bool
            {
                PlayerPrefs.SetInt("isTutorialCompleete", 0);
            }
            else
            {
                PlayerPrefs.SetInt("isTutorialCompleete", 1);
            }
        }
    }

    private void Awake()
    {
        _isTutorialCompleete = PlayerPrefs.GetInt("isTutorialCompleete") == 0 ? false : true ;
        _tutorialToggle.isOn = _isTutorialCompleete;
        _musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        _SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }
    private void OnDestroy()
    {
        if (!_isTutorialCompleete)//cause PlayerPrefs dosent have bool
        {
            PlayerPrefs.SetInt("isTutorialCompleete", 0);
        }
        else
        {
            PlayerPrefs.SetInt("isTutorialCompleete", 1);
        }
        PlayerPrefs.SetFloat("MusicVolume", _musicVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", _SFXVolumeSlider.value);
    }

}
