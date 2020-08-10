using UnityEngine;
using TMPro;

public class Upgrade : MonoBehaviour
{
    public float[] costs;
    public string upgradeName;
    public int upgradeMax;
    public string benefits;
    public string title;

    public TextMeshProUGUI progressText;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI benefitsText;
    public TextMeshProUGUI costText;


    void Start()
    {
        setTexts();
    }

    void upgrade()
    {
        if(PlayerPrefs.GetFloat("money",0) < costs[PlayerPrefs.GetInt(upgradeName, 0)])
        {
            GetComponent<Animator>().SetBool("fail", true);
        }
        else
        {
            PlayerPrefs.SetFloat("money", PlayerPrefs.GetFloat("money", 0) - costs[PlayerPrefs.GetInt(upgradeName, 0)]);
            PlayerPrefs.SetInt(upgradeName, PlayerPrefs.GetInt(upgradeName, 0) + 1);

            setTexts();
        }
    }

    void setTexts()
    {
        progressText.text = PlayerPrefs.GetInt(upgradeName, 0) + "/" + upgradeMax;
        titleText.text = title;
        benefitsText.text = benefits;
        costText.text = costs[PlayerPrefs.GetInt(upgradeName, 0)] + "$";
    }
}
