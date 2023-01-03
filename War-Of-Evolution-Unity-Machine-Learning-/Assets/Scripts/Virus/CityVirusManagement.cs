using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityVirusManagement : MonoBehaviour
{
    private Virus[] viruses;
    void Start()
    {
        viruses = GameObject.FindObjectsOfType<Virus>();
        viruses[Random.Range(0, viruses.Length)].GetInfected();
    }
}
