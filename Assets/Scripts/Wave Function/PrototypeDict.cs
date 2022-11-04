using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Prototype Dictionary", menuName = "ScriptableObjects/PrototypeDictionary", order = 1)]
public class PrototypeDict : ScriptableObject
{
    [SerializeField]
    private List<Prototype> _prototypes = new List<Prototype>();
    public void Awake()
    {
        ParseValidNeighbors(); 
        Debug.Log("Awake");
    }
    public void AddToList()
    {
        _prototypes.Add(new Prototype());
    }
    public Prototype GetPrototypeById(int id)//please dont try to search for what isnt in dictionary i beg you im to lazy right now to code null reference handle right now
    {
        return _prototypes.Find(x=> x.GetId() == id);
    }
    public List<Prototype> GetPrototypes() => _prototypes;
    public void ParseValidNeighbors()
    {
        foreach (Prototype x in _prototypes)
        {
            x.ClearDirections();
            /*x._validNeighborsPlusX.Clear();
            x._validNeighborsMinusX.Clear();
            x._validNeighborsPlusY.Clear();
            x._validNeighborsMinusY.Clear();*/
            foreach (Prototype y in _prototypes)
            {
                if (y == x)
                    continue;
                if(x._plusX == y._minusX)
                {
                    x.GetDirection(0).Add(y.GetId());
                }
                if (x._plusY == y._minusY)
                {
                    x.GetDirection(1).Add(y.GetId());
                }
                if (x._minusX == y._plusX)
                {
                    x.GetDirection(2).Add(y.GetId());
                }
                if (x._minusY == y._plusY)
                {
                    x.GetDirection(3).Add(y.GetId());
                }
            }
        }
    }
}
