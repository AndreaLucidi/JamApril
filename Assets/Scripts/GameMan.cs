﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour
{
    #region Properties
    // PUBLIC
    public int XofP1;
    public int YofP1;
    public GameObject startGrid;
    // PRIVATE
    [Header("Grids to create")]
    [SerializeField] private int gridsNumber = 2;
    [SerializeField] private float offset;
    #endregion

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        startGrid.GetComponent<GridGenerator>().CreateGrid();
        DuplicateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DuplicateGrid()
    {
        if (gridsNumber < 2)
            return;

        for (int i = 2; i <= gridsNumber; i++)
        {
            Vector3 pos = new Vector3(offset, 0.0f, 0.0f);
            GameObject newGrid = Instantiate(startGrid, startGrid.transform.position + (pos * i), Quaternion.identity);
            newGrid.name = "Grid_" + i.ToString();
        }
    }
}
