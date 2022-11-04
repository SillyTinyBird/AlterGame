using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class Prototype : IComparable<Prototype>
{
    private static int _idCount = 0;
    [ShowOnly, SerializeField] private int _id;
    public GameObject _prefab;
    public string _plusX;
    public string _minusX;
    public string _plusY;
    public string _minusY;
    /// <summary>
    /// List of cells directions. Each list in this list represent a direction:
    /// [0] = x+ ;[1] = y+; [2] = x-;[3] = y-
    /// </summary>
    [SerializeField] private DirectionsWrapper[] _directions;
    //[SerializeField] private List<DirectionsWrapper> _directions;//yeah...
    public DirectionsWrapper GetDirection(int index) => _directions[index];
    public Prototype()
    {
        _id = _idCount;
        _idCount += 1;
        _directions = new DirectionsWrapper[4];
    }
    /// <summary>
    /// Index represent a direction:
    /// [0] = x+ ;[1] = y+; [2] = x-;[3] = y-
    /// </summary>
    public void ClearDirections()
    {
        for (int i = 0; i < 4; i++)
        {
            _directions[i].Clear();
        }
        //_directions.ForEach(item => { item.Clear(); });
        Debug.LogFormat("Changed!");
    }
    public int GetId() => _id;
    //public void SetValidNeighbors(List<Prototype> validNeighbors) => _validNeighbors = validNeighbors;
    public void SetPrefabs(GameObject prefab) => _prefab = prefab;
    public void SetEdges(string minusX, string plusX, string minusY, string plusY)
    {
        _minusX = minusX;
        _plusX = plusX;
        _minusY = minusY;
        _plusY = plusY;
    }
    public int CompareTo(Prototype other) => other == null ? 1 : _id.CompareTo(other._id); //_prefab.name.CompareTo(other._prefab.name);
}
