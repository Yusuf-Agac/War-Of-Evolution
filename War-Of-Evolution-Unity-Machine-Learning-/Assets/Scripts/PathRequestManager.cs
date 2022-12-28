using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.XPath;
using Unity.VisualScripting;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    private Pathfinding pathfinding;
    private Queue<PathResult> results = new Queue<PathResult>();
    private static PathRequestManager instance;

    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    private void Update()
    {
        if (results.Count > 0)
        {
            int itemsInQueue = results.Count;
            lock (results)
            {
                for (int i = 0; i < itemsInQueue; i++)
                {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.success);
                }
            }
        }
    }

    public static void RequestPath(PathRequest request)
    {
        ThreadStart threadStart = delegate { instance.pathfinding.FindPath(request, instance.FinishedFindPath); };
        threadStart.Invoke();
    }

    public void FinishedFindPath(PathResult result)
    {
        lock (results)
        {
            results.Enqueue(result);
        }
    }
}
public struct PathResult
{
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;
        
    public PathResult(Vector3[] path, bool success, PathRequest request)
    {
        this.path = path;
        this.success = success;
        this.callback = request.callback;
    }
}

public struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[], bool> callback;

    public PathRequest(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        this.pathStart = pathStart;
        this.pathEnd = pathEnd;
        this.callback = callback;
    }
}
