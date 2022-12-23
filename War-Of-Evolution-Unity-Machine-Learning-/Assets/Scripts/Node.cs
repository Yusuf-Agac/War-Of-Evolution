using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  Node
{
    public Vector3 worldPosition;
    public bool walkable;
    public bool isEmpty;
    public Node(Vector3 worldPosition, bool walkable)
    {
        this.worldPosition = worldPosition;
        this.walkable = walkable;
        this.isEmpty = true;
    }
}
