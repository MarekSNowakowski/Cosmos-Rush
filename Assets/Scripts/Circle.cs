using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Circle : MonoBehaviour
{
    [Header("Explosion")]
    public GameObject explosionEffect;
    public GameObject scoreEffect;

    public AudioClip[] explosionSounds;
    private float effectTime = 2;


    public void explode()
    {
        var effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        
        effect.GetComponent<AudioSource>().clip = explosionSounds[Random.Range(0, explosionSounds.Length)];
        effect.GetComponent<AudioSource>().Play();
        Destroy(effect, effectTime);

        if (this.gameObject.CompareTag("Player")) this.gameObject.SetActive(false);
        else Destroy(gameObject);
    }

    public void explode(float score)
    {
        var effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        var scoreE = Instantiate(scoreEffect, transform.position, Quaternion.identity);

        scoreE.GetComponent<TextMeshPro>().text = "+" + score;
        effect.GetComponent<AudioSource>().clip = explosionSounds[Random.Range(0, explosionSounds.Length)];
        effect.GetComponent<AudioSource>().Play();

        Destroy(effect, effectTime);
        Destroy(scoreE, effectTime);

        if (this.gameObject.CompareTag("Player")) this.gameObject.SetActive(false);
        else Destroy(gameObject);
    }
}
