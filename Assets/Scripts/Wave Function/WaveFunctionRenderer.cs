using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionRenderer : MonoBehaviour
{
    private Prototype[,] _data;
    [SerializeField] private WaveFunctionSolver _solver;
    [SerializeField] private int _xDelta =  5;
    [SerializeField] private int _yDelta = 5;
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
    public void DrawWaveFunction()
    {
        Initialize();
        Render();
    }
    private void Initialize()
    {
        _solver.Initialize();
        _data =  _solver.GetWaveFunction();
    }
    private void Render()
    {
        for (int i = 0; i < _solver._sizeX; i++)
        {
            for (int j = 0; j < _solver._sizeY; j++)
            {
                GameObject myObject = Instantiate(_data[i,j]._prefab,new Vector2(_xDelta * i, _yDelta * j),Quaternion.identity);
                myObject.transform.parent = parent.transform;
            }
        }
    }
}
