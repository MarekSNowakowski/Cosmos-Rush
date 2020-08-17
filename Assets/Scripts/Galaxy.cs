using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galaxy : MonoBehaviour
{
    public float repellForce;
    private PolygonCollider2D colider;
    private LineRenderer lr;
    private EdgeCollider2D edgeCollider;

    [Header("Spawning")]
    public GameObject player;
    public Object[] circles;
    public int[] number;
    public float radiusCheck;

    private void Start()
    {
        buildBoundaries();
        //setPlayer();
    }

    private void buildBoundaries()
    {
        colider = GetComponent<PolygonCollider2D>();
        lr = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();

        Vector2[] path = colider.GetPath(0);
        int i;

        lr.positionCount = path.Length + 1;
        Vector2[] tempArray = new Vector2[path.Length + 1];

        for (i = 0; i < path.Length; i++)
        {
            tempArray.SetValue(path[i], i);
            lr.SetPosition(i, new Vector3(transform.position.x + path[i].x, transform.position.y + path[i].y, 0));
        }
        tempArray.SetValue(path[0], i);
        lr.SetPosition(i, new Vector3(transform.position.x + path[0].x, transform.position.y + path[0].y, 0));
        edgeCollider.points = tempArray;
    }

    public void Spawn()
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

    public void Bound()
    {
        int i = 0;
        var blackHoles = GameObject.FindGameObjectsWithTag("blackHole");
        var whiteHoles = GameObject.FindGameObjectsWithTag("whiteHole");
        foreach (var blackHole in blackHoles)
        {
            blackHole.GetComponent<BlackHole>().bound(whiteHoles[i]);
            i++;
        }
    }

    private void setPlayer()
    {
        player.SetActive(true);
        player.transform.position = this.transform.position;
    }

    public void Despawn()
    {
        string[] circleTags = { "100", "200a", "200b", "300", "400", "blackHole", "whiteHole", "500", "1000", "death", "Effect", "goldBall", "violetBall" };
        foreach (string circleTag in circleTags)
        {
            var clones = GameObject.FindGameObjectsWithTag(circleTag);
            foreach (var clone in clones) Destroy(clone);
        }
    }

    public void GoToMainMenu()
    {
        player.SetActive(true);
        player.transform.position = this.transform.position;
        Despawn();
        player.SetActive(false);
    }

    public void newGame()
    {
        Despawn();
        setPlayer();
        Spawn();
        Bound();
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
}
