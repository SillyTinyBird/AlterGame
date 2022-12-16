using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject objec = new();
                objec.name = typeof(T).Name;
                objec.hideFlags = HideFlags.DontSave;
                _instance = objec.AddComponent<T>();
            }
            return _instance;
        }
    }
    private void OnDestroy()
    {
        if(_instance == this)
            _instance = null;
    }
}
