using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Virus : MonoBehaviour
{
    public float infectionRadius = 1;
    
    public Material NormalCitizen;
    public Material InfectedCitizen;
    
    private MeshRenderer meshRenderer;
    private CitizenSkills citizenSkills;

    private CityManagement cityManagement;
    private CitizenSocialDistance citizenSocialDistance;
    
    private void Awake()
    {
        cityManagement = FindObjectOfType<CityManagement>();
        citizenSocialDistance = GetComponent<CitizenSocialDistance>();
        citizenSkills = GetComponent<CitizenSkills>();
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    public void GetInfected()
    {
        Debug.Log(transform.name + " got infected");
        meshRenderer.material = InfectedCitizen;
        citizenSkills.isVirus = true;
        StartCoroutine(Recovering());
    }
    
    public void GetCured()
    {
        meshRenderer.material = NormalCitizen;
        citizenSkills.isVirus = false;
    }

    private void FixedUpdate()
    {
        if (citizenSkills.isVirus)
        {
            if (Random.Range(0, 100) < 1.1f)
            {
                Collider[] near = Physics.OverlapSphere(transform.position, infectionRadius * cityManagement.citizenInfectionDistance, LayerMask.GetMask("Citizen"));
                if (near.Length > 0) { SetVirus(near, 0); }
            }
        }
    }

    private void SetVirus(Collider[] virusArray, int index)
    {
        if (virusArray.Length > index)
        {
            if(virusArray[index].GetComponent<CitizenSkills>().isVirus == false)
            {
                virusArray[index].GetComponent<Virus>().GetInfected();
            }
            else
            {
                SetVirus(virusArray, index + 1);
            }
        }
    }
    
    IEnumerator Recovering()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            if ((Random.Range(0, 25) / citizenSkills.immunity) < 1.1f)
            {
                GetCured();
                Debug.Log(transform.name + " Recovered");
                yield break;
            }
            else
            {
                Debug.Log(transform.name + " Recovering is failed");
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, infectionRadius * cityManagement.citizenInfectionDistance);
        }
    }
}
