using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionSolver : MonoBehaviour
{
    [SerializeField] private PrototypeDict _dictionary;
    private List<Prototype>[,] _waveFunction;// = new List<Prototype>[8, 8];
    public int _sizeX = 0;
    public int _sizeY = 0;
    private bool _isSolved = false;
    private readonly System.Random r = new System.Random();
    private List<Prototype> error;
    /*void Start()
    {
        Initialize();
        //Debug.Log(_dictionary.GetPrototypeById(7)._prefab);
        Prototype[,] bruh = GetWaveFunction();
        Debug.Log(_waveFunction.GetLength(0));
    }*/
    public Prototype[,] GetWaveFunction()
    {
        if(!_isSolved)
        {
            Solve();
        }
        return RemoveLists();
    }
    public void Initialize()
    {
        _waveFunction = new List<Prototype>[_sizeX, _sizeY];
        for (int i = 0; i < _sizeX; i++)
        {
            for (int j = 0; j < _sizeY; j++)
            {
                _waveFunction[i, j] = new List<Prototype>(_dictionary.GetPrototypes());
            }
        }
        error = new List<Prototype>() { _waveFunction[0,0][0]};
        _isSolved = false;
    }
    private void Solve()
    {
        while (!IsColapsed())
        {
            Iterate();
        }
        _isSolved = true;

    }
    private bool IsColapsed()
    {
        for (int i = 0; i < _sizeX; i++)
        {
            for (int j = 0; j < _sizeY; j++)
            {
                if(_waveFunction[i, j].Count > 1)
                {
                    return false;
                }
            }
        }
        return true;
    }
    private void Iterate()
    {
        Tuple<int, int> coords = GetMinCoords();
        Colapse(coords);
        Propagate(coords);
    }
    private Tuple<int, int> GetMinCoords()//its a mess but it is what it is i guess
    {
        List<Prototype> currentMin = _waveFunction[0, 0];
        int x = 0;
        int y = 0;
        for (int i = 0; i < _sizeX; i++)
        {
            for (int j = 0; j < _sizeY; j++)
            {
                if (currentMin.Count < _waveFunction[i, j].Count)//first run is needed so algorithm can start form anywhere but (0,0) cause 0,0 might be already solved
                {
                    currentMin = _waveFunction[i, j];
                    x = i;
                    y = j;
                }
            }
        }
        if(currentMin.Count == 1)
        {
            return null;
        }
        for (int i = 0; i < _sizeX; i++)
        {
            for (int j = 0; j < _sizeY; j++)
            {
                if (currentMin.Count > _waveFunction[i, j].Count && _waveFunction[i, j].Count != 1)
                {
                    currentMin = _waveFunction[i, j];
                    x = i;
                    y = j;
                }
            }
        }
        return Tuple.Create(x, y);
    }
    private void Colapse(Tuple<int, int> at)
    {
        int index = r.Next(0, _waveFunction[at.Item1, at.Item2].Count);
        //int index = UnityEngine.Random.Range(0,_waveFunction[at.Item1, at.Item2].Count - 1);//if you would like to add weights in the future, add them here
        _waveFunction[at.Item1, at.Item2] = new List<Prototype>() { _waveFunction[at.Item1, at.Item2][index] };//idk if this gonna work
    }
    private void Propagate(Tuple<int, int> at)
    {
        List<Tuple<int, int>> affectedCoords = new() {at};
        while(affectedCoords.Count > 0)
        {
            Tuple<int, int> currentCoord = affectedCoords[0];
            affectedCoords.RemoveAt(0);
            List<Tuple<int, int>> neighbors = GetDirectionsAtCoords(currentCoord);
            foreach(Tuple<int,int> neighbor in neighbors)//compare valid naigbors for curent cell (for each direction) with prototypes in neighboring cells 
            {
                if (_waveFunction[neighbor.Item1, neighbor.Item2].Count == 1)//if neighbor contains only one prototype, skip
                {
                    continue;
                }
                List<int> validPrototypesPerDirection = GetValidNeighborsSum(currentCoord,neighbor);
                List<Prototype> buffer = new List<Prototype>(_waveFunction[neighbor.Item1, neighbor.Item2]);
                foreach (Prototype possibleState in _waveFunction[neighbor.Item1, neighbor.Item2])
                {
                    int curId = possibleState.GetId();//_waveFunction[currentCoord.Item1, currentCoord.Item2][0].GetId();
                    if(!validPrototypesPerDirection.Contains(curId))
                    {
                        buffer.Remove(possibleState);//if prototype in neighbor is not present in validPrototypesPerDirection, remove it from cell
                        if(!affectedCoords.Contains(neighbor))
                        {
                            affectedCoords.Add(neighbor);//if cell was mdified, add to affectedCoords
                        }
                    }
                }
                if(buffer.Count == 0)
                {
                    _waveFunction[neighbor.Item1, neighbor.Item2] = error;
                    Debug.Log("errpr00");
                }
                _waveFunction[neighbor.Item1, neighbor.Item2] = buffer;//if none changes were made, dont 
            }
        }
    }

    private Prototype[,] RemoveLists()
    {
        Prototype[,] buf = new Prototype[_sizeX, _sizeY];
        for (int i = 0; i < _sizeX; i++)
        {
            for (int j = 0; j < _sizeY; j++)
            {
                buf[i, j] = _waveFunction[i, j][0];
            }
        }
        return buf;
    }
    private List<Tuple<int, int>> GetDirectionsAtCoords(Tuple<int, int> at)
    {
        List<Tuple<int, int>> coords = new List<Tuple<int, int>>();
        if(at.Item1 > 0)
        {
            coords.Add(new Tuple<int, int>(at.Item1 - 1, at.Item2));
        }
        if(at.Item2 > 0)
        {
            coords.Add(new Tuple<int, int>(at.Item1, at.Item2 - 1));
        }
        if(at.Item1 < _sizeX - 1)
        {
            coords.Add(new Tuple<int, int>(at.Item1 + 1, at.Item2));
        }
        if(at.Item2 < _sizeY - 1)
        {
            coords.Add(new Tuple<int, int>(at.Item1, at.Item2 + 1));
        }
        return coords;
    }
    private List<int> GetValidNeighborsSum(Tuple<int, int> at, Tuple<int, int> to)
    {
        List<int> sum = new List<int>();
        Tuple<int, int> direction = Tuple.Create(to.Item1 - at.Item1, to.Item2 - at.Item2);
        int direectionIndex = GetDirectionIndexFromTuple(direction);
        foreach(Prototype item in _waveFunction[at.Item1, at.Item2])
        {
            item.GetDirection(direectionIndex).myList.ForEach(x => { if (!sum.Contains(x)) sum.Add(x); });
            //item.GetDirection(direectionIndex).ForEach(x => { if(!sum.Contains(x))sum.Add(x); });
        }
        return sum;
    }
    private int GetDirectionIndexFromTuple(Tuple<int, int> direction)
    {
        int direectionIndex = -1;
        if (Equals(direction, Tuple.Create(1, 0)))
        {
            direectionIndex = 0;
        }
        if (Equals(direction, Tuple.Create(0, 1)))
        {
            direectionIndex = 1;
        }
        if (Equals(direction, Tuple.Create(-1, 0)))
        {
            direectionIndex = 2;
        }
        if (Equals(direction, Tuple.Create(0, -1)))
        {
            direectionIndex = 3;
        }
        if (direectionIndex == -1)
        {
            throw new Exception("GetDirectionIndexFromTuple parameters error: Tuple is not a direction");
        }
        return direectionIndex;
    }
}

