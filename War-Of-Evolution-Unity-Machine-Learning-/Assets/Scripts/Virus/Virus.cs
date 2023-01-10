using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus
{
    public float infectionRadius;
    public float infectiousness;
    public float resistance;
    public float virulence;
    public float speedImpact;
    
    public Virus(float infectionRadius, float resistance, float virulence, float infectiousness, float speedImpact)
    {
        this.infectionRadius = infectionRadius;
        this.resistance = resistance;
        this.virulence = virulence;
        this.infectiousness = infectiousness;
        this.speedImpact = speedImpact;
    }
}
