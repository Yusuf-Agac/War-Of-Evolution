using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CityManagement : MonoBehaviour
{
    [HideInInspector] public float citySocialDistanceMultiplier = 4f;
    public Vector2 minMaxCitySocialDistance = new Vector2(1f, 4f);
    public Slider citySocialDistanceSlider;
    [HideInInspector] public float citySocialDistanceForceMultiplier = 0.7f;
    public Vector2 minMaxCitySocialDistanceForce = new Vector2(0.1f, 0.7f);
    public Slider citySocialDistanceForceSlider;
    
    public float citizenInfectionDistance = 1f;
    
    public bool displayCitizenGizmos = false;

    private void Start()
    {
        UpdateCitySocialDistanceMultiplier();
        UpdateCitySocialDistanceForceMultiplier();
    }

    public void UpdateCitySocialDistanceMultiplier()
    {
        citySocialDistanceMultiplier = Mathf.Lerp(minMaxCitySocialDistance.x, minMaxCitySocialDistance.y, citySocialDistanceSlider.value);
    }
    public void UpdateCitySocialDistanceForceMultiplier()
    {
        citySocialDistanceForceMultiplier = Mathf.Lerp(minMaxCitySocialDistanceForce.x, minMaxCitySocialDistanceForce.y, citySocialDistanceForceSlider.value);
    }
}
