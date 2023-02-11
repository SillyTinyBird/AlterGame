using System;
using System.Collections;
using UnityEngine;
[DefaultExecutionOrder(-1)]
public class SwipeDetection : Singleton<SwipeDetection>
{
    [SerializeField] private float _minimumDistance = 0.2f;
    [SerializeField] private float _maximumTime = 1f;
    private InputManager inputManager;
    private Vector2 _startPos;
    private Vector2 _endPos;
    private float _startTime;
    private float _endTime;
    [SerializeField, Range(0f,1f)] private float _dirThreshold = .8f;

    public delegate void SwipeUp();
    public event SwipeUp OnSwipeUp;
    public delegate void SwipeDown();
    public event SwipeDown OnSwipeDown;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }
    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }
    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        _startPos = position;
        _startTime = time;
    }
    private void SwipeEnd(Vector2 position, float time)
    {
        _endPos = position;
        _endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Vector2.Distance(_startPos, _endPos) >= _minimumDistance && (_endTime - _startTime) <= _maximumTime)
        {
            Debug.DrawLine(_startPos,_endPos, Color.magenta, 5f);
            Vector3 direction = _endPos - _startPos;
            SwipeDirection(new Vector2(direction.x, direction.y).normalized);
        }
    }
    private void SwipeDirection(Vector2 direction)
    {
        if(Vector2.Dot(Vector2.up,direction)>_dirThreshold)
        {
            if (OnSwipeUp != null)
                OnSwipeUp();
        }
        else if (Vector2.Dot(Vector2.down, direction) > _dirThreshold)
        {
            if (OnSwipeDown != null)
                OnSwipeDown();
        }
        //add more directions here if necessary 
    }
}
