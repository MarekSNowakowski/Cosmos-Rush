using System;
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
    private float minVelocity = 5.1f; //minimal velocity needed to destroy a ball

    [Header("Reactions")]
    public float SFduration;
    public float slowBouncines;
    public float fastBouncines;
    public float transparentSlow;
    bool madness;

    [Header("Statistics")]
    public float comboTimeMultiplayer; //comboMaxTime shortens with every combo
    public int currentCombo;
    public float comboMaxTime;
    public float comboCurrentTime;

    public float score;
    public float highScore;
    public float destroyedBalls;
    public float destroyedBallsOverall;
    public float maxSpeed;
    public float maxSpeedOverall;
    public float distance;
    public float distanceOverall;
    public float money;

    private Vector2 currentPoint;
    private Vector2 previousPoint;

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
    public GameObject madnessEffect;
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
        if (Input.GetMouseButtonDown(0) && !madness)
        {
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15;
            Time.timeScale = slowmo;
            speedAndDistance();
        }
        if(Input.GetMouseButton(0) && !madness)
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
        if(Input.GetMouseButtonUp(0) && !madness)
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
            speedAndDistance();
        }
        if(madness)
        {
            Time.timeScale = 1f;
            endline();
        }
        Combo();
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
        speedAndDistance();
        Rotating(rb.velocity);

        if (rb.velocity.magnitude < minVelocity) return; 

        StartCoroutine(KickCo());
        

        if (collision.gameObject.CompareTag("100"))
        {
            BallDestroyed(100);
            collision.gameObject.GetComponent<Circle>().explode();
        }
        else if( collision.gameObject.CompareTag("200a"))
        {
            BallDestroyed(200);
            StopAllCoroutines();
            setUp();
            slowEffect.SetActive(true);
            StartCoroutine(changeBounceAndColorCo(slowBouncines, collision.gameObject.GetComponent<SpriteRenderer>().color));
            collision.gameObject.GetComponent<Circle>().explode();
            minVelocity *= slowBouncines;
            rb.velocity *= slowBouncines;
        }
        else if (collision.gameObject.CompareTag("200b"))
        {
            BallDestroyed(200);
            StopAllCoroutines();
            setUp();
            fastEffect.SetActive(true);
            StartCoroutine(changeBounceAndColorCo(fastBouncines, collision.gameObject.GetComponent<SpriteRenderer>().color));
            collision.gameObject.GetComponent<Circle>().explode();
            minVelocity *= fastBouncines;
            rb.velocity *= fastBouncines;
        }
        else if (collision.gameObject.CompareTag("300"))
        {
            gameOver();
        }
        else if (collision.gameObject.CompareTag("400"))
        {
            BallDestroyed(400);
            StopAllCoroutines();
            setUp();
            madnessEffect.SetActive(true);
            madness = true;
            StartCoroutine(changeBounceAndColorCo(1, collision.gameObject.GetComponent<SpriteRenderer>().color));
            collision.gameObject.GetComponent<Circle>().explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(KickCo());
        if (collision.gameObject.CompareTag("100"))
        {
            BallDestroyed(100);
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
        madnessEffect.SetActive(false);
        madness = false;
        minVelocity = 6;
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
        saveStatistics();
        Time.timeScale = 1;
        explode();
    }

    private void saveStatistics()
    {
        if (score > highScore) highScore = score;
        if (maxSpeed > maxSpeedOverall) maxSpeedOverall = maxSpeed;

        destroyedBallsOverall += destroyedBalls;
        distanceOverall += distance;

        money += score / 100;

        PlayerPrefs.SetFloat("HighScore", highScore);
        PlayerPrefs.SetFloat("maxSpeed", maxSpeedOverall);
        PlayerPrefs.SetFloat("destroyedBalls", destroyedBallsOverall);
        PlayerPrefs.SetFloat("distance", distanceOverall);
        PlayerPrefs.SetFloat("money", money);
        PlayerPrefs.Save();
    }

    /*private void loadStatistics()
    {
        highScore = PlayerPrefs.GetFloat("HighScore");
        maxSpeedOverall = PlayerPrefs.GetFloat("maxSpeed", maxSpeedOverall);
        destroyedBallsOverall = PlayerPrefs.GetFloat("destroyedBalls", destroyedBallsOverall);
        distanceOverall = PlayerPrefs.GetFloat("distance", distanceOverall);
        money = PlayerPrefs.GetFloat("money", money);
    }*/

    public void speedAndDistance()
    {
        currentPoint = transform.position;

        distance += (long)Vector2.Distance(previousPoint, currentPoint);

        previousPoint = currentPoint;

        if (rb.velocity.magnitude > maxSpeed) maxSpeed = rb.velocity.magnitude;
    }

    private void Combo()
    {
        comboCurrentTime -= Time.deltaTime;
        if(comboCurrentTime < 0)
        {
            currentCombo = 0;
            comboCurrentTime = comboMaxTime;
        }
    }

    public void BallDestroyed(short score)
    {
        //Increase Combo
        currentCombo++;
        comboCurrentTime = comboMaxTime * Mathf.Pow(comboTimeMultiplayer, currentCombo);

        //AddPoints
        int speedMultiplyer = (int)Math.Round(Convert.ToDouble(rb.velocity.magnitude) / 10, MidpointRounding.AwayFromZero);
        this.score += score * currentCombo * speedMultiplyer;

        destroyedBalls++;
    }

    public void setUpStatistics()
    {
        score = 0;
        destroyedBalls = 0;
        maxSpeed = 0;
        distance = 0;
        money = 0;
    }

}
