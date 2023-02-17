using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SwipeDetection _swipeDetection;
    private static int _layerID = 1;// 0 = lower; 1 = middle; 2 = upper.
    [SerializeField] private float _layerDelta = 5f;
    [SerializeField] private float _moveSpeed = 0.2f;
    [SerializeField] private float _jumpSpeed = 0.2f;
    [SerializeField] private float _jumpHeight = 2.5f;
    [SerializeField] private AnimationCurve _jumpCurve;
    private bool _stairsNearby = false;
    private string _currentTag = "";
    private bool _actionAllowed = false;// to prevent actions from accuring while another action is in motion, and also to esure that player can move only once per stair
    private bool _isJumpingOrSliding = false;
    private void Awake()
    {
        _swipeDetection = SwipeDetection.Instance;
        _layerID = 1;//we start on the middle layer
    }
    public static int LayerID => _layerID;
    private void OnEnable()
    {
        _swipeDetection.OnSwipeUp += SwipeUp;
        _swipeDetection.OnSwipeDown += SwipeDown;
    }
    private void OnDisable()
    {
        _swipeDetection.OnSwipeUp -= SwipeUp;
        _swipeDetection.OnSwipeDown -= SwipeDown;
    }
    private void SwipeUp()
    {
        if (_isJumpingOrSliding)
            return;
        if ((_layerID == 1 && _stairsNearby && _currentTag == "StairsUp" || _layerID == 0 && _stairsNearby) && _actionAllowed)
        {
            StartCoroutine(MoveToCoroutine(new Vector2(transform.position.x, transform.position.y + _layerDelta), _moveSpeed));
            _layerID += 1;
        }
        else
        {
            StartCoroutine(JumpAction(_jumpHeight, _jumpSpeed));
        }
    }
    private void SwipeDown()
    {
        if (_isJumpingOrSliding)
            return;
        if ((_layerID == 1 && _stairsNearby && _currentTag == "StairsDown" || _layerID == 2 && _stairsNearby) && _actionAllowed)
        {
            StartCoroutine(MoveToCoroutine(new Vector2(transform.position.x, transform.position.y - _layerDelta), _moveSpeed));
            _layerID -= 1;
        }
        else
        {
            Debug.Log("slide");
        }
    }
    IEnumerator MoveToCoroutine(Vector2 endPos, float moveSpeed)
    {
        float time = 0;
        Vector2 startPosition = transform.position;
        while (time < moveSpeed)
        {
            transform.position = Vector2.Lerp(startPosition, endPos, time / moveSpeed);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
        _actionAllowed = false;
    } 
    IEnumerator JumpAction(float height, float moveSpeed)
    {
        _isJumpingOrSliding = true;
        float time = 0;
        Vector2 startPosition = transform.position;
        Vector2 endPos = new Vector2(transform.position.x, transform.position.y + height);
        while (time < moveSpeed)
        {
            
            transform.position = Vector2.Lerp(startPosition, endPos, _jumpCurve.Evaluate(time / moveSpeed));
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;
        _isJumpingOrSliding = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _stairsNearby = true;
        _actionAllowed = true;
        _currentTag = collision.gameObject.tag;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _stairsNearby = false;
        _actionAllowed = false;
        _currentTag = "";
    }
}
