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
    
    public bool isQuarantined = false;

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

    public void StrengtheningImmune()
    {
        citizen.immunity += 0.01f;
    }
    
    public void StrengtheningImmuneForCure()
    {
        citizen.immunity += 0.2f;
    }
    
    private IEnumerator GainEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            if(isQuarantined){continue;}
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

    public void Quarantine()
    {
        transform.GetChild(2).gameObject.SetActive(!transform.GetChild(2).gameObject.activeInHierarchy);

        Unit unit = gameObject.GetComponent<Unit>();
        unit.enabled = !unit.enabled;
        
        CitizenSocialDistance citizenSocialDistance = gameObject.GetComponent<CitizenSocialDistance>();
        citizenSocialDistance.enabled = !citizenSocialDistance.enabled;
        
        VirusBehaviors virusBehaviors = gameObject.GetComponent<VirusBehaviors>();
        virusBehaviors.enabled = !virusBehaviors.enabled;
        isQuarantined = !isQuarantined;
    }
}
