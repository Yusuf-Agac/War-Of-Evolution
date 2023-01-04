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
    [HideInInspector] public float maxSocialDistanceForce = 3.75f;
    public Vector2 minMaxMaxSocialDistanceForce = new Vector2(1f, 3.75f);
    public Slider maxSocialDistanceForceSlider;
    [HideInInspector] public float minSocialDistanceForce = 0.2f;
    public Vector2 minMaxMinSocialDistanceForce = new Vector2(0.1f, 0.2f);
    public Slider minSocialDistanceForceSlider;
    
    public float citizenInfectionDistance = 1f;
    
    public bool displayCitizenGizmos = false;

    private void Start()
    {
        UpdateCitySocialDistanceMultiplier();
        UpdateCitySocialDistanceForceMultiplier();
        UpdateMaxSocialDistanceForce();
        UpdateMinSocialDistanceForce();
    }

    public void UpdateCitySocialDistanceMultiplier()
    {
        citySocialDistanceMultiplier = Mathf.Lerp(minMaxCitySocialDistance.x, minMaxCitySocialDistance.y, citySocialDistanceSlider.value);
    }
    public void UpdateCitySocialDistanceForceMultiplier()
    {
        citySocialDistanceForceMultiplier = Mathf.Lerp(minMaxCitySocialDistanceForce.x, minMaxCitySocialDistanceForce.y, citySocialDistanceForceSlider.value);
    }
    public void UpdateMaxSocialDistanceForce()
    {
        maxSocialDistanceForce = Mathf.Lerp(minMaxMaxSocialDistanceForce.x, minMaxMaxSocialDistanceForce.y, maxSocialDistanceForceSlider.value);
    }
    public void UpdateMinSocialDistanceForce()
    {
        minSocialDistanceForce = Mathf.Lerp(minMaxMinSocialDistanceForce.x, minMaxMinSocialDistanceForce.y, minSocialDistanceForceSlider.value);
    }
}
