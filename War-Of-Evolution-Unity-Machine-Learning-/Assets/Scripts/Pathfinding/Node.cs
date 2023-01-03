using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  Node : Heap<Node>.IHeapItem
{
    public Node parentNode;
    
    public Vector3 worldPosition;
    public int gridX, gridY;
    
    public bool walkable;
    public bool isEmpty;
    
    public float gCost;
    public float hCost;
    public float penalty;

    private int heapIndex;
    
    public Node(Vector3 worldPosition, bool walkable, int gridX, int gridY, float penalty)
    {
        this.worldPosition = worldPosition;
        this.walkable = walkable;
        this.isEmpty = true;
        this.gridX = gridX;
        this.gridY = gridY;
        this.penalty = penalty;
    }

    public int HeapIndex
    {
        get => heapIndex;
        set => heapIndex = value;
    }
    
    public int CompareTo(Node other)
    {
        int compare = fCost.CompareTo(other.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }

        return -compare;
    }
    
    public float fCost => gCost + hCost;
}
