using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private GridForPathFinding gridForPathFinding;

    public Transform seeker, target;

    private void Awake()
    {
        gridForPathFinding = GameObject.FindObjectOfType<GridForPathFinding>();
    }

    private void Update()
    {
        FindPath(seeker.position, target.position);
    }

    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = gridForPathFinding.NodeFromWorldPosition(startPos);
        Node targetNode = gridForPathFinding.NodeFromWorldPosition(targetPos);

        Heap<Node> openSet = new Heap<Node>(gridForPathFinding.gridSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirstItem();
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                ReTrace(startNode, targetNode);
                return;
            }

            foreach (Node neighbourNode in gridForPathFinding.GetNeighboursOfNode(currentNode))
            {
                if(!neighbourNode.walkable || closedSet.Contains(neighbourNode)){continue;}

                float CostOfGoingToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbourNode);
                if (CostOfGoingToNeighbour < currentNode.gCost || !openSet.Contains(neighbourNode))
                {
                    neighbourNode.gCost = CostOfGoingToNeighbour;
                    neighbourNode.hCost = GetDistance(neighbourNode, targetNode);
                    neighbourNode.parentNode = currentNode;
                    if(!openSet.Contains(neighbourNode)){openSet.Add(neighbourNode);}
                }
            }
        }
    }

    public void ReTrace(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node currentNode = end;
        while (currentNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
            if(currentNode == start){break;}
        }
        path.Reverse();

        gridForPathFinding.path = path;
    }

    public float GetDistance(Node nodeA, Node nodeB)
    {
        Vector3 distance = nodeA.worldPosition - nodeB.worldPosition;
        float distanceX = Mathf.Abs(distance.x);
        float distanceY = Mathf.Abs(distance.z);

        if (distanceX > distanceY)
        {
            return distanceY * 14f + ((distanceX - distanceY) * 10f);
        }
        return distanceX * 14f + ((distanceY - distanceX) * 10f);
    }
}
