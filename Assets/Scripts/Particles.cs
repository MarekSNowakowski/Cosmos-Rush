using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    public ParticleSystem effectPS;

    // Start is called before the first frame update
    void Start()
    {
        if (effectPS != null) effectPS.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (effectPS != null && collision.CompareTag("particleArea"))
        {
            effectPS.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (effectPS != null && collision.CompareTag("particleArea"))
        {
            effectPS.Stop();
        }
    }
}
