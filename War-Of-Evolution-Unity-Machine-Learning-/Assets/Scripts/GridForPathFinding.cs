using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridForPathFinding : MonoBehaviour
{  
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    [Range(0.1f, 0.9f)]
    public float debugCubeSpaceSize;
    Node[,] grid;
    
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }
    
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 gridsLeftUpPoint = new Vector3(transform.position.x - gridWorldSize.x / 2, 0, transform.position.z - gridWorldSize.y / 2);
        
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = gridsLeftUpPoint + new Vector3(x * nodeDiameter + nodeRadius, 0, y * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
                grid[x, y] = new Node(worldPoint, walkable);
            }
        }
    }

    public Node NodeFromWorldPosition(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridSizeX / 2) / gridSizeX;
        float percentY = (worldPosition.z + gridSizeY / 2) / gridSizeY;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 0, gridWorldSize.y));
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.color = (!n.isEmpty) ? Color.blue : Gizmos.color;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - debugCubeSpaceSize));
            }
        }
    }
}
