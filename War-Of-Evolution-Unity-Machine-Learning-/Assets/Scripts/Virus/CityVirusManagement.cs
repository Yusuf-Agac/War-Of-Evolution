using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CityVirusManagement : MonoBehaviour
{
    CityPopulation cityPopulation;
    
    private Virus[] viruses;
    void Start()
    {
        cityPopulation = GetComponent<CityPopulation>();
        viruses = GameObject.FindObjectsOfType<Virus>();
        viruses[Random.Range(0, viruses.Length)].GetInfected();
    }
    
    void Update()
    {
        if (cityPopulation.virusCount <= 0)
        {
            viruses[Random.Range(0, viruses.Length)].GetInfected();
        }
    }
}
