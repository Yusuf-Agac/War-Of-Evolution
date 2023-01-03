using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManagement : MonoBehaviour
{
    public float citySocialDistanceMultiplier = 1;
    public float citySocialDistanceForceMultiplier = 1;
    public float maxSocialDistanceForce = 5;
    public float minSocialDistanceForce = 1f;
    public float citizenInfectionDistance = 1f;
    
    public bool displayCitizenGizmos = false;
}
