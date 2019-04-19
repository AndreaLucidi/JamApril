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
    public LayerMask unwalkableMask;
    [Header("Objects for tiling")]
    public GameObject tile;
    [HideInInspector] public Vector3 playerPosition;
    [Header("Enemy prefab")]
    public GameObject enemy;
    //PRIVATE
    [SerializeField] private float obstaclePercentage = 0.3f;
    [SerializeField] private ObstacleLibrary obstacleLibrary;
    private List<Bounds> obstaclesBounds;
    #endregion

    void Awake()
    {
        obstaclesBounds = new List<Bounds>();
        playerPosition = new Vector3(0.0f, 100.0f, 0.0f);
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

        //DestroyTileUnderObstacle();
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

    public void DestroyTileUnderObstacle()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Contains("Tile"))
            {
                Vector3 pos = child.position;
                pos.y = 50.0f;

                Collider[] ca = Physics.OverlapBox(child.position, Vector3.one * 0.4f, Quaternion.identity, unwalkableMask);

                if (ca.Length > 0)
                {
                    foreach (Collider c in ca)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }
        }
    }

    public void SetAgents(GameObject player)
    {
        // Set player
        int xPlayer = 10;
        int yPlayer = 10;
        bool found = false;

        while (!found)
        {
            var t = transform.Find("Tile_" + xPlayer.ToString() + "_" + yPlayer.ToString());
            if (xPlayer % 2 == 0)
                xPlayer++;
            else
                yPlayer++;

            if (t)
            {
                Instantiate(player, t.position, Quaternion.identity);
                found = true;
            }
        }

        //Left bottom agent
        SetAgent(2, 2, 1, 1);
        // Right bottom
        SetAgent(19, 2, -1, 1);
        // Left upper agent
        SetAgent(2, 19, 1, -1);
        // Right upper agent
        SetAgent(19, 19, -1, -1);
    }

    private void SetAgent(int x, int y, int xSign, int ySign)
    {
        bool found = false;
        int xIndex = x;
        int yIndex = y;
        while (!found)
        {
            Transform t = transform.Find("Tile_" + xIndex.ToString() + "_" + yIndex.ToString());

            if (x % 2 == 0)
                xIndex += xSign;
            else
                yIndex += ySign;

            if (yIndex > cols || xIndex > rows)
                break;

            if (t)
            {
                Instantiate(enemy, t.position, Quaternion.identity);
                found = true;
            }
        }
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