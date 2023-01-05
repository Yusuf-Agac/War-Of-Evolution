using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VirusBehaviors : MonoBehaviour
{
    public Virus virus = new Virus(1, 1, 1);
    
    public Material NormalCitizen;
    public Material InfectedCitizen;

    private MeshRenderer meshRenderer;
    private CitizenSkills citizenSkills;

    private CityManagement cityManagement;
    private CityPopulation cityPopulation;
    private CitizenSocialDistance citizenSocialDistance;

    private GameObject DieParticlePrefab;

    private void Awake()
    {
        DieParticlePrefab = Resources.Load("ParticleEffect/Die") as GameObject;
        cityPopulation = FindObjectOfType<CityPopulation>();
        cityManagement = FindObjectOfType<CityManagement>();
        citizenSocialDistance = GetComponent<CitizenSocialDistance>();
        citizenSkills = GetComponent<CitizenSkills>();
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();

        StartCoroutine(InfectOtherCitizens());
    }

    IEnumerator InfectOtherCitizens()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (citizenSkills.isVirus)
            {
                if (Random.Range(0, 100) <= 50f)
                {
                    Collider[] near = Physics.OverlapSphere(transform.position,
                        virus.infectionRadius * cityManagement.citizenInfectionDistance, LayerMask.GetMask("Citizen"));
                    if (near.Length > 0)
                    {
                        SetVirus(near, 0);
                    }
                }
            }
        }
    }


    public void GetInfected(Virus virus)
    {
        if(virus == null || citizenSkills.isVirus){return;}
        Inherited(virus);
        Debug.Log(transform.name + " got infected");
        meshRenderer.material = InfectedCitizen;
        citizenSkills.isVirus = true;
        cityPopulation.IncreaseInfected();
        StartCoroutine(Recovering());
        StartCoroutine(Evolve());
    }

    private void Inherited(Virus virus)
    {
        this.virus.resistance = virus.resistance;
        this.virus.infectionRadius = virus.infectionRadius;
        this.virus.virulence = virus.virulence;
    }

    public void GetCured()
    {
        meshRenderer.material = NormalCitizen;
        citizenSkills.isVirus = false;
        ResetVirus();
        cityPopulation.IncreaseCured();
    }
    
    private void ResetVirus()
    {
        virus.virulence = 1;
        virus.resistance = 1;
        virus.infectionRadius = 1;
    }

    public void GetDead()
    {
        cityPopulation.Citizens.Remove(gameObject);
        cityPopulation.IncreaseDead();
        Instantiate(DieParticlePrefab, null).transform.position = transform.position;
        Destroy(gameObject);
    }

    private void SetVirus(Collider[] virusArray, int index)
    {
        if (virusArray.Length > index)
        {
            if (virusArray[index].GetComponent<CitizenSkills>().isVirus == false)
            {
                virusArray[index].GetComponent<VirusBehaviors>().GetInfected(virus);
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
            if ((Random.Range(0, 50 * virus.resistance) / citizenSkills.immunity) <= 1f)
            {
                GetCured();
                Debug.Log(transform.name + " Recovered");
                yield break;
            }
            else if (Random.Range(0, 100) <= virus.virulence)
            {
                GetDead();
                Debug.Log(transform.name + " Died");
                yield break;
            }
            else
            {
                Debug.Log(transform.name + " Recovering is failed");
            }
        }
    }

    IEnumerator Evolve()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            virus.resistance += 0.02f;
            virus.virulence += 0.02f;
            virus.infectionRadius += 0.02f;
            if(citizenSkills.isVirus == false) { yield break; }
        }
    }
    
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, virus.infectionRadius * cityManagement.citizenInfectionDistance);
        }
    }
}