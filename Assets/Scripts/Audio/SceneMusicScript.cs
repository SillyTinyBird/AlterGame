using UnityEngine;

public class SceneMusicScript : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    private void Start()
    {
        AudioSource audio = AudioManager.instance.GetMusicAudioSource();//we need to check clip so we need an instance of AudioManager
        if (audio.clip != clip)
        {
            audio.clip = clip;
            audio.Play();
        }
    }
}
