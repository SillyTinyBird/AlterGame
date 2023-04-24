using System;
using UnityEngine;

public class FailSFX : MonoBehaviour
{
    [SerializeField] ObstacleScript _obstacleScript;
    [SerializeField] AudioClip _fallSFX;
    [SerializeField] AudioClip _hitSFX;
    [SerializeField, Range(0f, 1f)] float _volume = 0.5f;
    public void PlayDeathSFX()
    {
        Tuple<bool, int> deathData = _obstacleScript.GetDeathData;
        if (deathData.Item1)//fall
        {
            AudioManager.PlaySFX(_fallSFX, _volume);
        }
        else//hit
        {
            AudioManager.PlaySFX(_hitSFX, _volume);
        }
    }
}
