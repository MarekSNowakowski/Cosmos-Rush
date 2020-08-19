using UnityEngine;
using UnityEngine.Advertisements;
using System;
using UnityEngine.UI;
using TMPro;

public class AdManager : MonoBehaviour, IUnityAdsListener
{

    string GooglePlayID = "3773641";
    public bool testMode = false;
    string continueID = "continuePlaying";
    string rewardID = "rewardedVideo";

    public UIaction UIaction;

    private int min = 2;
    private int max = 5;
    public int gamesLeft;
    private float reward;

    public Button continueButton;
    public Button rewardButton;

    public TextMeshProUGUI rewardAmountText;

    void Start()
    {
        // Initialize the Ads service:
        Advertisement.Initialize(GooglePlayID, testMode);
        Advertisement.AddListener(this);

        continueButton.enabled = false;
        rewardButton.enabled = false;

        randGamesLeft();
        decideReward();
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

    public void ShowContinueVideo()
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

    public void ShowRewardedVideo()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady(rewardID))
        {
            Advertisement.Show(rewardID);
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
            if (placementId == rewardID)
            {
                UIaction.player.loadStatistics();
                UIaction.player.money += reward;
                UIaction.player.moneyEarned += reward;

                PlayerPrefs.SetFloat("money", UIaction.player.money);
                PlayerPrefs.SetFloat("moneyEarned", UIaction.player.moneyEarned);
                PlayerPrefs.Save();

                UIaction.UpdateMoney();
            }
        }
        else if (showResult == ShowResult.Skipped)
        {
            if (placementId == continueID)
            {
                UIaction.noThanks();
            }
            if (placementId == rewardID)
            {

            }
        }
        else if (showResult == ShowResult.Failed)
        {
            if (placementId == continueID)
            {
                UIaction.noThanks();
            }
            if (placementId == rewardID)
            {

            }
            Debug.LogWarning("The ad did not finish due to an error.");
        }

        continueButton.enabled = false;
        rewardButton.enabled = false;
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == continueID)
        {
            // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
            continueButton.enabled = true;
        }
        if (placementId == rewardID)
        {
            decideReward();
            rewardButton.enabled = true;
        }
    }

    void decideReward()
    {
        if (PlayerPrefs.GetFloat("moneyEarned", 0) < 10000) reward = 1000;
        else if (PlayerPrefs.GetFloat("moneyEarned", 0) < 50000) reward = 2500;
        else if (PlayerPrefs.GetFloat("moneyEarned", 0) < 100000) reward = 5000;
        else if (PlayerPrefs.GetFloat("moneyEarned", 0) < 500000) reward = 10000;
        else if (PlayerPrefs.GetFloat("moneyEarned", 0) < 1000000) reward = 25000;
        else if (PlayerPrefs.GetFloat("moneyEarned", 0) < 5000000) reward = 50000;
        else reward = 1000000;

        rewardAmountText.text = reward + " $";
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