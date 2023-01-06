using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen
{
    public bool isVirus;
    public float immunity;
    public float health;
    public float speed;
    public float energy;
    public Citizen(float immunity, float energy)
    {
        this.isVirus = false;
        this.immunity = immunity;
        this.energy = energy;
    }
}
