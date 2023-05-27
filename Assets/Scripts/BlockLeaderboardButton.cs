using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(1)]
public class BlockLeaderboardButton : MonoBehaviour
{
    [SerializeField] private Button _leaderboardButton;
    void Start()
    {
        if(!PlayServicesLoginStatus.IsLoginSucceeded)
        {
            _leaderboardButton.interactable = false;
        }
    }
}
