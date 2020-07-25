using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{

    public GameObject explosionEffect;
    public AudioClip[] explosionSounds;

    private void Start()
    {
        
    }

    public void explode()
    {
        var effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        effect.GetComponent<AudioSource>().clip = explosionSounds[Random.Range(0, explosionSounds.Length)];
        effect.GetComponent<AudioSource>().Play();
        Destroy(effect,3);
        Destroy(gameObject);
    }
}
