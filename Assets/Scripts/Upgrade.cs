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
    AudioSource audio;

    public TextMeshProUGUI progressText;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI benefitsText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI costText2;
    public GameObject BuyPanel;

    public GameObject outline;


    void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        setTexts();
    }

    public void setColor()
    {
        if (upgradeName == "white") PlayerPrefs.SetInt("color", 0);
        else if (upgradeName == "blue" && PlayerPrefs.GetInt("blue",0) > 0) PlayerPrefs.SetInt("color", 1);
        else if (upgradeName == "violet" && PlayerPrefs.GetInt("violet", 0) > 0) PlayerPrefs.SetInt("color", 2);
        else if (upgradeName == "gold" && PlayerPrefs.GetInt("gold", 0) > 0) PlayerPrefs.SetInt("color", 3);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetInt("color", 0));
    }

    public void upgrade()
    {
        if (PlayerPrefs.GetInt(upgradeName, 0) == upgradeMax) ;
        else if (PlayerPrefs.GetFloat("money", 0) < costs[PlayerPrefs.GetInt(upgradeName, 0)]) ;
        else
        {
            PlayerPrefs.SetFloat("money", PlayerPrefs.GetFloat("money", 0) - costs[PlayerPrefs.GetInt(upgradeName, 0)]);
            PlayerPrefs.SetInt(upgradeName, PlayerPrefs.GetInt(upgradeName, 0) + 1);
            PlayerPrefs.Save();
            anim.SetTrigger("Normal");
            setTexts();
            audio.Play();
        }
        setColor();
    }

    void setTexts()
    {
        progressText.text = PlayerPrefs.GetInt(upgradeName, 0) + "/" + upgradeMax;
        titleText.text = title;
        if (PlayerPrefs.GetInt(upgradeName, 0) == upgradeMax)
        {
            benefitsText.text = "MAX";
            costText.text = "-";
            costText2.text = "Upgraded to maximum level";
        }
        else
        {
            string blue = "slowMotion - 0.1" + Environment.NewLine + "ballPower + 10" + Environment.NewLine + "comboTimeMultiplayer -0.01";
            string red = 

            progressText.text = PlayerPrefs.GetInt(upgradeName, 0) + "/" + upgradeMax;
            titleText.text = title;
            benefitsText.text = benefits;
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
}
