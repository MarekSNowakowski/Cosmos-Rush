using System;
using System.Collections;
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
    public float slowmo;
    public ParticleSystem playerPS;
    private float angle;
    private float minVelocity = 5.1f; //minimal velocity needed to destroy a ball

    [Header("Reactions")]
    public float effectDuration;
    public float effectTimer;
    public float slowBouncines;
    public float fastBouncines;
    public float transparentSlow;
    bool madness;

    [Header("Statistics")]
    public float comboTimeMultiplayer; //comboMaxTime shortens with every combo
    public int currentCombo;
    public float comboMaxTime;
    public float comboCurrentTime;

    [HideInInspector]
    [Header("GameOverStatistics")]
    public float score;
    public float highScore;
    public float oldHighScore;
    public float destroyedBalls;
    public float destroyedBallsOverall;
    public float maxSpeed;
    public float maxSpeedOverall;
    public float distance;
    public float distanceOverall;
    public float money;
    public int maxCombo;
    public int maxComboOverall;
    public float moneyEarned;
  
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
    public UIaction UIaction;

    public void Awake()
    {
        line = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        tr = GetComponent<TrailRenderer>();
        explosionParticles = explosionEffect.GetComponent<ParticleSystem>().main;

        StartCoroutine(turnOnTrail());
    }

    public void Start()
    {
        loadStatistics();

        minimumPower = new Vector2(-minMaxPower, -minMaxPower);
        maximumPower = new Vector2(minMaxPower, minMaxPower);

        Time.fixedDeltaTime = 0.02F * Time.timeScale;

        setUp();

        loadUpgrades();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !madness)
        {
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15;
            Time.timeScale = slowmo;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
            speedAndDistance();
        }
        if (Input.GetMouseButton(0) && !madness)
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
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
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
            effectTimer -= Time.deltaTime;
        }else if(rb.sharedMaterial.bounciness != 1) effectTimer -= Time.deltaTime;
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
        float scaleX = transform.localScale.x;
        if (scaleX < 1) scaleX += 0.015f;
        float length = Mathf.Sqrt(Mathf.Pow(lookAt.x,2f) + Mathf.Pow(lookAt.y, 2f));
        float scaleY = 1 - length / 100;
        transform.localScale = new Vector3(scaleX, scaleY, 1);
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
        else if (collision.gameObject.CompareTag("200a"))
        {
            BallDestroyed(200);
            StopAllCoroutines();
            setUp();
            slowEffect.SetActive(true);
            effectTimer = effectDuration;
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
            effectTimer = effectDuration;
            StartCoroutine(changeBounceAndColorCo(fastBouncines, collision.gameObject.GetComponent<SpriteRenderer>().color));
            collision.gameObject.GetComponent<Circle>().explode();
            minVelocity *= fastBouncines;
            rb.velocity *= fastBouncines;
        }
        else if (collision.gameObject.CompareTag("300"))
        {
            if(PlayerPrefs.GetInt("spikeCrusher",0) > 0)
            {
                BallDestroyed(300);
                collision.gameObject.GetComponent<Circle>().explode();

            } else gameOver();
        }
        else if (collision.gameObject.CompareTag("400"))
        {
            BallDestroyed(400);
            StopAllCoroutines();
            setUp();
            madnessEffect.SetActive(true);
            madness = true;
            effectTimer = effectDuration;
            StartCoroutine(changeBounceAndColorCo(1, collision.gameObject.GetComponent<SpriteRenderer>().color));
            collision.gameObject.GetComponent<Circle>().explode();
        }
        else if (collision.gameObject.CompareTag("blackHole"))
        {
            if(PlayerPrefs.GetInt("warpHoles", 0) == 1)
            {
            Warp(collision.gameObject.GetComponent<BlackHole>().whiteHole);
            IncreaseCombo();
            AddPoints(500);
            } else gameOver();
        }
        else if (collision.gameObject.CompareTag("500"))
        {
            if (PlayerPrefs.GetInt("spikeCrusher", 0) > 1)
            {
                BallDestroyed(500);
                collision.gameObject.GetComponent<Circle>().explode();

            }
            else gameOver();
        }
        else if (collision.gameObject.CompareTag("1000"))
        {
            if (PlayerPrefs.GetInt("spikeCrusher", 0) > 2)
            {
                BallDestroyed(1000);
                collision.gameObject.GetComponent<Circle>().explode();

            }
            else gameOver();
        }
        else if (collision.gameObject.CompareTag("death"))
        {
            gameOver();
        }
    }

    void Warp(GameObject whiteHole) 
    {
        StartCoroutine(turnOnTrail());
        transform.position = whiteHole.transform.position;
        whiteHole.GetComponent<BlackHole>().Warp();
        rb.velocity *= -0.5f;
    }

    private IEnumerator turnOnTrail()
    {
        float number = 100;
        tr.time = 0;
        for (int i = 0; i < number; i++)
        {
            yield return new WaitForSeconds(1 / number);
            tr.time += 1 / number;
        }
        tr.time = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("100"))
        {
            BallDestroyed(100);
            collision.gameObject.GetComponent<Circle>().explode();
            if (slowEffect.activeInHierarchy) rb.velocity *= slowBouncines;
            else if (fastEffect.activeInHierarchy) rb.velocity *= fastBouncines;
            else rb.velocity *= transparentSlow;
            StartCoroutine(KickCo());
        }
    }

    private IEnumerator changeBounceAndColorCo(float bounc, Color col)
    {
        rb.sharedMaterial.bounciness = bounc;   //bouncines changed

        changeColor(col); 

        yield return new WaitForSeconds(effectDuration);

        effectTimer = 0;
        setUp();
    }

    private void changeColor(Color col)
    {
        ParticleSystem.MinMaxGradient grad = new ParticleSystem.MinMaxGradient(color, col);
        sr.color = col;     //PlayerColor changed
        explosionParticles.startColor = grad;     //ParticleColor changed
        changeTrailColor(col);      //TrailColor changed
    }

    private void changeColor()
    {
        sr.color = color;     //PlayerColor changed
        explosionParticles.startColor = color;     //ParticleColor changed
        changeTrailColor(color);      //TrailColor changed
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
        effectTimer = 0;
        oldHighScore = highScore;
        tr.time = 1;
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

    public void gameOver()
    {
        tr.enabled = false;
        saveStatistics();
        Time.timeScale = 1;
        explode();
        UIaction.gameOver();
    }

    public void newGame()
    {
        tr.enabled = true;
        setUp();
        currentCombo = 0;
        setUpStatistics();
    }

    private void saveStatistics()
    {
        if (score > highScore) highScore = score;
        if (maxSpeed > maxSpeedOverall) maxSpeedOverall = maxSpeed;
        if (maxCombo > maxComboOverall) maxComboOverall = maxCombo;

        destroyedBallsOverall += destroyedBalls;
        distanceOverall += distance;

        money += score / 100;
        moneyEarned += score / 100;

        PlayerPrefs.SetFloat("HighScore", highScore);
        PlayerPrefs.SetFloat("maxSpeed", maxSpeedOverall);
        PlayerPrefs.SetFloat("destroyedBalls", destroyedBallsOverall);
        PlayerPrefs.SetFloat("distance", distanceOverall);
        PlayerPrefs.SetFloat("money", money);
        PlayerPrefs.SetFloat("moneyEarned", moneyEarned);
        PlayerPrefs.SetInt("maxCombo", maxComboOverall);
        PlayerPrefs.Save();
    }

    public void loadStatistics()
    {
        highScore = PlayerPrefs.GetFloat("HighScore");
        maxSpeedOverall = PlayerPrefs.GetFloat("maxSpeed", maxSpeedOverall);
        destroyedBallsOverall = PlayerPrefs.GetFloat("destroyedBalls", destroyedBallsOverall);
        distanceOverall = PlayerPrefs.GetFloat("distance", distanceOverall);
        money = PlayerPrefs.GetFloat("money", money);
        moneyEarned = PlayerPrefs.GetFloat("moneyEarned", moneyEarned);
        maxComboOverall = PlayerPrefs.GetInt("maxCombo", maxComboOverall);
    }

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
        IncreaseCombo();

        AddPoints(score);

        destroyedBalls++;
    }

    private void IncreaseCombo()
    {
        currentCombo++;
        comboCurrentTime = comboMaxTime * Mathf.Pow(comboTimeMultiplayer, currentCombo);
        if (currentCombo > maxCombo) maxCombo = currentCombo;
    }

    private void AddPoints(short score)
    {
        int speedMultiplyer = (int)Math.Round(Convert.ToDouble(rb.velocity.magnitude) / 10, MidpointRounding.AwayFromZero);
        this.score += score * currentCombo * speedMultiplyer;
    }

    public void setUpStatistics()
    {
        score = 0;
        destroyedBalls = 0;
        maxSpeed = 0;
        distance = 0;
        maxCombo = 0;
    }

    public void loadUpgrades()
    {
        slowmo = 0.3f - (0.02f * PlayerPrefs.GetInt("slowMo", 0));
        BallPower = 15 + (2 * PlayerPrefs.GetInt("force", 0));
        minMaxPower = 100;

        effectDuration = 4;
        comboMaxTime = 4;
        comboTimeMultiplayer = 0.92f;


        switch (PlayerPrefs.GetInt("ballColor", 0))
        {
            case 0:
                color = Color.white;
                //White
                // - normall
                slowmo = 0.3f - (0.02f * PlayerPrefs.GetInt("slowMo", 0));
                BallPower = 15 + (2 * PlayerPrefs.GetInt("force", 0));
                break;
            case 1:
                color = Color.blue;
                //Blue
                // slowMotion - 0.1
                // ballPower + 10
                // comboTimeMultiplayer -0.01
                slowmo = 0.3f - (0.01f * PlayerPrefs.GetInt("slowMo", 0)) - (PlayerPrefs.GetInt("blue",0)*0.02f);
                BallPower = 15 +(2 * PlayerPrefs.GetInt("blue")) + (1 * PlayerPrefs.GetInt("force", 0));
                comboTimeMultiplayer = 0.92f + (0.002f * PlayerPrefs.GetInt("blue"));

                break;
            case 2:
                color = Color.red;
                //Red
                // ballPower + 25
                // SlowMotion + 0.2
                // effectDuration +2s
                effectDuration = 4 + (0.4f * PlayerPrefs.GetInt("red", 0));
                slowmo = 0.3f - (0.02f * PlayerPrefs.GetInt("slowMo", 0)) + (PlayerPrefs.GetInt("red", 0) * 0.04f);
                BallPower = 20 + (2 * PlayerPrefs.GetInt("force", 0)) + (4*PlayerPrefs.GetInt("red",0));
                minMaxPower = 100 + (PlayerPrefs.GetInt("red", 0) * 4); 
                break;
            case 3:
                color = new Color(58, 0, 83);
                //Violet
                // comboMaxTime + 2s
                // comboTimeMultiplayer + 0.03
                // ballPower - 5
                //slowMotion + 0.1
                effectDuration = 4;
                comboMaxTime = 4 + (PlayerPrefs.GetInt("violet", 0) * 0.4f);
                comboTimeMultiplayer = 0.92f + (PlayerPrefs.GetInt("violet", 0) * 0.06f);
                slowmo = 0.3f - (0.02f * PlayerPrefs.GetInt("slowMo", 0)) + (PlayerPrefs.GetInt("violet",0) * 0.02f);
                BallPower = 15 + (2 * PlayerPrefs.GetInt("force", 0)) - (1 * PlayerPrefs.GetInt("violet",0));
                break;
            case 4:
                color = new Color(100, 84, 0);
                //Gold
                //Effect duration -1s
                //comboMaxTime +1s
                // comboTimeMultiplayer -0.01
                //slowMotion -0.15
                //BallPower + 15
                effectDuration = 4 - (0.2f * PlayerPrefs.GetInt("gold",0));
                comboMaxTime = 4 + (0.02f * PlayerPrefs.GetInt("gold",0));
                comboTimeMultiplayer = 0.93f;
                slowmo = 0.3f - (0.02f * PlayerPrefs.GetInt("slowMo", 0)) - (PlayerPrefs.GetInt("gold",0)*0.3f);
                BallPower = 15 + (2 * PlayerPrefs.GetInt("force", 0)) + (PlayerPrefs.GetInt("gold",0)*3);
                minMaxPower = 110;
                break;

        }

    }

}
