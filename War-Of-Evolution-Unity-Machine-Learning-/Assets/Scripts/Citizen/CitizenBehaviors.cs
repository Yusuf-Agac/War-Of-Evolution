using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CitizenBehaviors : MonoBehaviour
{
    private Transform EmptyCitizensTransform;
    private CityPopulation cityPopulation;
    private CityManagement cityManagement;
    private GridForPathFinding gridForPathFinding;

    public Citizen citizen = new Citizen(1, 0.5f);

    private void Awake()
    {
        gridForPathFinding = FindObjectOfType<GridForPathFinding>();
        cityManagement = FindObjectOfType<CityManagement>();
        cityPopulation = FindObjectOfType<CityPopulation>();
        cityPopulation.Citizens.Add(gameObject);
        cityPopulation.IncreaseBirth();
        EmptyCitizensTransform = GameObject.Find("Citizens").transform;
        StartCoroutine(GainEnergy());
    }

    private IEnumerator GainEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            citizen.energy += cityManagement.citizenEnergyPerSecond * Random.Range(0.5f, 1.5f);
            if (citizen.energy >= 1)
            {
                DuplicateCitizen();
            }
        }
    }
    
    public void GainPrizeForArriving()
    {
        citizen.energy += cityManagement.citizenEnergyPrize * Random.Range(0.5f, 1.5f);
    }

    public void DuplicateCitizen()
    {
        List<Node> neighbours = gridForPathFinding.GetNeighboursOfNode(gridForPathFinding.NodeFromWorldPosition(transform.position));
        foreach (Node node in neighbours)
        {
            if (node.walkable)
            {
                citizen.energy = 0f;
                Instantiate(gameObject, EmptyCitizensTransform).transform.position = node.worldPosition;
                break;
            }
        }
    }
}
