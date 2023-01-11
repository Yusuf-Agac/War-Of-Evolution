using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CitizenGridComponent : MonoBehaviour
{
    private GridForPathFinding gridForPathFinding;
    private Node temporaryNode;
    private Node citizensNode;
    public bool isAddedNow = false;
    public bool isAddedTemp = false;
    

    private void Start()
    {
        gridForPathFinding = GameObject.FindObjectOfType<GridForPathFinding>();
    }

    private void Update()
    {
        citizensNode = gridForPathFinding.NodeFromWorldPosition(transform.position);
        if(citizensNode.isEmpty){isAddedNow = false;}
        if (temporaryNode != null && !temporaryNode.Equals(citizensNode))
        {
            if (!temporaryNode.isEmpty && isAddedTemp)
            {
                RemovePenaltyFromTemporary();
            }
            temporaryNode.isEmpty = true;
        }

        if (citizensNode.isEmpty && !isAddedNow)
        {
            AddPenaltyNow();
        }
        citizensNode.isEmpty = false;
        temporaryNode = citizensNode;
        isAddedTemp = isAddedNow;
    }

    public void AddPenaltyNow()
    {
        citizensNode.penalty += 400;
        List<Node> nodes = gridForPathFinding.GetNeighboursOfNode(citizensNode);
        foreach (var node in nodes)
        {
            node.penalty += 125;
        }
        isAddedNow = true;
    }

    public void RemovePenaltyFromNow()
    {
        if (isAddedNow)
        {
            citizensNode.penalty -= 400;
            List<Node> nodes = gridForPathFinding.GetNeighboursOfNode(citizensNode);
            foreach (var node in nodes)
            {
                node.penalty -= 125;
            }
            isAddedNow = false;
        }
    }

    public void RemovePenaltyFromTemporary()
    {
        if (isAddedTemp)
        {
            temporaryNode.penalty -= 400;
            List<Node> nodes = gridForPathFinding.GetNeighboursOfNode(temporaryNode);
            foreach (var node in nodes)
            {
                node.penalty -= 125;
            }
            isAddedTemp = false;
        }
    }
    
    public void RemovePenalty()
    {
        RemovePenaltyFromNow();
    }
}
