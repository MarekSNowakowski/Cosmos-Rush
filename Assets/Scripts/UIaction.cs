using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIaction : MonoBehaviour
{
    public GameObject playerObject;
    private Player player;
    private Rigidbody2D rb;
    public GameObject gameOverPanel;
    private Animator animator;

    [Header("In game UI")]
    public GameObject comboTimer;
    public GameObject combo;
    public GameObject score;
    public GameObject speedM;
    public GameObject speed;

    private TextMeshProUGUI comboTimerT;
    private TextMeshProUGUI comboT;
    private TextMeshProUGUI scoreT;
    private TextMeshProUGUI speedMT;
    private TextMeshProUGUI speedT;


    private void Start()
    {
        player = playerObject.GetComponent<Player>();
        rb = playerObject.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();

        comboTimerT = comboTimer.GetComponent<TextMeshProUGUI>();
        comboT = combo.GetComponent<TextMeshProUGUI>();
        scoreT = score.GetComponent<TextMeshProUGUI>();
        speedMT = speedM.GetComponent<TextMeshProUGUI>();
        speedT = speed.GetComponent<TextMeshProUGUI>();
    }

    public void gameOver()
    {
        animator.SetBool("gameRunning", false);
    }

    public void newGame()
    {
        animator.SetBool("mainMenu", false);
        animator.SetBool("gameRunning", true);
        gameOverPanel.SetActive(false);
    }

    public void GoToMainMenu()
    {
        animator.SetBool("mainMenu", true);
    }

    void Update()
    {
        if (rb == null) return;

        int speedMultiplyer = (int)Math.Round(Convert.ToDouble(rb.velocity.magnitude) / 10, MidpointRounding.AwayFromZero);

        if (player.currentCombo == 0) comboTimerT.text = "-";
        else comboTimerT.text = player.comboCurrentTime.ToString("F2");
        comboT.text = "x" + player.currentCombo;
        scoreT.text = player.score.ToString();
        speedMT.text = "x" + speedMultiplyer;
        speedT.text = rb.velocity.magnitude.ToString("F1");
    }
}
