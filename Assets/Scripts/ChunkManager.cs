using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] private int _xDelta = 5;
    [SerializeField] private int _yDelta = 5;
    [SerializeField] private int _sizeX = 0;
    [SerializeField] private int _sizeY = 0;
    [SerializeField] private GameObject _chunk1Object;
    [SerializeField] private GameObject _chunk2Object;
    [SerializeField] private GameObject _clearHitbox;//it was a hitbox i promise
    [SerializeField] private GameObject _nextHitbox;//now its just here so i can have transform i can refer to when deciding next chunk generation timing
    [SerializeField, Range(0f, 1f)] private float _clearPositionRatio;
    [SerializeField, Range(0f, 1f)] private float _nexxtPositionRatio;
    [SerializeField, Range(0f, 5f)] private float _scrollAmount;
    [SerializeField] private PrototypeDict _dictionary;
    [SerializeField] private int[] _firstLaneIds = new int[3];
    private bool _nextCleared;
    private bool _nextReady;
    private WaveFunctionRenderer _chunk1;
    private WaveFunctionRenderer _chunk2;
    private int _currentChunkId;
    private void Start()
    {
        _chunk1 = _chunk1Object.GetComponent<WaveFunctionRenderer>();
        _chunk2 = _chunk2Object.GetComponent<WaveFunctionRenderer>();
        _clearHitbox.transform.position = new Vector2(_chunk1.transform.position.x + _sizeX * _clearPositionRatio * _xDelta, _clearHitbox.transform.position.y);
        _nextHitbox.transform.position = new Vector2(_chunk1.transform.position.x + _sizeX * _nexxtPositionRatio * _xDelta, _clearHitbox.transform.position.y);
        DrawFirst();
        _nextReady = true;
        _nextCleared = true;
        ScrollScript.SetSpeed(_scrollAmount);
    }
    private void FixedUpdate()
    {
        if (_clearHitbox.transform.position.x <= transform.position.x && 
            _clearHitbox.transform.position.x +_xDelta >= transform.position.x && _nextReady == true)
        {
            ClearLast();
            _nextCleared = true;
            _nextReady = false;
        }
        if (_nextHitbox.transform.position.x <= transform.position.x && 
            _nextHitbox.transform.position.x + _xDelta >= transform.position.x && _nextCleared == true)
        {
            DrawNext();
            _nextReady = true;
            _nextCleared = false;
        }
    }
    public void DrawFirst()
    {
        _chunk1.DrawWaveFunction(_sizeX, _sizeY, _xDelta, _yDelta,new Prototype[] {
            _dictionary.GetPrototypeById(_firstLaneIds[0]),
            _dictionary.GetPrototypeById(_firstLaneIds[1]), 
            _dictionary.GetPrototypeById(_firstLaneIds[2])});
        _currentChunkId = 1;
    }
    public void ClearLast()
    {
        switch (_currentChunkId)
        {
            case 1:
                _chunk2.Clear();
                break;
            case 2:
                _chunk1.Clear();
                break;
            default:
                break;
        }
    }
    public void DrawNext()
    {
        switch (_currentChunkId)
        {
            case 1:
                _chunk2Object.transform.position = new Vector2(_chunk1Object.transform.position.x - _scrollAmount + (_sizeX - 1) * _xDelta, _chunk1Object.transform.position.y);
                _chunk2.DrawWaveFunction(_sizeX, _sizeY, _xDelta, _yDelta,_chunk1.GetLastLineY());
                _currentChunkId = 2;
                _clearHitbox.transform.position = new Vector2(_chunk2.transform.position.x - _scrollAmount + _sizeX * _clearPositionRatio * _xDelta, _clearHitbox.transform.position.y);
                _nextHitbox.transform.position = new Vector2(_chunk2.transform.position.x - _scrollAmount + _sizeX * _nexxtPositionRatio * _xDelta, _clearHitbox.transform.position.y);
                break;
            case 2:
                _chunk1Object.transform.position = new Vector2(_chunk2Object.transform.position.x - _scrollAmount + (_sizeX - 1) * _xDelta, _chunk2Object.transform.position.y);
                _chunk1.DrawWaveFunction(_sizeX, _sizeY, _xDelta, _yDelta, _chunk2.GetLastLineY());
                _currentChunkId = 1;
                _clearHitbox.transform.position = new Vector2(_chunk1.transform.position.x - _scrollAmount + _sizeX * _clearPositionRatio * _xDelta, _clearHitbox.transform.position.y);
                _nextHitbox.transform.position = new Vector2(_chunk1.transform.position.x - _scrollAmount + _sizeX * _nexxtPositionRatio * _xDelta, _clearHitbox.transform.position.y);
                break;
            default:
                break;
        }
    }
}
