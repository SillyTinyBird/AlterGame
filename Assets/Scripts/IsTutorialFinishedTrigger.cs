using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTutorialFinishedTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SettingsSaver.IsTutorialCompleete = true;
    }
}
