using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public GameObject playerObject;
    private Player player;
    private Rigidbody2D rb;
    public Text comboTimer;
    public Text combo;
    public Text score;
    public Text speedM;
    public Text speed;


    private void Start()
    {
        player = playerObject.GetComponent<Player>();
        rb = playerObject.GetComponent<Rigidbody2D>();
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
