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
                        infectionRadius * cityManagement.citizenInfectionDistance, LayerMask.GetMask("Citizen"));
                    if (near.Length > 0)
                    {
                        SetVirus(near, 0);
                    }
                }
            }
        }
    }


    public void GetInfected()
    {
        Debug.Log(transform.name + " got infected");
        meshRenderer.material = InfectedCitizen;
        citizenSkills.isVirus = true;
        cityPopulation.IncreaseInfected();
        StartCoroutine(Recovering());
    }

    public void GetCured()
    {
        meshRenderer.material = NormalCitizen;
        citizenSkills.isVirus = false;
        cityPopulation.IncreaseCured();
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
            if ((Random.Range(0, 100) / citizenSkills.immunity) <= 4f)
            {
                GetCured();
                Debug.Log(transform.name + " Recovered");
                yield break;
            }
            else if (Random.Range(0, 100) <= 1f)
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

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, infectionRadius * cityManagement.citizenInfectionDistance);
        }
    }
}