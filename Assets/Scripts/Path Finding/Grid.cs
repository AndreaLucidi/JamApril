using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	public bool displayGizmos;
	public bool displayOnlyPath;
	public Transform player;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public List<Node> path;
	public TerrainType[] walkableRegions;
	public int proximityObstaclePenalty = 10;

	private Node[,] grid;
	private float nodeDiameter;
	private int gridSizeX;
	private int gridsizeY;
	private int penaltyMax;
	private int penaltyMin;
	private LayerMask walkableMask;
	private Dictionary<int, int> walkableRegionsDic;

	void Awake(){

		this.nodeDiameter = 2 * this.nodeRadius;
		this.gridSizeX = Mathf.RoundToInt(gridWorldSize.x/this.nodeDiameter);
		this.gridsizeY = Mathf.RoundToInt(gridWorldSize.y/this.nodeDiameter);
		this.penaltyMax = int.MinValue;
		this.penaltyMin = int.MaxValue;

		walkableRegionsDic = new Dictionary<int, int> ();

		foreach (TerrainType region in walkableRegions) {
			walkableMask.value += region.terrainMask.value;
			//print (walkableMask.value);
			walkableRegionsDic.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
		}

		//this.CreateGrid();
	}

	void Start() {

		//this.CreateGrid();
	}

	/**
	 * Return the size of the grid.
	 */
	public int MaxSize{
		get {
			return gridSizeX * gridsizeY;
		}
	}

	/**
	 * Create grid.
	 */
	public void CreateGrid(){
		
		this.grid = new Node[this.gridSizeX, this.gridsizeY];

        Vector3 worldBottomLeft = transform.position; //- Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;
        
		for(int x = 0; x < this.gridSizeX; x++){
			for(int y = 0; y < this.gridsizeY; y++){
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * this.nodeDiameter + this.nodeRadius) + Vector3.forward * (y * this.nodeDiameter + this.nodeRadius) + Vector3.up * 0;
				bool walkable = !(Physics.CheckSphere(worldPoint, this.nodeRadius, this.unwalkableMask, QueryTriggerInteraction.Ignore));

				int movementPenalty = 0;

				Ray ray = new Ray (worldPoint + Vector3.up * 50, Vector3.down);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit, 100, walkableMask))
					walkableRegionsDic.TryGetValue (hit.collider.gameObject.layer, out movementPenalty);
				
				if (!walkable)
					movementPenalty += proximityObstaclePenalty;
				
				grid[x,y] = new Node(walkable, worldPoint, x, y, movementPenalty);
			}
		}

		BlurPenaltyMap (3);
	}





	/**
	 * Retrieve the node from world position.
	 */
	public Node NodeFromWorldPoint(Vector3 worldPosition){

        /*
		float percentX = Mathf.Clamp01((worldPosition.x + gridWorldSize.x/2)/gridWorldSize.x);
		float percentY = Mathf.Clamp01((worldPosition.z + gridWorldSize.y/2)/gridWorldSize.y);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridsizeY - 1) * percentY);
        */
        float distance = 100.0f;
        int indexX = 0;
        int indexY = 0;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridsizeY; y++)
            {
                Vector3 pos = grid[x, y].worldPosition;
                float d = Vector3.Distance(pos, worldPosition);
                if (d < distance)
                {
                    indexX = x;
                    indexY = y;
                    distance = d;
                }
            }
        }



        return this.grid [indexX, indexY];

	}


	/**
	 * Retrieve neighbours node of node.
	 */
	public List<Node> GetNeighbours(Node node){

		List<Node> neighbours = new List<Node> ();

		for (int x = -1; x <= 1; x++)
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridsizeY)
					neighbours.Add (grid[checkX,checkY]);
			}

		return neighbours;
	}


	/**
	 * Blur map method for smooth weights.
	 */
	void BlurPenaltyMap (int blurSize){

		int kernelSize = blurSize * 2 + 1;
		int kernelExtents = (kernelSize - 1)/2;

		int[,] penaltiesHorPass = new int[gridSizeX, gridsizeY];
		int[,] penaltiesVerPass = new int[gridSizeX, gridsizeY];

		for (int y = 0; y < gridsizeY; y++) {
			for (int x = -kernelExtents; x <= kernelExtents; x++) {
				int sampleX = Mathf.Clamp (x, 0, kernelExtents);
				penaltiesHorPass [0,y] += grid [sampleX, y].movementPenalty;
			}

			for (int x = 1; x < gridSizeX; x++) {
				int removeIndex = Mathf.Clamp (x - kernelSize -1, 0, gridSizeX);
				int addIndex = Mathf.Clamp (x + kernelExtents, 0, gridSizeX -1);

				penaltiesHorPass[x,y] = penaltiesHorPass[x-1,y] - grid[removeIndex,y].movementPenalty + grid[addIndex,y].movementPenalty;
			}
		}

		for (int x = 0; x < gridSizeX; x++) {
			for (int y = -kernelExtents; y <= kernelExtents; y++) {
				int sampleY = Mathf.Clamp (y, 0, kernelExtents);
				penaltiesVerPass [x,0] += penaltiesHorPass[x, sampleY];
			}

			int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerPass[x,0] / (kernelSize * kernelSize));
			grid [x, 0].movementPenalty = blurredPenalty;

			for (int y = 1; y < gridsizeY; y++) {
				int removeIndex = Mathf.Clamp (y - kernelSize -1, 0, gridsizeY);
				int addIndex = Mathf.Clamp (y + kernelExtents, 0, gridsizeY -1);

				penaltiesVerPass[x,y] = penaltiesVerPass[x,y-1] - penaltiesHorPass[x,removeIndex] + penaltiesHorPass[x,addIndex];
				blurredPenalty = Mathf.RoundToInt((float)penaltiesVerPass[x,y] / (kernelSize * kernelSize));
				grid [x, y].movementPenalty = blurredPenalty;

				if (blurredPenalty > penaltyMax)
					penaltyMax = blurredPenalty;

				if (blurredPenalty < penaltyMin)
					penaltyMin = blurredPenalty;
			}
		}
	}

	/**
	 * Call for drawing Gizmos.
	 */
	void OnDrawGizmos(){

		if (this.displayGizmos) {
			//Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
			if (grid != null && displayOnlyPath) {
				foreach (Node n in grid) {
					Gizmos.color = Color.Lerp (Color.cyan, Color.black, Mathf.InverseLerp (penaltyMin, penaltyMax, n.movementPenalty));
					Gizmos.color = (n.walkable) ? Gizmos.color : Color.red;
					Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter));
				}
			}
		}
	}



	[System.Serializable]
	public class TerrainType {

		public LayerMask terrainMask;
		public int terrainPenalty;
	}
}
