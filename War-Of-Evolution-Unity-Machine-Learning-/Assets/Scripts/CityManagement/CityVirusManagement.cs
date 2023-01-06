using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CityVirusManagement : MonoBehaviour
{
    CityPopulation cityPopulation;
    
    public List<VirusBehaviors> viruses;
    void Start()
    {
        cityPopulation = GetComponent<CityPopulation>();
        StartCoroutine(InfectRandomPerson());
    }
    
    IEnumerator InfectRandomPerson()
    {
        while (true)
        {
            yield return new WaitForSeconds(500 / cityPopulation.Citizens.Count);
            if (cityPopulation.virusCount <= 0)
            {
                viruses[Random.Range(0, viruses.Count)].GetInfected(new Virus(1, 1, 1));
            }
        }
    }
    
}
