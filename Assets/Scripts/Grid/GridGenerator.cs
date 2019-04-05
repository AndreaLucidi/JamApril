using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    #region Properties
    [Header("Grid properties")]
    public float cellSize = 1.0f;
    public int rows;
    public int cols;
    [Header("Object for tiling")]
    public GameObject tile;
    #endregion

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
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                Vector3 pos = transform.position + (new Vector3(x, 0.0f, y) * cellSize);
                GameObject tileObj = Instantiate(tile, pos, Quaternion.identity, transform);
                tileObj.name = "Tile_" + (x+1).ToString() + "_" + (y+1).ToString();
            }
        }
    }


}
