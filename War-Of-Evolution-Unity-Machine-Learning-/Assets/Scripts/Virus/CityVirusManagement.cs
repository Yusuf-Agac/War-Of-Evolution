using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CityVirusManagement : MonoBehaviour
{
    CityPopulation cityPopulation;
    
    private VirusBehaviors[] viruses;
    void Start()
    {
        cityPopulation = GetComponent<CityPopulation>();
        viruses = GameObject.FindObjectsOfType<VirusBehaviors>();
        viruses[Random.Range(0, viruses.Length)].GetInfected(new Virus(1, 1, 1));
    }
    
    void Update()
    {
        if (cityPopulation.virusCount <= 0)
        {
            viruses[Random.Range(0, viruses.Length)].GetInfected(new Virus(1, 1, 1));
        }
    }
}
