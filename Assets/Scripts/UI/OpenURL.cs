using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public void OpenURLFormString(string url)
    {
        Application.OpenURL(url);
    }
}
