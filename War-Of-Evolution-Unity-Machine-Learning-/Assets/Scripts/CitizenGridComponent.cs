using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenGridComponent : MonoBehaviour
{
    private GridForPathFinding gridForPathFinding;
    private Node temporaryNode;

    private void Start()
    {
        gridForPathFinding = GameObject.FindObjectOfType<GridForPathFinding>();
    }

    private void Update()
    {
        Node citizensNode = gridForPathFinding.NodeFromWorldPosition(transform.position);
        if (temporaryNode != null && !temporaryNode.Equals(citizensNode))
        {
            temporaryNode.isEmpty = true;
        }
        citizensNode.isEmpty = false;
        temporaryNode = citizensNode;
    }
}
