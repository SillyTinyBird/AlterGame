using System.Collections;
using UnityEngine;

public class FootstepsSFXScript : MonoBehaviour
{
    private AudioClip[] _AudioClipMetalList;
    private AudioClip[] _AudioClipRooflList;
    private AudioClip[] _AudioClipScafflList;
    private bool _isPlaying = false;
    [Header("This script needs PlayerControoler attached to be working.")]
    [SerializeField] PlayerController _playerController;
    [Header("Parametters:")]
    [SerializeField] private float _delayInSeconds = 1f;
    [SerializeField, Range(0f, 1f)] float _volume = 0.5f;

    void Start()
    {
        _AudioClipMetalList = Resources.LoadAll<AudioClip>("Audio/SFX/Footsteps/Metal");//!!!!!!!!!!!!!
        _AudioClipRooflList = Resources.LoadAll<AudioClip>("Audio/SFX/Footsteps/Gravel");//if this static paths dont work you WILL get null exception
        _AudioClipScafflList = Resources.LoadAll<AudioClip>("Audio/SFX/Footsteps/Wood");//!!!!!!!!!!!!!
    }
    void Update()
    {
        if (_isPlaying || _playerController.IsActionBeeingPerformed || Time.timeScale == 0)
        {
            return;
        }
        if (_AudioClipMetalList.Length == 0 || _AudioClipRooflList.Length == 0 || _AudioClipScafflList.Length == 0)
        {
            Debug.LogWarning("Check audio clip paths in FootstepsSFXScript script! (they are static, sorry)");
            return;
        }
        AudioClip nextAudio;
        switch (_playerController.LayerID)
        {
            case 0:
                nextAudio = _AudioClipScafflList[Random.Range(0, _AudioClipScafflList.Length)];
                
                break;
            case 1:
                nextAudio = _AudioClipRooflList[Random.Range(0, _AudioClipRooflList.Length)];
                break;
            case 2:
                nextAudio = _AudioClipMetalList[Random.Range(0, _AudioClipMetalList.Length)];
                break;
            default:
                nextAudio = _AudioClipMetalList[Random.Range(0, _AudioClipMetalList.Length)];
                break;
        }
        AudioManager.PlaySFX(nextAudio, _volume);
        StartCoroutine(FlagWaitCoroutine(_delayInSeconds));
        //^^^is needed to stop audio flood cause AudioManager.PlaySFX is using AudioSource.PlayOneShot and thus clips can layer onto each other
    }
    IEnumerator FlagWaitCoroutine(float seconds)
    {
        _isPlaying = true;
        yield return new WaitForSeconds(seconds);//because its usses unity timescale, it will wait indefinitely on pause
        _isPlaying = false;
    }
}
