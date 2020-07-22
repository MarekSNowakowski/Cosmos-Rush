using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float BallPower;
    public Rigidbody2D rb;

    public Vector2 minimumPower;
    public Vector2 maximumPower;
    public LineRenderer line;
    Camera camera;
    Vector2 ballForce;
    Vector3 startPoint;
    Vector3 endPoint;

    public void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    public void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = camera.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15;
        }
        if(Input.GetMouseButton(0))
        {
            Vector3 currentPoint = camera.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 15;
            DrawLine(startPoint, currentPoint);
        }
        if(Input.GetMouseButtonUp(0))
        {
            endPoint = camera.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 15;

            ballForce = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minimumPower.x, maximumPower.x),
                Mathf.Clamp(startPoint.y - endPoint.y, minimumPower.y, maximumPower.y));
            rb.AddForce(ballForce * BallPower, ForceMode2D.Impulse);
            endline();
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
        Allpoint[1] = new Vector3(rb.position.x - startPoint.x + endPoint.x, rb.position.y - startPoint.y + endPoint.y);
        line.SetPositions(Allpoint);
    }
}
