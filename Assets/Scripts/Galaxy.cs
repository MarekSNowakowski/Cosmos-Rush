using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galaxy : MonoBehaviour
{
    public float repellForce;
    public GameObject virtualCamera;
    private PolygonCollider2D colider;
    [Header("Spawning")]
    public Object[] circles;
    public int[] number;
    public float radiusCheck;

    private void Start()
    {
        colider = GetComponent<PolygonCollider2D>();
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
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.GetComponent<Rigidbody2D>().AddForce((transform.position - collision.transform.position) * repellForce, ForceMode2D.Force); 
        }
    }
}
