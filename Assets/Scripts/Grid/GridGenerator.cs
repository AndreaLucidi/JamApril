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
    public GameObject obstacle;
    //PRIVATE
    [SerializeField] private float obstaclePercentage = 0.3f;
    private int[,] instantiator;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        instantiator = new int[rows, cols];
        CreateGrid();
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

    private void CreateGrid()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                Vector3 pos = transform.position + (new Vector3(x, 0.0f, y) * cellSize);
                GameObject tileObj = Instantiate(tile, pos, Quaternion.identity, transform);
                tileObj.name = "Tile_" + (x+1).ToString() + "_" + (y+1).ToString();
                tileObj.GetComponent<Tile>().SetCoordinates(x, y);
                if (GameMan.instance.IsObstacle(x, y) == 0)
                    Instantiate(obstacle, pos, Quaternion.identity, transform);
            }
        }
    }

    private void GenerateString()
    {
        float generate = 0.0f;

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                generate = Random.Range(0.0f, 1.0f);
                if (generate < obstaclePercentage)
                    instantiator[x, y] = 0;
                else
                    instantiator[x, y] = 1;
                Debug.Log(name + " " + x + " - " + y + " generate " + generate+ " -> "+instantiator[x, y]);
            }
        }
    }

}
