using System.Collections.Generic;
[System.Serializable]
public class DirectionsWrapper
{
    public List<int> myList;
    public int this[int key]
    {
        get
        {
            return myList[key];
        }
        set
        {
            myList[key] = value;
        }
    }
    public DirectionsWrapper()
    {
        myList = new List<int>();
    }
    public void Clear()
    {
        myList.Clear();
    }
    public void Add(int item)
    {
        myList.Add(item);
    }
}
