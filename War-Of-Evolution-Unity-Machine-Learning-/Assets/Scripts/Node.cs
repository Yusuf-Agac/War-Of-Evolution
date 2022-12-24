using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  Node : Heap<Node>.IHeapItem<Node>
{
    public Node parentNode;
    
    public Vector3 worldPosition;
    public int gridX, gridY;
    
    public bool walkable;
    public bool isEmpty;
    
    public float gCost;
    public float hCost;

    private int heapIndex;
    
    public Node(Vector3 worldPosition, bool walkable, int gridX, int gridY)
    {
        this.worldPosition = worldPosition;
        this.walkable = walkable;
        this.isEmpty = true;
        this.gridX = gridX;
        this.gridY = gridY;
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
