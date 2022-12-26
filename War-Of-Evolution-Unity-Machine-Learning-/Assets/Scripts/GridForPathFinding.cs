using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class GridForPathFinding : MonoBehaviour
{
    public TerrainType[] walkableRegions;
    public LayerMask unwalkableMask;
    LayerMask walkableMask;
    public Dictionary<float, float> walkableRegionDictionary = new Dictionary<float, float>();
    public bool displayGrid;
    
    public Vector2 gridWorldSize;
    public float nodeRadius;
    [Range(0.1f, 0.9f)]
    public float debugCubeSpaceSize;
    Node[,] grid;
    
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    public int gridSize => gridSizeX * gridSizeY;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        foreach (var region in walkableRegions)
        {
            //Bitwise Operation
            walkableMask.value |= region.terrainLayerMask.value;
            walkableRegionDictionary.Add(Mathf.Log(region.terrainLayerMask.value, 2), region.terrainPenalty);
        }
        
        CreateGrid();
    }
    
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        var position = transform.position;
        Vector3 gridsLeftUpPoint = new Vector3(position.x - gridWorldSize.x / 2, 0, position.z - gridWorldSize.y / 2);
        
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = gridsLeftUpPoint + new Vector3(x * nodeDiameter + nodeRadius, 0, y * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);

                float penalty = 0;

                Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, walkableMask))
                {
                    walkableRegionDictionary.TryGetValue(hit.collider.gameObject.layer, out penalty);
                }
                
                grid[x, y] = new Node(worldPoint, walkable, x, y, penalty);
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

    public List<Node> GetNeighboursOfNode(Node node)
    {
        List<Node> neighbours = new List<Node>();
        
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(y == 0 & x == 0) {continue;}

                int nodeX = x + node.gridX;
                int nodeY = y + node.gridY;
                if (nodeX >= 0 && nodeX < gridSizeX && nodeY >= 0 && nodeY < gridSizeY)
                {
                    neighbours.Add(grid[nodeX, nodeY]);
                }
            }
        }
        
        return neighbours;
    }
    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainLayerMask;
        public float terrainPenalty;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 0, gridWorldSize.y));
        if (grid != null && displayGrid)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                Gizmos.color = (!node.isEmpty) ? Color.blue : Gizmos.color;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - debugCubeSpaceSize));
            }
        }
    }
}
