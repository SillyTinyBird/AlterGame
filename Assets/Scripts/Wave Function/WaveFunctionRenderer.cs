using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionRenderer : MonoBehaviour
{
    private Prototype[,] _data;
    [SerializeField] private WaveFunctionSolver _solver;
    [SerializeField] private PlaymodeInterfaceScript _pauseManager;
    private int _xDelta =  5;
    private int _yDelta = 5;
    public GameObject parent;
    public void Clear()
    {
        if (parent != null)
        {
            int i = 0;
            while(i < parent.transform.childCount)
            {
                Destroy(parent.transform.GetChild(i).gameObject);
                i++;
            }
        }
    }
    public IEnumerator DrawWaveFunction(int sizeX, int sizeY, int xDelta, int yDelta, bool firstCall = false)
    {
        yield return StartCoroutine(Initialize(sizeX, sizeY));
        Render(xDelta, yDelta);
        if (_pauseManager != null && firstCall)
        {
            _pauseManager.LoadingCompleetAction();
        }
    }
    public IEnumerator DrawWaveFunction(int sizeX, int sizeY, int xDelta, int yDelta, Prototype[] firstLine,bool firstCall = false)
    {
        yield return StartCoroutine(Initialize(sizeX, sizeY, firstLine, firstCall));
        Render(xDelta, yDelta);
        if(_pauseManager != null && firstCall)
        {
            _pauseManager.LoadingCompleetAction();
        }
    }
    public Prototype[] GetLastLineY()
    {
        Prototype[] buffer = new Prototype[_solver.GetDimensions().Item2];
        for (int i = 0; i < _solver.GetDimensions().Item2; i++)
        {
            buffer[i] = _data[_solver.GetDimensions().Item1 - 1, i];
        }
        return buffer;
    }
    /*public void PrepareNext()
    {
        nextPoolObject.transform.position = new Vector2(this.transform.position.x + (_solver._sizeX + 1) * _xDelta, this.transform.position.y);
        Prototype[] buffer = new Prototype[_solver._sizeY];
        for (int i = 0; i < _solver._sizeY; i++)
        {
            buffer[i] = _data[_solver._sizeX,i];
        }
        DrawWaveFunction(buffer);
    }*/
    private IEnumerator Initialize(int sizeX, int sizeY)
    {
        _solver.Initialize(sizeX, sizeY);
        yield return StartCoroutine(_solver.GetWaveFunction());
        _data = _solver.GetWaveFunctionSolved();
    }
    private IEnumerator Initialize(int sizeX, int sizeY, Prototype[] firstLine, bool firstCall = false)
    {
        yield return StartCoroutine(_solver.Initialize(sizeX, sizeY, firstLine, firstCall));
        yield return StartCoroutine(_solver.GetWaveFunction());
        _data = _solver.GetWaveFunctionSolved();
    }
    private void Render(int xDelta, int yDelta)
    {
        _xDelta = xDelta;
        _yDelta = yDelta;
        for (int i = 0; i < _solver.GetDimensions().Item1 - 1; i++)
        {
            for (int j = 0; j < _solver.GetDimensions().Item2; j++)
            {
                GameObject myObject = Instantiate(_data[i,j]._prefab,new Vector2(parent.transform.position.x + _xDelta * i, parent.transform.position.y + _yDelta * j),Quaternion.identity);
                myObject.transform.parent = parent.transform;
            }
        }
    }
}
