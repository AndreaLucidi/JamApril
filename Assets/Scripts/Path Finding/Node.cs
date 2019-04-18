using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {

	public bool walkable;
	public Vector3 worldPosition;

	public int gridX;
	public int gridY;
	public int movementPenalty;

	public int gCost;
	public int hCost;

	public Node parent;

	private int heapIndex;

	/**
	 * Constructor
	 */
	public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY, int _penalty){

		this.walkable = _walkable;
		this.worldPosition = _worldPosition;
		this.gridX = _gridX;
		this.gridY = _gridY;
		this.movementPenalty = _penalty;
	}


	public int fCost{
		get {
			return gCost + hCost;
		}
	}

	public int HeapIndex {

		get {
			return this.heapIndex;
		}

		set {
			heapIndex = value;
		}
	}


	public int CompareTo(Node nodeToCompare){

		int compare = fCost.CompareTo(nodeToCompare.fCost);

		if(compare == 0)
			compare = hCost.CompareTo(nodeToCompare.hCost);

		return -compare;
	}
}
