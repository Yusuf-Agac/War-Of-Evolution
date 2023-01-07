using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Unit : MonoBehaviour
{
    const float pathUpdateMoveThreshold = 0.5f;
    public float pathUpdateMoveThresholdForPlayer = 0.5f;

    private Vector3 target;
    private Rigidbody rb;
    private CitizenBehaviors citizenBehaviors;
    public float speed;
    public int turnDst = 5;
    public int stoppingDst = 10;
    private Path path;
    bool followingPathThreshold = false;
    private GridForPathFinding gridForPathFinding;
    private CityManagement cityManagement;

    private void Start()
    {
        citizenBehaviors = GetComponent<CitizenBehaviors>();
        cityManagement = FindObjectOfType<CityManagement>();
        rb = GetComponent<Rigidbody>();
        gridForPathFinding = GameObject.FindObjectOfType<GridForPathFinding>();
        RandomTargetPosition();
        StartCoroutine(UpdatePath());
    }

    private void OnPathFound(Vector3[] waypoints, bool successful)
    {
        if (this != null && successful)
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
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            if ((target - targetPosOld).sqrMagnitude > sqrMoveThreshold || (followingPathThreshold && (target - transform.position).sqrMagnitude > sqrMoveThreshold))
            {
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
                    citizenBehaviors.GainPrizeForArriving();
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
                        RandomTargetPosition();
                        citizenBehaviors.GainPrizeForArriving();
                    }
                }
                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
            }
            yield return null;
        }
    }

    public void RandomTargetPosition()
    {
        target = gridForPathFinding.GetRandomWalkable().worldPosition + Vector3.up;
    }

    IEnumerator CheckIfUnitNodeWalkable()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4f, 10f));
            if (gridForPathFinding.NodeFromWorldPosition(transform.position).walkable == false)
            {
                List<Node> neighbours = gridForPathFinding.GetNeighboursOfNode(gridForPathFinding.NodeFromWorldPosition(transform.position));
                foreach (Node node in neighbours)
                {
                    if (node.walkable)
                    {
                        transform.position = node.worldPosition;
                        break;
                    }
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target, 0.2f);
        if (path != null)
        {
            path.DrawWithGizmos(cityManagement);
        }
    }
}
