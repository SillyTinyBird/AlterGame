using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionRenderer : MonoBehaviour
{
    private Prototype[,] _data;
    [SerializeField] private WaveFunctionSolver _solver;
    private int _xDelta =  5;
    private int _yDelta = 5;
    public GameObject parent;
    public void Clear()
    {
        if (parent != null)
        {
            while(parent.transform.childCount > 0)
            {
                DestroyImmediate(parent.transform.GetChild(0).gameObject);
            }
        }
    }
    public void DrawWaveFunction(int sizeX, int sizeY, int xDelta, int yDelta)
    {
        Initialize(sizeX, sizeY);
        Render(xDelta, yDelta);
    }
    public void DrawWaveFunction(int sizeX, int sizeY, int xDelta, int yDelta, Prototype[] firstLine)
    {
        Initialize(sizeX, sizeY, firstLine);
        Render(xDelta, yDelta);
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
    private void Initialize(int sizeX, int sizeY)
    {
        _solver.Initialize(sizeX, sizeY);
        _data =  _solver.GetWaveFunction();
    }
    private void Initialize(int sizeX, int sizeY, Prototype[] firstLine)
    {
        _solver.Initialize(sizeX, sizeY, firstLine);
        _data = _solver.GetWaveFunction();
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
