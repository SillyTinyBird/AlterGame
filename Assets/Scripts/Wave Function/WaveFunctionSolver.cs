using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionSolver : MonoBehaviour
{
    [SerializeField] private PrototypeDict _dictionary;
    private List<Prototype>[,] _waveFunction;// = new List<Prototype>[8, 8];
    private int _sizeX = 0;
    private int _sizeY = 0;
    private bool _isSolved = false;
    private readonly System.Random r = new();
    private List<Prototype> error;
    public IEnumerator GetWaveFunction()
    {
        if(!_isSolved)
        {
            yield return StartCoroutine(Solve());
        }
    }
    public Prototype[,] GetWaveFunctionSolved()
    {
        if (_isSolved)
        {
            return RemoveLists();
        }
        throw new Exception(
            "GetWaveFunctionSolved() was terminated because Wave Function was not solved beforhand (use GetWaveFunction() before calling this method)");

    }
    public Tuple<int,int> GetDimensions() => Tuple.Create(_sizeX, _sizeY);
    public IEnumerator Initialize(int sizeX, int sizeY)
    {
        _sizeX = sizeX;
        _sizeY = sizeY;
        _waveFunction = new List<Prototype>[_sizeX, _sizeY];
        for (int i = 0; i < _sizeX; i++)
        {
            for (int j = 0; j < _sizeY; j++)
            {
                _waveFunction[i, j] = new List<Prototype>(_dictionary.GetPrototypes());
            }
        }
        error = new List<Prototype>() { _waveFunction[0,0][0]};
        yield return null;
        _isSolved = false;
    }
    public IEnumerator Initialize(int sizeX, int sizeY, Prototype[] firstLine)
    {
        _sizeX = sizeX;
        _sizeY = sizeY;
        List<Tuple<int, int>> affectedCoords = new();
        _waveFunction = new List<Prototype>[_sizeX, _sizeY];
        for (int i = 0; i < _sizeX; i++)
        {
            for (int j = 0; j < _sizeY; j++)
            {
                if(i == 0)
                {
                    _waveFunction[i, j] = new List<Prototype>() { firstLine[j] };
                    affectedCoords.Add(Tuple.Create(i, j));
                    continue;
                }
                _waveFunction[i, j] = new List<Prototype>(_dictionary.GetPrototypes());
            }
        }
        error = new List<Prototype>() { _waveFunction[0, 0][0] };//needs a change
        yield return StartCoroutine(PlacePrototypesInFirstColumn(affectedCoords));
        //affectedCoords.ForEach(x => { StartCoroutine(Propagate(x)); });
        _isSolved = false;
    }
    private IEnumerator PlacePrototypesInFirstColumn(List<Tuple<int, int>> list)
    {
        foreach (var x in list)
        {
            yield return StartCoroutine(Propagate(x,false));
        }
        //list.ForEach(x => { });
    }
    private IEnumerator Solve()
    {
        while (!IsColapsed())
        {
            yield return StartCoroutine(Iterate());
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
    private IEnumerator Iterate()
    {
        Tuple<int, int> coords = GetMinCoords();
        yield return StartCoroutine(Colapse(coords));
        yield return StartCoroutine(Propagate(coords));
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
    private IEnumerator Colapse(Tuple<int, int> at)
    {
        //int index = r.Next(0, _waveFunction[at.Item1, at.Item2].Count);
        int index = GetRandomWeightedIndex(_waveFunction[at.Item1, at.Item2]);
        //int index = UnityEngine.Random.Range(0,_waveFunction[at.Item1, at.Item2].Count - 1);//if you would like to add weights in the future, add them here
        _waveFunction[at.Item1, at.Item2] = new List<Prototype>() { _waveFunction[at.Item1, at.Item2][index] };//idk if this gonna work
        yield return null;
    }
    private IEnumerator Propagate(Tuple<int, int> at, bool makeQuick = true)//makeQuick allows for more frequent yield return null; calls
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
                List<Prototype> buffer = new(_waveFunction[neighbor.Item1, neighbor.Item2]);
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
                
                if (buffer.Count == 0)
                {
                    _waveFunction[neighbor.Item1, neighbor.Item2] = error;//need better error handeling
                    Debug.Log("errpr00");
                }
                else
                {
                    _waveFunction[neighbor.Item1, neighbor.Item2] = buffer;
                }
            }
            if (!makeQuick)
            {
                yield return null;
            }
        }
        if (makeQuick)
        {
            yield return null;
        }
    }
    /// <summary>
    /// extracts prototypes from lists
    /// </summary>
    /// <returns>compleet chunk</returns>
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
        List<Tuple<int, int>> coords = new();
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
        List<int> sum = new();
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
    public int GetRandomWeightedIndex(List<Prototype> list)
    {
        int weightSum = 0;
        // Get the total sum of all the weights.
        list.ForEach(i => { weightSum += i._weight; });

        // Step through all the possibilities, one by one, checking to see if each one is selected.
        int index = 0;
        int lastIndex = list.Count - 1;
        while (index < lastIndex)
        {
            // Do a probability check with a likelihood of weights[index] / weightSum.
            if (r.Next(0, weightSum) < list[index]._weight)
            {
                return index;
            }

            // Remove the last item from the sum of total untested weights and try again.
            weightSum -= list[index++]._weight;
        }

        // No other item was selected, so return very last index.
        return index;
    }

}

