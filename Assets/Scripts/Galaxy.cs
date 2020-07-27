﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galaxy : MonoBehaviour
{
    public float repellForce;
    public GameObject virtualCamera;
    private PolygonCollider2D colider;
    private LineRenderer lr;
    private EdgeCollider2D edgeCollider;

    [Header("Spawning")]
    public Object[] circles;
    public int[] number;
    public float radiusCheck;

    private void Start()
    {
        colider = GetComponent<PolygonCollider2D>();
        lr = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();

        Vector2[] path = colider.GetPath(0);
        int i;

        lr.positionCount = path.Length + 1;
        Vector2[] tempArray = new Vector2[path.Length+1];

        for (i = 0 ; i < path.Length; i++)
        {
            tempArray.SetValue(path[i], i);
            lr.SetPosition(i, new Vector3(path[i].x, path[i].y, 0));
        }
        tempArray.SetValue(path[0], i);
        lr.SetPosition(i, new Vector3(path[0].x, path[0].y, 0));
        edgeCollider.points = tempArray;

        Spawn();
    }

    private void Spawn()
    {
        int i = 0;
        foreach (Object ob in circles)
        {
            for (int j = 0 ; j < number[i] ; j++)
            {
                Instantiate(ob, PointInArea(), Quaternion.identity);
            }
            i++;
        }
    }

    private Vector3 PointInArea()
    {
        var bounds = colider.bounds;
        var center = bounds.center;

        float x = 0;
        float y = 0;
        int attempt = 0;
        do
        {
            x = UnityEngine.Random.Range(center.x - bounds.extents.x, center.x + bounds.extents.x);
            y = UnityEngine.Random.Range(center.y - bounds.extents.y, center.y + bounds.extents.y);
            attempt++;
        } while ((!colider.OverlapPoint(new Vector2(x, y)) || isObjectHere(new Vector2(x,y)))  && attempt <= 100);

        return new Vector3(x, y, 0);
    }

    bool isObjectHere(Vector2 position)
    {
        Collider2D[] intersecting = Physics2D.OverlapCircleAll(position, radiusCheck);
        return intersecting.Length != 1;
    }

    private void OnEnable()
    {
        virtualCamera.SetActive(true);
    }

    private void OnDisable()
    {
        virtualCamera.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.AddForce((transform.position - collision.transform.position) * repellForce, ForceMode2D.Force); 
        }
    }
}
