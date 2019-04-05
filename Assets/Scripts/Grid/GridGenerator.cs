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
    public int[,] mapTile;
    //PRIVATE
    #endregion

    void Awake()
    {
        mapTile = new int[rows, cols];
    }
    // Start is called before the first frame update
    void Start()
    {
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
        GameMan objMan = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMan>();
        Debug.Log(objMan.XofP1);
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                Vector3 pos = transform.position + (new Vector3(x, 0.0f, y) * cellSize);
                GameObject tileObj = Instantiate(tile, pos, Quaternion.identity, transform);
                tileObj.name = "Tile_" + (x+1).ToString() + "_" + (y+1).ToString();
                tileObj.GetComponent<Tile>().SetCoordinates(x, y);
                if (mapTile[x, y] == 0)
                    Instantiate(obstacle, pos, Quaternion.identity, transform);
            }
        }
    }

    public void SetMapTile(int x, int y, int val)
    {
        mapTile[x, y] = val;
        Debug.Log("ora il valore " + mapTile[x, y]);
    }

}
