using UnityEngine;
using TMPro;
using System;

public class Upgrade : MonoBehaviour
{
    public float[] costs;
    public string upgradeName;
    public int upgradeMax;
    public string benefits;
    public string title;

    Animator anim;
    AudioSource audioS;

    public TextMeshProUGUI progressText;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI benefitsText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI costText2;
    public GameObject BuyPanel;

    public AudioClip select;
    public AudioClip upgradeSound;
    public AudioClip purchaseFailed;

    public GameObject outline;


    void Start()
    {
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        setTexts();
    }

    public void setColor()
    {
        if (upgradeName == "white" && (PlayerPrefs.GetInt("color", 0) != 0))
        {
            PlayerPrefs.SetInt("color", 0);
        }
        else if (upgradeName == "blue" && PlayerPrefs.GetInt("blue", 0) > 0 && (PlayerPrefs.GetInt("color", 0) != 1))
        {
            PlayerPrefs.SetInt("color", 1);
        }
        else if (upgradeName == "violet" && PlayerPrefs.GetInt("violet", 0) > 0 && (PlayerPrefs.GetInt("color", 0) != 2))
        {
            PlayerPrefs.SetInt("color", 2);
        }
        else if (upgradeName == "gold" && PlayerPrefs.GetInt("gold", 0) > 0 && (PlayerPrefs.GetInt("color", 0) != 3))
        {
            PlayerPrefs.SetInt("color", 3);
        }
        PlayerPrefs.Save();
    }

    public void upgrade()
    {
        if (PlayerPrefs.GetInt(upgradeName, 0) == upgradeMax)
        {
            PlaySound(purchaseFailed);
        }
        else if (PlayerPrefs.GetFloat("money", 0) < costs[PlayerPrefs.GetInt(upgradeName, 0)])
        {
            PlaySound(purchaseFailed);
        }
        else
        {
            PlayerPrefs.SetFloat("money", PlayerPrefs.GetFloat("money", 0) - costs[PlayerPrefs.GetInt(upgradeName, 0)]);
            PlayerPrefs.SetInt(upgradeName, PlayerPrefs.GetInt(upgradeName, 0) + 1);
            PlayerPrefs.Save();
            anim.SetTrigger("Normal");
            setTexts();
            PlaySound(upgradeSound);
        }
        setColor();
    }

    public void setTexts()
    {
        progressText.text = PlayerPrefs.GetInt(upgradeName, 0) + "/" + upgradeMax;
        if (upgradeName == "white") progressText.text = "1/1";
        titleText.text = title;
        benefitsText.text = benefits;

        if (PlayerPrefs.GetInt(upgradeName, 0) == upgradeMax)
        {
            costText.text = "MAX";
            costText2.text = "Upgraded to maximum level";
        }
        else
        {
            costText.text = costs[PlayerPrefs.GetInt(upgradeName, 0)] + "$";

            if (PlayerPrefs.GetFloat("money", 0) < costs[PlayerPrefs.GetInt(upgradeName, 0)])
            {
                costText2.text = "Not enought money!";
            }
            else
            {
                costText2.text = "Buy for" + Environment.NewLine + costText.text;
            }
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioS.clip = clip;
        audioS.Play();
    }
}
