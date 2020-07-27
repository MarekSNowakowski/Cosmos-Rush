﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Circle
{

    [Header("Movement")]
    public float BallPower;
    public float minMaxPower = 1f;
    private Vector2 minimumPower;
    private Vector2 maximumPower;
    private Vector2 ballForce;
    private Vector3 startPoint;
    private Vector3 endPoint;
    public float slowmo = 0.5f;
    public ParticleSystem playerPS;
    public PolygonCollider2D boundries;
    private float angle;

    [Header("Reactions")]
    public float SFduration;
    public float slowBouncines;
    public float fastBouncines;
    public float transparentSlow;

    [Header("Components")]
    public Camera cam;
    public GameObject vcam;
    private LineRenderer line;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private SpriteRenderer sr;
    private TrailRenderer tr;
    public GameObject fastEffect;
    public GameObject slowEffect;
    public Color color;
    ParticleSystem.MainModule explosionParticles;

    public void Awake()
    {
        line = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        tr = GetComponent<TrailRenderer>();
        explosionParticles = explosionEffect.GetComponent<ParticleSystem>().main;
    }

    public void Start()
    {
        minimumPower = new Vector2(-minMaxPower, -minMaxPower);
        maximumPower = new Vector2(minMaxPower, minMaxPower);

        setUp();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15;
            Time.timeScale = slowmo;
        }
        if(Input.GetMouseButton(0))
        {
            Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 15;
            DrawLine(startPoint, currentPoint);
            Vector2 lookAt = new Vector2(startPoint.x - currentPoint.x, startPoint.y - currentPoint.y);
            Rotating(lookAt);
            Squeeze(lookAt);
        }
        else
        {
            Squeeze();
        }
        if(Input.GetMouseButtonUp(0))
        {
            Time.timeScale = 1;
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 15;
            transform.localScale = Vector3.one;
            ballForce = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minimumPower.x, maximumPower.x),
                Mathf.Clamp(startPoint.y - endPoint.y, minimumPower.y, maximumPower.y));
            rb.velocity = Vector2.zero;
            rb.AddForce(ballForce * BallPower, ForceMode2D.Impulse);
            endline();
            audioSource.Play();
            playerPS.Play();
        }
    }

    private void Rotating(Vector2 lookAt)
    {
        float atan = Mathf.Atan(lookAt.x / lookAt.y) * Mathf.Rad2Deg;
        if (Mathf.Abs(lookAt.y) < Mathf.Epsilon) atan = 0;
        if (lookAt.x > 0)
        {
            if (lookAt.y > 0) angle = -atan; //I
            else if (lookAt.y < 0) angle = 180 - atan;        //IV
            else angle = -90;
        }
        else
        {
            if (lookAt.y > 0) angle = -atan; //II
            else if (lookAt.y < 0) angle = -180 - atan;        //III
            else angle = 90;
        }
        transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    private void Squeeze(Vector3 lookAt)
    {
        float length = Mathf.Sqrt(Mathf.Pow(lookAt.x,2f) + Mathf.Pow(lookAt.y, 2f));
        float scaleY = 1 - length / 100;
        transform.localScale = new Vector3(1, scaleY, 1);
    }

    private void Squeeze()
    {
        float scaleX = 1 - rb.velocity.magnitude / 120;
        transform.localScale = new Vector3(scaleX, 1, 1);
    }

    private void endline()
    {
        line.positionCount = 0;
    }

    private void DrawLine(Vector3 startPoint, Vector3 endPoint)
    {
        line.positionCount = 2;
        Vector3[] Allpoint = new Vector3[2];
        Allpoint[0] = rb.position;
        Allpoint[1] = new Vector3(rb.position.x + startPoint.x - endPoint.x, rb.position.y + startPoint.y - endPoint.y);
        line.SetPositions(Allpoint);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(KickCo());
        Rotating(rb.velocity);
        if (collision.gameObject.CompareTag("100"))
        {
            collision.gameObject.GetComponent<Circle>().explode();
        }
        else if( collision.gameObject.CompareTag("200a"))
        {
            StopAllCoroutines();
            setUp();
            slowEffect.SetActive(true);
            StartCoroutine(changeBounceAndColorCo(slowBouncines, collision.gameObject.GetComponent<SpriteRenderer>().color));
            collision.gameObject.GetComponent<Circle>().explode();
            rb.velocity *= slowBouncines;
        }
        else if (collision.gameObject.CompareTag("200b"))
        {
            StopAllCoroutines();
            setUp();
            fastEffect.SetActive(true);
            StartCoroutine(changeBounceAndColorCo(fastBouncines, collision.gameObject.GetComponent<SpriteRenderer>().color));
            collision.gameObject.GetComponent<Circle>().explode();
            rb.velocity *= fastBouncines;
        }
        else if (collision.gameObject.CompareTag("300"))
        {
            gameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(KickCo());
        if (collision.gameObject.CompareTag("100"))
        {
            collision.gameObject.GetComponent<Circle>().explode();
            if (slowEffect.activeInHierarchy) rb.velocity *= slowBouncines;
            else if (fastEffect.activeInHierarchy) rb.velocity *= fastBouncines;
            else rb.velocity *= transparentSlow;
        }
    }

    private IEnumerator changeBounceAndColorCo(float bounc, Color col)
    {
        ParticleSystem.MinMaxGradient grad = new ParticleSystem.MinMaxGradient(color, col);

        rb.sharedMaterial.bounciness = bounc;   //bouncines changed
        sr.color = col;     //PlayerColor changed
        explosionParticles.startColor = grad;     //ParticleColor changed
        changeTrailColor(col);      //TrailColor changed

        yield return new WaitForSeconds(SFduration);

        setUp();
    }

    private void setUp()
    {
        sr.color = color;
        explosionParticles.startColor = color;
        rb.sharedMaterial.bounciness = 1;
        changeTrailColor(color);
        slowEffect.SetActive(false);
        fastEffect.SetActive(false);
    }

    private void changeTrailColor(Color col)
    {
        float trailStartOpacity = 0.85f;

        col.a = trailStartOpacity;
        tr.startColor = col;
        col.a = 0f;
        tr.endColor = col;
    }

    private IEnumerator KickCo()
    {
        vcam.GetComponent<Animator>().SetBool("ScreenKick", true);
        yield return null;
        vcam.GetComponent<Animator>().SetBool("ScreenKick", false);
    }

    private void gameOver()
    {
        Time.timeScale = 1;
        explode();
    }

}
