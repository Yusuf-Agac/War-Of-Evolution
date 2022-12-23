using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  Node
{
    public Node parentNode;
    
    public Vector3 worldPosition;
    public int gridX, gridY;
    
    public bool walkable;
    public bool isEmpty;
    
    public float gCost;
    public float hCost;
    
    public Node(Vector3 worldPosition, bool walkable, int gridX, int gridY)
    {
        this.worldPosition = worldPosition;
        this.walkable = walkable;
        this.isEmpty = true;
        this.gridX = gridX;
        this.gridY = gridY;
    }
    
    public float fCost {
        get {
            return gCost + hCost;
        }    
    }
}
