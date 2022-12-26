using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    public float speed;
    private Vector3[] path;
    private int currentIndex;

    private void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound);
    }

    private void OnPathFound(Vector3[] newPath, bool successful)
    {
        if (successful)
        {
            path = newPath;
            StopCoroutine(nameof(MoveOnPath));
            StartCoroutine(nameof(MoveOnPath));
        }
    }

    IEnumerator MoveOnPath()
    {
        Vector3 currentWaypoint = path[0];
        
        while (true)
        {
            if (currentWaypoint == transform.position)
            {
                currentIndex++;
                if (currentIndex >= path.Length)
                {
                    yield break;
                }

                currentWaypoint = path[currentIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = currentIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one / 5);
                if (i == currentIndex)
                {
                    Gizmos.DrawLine(transform.position, path[currentIndex]);
                }
                else if(i > 0)
                {
                    Gizmos.DrawLine(path[i-1], path[i]);
                }
            }
        }
    }
}
