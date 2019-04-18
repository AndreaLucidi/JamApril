using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	const float minPathUpdateTime = 0.2f;
	const float pathUpdateMoveThreshold = 10f;

	public Vector3 target = Vector3.zero;
	public float speed = 20;
	public float turnSpeed = 3.0f;
	public float turnDst = 5;
	public float stoppingDst = 10;

	private Path path;
	private bool canMove = true;


	void Start(){

		StartCoroutine (UpdatePath ());
	}


	public void OnDrawGizmos() {

		if (path != null)
			path.DrawWithGizmos ();
	}


	public void OnPathFound(Vector3[] waypoint, bool pathSuccessful) {
		
		if (pathSuccessful) {
			path = new Path (waypoint, transform.position, turnDst, stoppingDst);
			StopCoroutine ("FollowPath");
			StartCoroutine ("FollowPath");
		}
	}


	IEnumerator UpdatePath(){

		if (Time.timeSinceLevelLoad < 0.3f)
			yield return new WaitForSeconds (1.0f);
		
		if (canMove)
			PathRequestManager.RequestPath (new PathRequest (transform.position, target, OnPathFound));

		float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
		Vector3 targetPosOld = target;

		while (true) {
			yield return new WaitForSeconds (minPathUpdateTime);
			if(canMove && targetPosOld != target){   //  (target - targetPosOld).sqrMagnitude > sqrMoveThreshold if the target can move, but we are using island!
				PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound));
				targetPosOld = target;
			}
		}
	}


	IEnumerator FollowPath() {
		
		bool followingPath = true;
		int pathIndex = 0;
		if(path.lookPoints.Length > 0)
			transform.LookAt (path.lookPoints [0]);
		
		float speedPercent = 1;

		while (followingPath && path != null) {
			Vector2 pos2D = new Vector2 (transform.position.x, transform.position.z);

			while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D)) {
				if (pathIndex == path.finishLineIndex || pathIndex > path.finishLineIndex) {
					followingPath = false;
					break;
				} else
					pathIndex++;
			}

			if(this.canMove && followingPath && path != null && Vector3.Distance(transform.position, target) > stoppingDst){
				
				if (pathIndex >= path.slowDownIndex && stoppingDst > 0) {
					speedPercent = Mathf.Clamp01 (path.turnBoundaries [path.finishLineIndex].DistanceFromPoint (pos2D) / stoppingDst);
					if (speedPercent < 0.1f)
						followingPath = false;
				}

				Quaternion targetRotation = Quaternion.LookRotation (path.lookPoints[pathIndex] - transform.position);
				transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
				transform.Translate (Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
			}

			yield return null;

		}
	}


}