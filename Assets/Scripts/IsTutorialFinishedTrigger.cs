using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTutorialFinishedTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TutorialEnd"))
        {
            SettingsSaver.IsTutorialCompleete = true;
        }
    }
}
