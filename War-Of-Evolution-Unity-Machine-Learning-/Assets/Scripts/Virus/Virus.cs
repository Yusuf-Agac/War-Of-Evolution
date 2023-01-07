using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus
{
    public float infectionRadius = 1;
    public float infectiousness = 1f;
    public float resistance = 1;
    public float virulence = 1;
    
    public Virus(float infectionRadius, float resistance, float virulence, float infectiousness)
    {
        this.infectionRadius = infectionRadius;
        this.resistance = resistance;
        this.virulence = virulence;
        this.infectiousness = infectiousness;
    }
}
