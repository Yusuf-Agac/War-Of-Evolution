using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CitizenBehaviors : MonoBehaviour
{
    private Transform EmptyCitizensTransform;
    private CityPopulation cityPopulation;

    public Citizen citizen = new Citizen(1, 0.5f);

    private void Awake()
    {
        cityPopulation = FindObjectOfType<CityPopulation>();
        cityPopulation.Citizens.Add(gameObject);
        cityPopulation.UpdatePopulationText();
        EmptyCitizensTransform = GameObject.Find("Citizens").transform;
        StartCoroutine(GainEnergy());
    }

    private IEnumerator GainEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            citizen.energy += 0.001f;
            if (citizen.energy >= 1)
            {
                DuplicateCitizen();
            }
        }
    }
    
    public void GainPrizeForArriving()
    {
        citizen.energy += 0.01f;
    }

    public void DuplicateCitizen()
    {
        citizen.energy = 0f;
        Instantiate(gameObject, EmptyCitizensTransform).transform.position = transform.position + new Vector3(0.5f, 0, 0.5f);
    }
}
