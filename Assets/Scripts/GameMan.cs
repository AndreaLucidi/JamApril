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
    public static GameMan instance;
    // PRIVATE
    [SerializeField] private float obstaclePercentage = 0.3f;
    private int[,] mapTile;
    #endregion

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        mapTile = new int[gridLeft.rows, gridLeft.cols];
        GenerateObstacle();
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
                    mapTile[x, y] = 0;
                else
                    mapTile[x, y] = 1;
            }
        }
    }

    public int IsObstacle(int x, int y)
    {
        return mapTile[x, y];
    }
}
