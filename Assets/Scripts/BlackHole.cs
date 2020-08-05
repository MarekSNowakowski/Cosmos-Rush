using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float mass;
    public float G;
    private float R;
    private float scale;

    private Vector2 vector;
    private float force;

    private CircleCollider2D area;

    void Update()
    {

    }

    private void Start()
    {
        scale = Random.Range(0.5f, 1.5f);
        transform.localScale = new Vector3(scale, scale, 1);
        mass = 1000 * scale + Random.Range(0, 999);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            vector = transform.position - collision.transform.position;
            R = Vector2.Distance(transform.position, collision.transform.position);
            force = G * mass * rb.mass / Mathf.Pow(R,2);
            rb.AddForce(vector.normalized * force);
        }
    }
}
