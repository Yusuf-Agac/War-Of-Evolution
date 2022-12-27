using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class GridForPathFinding : MonoBehaviour
{
    [Range(0f, 0.9f)]
    public float debugCubeSpaceSize;
    public bool displayGrid;

    public TerrainType[] walkableRegions;
    public LayerMask unwalkableMask;
    LayerMask walkableMask;
    public Dictionary<float, float> walkableRegionDictionary = new Dictionary<float, float>();

    public float obstacleProximityPenalty = 10;
    private float penaltyMin = float.MaxValue;
    private float penaltyMax = float.MinValue;
    
    public Vector2 gridWorldSize;
    public float nodeRadius;
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
                if (walkable)
                {
                    Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                    if (Physics.Raycast(ray, out RaycastHit hit, 100, walkableMask))
                    {
                        walkableRegionDictionary.TryGetValue(hit.collider.gameObject.layer, out penalty);
                    }
                }
                if (!walkable) { penalty = obstacleProximityPenalty; }
                grid[x, y] = new Node(worldPoint, walkable, x, y, penalty);
            }
        }
        
        BluryPenaltyMap(2);
    }

    private void BluryPenaltyMap(int blurSize)
    {
        int kernelSize = (blurSize * 2) + 1;
        int kernelExtents = (kernelSize - 1) / 2;
        float[,] penaltiesHorizontalPass = new float[gridSizeX, gridSizeY];
        float[,] penaltiesVerticalPass = new float[gridSizeX, gridSizeY];

        for (int y = 0; y < gridSizeY; y++)
        {
            //for first row
            for (int x = -kernelExtents; x <= kernelExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                penaltiesHorizontalPass[0, y] += grid[sampleX, y].penalty;
            }
            
            for (int x = 1; x < gridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX-1);
                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[removeIndex, y].penalty + grid[addIndex, y].penalty;
            }
        }
        
        for (int x = 0; x < gridSizeX; x++)
        {
            //for first column
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernelExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
            }
            
            float newBlurredPenalty = penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize);
            grid[x, 0].penalty = newBlurredPenalty;

            for (int y = 1; y < gridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY-1);
                penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
                newBlurredPenalty = penaltiesVerticalPass[x, y] / (kernelSize * kernelSize);
                grid[x, y].penalty = newBlurredPenalty;
                
                if (penaltyMax < newBlurredPenalty) { penaltyMax = newBlurredPenalty; }
                if (penaltyMin > newBlurredPenalty) { penaltyMin = newBlurredPenalty; }
            }
        }
    }

    public Node NodeFromWorldPosition(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + (gridWorldSize.x / 2)) / gridWorldSize.x;
        float percentY = (worldPosition.z + (gridWorldSize.y / 2)) / gridWorldSize.y;
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
                Gizmos.color =  Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, node.penalty));
                //Gizmos.color = (node.walkable) ? Gizmos.color : Color.red;
                Gizmos.color = (!node.isEmpty) ? Color.blue : Gizmos.color;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - debugCubeSpaceSize));
            }
        }
    }
}
