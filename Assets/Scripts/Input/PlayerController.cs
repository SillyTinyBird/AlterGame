using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float _jumpSpeed = 0.2f;
    [SerializeField] private float _jumpHeight = 2.5f;
    [SerializeField] private AnimationCurve _jumpCurve;
    [Header("Slide Settings")]
    [SerializeField] private float _slideSpeed = 0.2f;
    [Header("Ascend/Descend Settings")]
    [SerializeField] private float _layerDelta = 5f;//im keeping this only for reference
    [SerializeField] private float _moveSpeed = 0.2f;
    [SerializeField] float _interactedGameObjectXAxisOffset = 0;
    [SerializeField] private float _ascendDescendHopHeight = 1.6f;//note that whenever this value changes, animation curve shoud be changed in inspector accordingly
    [SerializeField] private AnimationCurve _extrapolationCurve;
    [Header("Misc Settings")]
    private float _cancelSpeedFraction = 0.15f;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRendererSoWeCanChangeRenderOrderOnTheGo;
    [SerializeField] private LayerDependantSFXScript _SFXscript;
    [SerializeField] private DoughnutPickupSFX _coinSFXscript;

    private SwipeDetection _swipeDetection;
    /// <summary>
    /// 0 = lower; 1 = middle; 2 = upper.
    /// </summary>
    private int _layerID = 1;
    private bool _isPerformingAction = false;//its all static cause there will be only one instance attached to player anyway

    private string _currentTag = "";
    private int _countOfCurrentlyColidedObjects = 0;
    private Transform _interactedGameObjectTransform;

    private bool _isSlideCanceled = false;
    private bool _isJumpCanceled = false;


    private void Awake()
    {
        _swipeDetection = SwipeDetection.Instance;
        _layerID = 1;//we start on the middle layer
        UpdateSpriteLayer();
        _isPerformingAction = false;
    }
    /// <summary>
    /// 0 = lower; 1 = middle; 2 = upper.
    /// </summary>
    public int LayerID => _layerID;
    /// <summary>
    /// Returns bool of wether player is currently jumping, sliding, or changing layers.
    /// </summary>
    public bool IsActionBeeingPerformed => _isPerformingAction;
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
        if (!_isSlideCanceled)
        {
            _isSlideCanceled = true;
        }
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
        if (!_isJumpCanceled) {
            _isJumpCanceled = true;
        }
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
            StartCoroutine(SlideAnimationCoroutine(0, _slideSpeed));
            Debug.Log("slide");
        }
    }
    IEnumerator DescendAnimationCoroutine(float height, float moveSpeed)
    {
        _isPerformingAction = true;
        _animator.SetBool("IsJumping", true);
        _SFXscript.PlayDescendStartSFX(_layerID);
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
        _SFXscript.PlayDescendEndSFX(_layerID);
        _animator.SetBool("IsJumping", false);
        _isJumpCanceled = false;
        _isPerformingAction = false;
    }
    IEnumerator AscendAnimationCoroutine(float height, float moveSpeed)
    {
        _isPerformingAction = true;
        _animator.SetBool("IsJumping", true);
        _SFXscript.PlayAscendStartSFX(_layerID);
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
        _SFXscript.PlayTrampolineSFX();
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
        _SFXscript.PlayAscendEndSFX(_layerID);
        _animator.SetBool("IsJumping", false);
        _isSlideCanceled = false;
        _isPerformingAction = false;
    }
    IEnumerator JumpAnimationCoroutine(float height, float moveSpeed)
    {
        _isPerformingAction = true;
        _animator.SetBool("IsJumping", true);
        _SFXscript.PlayJumpSFX(_layerID);
        float time = 0;
        Vector2 startPosition = transform.position;
        Vector2 endPos = new(transform.position.x, transform.position.y + height);
        while (time < moveSpeed)
        {
            transform.position = Vector2.Lerp(startPosition, endPos, _jumpCurve.Evaluate(time / moveSpeed));
            time += Time.deltaTime;
            if (_isJumpCanceled)
            {
                StartCoroutine(JumpCancelAnimationCoroutine(startPosition, moveSpeed));
                yield break;
            }
            yield return null;
        }
        transform.position = startPosition;
        _SFXscript.PlayJumpSFX(_layerID);
        _animator.SetBool("IsJumping", false);
        _isSlideCanceled = false;
        _isPerformingAction = false;
    }
    IEnumerator JumpCancelAnimationCoroutine(Vector2 startPos, float moveSpeed)
    {
        _isPerformingAction = true;
        _isSlideCanceled = false;
        float time = 0;
        Vector2 startPosition = transform.position;
        while (time < moveSpeed * _cancelSpeedFraction)
        {
            transform.position = Vector2.Lerp(startPosition, startPos, time/(moveSpeed * _cancelSpeedFraction));
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = startPos;
        _SFXscript.PlayJumpSFX(_layerID);
        _animator.SetBool("IsJumping", false);
        _isPerformingAction = false;
        StartCoroutine(SlideAnimationCoroutine(0, _slideSpeed));
    }
    IEnumerator SlideAnimationCoroutine(float height, float moveSpeed)//heads up: animation also changes boxcolider size and offset
    {
        _isPerformingAction = true;
        _SFXscript.PlaySlideSFX();
        _animator.SetBool("IsSliding", true);
        float time = 0;
        while (time < moveSpeed)
        {
            time += Time.deltaTime;
            if (_isSlideCanceled)
            {
                StartCoroutine(SlideCancelAnimationCoroutine(0, moveSpeed));
                yield break;
            }
            yield return null;
        }
        _animator.SetBool("IsSliding", false);
        _isJumpCanceled = false;
        _isPerformingAction = false;
    }
    IEnumerator SlideCancelAnimationCoroutine(float height, float moveSpeed)
    {
        _isPerformingAction = true;
        _isJumpCanceled = false;
        _animator.SetBool("IsSliding", false);
        yield return null;
        _isPerformingAction = false;
        StartCoroutine(JumpAnimationCoroutine(_jumpHeight, _jumpSpeed));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("Layer"))//so we dont count obstacles
        {
            return;
        }
        _countOfCurrentlyColidedObjects++;
        if (collision.gameObject.CompareTag("Coin"))//coin collection logic
        {
            ScoreSystem.Instance.AddPoints(250);
            ScoreSystem.Instance.AddDoughnut();
            _coinSFXscript.PickupSFX();
            collision.gameObject.SetActive(false);
            return;//cause we didnt actually entered stairs zone, no need for anything else below
        }
        _interactedGameObjectTransform = collision.transform;
        Debug.Log(_countOfCurrentlyColidedObjects);
        _currentTag = collision.gameObject.tag;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("Layer"))//so we dont count obstacles
        {
            return;
        }
        _countOfCurrentlyColidedObjects--;
        Debug.Log(_countOfCurrentlyColidedObjects);
    }
}
