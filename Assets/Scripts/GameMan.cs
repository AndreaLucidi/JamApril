using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour
{
    #region Properties
    // PUBLIC
    public int XofP1;
    public int YofP1;
    public GameObject startGrid;
    [Header("Players to instantiate")]
    public GameObject p1;
    public GameObject p2;
    // PRIVATE
    [Header("Grids to create")]
    [SerializeField] private int gridsNumber = 2;
    [SerializeField] private float offset;
    private List<GameObject> grids;
    #endregion

    void Awake()
    {
        grids = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startGrid.GetComponent<GridGenerator>().CreateGrid();
        DuplicateGrid();
        //startGrid.SetActive(false);
        Destroy(startGrid);
        StartCoroutine(PlaceAgents());
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
            newGrid.name = "Grid_" + (i-1).ToString();
            newGrid.GetComponent<Grid>().CreateGrid();
            newGrid.GetComponent<GridGenerator>().DestroyTileUnderObstacle();
            grids.Add(newGrid);
        }
    }

    IEnumerator PlaceAgents()
    {
        yield return new WaitForSeconds(1.2f);
        int count = 1;
        foreach (GameObject g in grids)
        {
            if(count == 1)
                g.GetComponent<GridGenerator>().SetAgents(p1);
            else
                g.GetComponent<GridGenerator>().SetAgents(p2);
            count++;
        }
    }
}
