using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHole : MonoBehaviour
{

    public Galaxy thisGalaxy;
    public Galaxy travelToGalaxy;

    public GameObject pair;

    public float requirement;
    public float galaxyBonus;
    public string galaxyName;

    [HideInInspector]
    private AudioSource audioS;
    private CircleCollider2D col;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        col = GetComponent<CircleCollider2D>();
    }

    public void Warp()
    {
        travelToGalaxy.Despawn();
        audioS.Play();
        thisGalaxy.Spawn();
        thisGalaxy.Bound();
        StartCoroutine(turnOffCo());
        //if(this.galaxyBonus == 1.25) { CloudOnce.Achievements.andromeda.Unlock(); }   v0.4 artefact
        //if(this.galaxyBonus == 2) { CloudOnce.Achievements.triangulum.Unlock(); }
    }

    public IEnumerator turnOffCo()
    {
        col.enabled = false;
        yield return new WaitForSeconds(1);
        col.enabled = true;
    }
}
