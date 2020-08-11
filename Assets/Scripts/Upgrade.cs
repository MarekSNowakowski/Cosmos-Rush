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

    public TextMeshProUGUI progressText;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI benefitsText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI costText2;
    public GameObject BuyPanel;


    void Start()
    {
        setTexts();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        setTexts();
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
        }
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
