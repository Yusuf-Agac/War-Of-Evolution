using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    const float pathUpdateMoveThreshold = 0.5f;
    public float pathUpdateMoveThresholdForPlayer = 0.5f;

    private Vector3 target;
    private Rigidbody rb;
    public float speed;
    public int turnDst = 5;
    public int stoppingDst = 10;
    private Path path;
    bool followingPathThreshold = false;
    private GridForPathFinding gridForPathFinding;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gridForPathFinding = GameObject.FindObjectOfType<GridForPathFinding>();
        RandomTargetPosition();
        StartCoroutine(UpdatePath());
    }

    private void OnPathFound(Vector3[] waypoints, bool successful)
    {
        if (successful)
        {
            path = new Path(waypoints, transform.position, turnDst, stoppingDst);
            StopCoroutine(nameof(FollowPath));
            StartCoroutine(nameof(FollowPath));
        }
    }
    
    IEnumerator UpdatePath()
    {
        if(Time.timeSinceLevelLoad < 0.3f)
        {
            yield return new WaitForSeconds(0.3f);
        }
        PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound));
        
        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target;
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            if(followingPathThreshold){Debug.Log("SqrMagnitude " + (target - transform.position).sqrMagnitude);}
            
            if ((target - targetPosOld).sqrMagnitude > sqrMoveThreshold || (followingPathThreshold && (target - transform.position).sqrMagnitude > sqrMoveThreshold))
            {
                Debug.Log("Requesting new path " + transform.name);
                PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound));
                targetPosOld = target;
            }
        }
    }
    
    IEnumerator FollowPath()
    {
        bool followingPath = true;
        followingPathThreshold = false;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);
        float speedPercent = 1;
        while (followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    followingPathThreshold = true;
                    Debug.Log("Name " + transform.name + " path is over");
                    RandomTargetPosition();
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }
            if (followingPath)
            {
                if (pathIndex >= path.slowDownIndex && turnDst > 0)
                {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / turnDst);
                    if (speedPercent < 0.05f)
                    {
                        followingPath = false;
                        followingPathThreshold = true;
                        Debug.Log("Name " + transform.name + " path is over");
                        RandomTargetPosition();
                    }
                }
                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                //transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
                transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
            }
            yield return null;
        }
    }

    public void RandomTargetPosition()
    {
        target = gridForPathFinding.GetRandomWalkable().worldPosition + Vector3.up;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target, 0.2f);
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }
}
