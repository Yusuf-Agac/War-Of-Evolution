using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenSocialDistance : MonoBehaviour
{
    public LayerMask citizenMask;

    public float force = 10f;
    public float socialDistance = 1f;
    
    private CityManagement cityManagement;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cityManagement = FindObjectOfType<CityManagement>();
    }
    
    void FixedUpdate()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, socialDistance * cityManagement.citySocialDistanceMultiplier, citizenMask);
        foreach (Collider hit in hits)
        {
            if (hit.transform.GetInstanceID() != transform.GetInstanceID() && hit.transform.CompareTag("Citizen"))
            {
                Vector3 direction = hit.transform.position - transform.position;
                direction.y = 0;
                Vector3 calculatedForce = direction.normalized * Mathf.Clamp((force * cityManagement.citySocialDistanceForceMultiplier * (1 / direction.magnitude)), cityManagement.minSocialDistanceForce, cityManagement.maxSocialDistanceForce);
                hit.GetComponent<Rigidbody>().AddForce(calculatedForce, ForceMode.Impulse);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, socialDistance * cityManagement.citySocialDistanceMultiplier);
        }
    }
}