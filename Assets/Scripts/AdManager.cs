using UnityEngine;
using UnityEngine.Advertisements;
using System;
using UnityEngine.UI;

public class AdManager : MonoBehaviour, IUnityAdsListener
{

    string GooglePlayID = "3773641";
    public bool testMode = false;
    string continueID = "continuePlaying";
    //string moneyID = "money";

    public UIaction UIaction;

    private int min = 2;
    private int max = 5;
    public int gamesLeft;

    public Button continueButton;

    void Start()
    {
        // Initialize the Ads service:
        Advertisement.Initialize(GooglePlayID, testMode);
        Advertisement.AddListener(this);

        continueButton.enabled = false;

        randGamesLeft();
    }

    public void play()
    {
        if(gamesLeft==0)
        {
            randGamesLeft();
            ShowInterstitialAd();
        }
        gamesLeft--;
    }

    private void randGamesLeft()
    {
        System.Random r = new System.Random();
        gamesLeft = r.Next(min, max);
    }

    public void ShowInterstitialAd()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
        else
        {
            randGamesLeft();
        }
    }

    public void ShowRewardedVideo()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady(continueID))
        {
            Advertisement.Show(continueID);
        }
        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            if(placementId == continueID)
            {
                UIaction.continuePlaying();
            }
        }
        else if (showResult == ShowResult.Skipped)
        {
            if (placementId == continueID)
            {
                UIaction.noThanks();
            }
        }
        else if (showResult == ShowResult.Failed)
        {
            if (placementId == continueID)
            {
                UIaction.noThanks();
            }
            Debug.LogWarning("The ad did not finish due to an error.");
        }

        continueButton.enabled = false;
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == continueID)
        {
            // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
            continueButton.enabled = true;
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
}   