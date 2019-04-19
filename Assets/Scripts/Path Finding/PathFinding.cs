using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class PathFinding : MonoBehaviour {
	
	private Grid grid;


	void Awake(){
		
		this.grid = GetComponent<Grid> ();
	}


	/**
	 * Find the path from starting position to target position.
	 */
	public Vector3[] PathFind(Vector3 start, Vector3 target) {

		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

		Node startNode = grid.NodeFromWorldPoint(start);
		Node targetNode = grid.NodeFromWorldPoint(target);

		if (startNode.walkable && targetNode.walkable) {
			Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);

			while (openSet.Count > 0) {
				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				if (currentNode == targetNode) {
					pathSuccess = true;
					break;
				}

				foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
					if (!neighbour.walkable || closedSet.Contains(neighbour)) {
						continue;
					}

					int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;

						if (!openSet.Contains (neighbour))
							openSet.Add (neighbour);
						else
							openSet.UpdateItem (neighbour);
					}
				}
			}
		}

		if (pathSuccess) {
			waypoints = RetracePath (startNode, targetNode);
			pathSuccess = waypoints.Length > 0;
		}

        return waypoints;
	}


	/**
	 * Return distance between two nodes, with a cost for diagonal path of 14 and 10 for straight moves.
	 */
	private int GetDistance(Node nodeA, Node nodeB){

		int dstX = Mathf.Abs (nodeA.gridX -  nodeB.gridX);
		int dstY = Mathf.Abs (nodeA.gridY - nodeB.gridY);

		if(dstX > dstY)
			return 14*dstY + 10 * (dstX-dstY);
		else
			return 14*dstX + 10 * (dstY-dstX);
	}

	/**
	 * Retrace the path find previously.
	 */
	private Vector3[] RetracePath(Node startNode, Node endNode) {
		
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		Vector3[] waypoints = SimplifyPath(path);
		Array.Reverse(waypoints);

		return waypoints;

	}


	/**
	 * Simplify the path eliminating the node in the same direction.
	 */
	private Vector3[] SimplifyPath(List<Node> path) {
		
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;

		for (int i = 1; i < path.Count; i ++) {
			Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX,path[i-1].gridY - path[i].gridY);
			if (directionNew != directionOld) {
				waypoints.Add(path[i].worldPosition);
			}
			directionOld = directionNew;
		}

		return waypoints.ToArray();
	}
}
