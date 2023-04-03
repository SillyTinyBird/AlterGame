using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SwipeDetection _swipeDetection;
    private static int _layerID = 1;// 0 = lower; 1 = middle; 2 = upper.
    [Header("Jump Settings")]
    [SerializeField] private float _jumpSpeed = 0.2f;
    [SerializeField] private float _jumpHeight = 2.5f;
    [SerializeField] private AnimationCurve _jumpCurve;
    [Header("Ascend/Descend Settings")]
    [SerializeField] private float _layerDelta = 5f;//im keeping this only for reference
    [SerializeField] private float _moveSpeed = 0.2f;
    [SerializeField] float _interactedGameObjectXAxisOffset = 0;
    [SerializeField] private float _ascendDescendHopHeight = 1.6f;//note that whenever this value changes, animation curve shoud be changed in inspector accordingly
    [SerializeField] private AnimationCurve _extrapolationCurve;
    [Header("Misc Settings")]
    [SerializeField] private Animator _animarot;
    [SerializeField] private SpriteRenderer _spriteRendererSoWeCanChangeRenderOrderOnTheGo;
    private string _currentTag = "";
    private int _countOfCurrentlyColidedObjects = 0;
    private bool _isPerformingAction = false;
    
    private Transform _interactedGameObjectTransform;
    
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
        if (_isPerformingAction)
            return;
        if ((_layerID == 1 && _currentTag == "StairsUp" || _layerID == 0) && _countOfCurrentlyColidedObjects > 0)
        {
            //StartCoroutine(MoveToCoroutine(new Vector2(transform.position.x, transform.position.y + _layerDelta), _moveSpeed));
            StartCoroutine(AscendAnimationCoroutine(_ascendDescendHopHeight, _moveSpeed));
            Debug.Log("Ascend");
            _layerID += 1;
            UpdateSpriteLayer();
        }
        else
        {
            StartCoroutine(JumpAnimationCoroutine(_jumpHeight, _jumpSpeed));
            Debug.Log("Jump");
        }
    }
    private void SwipeDown()
    {
        if (_isPerformingAction)
            return;
        if ((_layerID == 1 && _currentTag == "StairsDown" || _layerID == 2) && _countOfCurrentlyColidedObjects > 0)
        {
            StartCoroutine(DescendAnimationCoroutine(_ascendDescendHopHeight, _moveSpeed));
            _layerID -= 1;
            UpdateSpriteLayer();
        }
        else
        {
            StartCoroutine(SlideAnimationCoroutine(_jumpHeight, _jumpSpeed));
            /*if(_interactedGameObjectTransform != null)
            {
                StartCoroutine(AscendAnimationCoroutine(0,0));
            }*/

            Debug.Log("slide");
        }
    }
    IEnumerator DescendAnimationCoroutine(float height, float moveSpeed)
    {
        _isPerformingAction = true;
        _animarot.SetBool("IsJumping", true);
        float time = 0;
        Vector2 startPosition = transform.position;
        Vector2 endPos = new(transform.position.x, transform.position.y + height);
        while (time < moveSpeed)
        {
            transform.position = Vector2.LerpUnclamped(startPosition, endPos, _extrapolationCurve.Evaluate(time / moveSpeed));
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = new(startPosition.x, startPosition.y - _layerDelta);//to make sure we know exactly where we are
        /*_animarot.SetBool("isRolling", true);
        
        Debug.Log(transform.position);
        time = 0f;
        while (time < moveSpeed/2)//wait for roll
        {
            time += Time.deltaTime;
            yield return null;
        }
        _animarot.SetBool("IsJumping", false);
        _animarot.SetBool("isRolling", false);*///in case i need cool roll again idk
        _animarot.SetBool("IsJumping", false);
        _isPerformingAction = false;
    }
    IEnumerator AscendAnimationCoroutine(float height, float moveSpeed)
    {
        _isPerformingAction = true;
        _animarot.SetBool("IsJumping", true);
        float startDistance = _interactedGameObjectTransform.position.x + _interactedGameObjectXAxisOffset - transform.position.x;
        Vector2 startPosition = transform.position;
        Vector2 endPos = new(transform.position.x, transform.position.y + (_jumpHeight * 0.75f));
        float currentDistance;
        do //cause i didnt wanted to calculate currentDistance more then once
        {
            currentDistance = _interactedGameObjectTransform.position.x + _interactedGameObjectXAxisOffset - transform.position.x;
            transform.position = Vector2.Lerp(startPosition, endPos, _jumpCurve.Evaluate(currentDistance / startDistance));
            yield return null;
        } while (currentDistance > 0);//it should go to the negatives
        transform.position = new(startPosition.x, startPosition.y + _layerDelta);//to ofset negated curve (im kinda reusing same animation curve here sooo yeah)
        startPosition = transform.position;
        endPos = new(transform.position.x, transform.position.y + height);
        float time = 0;
                while (time < moveSpeed)
        {
            transform.position = Vector2.LerpUnclamped(startPosition, endPos,  _extrapolationCurve.Evaluate(1 - (time / moveSpeed)));
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;
        _animarot.SetBool("IsJumping", false);
        _isPerformingAction = false;
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
    } 
    IEnumerator JumpAnimationCoroutine(float height, float moveSpeed)
    {
        _isPerformingAction = true;
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
        _isPerformingAction = false;
    }
    IEnumerator SlideAnimationCoroutine(float height, float moveSpeed)
    {
        _isPerformingAction = true;
        _animarot.SetBool("IsSliding", true);
        float time = 0;
        while (time < moveSpeed/2)
        {
            time += Time.deltaTime;
            yield return null;
        }
        _animarot.SetBool("IsSliding", false);
        _isPerformingAction = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Coin"))//coin collection logic
        {
            ScoreSystem.AddPoints(250);
            collision.gameObject.SetActive(false);
            return;//cause we didnt actually entered stairs zone, no need for anything else below
        }
        _interactedGameObjectTransform = collision.transform;
        _countOfCurrentlyColidedObjects++;
        _currentTag = collision.gameObject.tag;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _countOfCurrentlyColidedObjects--;
    }
}
