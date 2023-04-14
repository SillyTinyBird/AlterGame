using UnityEngine.Audio;
using UnityEngine;

public class VolumeSliderScript : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public void SetVolumeMusic(float volume) => audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    public void SetVolumeSFX(float volume) => audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
}
