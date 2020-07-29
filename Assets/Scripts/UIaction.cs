using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIaction : MonoBehaviour
{
    public GameObject playerObject;
    private Player player;
    private Rigidbody2D rb;
    public Text comboTimer;
    public Text combo;
    public Text score;
    public Text speedM;
    public Text speed;
    public GameObject gameOverPanel;

    private Animator animator;

    private void Start()
    {
        player = playerObject.GetComponent<Player>();
        rb = playerObject.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
    }

    public void gameOver()
    {
        animator.SetBool("gameRunning", false);
    }

    public void newGame()
    {
        animator.SetBool("gameRunning", true);
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (rb == null) return;

        int speedMultiplyer = (int)Math.Round(Convert.ToDouble(rb.velocity.magnitude) / 10, MidpointRounding.AwayFromZero);

        if (player.currentCombo == 0) comboTimer.text = "-";
        else comboTimer.text = player.comboCurrentTime.ToString("F2");
        combo.text = "x" + player.currentCombo;
        score.text = player.score.ToString();
        speedM.text = "x" + speedMultiplyer;
        speed.text = rb.velocity.magnitude.ToString("F1");
    }
}
