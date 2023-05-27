using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _duration;
    [SerializeField] private AnimationCurve _fallCurve;
    [SerializeField] private AnimationCurve _bonkCurve;
    [SerializeField] private VFXcontroller _vfx;
    static bool _isPlayingAnimation = false;

    public static bool IsPlayingAnimation { get => _isPlayingAnimation; set => _isPlayingAnimation = value; }

    public IEnumerator FallDeathAnimation()
    {
        _isPlayingAnimation = true;
        _animator.SetBool("IsFalling", true);
        _vfx.SetActiveDustRun(false);
        float time = 0;
        while (time < _duration+0.5f)
        {
            time += Time.deltaTime;
            transform.position = new Vector3(transform.position.x , transform.position.y + _fallCurve.Evaluate(time / _duration));
            yield return null;
        }
        _animator.SetBool("IsFalling", false);
        _isPlayingAnimation = false;
    }
    public IEnumerator BonkDeathAnimation()
    {
        _isPlayingAnimation = true;
        _animator.SetBool("IsBonked", true);
        float time = 0;
        while (time < _duration+0.5f)
        {
            time += Time.deltaTime;
            transform.position = new Vector3(transform.position.x - Mathf.Lerp(0f, 0.115f, _bonkCurve.Evaluate(time / _duration)), transform.position.y);
            yield return null;
        }
        _animator.SetBool("IsBonked", false);
        _isPlayingAnimation = false;
    }
}
