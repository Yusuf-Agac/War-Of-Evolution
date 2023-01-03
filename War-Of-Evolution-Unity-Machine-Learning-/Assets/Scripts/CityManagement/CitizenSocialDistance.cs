using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenSocialDistance : MonoBehaviour
{
    public LayerMask citizenMask;

    public float force = 10f;
    public float friction = 0.5f;
    public float socialDistance = 1f;
    
    private CityManagement cityManagement;

    private Rigidbody rb;

    private void Start()
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
                hit.GetComponent<Rigidbody>().AddForce(direction.normalized * (force * cityManagement.citySocialDistanceForceMultiplier * (1 / direction.magnitude)), ForceMode.Force);
                Debug.Log("Forced Name " + hit.name + " / Direction " + direction);
            }
        }
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * friction);
    }
}