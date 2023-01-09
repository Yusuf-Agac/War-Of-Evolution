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
            if (!temporaryNode.isEmpty)
            {
                temporaryNode.penalty -= 400;
                List<Node> nodes = gridForPathFinding.GetNeighboursOfNode(temporaryNode);
                foreach (var node in nodes)
                {
                    node.penalty -= 125;
                }
            }
            temporaryNode.isEmpty = true;
        }

        if (citizensNode.isEmpty)
        {
            citizensNode.penalty += 400;
            List<Node> nodes = gridForPathFinding.GetNeighboursOfNode(citizensNode);
            foreach (var node in nodes)
            {
                node.penalty += 125;
            }
        }
        citizensNode.isEmpty = false;
        temporaryNode = citizensNode;
    }
}
