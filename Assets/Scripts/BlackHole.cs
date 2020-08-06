using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float mass;
    public float G;
    private float R;
    private float scale;
    public GameObject whiteHole;
    public Vector2 minMax;

    private Vector2 vector;
    private float force;

    private CircleCollider2D area;

    [HideInInspector]
    public new AudioSource audio;

    void Update()
    {

    }

    public void bound(GameObject whiteHole)
    {
        this.whiteHole = whiteHole;
    }

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        scale = Random.Range(minMax.x, minMax.y);
        transform.localScale = new Vector3(scale, scale, 1);
        mass = mass * scale + Random.Range(0, 999);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            vector = transform.position - collision.transform.position;
            R = Vector2.Distance(transform.position, collision.transform.position);
            force = G * mass * rb.mass / Mathf.Pow(R,2);
            if(G!=0) rb.AddForce(vector.normalized * force);
        }
    }

    public void Warp()
    {
        StartCoroutine(lowerForceCo());
        audio.Play();
    }

    private IEnumerator lowerForceCo()
    {
        var pG = G;
        G = 0;
        yield return new WaitForSeconds(0.1f);
        G = pG / 2;
        yield return new WaitForSeconds(0.3f);
        G = pG;
    }
}
