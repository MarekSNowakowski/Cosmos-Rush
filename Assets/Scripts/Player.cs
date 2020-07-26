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

    [Header("Components")]
    public Camera cam;
    public GameObject vcam;
    private LineRenderer line;
    private Rigidbody2D rb;
    private AudioSource audioSource;

    public void Awake()
    {
        line = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Start()
    {
        minimumPower = new Vector2(-minMaxPower, -minMaxPower);
        maximumPower = new Vector2(minMaxPower, minMaxPower);
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
        }
        if(Input.GetMouseButtonUp(0))
        {
            Time.timeScale = 1;
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 15;

            ballForce = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minimumPower.x, maximumPower.x),
                Mathf.Clamp(startPoint.y - endPoint.y, minimumPower.y, maximumPower.y));
            rb.velocity = Vector2.zero;
            rb.AddForce(ballForce * BallPower, ForceMode2D.Impulse);
            endline();
            audioSource.Play();
            playerPS.Play();
        }
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
        vcam.GetComponent<Animator>().SetBool("ScreenKick", true);
        StartCoroutine(KickCo());
        if(collision.gameObject.CompareTag("100"))
        {
            collision.gameObject.GetComponent<Circle>().explode();
        }
        if(collision.gameObject.CompareTag("300"))
        {
            gameOver();
        }
    }

    private IEnumerator KickCo()
    {
        yield return null;
        vcam.GetComponent<Animator>().SetBool("ScreenKick", false);
    }

    private void gameOver()
    {
        Time.timeScale = 1;
        explode();
    }

}
