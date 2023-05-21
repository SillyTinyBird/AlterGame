using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronSourceAdsScript : MonoBehaviour
{
    [SerializeField] private string _androidAppkey = "";
    [SerializeField] private PlaymodeInterfaceScript _rewardManager;
    [SerializeField] private ObstacleScript _obstacleManager;
    [SerializeField] GameObject _buttonToDisableOnceRewardGot;
    void Start()
    {
        IronSource.Agent.init(_androidAppkey, IronSourceAdUnits.REWARDED_VIDEO);
        //IronSource.Agent.init(_androidAppkey, IronSourceAdUnits.INTERSTITIAL);
        //IronSource.Agent.init(_androidAppkey, IronSourceAdUnits.BANNER);
    }

    private void OnEnable()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;

        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;

    }
    public void ShowRewardedAd()
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            Debug.LogWarning("Rewarded ad is not ready.");
        }
    }

    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }

    private void SdkInitializationCompletedEvent() {
        IronSource.Agent.validateIntegration();
    }

    //Rewarded callbacks

    /************* RewardedVideo AdInfo Delegates *************/
    // Indicates that there’s an available ad.
    // The adInfo object includes information about the ad that was loaded successfully
    // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
    }
    // Indicates that no ads are available to be displayed
    // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
    void RewardedVideoOnAdUnavailable()
    {
    }
    // The Rewarded Video ad view has opened. Your activity will loose focus.
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
    }
    // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
    }
    // The user completed to watch the video, and should be rewarded.
    // The placement parameter will include the reward data.
    // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        _rewardManager.RewardedAdCompleete();
        _obstacleManager.InvisabilityFrames();
        _buttonToDisableOnceRewardGot.SetActive(false);
    }
    // The rewarded video ad was failed to show.
    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
    }
    // Invoked when the video ad was clicked.
    // This callback is not supported by all networks, and we recommend using it only if
    // it’s supported by all networks you included in your build.
    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
    }


}
