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
    private Node dieNode;
    public bool isAddedNow = false;
    public bool isRemovedTemp = false;
    public int counter = 0;
    

    private void Start()
    {
        gridForPathFinding = GameObject.FindObjectOfType<GridForPathFinding>();
    }

    private void Update()
    {
        citizensNode = gridForPathFinding.NodeFromWorldPosition(transform.position);
        if(!citizensNode.objectsOnNode.Contains(gameObject)){isAddedNow = false;}
        if (temporaryNode != null && !temporaryNode.Equals(citizensNode))
        {
            if (temporaryNode.objectsOnNode.Contains(gameObject) && isRemovedTemp)
            {
                RemovePenaltyFromTemporary();
            }
        }

        if (!citizensNode.objectsOnNode.Contains(gameObject) && !isAddedNow)
        {
            AddPenaltyNow();
        }
        temporaryNode = citizensNode;
        isRemovedTemp = isAddedNow;
    }

    public void AddPenaltyNow()
    {
        if (!isAddedNow && !citizensNode.objectsOnNode.Contains(gameObject))
        {
            counter++;
            citizensNode.penalty += 400;
            dieNode = citizensNode;
            List<Node> nodes = gridForPathFinding.GetNeighboursOfNode(citizensNode);
            foreach (var node in nodes)
            {
                node.penalty += 125;
            }
            isAddedNow = true;
            citizensNode.objectsOnNode.Add(gameObject);
        }
    }

    public void RemovePenaltyFromTemporary()
    {
        if (isRemovedTemp && temporaryNode.objectsOnNode.Contains(gameObject))
        {
            counter--;
            temporaryNode.penalty -= 400;
            List<Node> nodes = gridForPathFinding.GetNeighboursOfNode(temporaryNode);
            foreach (var node in nodes)
            {
                node.penalty -= 125;
            }
            isRemovedTemp = false;
            temporaryNode.objectsOnNode.Remove(gameObject);
        }
    }

    public void RemovePenaltyWhenDie()
    {
        if (dieNode.objectsOnNode.Contains(gameObject))
        {
            dieNode.penalty -= 400;
            List<Node> nodes = gridForPathFinding.GetNeighboursOfNode(dieNode);
            foreach (var node in nodes)
            {
                node.penalty -= 125;
            }

            dieNode.objectsOnNode.Remove(gameObject);
        }
    }
    public void RemovePenalty()
    {
        RemovePenaltyWhenDie();
    }
}
