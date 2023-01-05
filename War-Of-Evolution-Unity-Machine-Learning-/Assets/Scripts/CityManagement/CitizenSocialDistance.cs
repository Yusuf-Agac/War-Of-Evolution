using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CitizenSocialDistance : MonoBehaviour
{
    public float force = 1f;
    public float socialDistance = 1f;
    
    private CityManagement cityManagement;
    
    private CitizenSkills citizenSkills;
    private Rigidbody rb;

    private void Awake()
    {
        citizenSkills = GetComponent<CitizenSkills>();
        rb = GetComponent<Rigidbody>();
        cityManagement = FindObjectOfType<CityManagement>();
    }
    
    void FixedUpdate()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, socialDistance * cityManagement.citySocialDistanceMultiplier, LayerMask.GetMask("Citizen"));
        foreach (Collider hit in hits)
        {
            if (hit.transform.GetInstanceID() != transform.GetInstanceID() && hit.transform.CompareTag("Citizen"))
            {
                Vector3 direction = hit.transform.position - transform.position;
                direction.y = 0;
                Vector3 calculatedForce = direction.normalized * (force * cityManagement.citySocialDistanceForceMultiplier * (1 / direction.magnitude));
                hit.GetComponent<Rigidbody>().AddForce(calculatedForce, ForceMode.Impulse);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, socialDistance * cityManagement.citySocialDistanceMultiplier);
        }
    }
}