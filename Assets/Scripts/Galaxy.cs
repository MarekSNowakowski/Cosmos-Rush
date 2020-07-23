using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galaxy : MonoBehaviour
{
    public float repellForce;
    public GameObject virtualCamera;

    private void OnEnable()
    {
        virtualCamera.SetActive(true);
    }

    public void OnDisable()
    {
        virtualCamera.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.GetComponent<Rigidbody2D>().AddForce((transform.position - collision.transform.position) * repellForce, ForceMode2D.Force); 
        }
    }
}
