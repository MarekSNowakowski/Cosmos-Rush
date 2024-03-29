﻿using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIaction : MonoBehaviour
{
    public GameObject playerObject;
    public Player player;
    private Rigidbody2D rb;
    public GameObject gameOverPanel;
    private Animator animator;

    [Header("audio")]
    public Camera cam;
    private AudioReverbFilter audioFilter;
    private AudioSource musicSource;
    public float dryLevel;
    public AudioReverbPreset menuEffect;
    public Slider musicVolumeSlider;
    public Slider VolumeSlider;
    public GameObject muteVol;
    public GameObject muteM;
    public GameObject unMuteVol;
    public GameObject unMuteM;


    [Header("In game UI")]
    public TextMeshProUGUI comboTimer;
    public TextMeshProUGUI combo;
    public TextMeshProUGUI score;
    public TextMeshProUGUI speedM;
    public TextMeshProUGUI speed;
    public GameObject effectText;
    public GameObject effectTime;
    private TextMeshProUGUI comboTimerT;
    private TextMeshProUGUI comboT;
    private TextMeshProUGUI scoreT;
    private TextMeshProUGUI speedMT;
    private TextMeshProUGUI speedT;
    private TextMeshProUGUI effectTimeT;
    public TextMeshProUGUI galaxy;

    [Header("GameOver Stats")]
    public TextMeshProUGUI scoreF;
    public TextMeshProUGUI HighScoreT;
    public TextMeshProUGUI HighScore;
    public TextMeshProUGUI maxSpeed;
    public TextMeshProUGUI maxCombo;
    public TextMeshProUGUI ballsDestroyed;
    public TextMeshProUGUI money;
    public float startMoney;
    private float moneyDifference;

    [Header("Overall Statistics")]
    public TextMeshProUGUI highScoreO;
    public TextMeshProUGUI maxSpeedO;
    public TextMeshProUGUI destroyedBallsO;
    public TextMeshProUGUI maxComboO;
    public TextMeshProUGUI distanceO;
    public TextMeshProUGUI moneyEarned;

    [Header("money")]
    public TextMeshProUGUI mainMenuMoney;
    public TextMeshProUGUI upgradeMoney;

    [Header("Upgrades")]
    public GameObject white;
    public GameObject blue;
    public GameObject violet;
    public GameObject gold;
    public GameObject force;
    public GameObject slowMo;
    public GameObject spikeCrusher;
    public GameObject warpHoles;

    [Header("Ads")]
    //public AdManager adManager;
    public bool canContinue = true;
    public TextMeshProUGUI continueTimer;

    [Header("Simple mode")]
    public GameObject simpleModeToggle;
    private Animator slowMoAnim;

    private void Start()
    {
        player = playerObject.GetComponent<Player>();
        rb = playerObject.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        slowMoAnim = cam.GetComponent<Animator>();

        comboTimerT = comboTimer.GetComponent<TextMeshProUGUI>();
        comboT = combo.GetComponent<TextMeshProUGUI>();
        scoreT = score.GetComponent<TextMeshProUGUI>();
        speedMT = speedM.GetComponent<TextMeshProUGUI>();
        speedT = speed.GetComponent<TextMeshProUGUI>();
        effectTimeT = effectTime.GetComponent<TextMeshProUGUI>();

        audioFilter = cam.GetComponent<AudioReverbFilter>();
        musicSource = cam.GetComponent<AudioSource>();

        audioFilter.reverbPreset = menuEffect;
        audioFilter.dryLevel = -dryLevel;

        mainMenuMoney.text = PlayerPrefs.GetFloat("money") + " $";

        VolumeSlider.value = PlayerPrefs.GetFloat("volume", 0.5f);
        changeVolume();

        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        changeMusicVolume();

        if(PlayerPrefs.GetInt("simpleMode",0)!=0)
        {
            simpleModeToggle.SetActive(true);
        }

        setSimpleMode();
    }

    public void Statistics()
    {
        animator.SetBool("Statistics", true);
        highScoreO.text = PlayerPrefs.GetFloat("HighScore").ToString();
        maxSpeedO.text = PlayerPrefs.GetFloat("maxSpeed").ToString("F1");
        destroyedBallsO.text = PlayerPrefs.GetFloat("destroyedBalls").ToString();
        distanceO.text = PlayerPrefs.GetFloat("distance").ToString();
        moneyEarned.text = PlayerPrefs.GetFloat("moneyEarned").ToString();
        maxComboO.text = PlayerPrefs.GetInt("maxCombo").ToString();
    }

    public void About()
    {
        animator.SetBool("About", true);
    }

    public void Settings()
    {
        animator.SetBool("Settings", true);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        VolumeSlider.value = PlayerPrefs.GetFloat("volume", 0.5f);
    }

    public void gameOver()
    {
        audioFilter.reverbPreset = menuEffect;
        audioFilter.dryLevel = -dryLevel;
        player.StopAllCoroutines();
        canContinue = false;
        if (canContinue)
        {
            StartCoroutine(continueCo());
        }
        else
        {
            player.saveStatistics();
            animator.SetBool("gameRunning", false);

            scoreF.text = player.score.ToString();
            HighScore.text = player.highScore.ToString();
            if (player.oldHighScore != player.highScore)
            {
                animator.SetBool("highScore", true);
            }
            maxSpeed.text = player.maxSpeed.ToString("F1");
            maxCombo.text = player.maxCombo.ToString();
            ballsDestroyed.text = player.destroyedBalls.ToString();
            moneyDifference = player.money - startMoney;
            StartCoroutine(addMoney());
        }
    }

    IEnumerator continueCo()
    {
        animator.SetBool("canContinue", true);
        animator.SetBool("gameRunning", false);
        continueTimer.text = "5";
        yield return new WaitForSeconds(1f);
        continueTimer.text = "4";
        yield return new WaitForSeconds(1f);
        continueTimer.text = "3";
        yield return new WaitForSeconds(1f);
        continueTimer.text = "2";
        yield return new WaitForSeconds(1f);
        continueTimer.text = "1";
        yield return new WaitForSeconds(1f);
        continueTimer.text = "0";
        yield return new WaitForSeconds(1f);
        noThanks();
    }

    public void continuePlaying()
    {
        StopAllCoroutines();
        canContinue = false;
        animator.SetBool("canContinue", false);
        animator.SetBool("gameRunning", true);

        player.gameObject.SetActive(true);
        player.tr.enabled = true;

        audioFilter.reverbPreset = AudioReverbPreset.User;
        audioFilter.dryLevel = 0;
    }

    public void noThanks()
    {
        StopAllCoroutines();
        canContinue = false;
        animator.SetBool("canContinue", false);
        gameOver();
    }

    public void highScoreStart()
    {
        HighScore.text = player.oldHighScore.ToString();
        animator.SetBool("highScore", false);
        HighScoreT.text = "NEW HIGH SCORE!";
    }

    public void highScoreEnd()
    {
        HighScore.text = player.score.ToString();
        HighScoreT.text = "HIGH SCORE:";
    }

    public IEnumerator addMoney()
    {
        var diffConst = player.money - startMoney;
        money.text = startMoney + " $" + Environment.NewLine + " + " + (moneyDifference) + " $";
        yield return new WaitForSeconds(1);
        for (int j = 12; j >= 0; j--)
        {
            if (Mathf.Pow(10, j) > moneyDifference) yield return null;
            else
            {
                do
                {
                    startMoney += Mathf.Pow(10, j);
                    moneyDifference -= Mathf.Pow(10, j);
                    money.text = startMoney + " $" + Environment.NewLine + " + " + (moneyDifference) + " $";
                    if (moneyDifference == 0) break;
                    yield return new WaitForSeconds(0.1f);
                } while (Mathf.Pow(10, j) < moneyDifference - (Mathf.Pow(10, j)));
            }

        }

        money.text = PlayerPrefs.GetFloat("money",0).ToString() + " $";
    }

    public void newGame()
    {
        canContinue = true;
        cam.GetComponent<AudioReverbFilter>().dryLevel = 0;
        animator.SetBool("mainMenu", false);
        animator.SetBool("gameRunning", true);
        gameOverPanel.SetActive(false);

        audioFilter.reverbPreset = AudioReverbPreset.User;
        audioFilter.dryLevel = 0;
        startMoney = PlayerPrefs.GetFloat("money");
        StopAllCoroutines();
    }

    public void GoToMainMenu()
    {
        animator.SetBool("mainMenu", true);
        animator.SetBool("Statistics", false);
        animator.SetBool("About", false);
        animator.SetBool("Upgrades", false);
        animator.SetBool("Settings", false);
        animator.SetBool("Colors", false);
        StopAllCoroutines();
        mainMenuMoney.text = PlayerPrefs.GetFloat("money") + " $";
    }

    public void ResetProgress()
    {
        //Statistics
        PlayerPrefs.SetFloat("HighScore", 0);
        PlayerPrefs.SetFloat("maxSpeed", 0);
        PlayerPrefs.SetFloat("destroyedBalls", 0);
        PlayerPrefs.SetFloat("distance", 0);
        PlayerPrefs.SetFloat("money", 0);
        PlayerPrefs.SetFloat("moneyEarned", 0);
        PlayerPrefs.SetInt("maxCombo", 0);
        PlayerPrefs.Save();
        player.loadStatistics();

        //Upgrades
        PlayerPrefs.SetInt("spikeCrusher", 0); //max 3
        PlayerPrefs.SetInt("warpHoles", 0); //max 1
        PlayerPrefs.SetInt("slowMo", 0); //max 10
        PlayerPrefs.SetInt("force", 0); //max 10

        //Colors, max 5
        PlayerPrefs.SetInt("color", 0);
        PlayerPrefs.SetInt("blue", 0);
        PlayerPrefs.SetInt("red", 0);
        PlayerPrefs.SetInt("violet", 0);
        PlayerPrefs.SetInt("gold", 0);
        PlayerPrefs.Save();
    }

    void Update()
    {
        if (rb == null) return;

        int speedMultiplyer = (int)Math.Round(Convert.ToDouble(rb.velocity.magnitude) / 10, MidpointRounding.AwayFromZero);

        if (player.currentCombo == 0) comboTimerT.text = "-";
        else comboTimerT.text = player.comboCurrentTime.ToString("F1");
        comboT.text = "x" + player.currentCombo;
        scoreT.text = player.score.ToString();
        speedMT.text = "x" + speedMultiplyer;
        speedT.text = rb.velocity.magnitude.ToString("F1");

        if (player.effectTimer == 0)
        {
            effectText.SetActive(false);
            effectTime.SetActive(false);
        } else if (!effectText.activeInHierarchy || !effectTime.activeInHierarchy)
        {
            effectText.SetActive(true);
            effectTime.SetActive(true);
        } else
        {
            effectTimeT.text = player.effectTimer.ToString("F1");
        }
    }

    public void changeVolume()
    {
        if (PlayerPrefs.GetFloat("volume") == 0 && VolumeSlider.value != 0)
        {
            muteVol.SetActive(true);
            unMuteVol.SetActive(false);
        }
        if (VolumeSlider.value == 0)
        {
            muteVol.SetActive(false);
            unMuteVol.SetActive(true);
        }
        PlayerPrefs.SetFloat("volume", VolumeSlider.value);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }

    public void changeMusicVolume()
    {
        if (PlayerPrefs.GetFloat("musicVolume") == 0 && musicVolumeSlider.value != 0)
        {
            muteM.SetActive(true);
            unMuteM.SetActive(false);
        }
        if (musicVolumeSlider.value == 0)
        {
            muteM.SetActive(false);
            unMuteM.SetActive(true);
        }
        PlayerPrefs.SetFloat("musicVolume", musicVolumeSlider.value);
        musicSource.volume = PlayerPrefs.GetFloat("musicVolume");
    }

    public void muteVolume()
    {
        PlayerPrefs.SetFloat("volume", 0);
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 0);
        VolumeSlider.value = PlayerPrefs.GetFloat("volume", 0);
    }

    public void muteMusic()
    {
        PlayerPrefs.SetFloat("musicVolume", 0);
        musicSource.volume = PlayerPrefs.GetFloat("musicVolume", 0);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 0);
    }

    public void unMuteVolume()
    {
        PlayerPrefs.SetFloat("volume", 0.5f);
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 0.5f);
        VolumeSlider.value = PlayerPrefs.GetFloat("volume", 0.5f);
    }

    public void unMuteMusic()
    {
        PlayerPrefs.SetFloat("musicVolume", 0.5f);
        musicSource.volume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.5f);
    }

    public void Upgrades()
    {
        animator.SetBool("Upgrades", true);
        animator.SetBool("Colors", false);
        UpdateMoney();
    }

    public void UpdateMoney()
    {
        upgradeMoney.text = PlayerPrefs.GetFloat("money") + " $";
        mainMenuMoney.text = PlayerPrefs.GetFloat("money") + " $";

        setUpUpgradesTexts();
    }

    public void setUpUpgradesTexts()
    {
        white.GetComponent<Upgrade>().setTexts();
        blue.GetComponent<Upgrade>().setTexts();
        violet.GetComponent<Upgrade>().setTexts();
        gold.GetComponent<Upgrade>().setTexts();

        force.GetComponent<Upgrade>().setTexts();
        slowMo.GetComponent<Upgrade>().setTexts();
        spikeCrusher.GetComponent<Upgrade>().setTexts();
        warpHoles.GetComponent<Upgrade>().setTexts();
    }

    public void Colors()
    {
        animator.SetBool("Colors", true);
        animator.SetBool("Upgrades", false);
        setOutline();
    }

    public void setOutline()
    {
        white.GetComponent<Upgrade>().outline.SetActive(PlayerPrefs.GetInt("color", 0) == 0);
        blue.GetComponent<Upgrade>().outline.SetActive(PlayerPrefs.GetInt("color", 0) == 1);
        violet.GetComponent<Upgrade>().outline.SetActive(PlayerPrefs.GetInt("color", 0) == 2);
        gold.GetComponent<Upgrade>().outline.SetActive(PlayerPrefs.GetInt("color", 0) == 3);
    }

    public IEnumerator enterGalaxyCo(String galaxyName, float galaxyBonus)
    {
        galaxy.text = "Galaxy " + galaxyName + Environment.NewLine + Environment.NewLine + "Score +" + (galaxyBonus - 1) * 100 + "%";
        galaxy.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        galaxy.gameObject.SetActive(false);
    }

    public IEnumerator GalaxyLockedCo(float requirement)
    {
        galaxy.text = "High score needed" + Environment.NewLine + Environment.NewLine + requirement + "+";
        galaxy.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        galaxy.gameObject.SetActive(false);
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    public void simpleModeCheck()
    {
        if (PlayerPrefs.GetInt("simpleMode",0) == 0)
        {
            PlayerPrefs.SetInt("simpleMode", 1);
            simpleModeToggle.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("simpleMode", 0);
            simpleModeToggle.SetActive(false);
        }
        PlayerPrefs.Save();
    }

    public void setSimpleMode()
    {
        if (PlayerPrefs.GetInt("simpleMode", 0) == 0)
        {
            slowMoAnim.SetBool("simpleMode", false);
        }
        else
        {
            slowMoAnim.SetBool("simpleMode", true);
        }
    }
}
