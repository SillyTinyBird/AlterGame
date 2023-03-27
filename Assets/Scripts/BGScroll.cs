using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{
    [SerializeField] private Vector2 _startPos;
    [SerializeField] private Vector2 _endPos;
    private float _timeElapsed;
    [SerializeField] private float _lerpDuration = 3;
    private float _lerpSpeed;
    [SerializeField] private bool _isInitialPosRandom = false;
    [SerializeField] private bool _isSpeedRandom = false;
    [SerializeField] private float _speedDelta = 10;
    private void Start()
    {
        if( _isInitialPosRandom)
        {
            _timeElapsed = Random.Range(0, _lerpDuration);
        }
        if( _isSpeedRandom)
        {
            _lerpSpeed = Random.Range(_lerpDuration - _speedDelta, _lerpDuration + _speedDelta);
        }
        else
        {
            _lerpSpeed = _lerpDuration;
        }
    }
    void Update()
    {
        if (_timeElapsed < _lerpSpeed)
        {
            transform.position = Vector2.Lerp(_startPos, _endPos, _timeElapsed / _lerpSpeed);
            _timeElapsed += Time.deltaTime;
        }
        else
        {
            if (_isSpeedRandom)
            {
                _lerpSpeed = Random.Range(_lerpDuration - _speedDelta, _lerpDuration + _speedDelta);
            }
            _timeElapsed = 0;
        }
    }
}
