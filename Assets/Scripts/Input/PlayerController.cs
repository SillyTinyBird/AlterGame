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
    [SerializeField] private SpriteRenderer _spriteRendererSoWeCanChangeRenderOrderOnTheGo;
    private bool _stairsNearby = false;
    private string _currentTag = "";
    private bool _actionAllowed = false;// to prevent actions from accuring while another action is in motion, and also to esure that player can move only once per stair
    private bool _isJumpingOrSliding = false;
    [SerializeField] private Animator _animarot; 
    private void Awake()
    {
        _swipeDetection = SwipeDetection.Instance;
        _layerID = 1;//we start on the middle layer
        UpdateSpriteLayer();
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
    private void UpdateSpriteLayer()
    {
        switch (_layerID)
        {
            case 0:
                _spriteRendererSoWeCanChangeRenderOrderOnTheGo.sortingOrder = 18; return;
            case 1:
                _spriteRendererSoWeCanChangeRenderOrderOnTheGo.sortingOrder = 15; return;
            case 2:
                _spriteRendererSoWeCanChangeRenderOrderOnTheGo.sortingOrder = 12; return;
            default: 
                _spriteRendererSoWeCanChangeRenderOrderOnTheGo.sortingOrder = 12; return;
        }
            
    }
    private void SwipeUp()
    {
        if (_isJumpingOrSliding)
            return;
        if ((_layerID == 1 && _stairsNearby && _currentTag == "StairsUp" || _layerID == 0 && _stairsNearby) && _actionAllowed)
        {
            StartCoroutine(MoveToCoroutine(new Vector2(transform.position.x, transform.position.y + _layerDelta), _moveSpeed));
            _layerID += 1;
            UpdateSpriteLayer();
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
            UpdateSpriteLayer();
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
        _animarot.SetBool("IsJumping", true);
        float time = 0;
        Vector2 startPosition = transform.position;
        Vector2 endPos = new(transform.position.x, transform.position.y + height);
        while (time < moveSpeed)
        {
            
            transform.position = Vector2.Lerp(startPosition, endPos, _jumpCurve.Evaluate(time / moveSpeed));
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;
        _animarot.SetBool("IsJumping", false);
        _isJumpingOrSliding = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Coin"))//coin collection logic
        {
            ScoreSystem.AddPoints(250);
            collision.gameObject.SetActive(false);
            return;//cause we didnt actually entered stairs zone, no need for anything else below
        }
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
