using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [SerializeField] private PlaymodeInterfaceScript _rewardManager;
    [SerializeField] private ObstacleScript _obstacleManager;
    [SerializeField] private PlayerController _pc;
    [SerializeField] GameObject _buttonToDisableOnceRewardGot;

    public void GiveReward()
    {
        _rewardManager.RewardedAdCompleete();
        _pc.ResetPlayerPosition();
        _obstacleManager.InvisabilityFrames();
        _buttonToDisableOnceRewardGot.SetActive(false);
    }
}
