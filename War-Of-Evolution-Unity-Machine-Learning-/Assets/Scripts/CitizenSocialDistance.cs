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

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, socialDistance, citizenMask);
        foreach (Collider hit in hits)
        {
            if (hit.transform.GetInstanceID() != transform.GetInstanceID() && hit.transform.CompareTag("Citizen"))
            {
                Vector3 direction = hit.transform.position - transform.position;
                Debug.Log("Hit position: " + hit.transform.position + " Self position: " + transform.position);
                hit.GetComponent<Rigidbody>().AddForce(direction.normalized * force, ForceMode.Impulse);
                Debug.Log("Name " + hit.name + " / Direction " + direction);
            }
        }
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * friction);
    }
}