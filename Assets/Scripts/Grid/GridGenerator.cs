using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    #region Properties
    //PUBLIC
    [Header("Grid properties")]
    public float cellSize = 1.0f;
    public int rows;
    public int cols;
    [Header("Objects for tiling")]
    public GameObject tile;
    //PRIVATE
    [SerializeField] private float obstaclePercentage = 0.3f;
    [SerializeField] private ObstacleLibrary obstacleLibrary;
    private List<Bounds> obstaclesBounds;
    #endregion

    void Awake()
    {
        obstaclesBounds = new List<Bounds>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        //Gizmos.DrawWireCube();
    }

    public void CreateGrid()
    {
        GameMan objMan = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMan>();

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                Vector3 pos = transform.position + (new Vector3(x, 0.0f, y) * cellSize);
                if (!GenerateObstacle(pos, x, y))
                {
                    GameObject tileObj = Instantiate(tile, pos, Quaternion.identity, transform);
                    tileObj.name = "Tile_" + (x + 1).ToString() + "_" + (y + 1).ToString();
                    tileObj.GetComponent<Tile>().SetCoordinates(x, y);
                }
            }
        }
    }

    public bool GenerateObstacle(Vector3 pos, int xGrid, int yGrid)
    {
        if (xGrid == 0 || xGrid == rows - 1 || yGrid == 0 || yGrid == cols - 1)
            return false; // Exit if it is the first or lat row/col

        float generate = Random.Range(0.0f, 1.0f);
        if (generate < obstaclePercentage)
        {
            GameObject newObstacle = Instantiate(obstacleLibrary.GetRandomForm(), pos, RandomRotation());

            Obstacle obstacle = newObstacle.GetComponent<Obstacle>();
            foreach (Bounds ob in obstaclesBounds)
            {
                if (obstacle.IsIntersecting(ob))
                {
                    Destroy(newObstacle);
                    return false;
                }
            }

            obstaclesBounds.Add(obstacle.GetBounds());
            obstacle.SetCoords(xGrid, yGrid);
            newObstacle.transform.parent = transform;
            newObstacle.name = "Obstacle_" + (xGrid + 1).ToString() + "_" + (yGrid + 1).ToString();
            return true;
        }

        return false;
    }

    private Quaternion RandomRotation()
    {
        int r = Random.Range(1, 5);
        Vector3 rot = Vector3.zero;

        switch (r)
        {
            default:
            case 1:
                rot = new Vector3(0.0f, 90.0f, 0.0f);
                break;

            case 2:
                rot = new Vector3(0.0f, 180.0f, 0.0f);
                break;

            case 3:
                rot = new Vector3(0.0f, 270.0f, 0.0f);
                break;

            case 4:
                rot = new Vector3(0.0f, 0.0f, 0.0f);
                break;
        }

        return Quaternion.Euler(rot);
    }

}