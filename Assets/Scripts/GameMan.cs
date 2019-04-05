using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour
{
    #region Properties
    // PUBLIC
    public int XofP1;
    public int YofP1;
    public GridGenerator gridLeft;
    public GridGenerator gridRight;
    // PRIVATE
    [SerializeField] private float obstaclePercentage = 0.3f;
    public int[,] mapTile;
    #endregion

    void Awake()
    {
        mapTile = new int[gridLeft.rows, gridLeft.cols];
        GenerateObstacle();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateObstacle()
    {
        float generate = 0.0f;
        
        for (int x = 0; x < gridLeft.rows; x++)
        {
            for (int y = 0; y < gridLeft.cols; y++)
            {
                generate = Random.Range(0.0f, 1.0f);
                if (generate < obstaclePercentage)
                {
                    gridLeft.SetMapTile(x, y, 0);
                    gridRight.SetMapTile(x, y, 0);
                } else
                {
                    gridLeft.SetMapTile(x, y, 1);
                    gridRight.SetMapTile(x, y, 1);
                }
            }
        }
    }

    public int IsObstacle(int x, int y)
    {
        Debug.Log("dbfsdkj "+mapTile[0,0]);
        return mapTile[1, 1];
    }
}
