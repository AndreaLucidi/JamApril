﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line {

	const float verticalLineGradient = 1e5f;

	private float gradient;
	private float y_intercept;
	private float gradientPerpendicular;
	private bool approachSide;
	private Vector2 pointOnLine_1;
	private Vector2 pointOnLine_2;

	/**
	 * Constructor.
	 */
	public Line(Vector2 pointOnLine, Vector2 pointPerpendicularToLine) {

		float dx = pointOnLine.x - pointPerpendicularToLine.x;
		float dy = pointOnLine.y - pointPerpendicularToLine.y;

		if (dx == 0)
			this.gradientPerpendicular = verticalLineGradient;
		else
			this.gradientPerpendicular = dy / dx;

		if (gradientPerpendicular == 0)
			gradient = verticalLineGradient;
		else
			gradient = -1 / gradientPerpendicular;

		y_intercept = pointOnLine.y - gradient * pointOnLine.x;
		pointOnLine_1 = pointOnLine;
		pointOnLine_2 = pointOnLine + new Vector2 (1, gradient);

		approachSide = false;
		approachSide = GetSide (pointPerpendicularToLine);
	}


	/**
	 * Given a point this function return the side (true or false) of the point.
	 */
	private bool GetSide(Vector2 p){

		return (p.x - pointOnLine_1.x) * (pointOnLine_2.y - pointOnLine_1.y) > (p.y - pointOnLine_1.y) * (pointOnLine_2.x - pointOnLine_1.x);
	}


	public bool HasCrossedLine(Vector2 p) {

		return GetSide (p) != approachSide;
	}


	/**
	 * Calculate the distance from point.
	 */
	public float DistanceFromPoint(Vector2 p){

		float yIntersectPerpendicular = p.y - gradientPerpendicular * p.x;
		float intersectX = (yIntersectPerpendicular - y_intercept) / (gradient - gradientPerpendicular);
		float intersectY = gradient * intersectX + y_intercept;

		return Vector2.Distance (p, new Vector2(intersectX, intersectY));
	}

	/**
	 * Draw a 3d versione of the line with Gizmos.
	 */
	public void DrawWithGizmos(float length) {

		Vector3 lineDir = new Vector3 (1, 0, gradient).normalized;
		Vector3 lineCentre = new Vector3 (pointOnLine_1.x, 0, pointOnLine_1.y) + Vector3.up;
		Gizmos.DrawLine (lineCentre - lineDir * length / 2f, lineCentre + lineDir * length / 2f);
	}
}
