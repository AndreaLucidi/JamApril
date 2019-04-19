using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Properties
    // PUBLIC
    public float speed = 10.0f;
    public float journeyLength = 10.0f;
    public float timeToTarget = 5.0f;
    private Vector3 target;
    public PathFinding pathObj;
    public GameObject playerToFollow;
    // PRIVATE
    private float startTime;
    private Vector3 prevNode;
    private List<Vector3> pathResult;
    #endregion

    void Awake()
    {
        prevNode = Vector3.positiveInfinity;
        pathResult = new List<Vector3>();
    }

    // Start is called before the first frame update
    void Start()
    {
        pathObj = GameObject.Find("Grid_1").GetComponent<PathFinding>();
        playerToFollow = GameObject.FindGameObjectWithTag("P1");
        StartCoroutine(ChangeTarget());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (pathObj)
        {
            int count = pathResult.Count;
            if (count > 0)
            {
                //if (prevNode == Vector3.positiveInfinity)
                prevNode = transform.position;
                
                if (PerformMovement(prevNode, pathResult[0]))
                {
                    prevNode = pathResult[0];
                    pathResult.RemoveAt(0);
                    startTime = Time.time;

                    if (count == 1)
                    {
                        prevNode = Vector3.positiveInfinity;
                    }
                }
            }
        }
    }

    private bool LerpMovement(Vector3 start, Vector3 target)
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(transform.position, target, fracJourney);

        if (distCovered >= journeyLength)
            return true;
        else
            return false;
    }

    private bool PerformMovement(Vector3 start, Vector3 target)
    {
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(start, target, step);

        if (Vector3.Distance(transform.position, target) < 0.5f)
            return true;
        else
            return false;
    }

    private void PathFind(Vector3 start, Vector3 target)
    {
        Debug.Log("inizio "+start+" fine "+target);
        Vector3[] myPath = pathObj.PathFind(start, target);

        pathResult.Clear();

        if (myPath.Length > 0)
        {
            foreach (Vector3 v in myPath)
            {
                pathResult.Add(v);
            }
        }

    }

    void OnDrawGizmos()
    {
        /*
        if (pathResult.Count > 0)
        {
            foreach (Vector3 v in pathResult)
            {
                Gizmos.DrawSphere(v, 0.5f);
            }
        }
        */
    }

    IEnumerator ChangeTarget()
    {
        yield return new WaitForSeconds(0.2f);
        target = playerToFollow.transform.position;
        PathFind(transform.position, target);

        while (true)
        {
            yield return new WaitForSeconds(timeToTarget);
            target = playerToFollow.transform.position;
            PathFind(transform.position, target);
            Debug.Log("Cerco");
        }
    }
}
