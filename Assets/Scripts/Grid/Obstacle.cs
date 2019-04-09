using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    #region Properties
    // PUBLIC
    public List<GameObject> obstacles;
    // PRIVATE
    private int xGrid;
    private int yGrid;
    private Bounds bounds;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        RetrieveChilds();
        CreateBounds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Bounds GetBounds()
    {
        return bounds;
    }

    public void SetCoords(int x_, int y_)
    {
        xGrid = x_;
        yGrid = y_;
    }

    public bool IsIntersecting(Bounds boundsIntersect)
    {
        return bounds.Intersects(boundsIntersect);
    }

    private void RetrieveChilds()
    {
        obstacles.Clear();
        for(int i = 0; i < transform.childCount; i++)
        {
            obstacles.Add(transform.GetChild(i).gameObject);
        }
    }

    private void CreateBounds()
    {
        bounds = new Bounds(obstacles[0].transform.position, Vector3.zero);
        foreach (GameObject obj in obstacles)
        {
            bounds.Encapsulate(obj.transform.position);
        }

        Vector3 upper = new Vector3(bounds.max.x + 1.5f, bounds.max.y, bounds.max.z + 1.5f);
        Vector3 down = new Vector3(bounds.min.x - 1.5f, bounds.min.y, bounds.min.z - 1.5f);

        bounds.Encapsulate(upper);
        bounds.Encapsulate(down);
        
    }

    void OnDrawGizmos()
    {
        /*
        if (bounds != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(bounds.center, bounds.size);
        }
        */
    }
}
