using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CityPopulation : MonoBehaviour
{
    public List<GameObject> Citizens;
    public TextMeshProUGUI populationText;
    
    public TextMeshProUGUI virusPercentText;
    public int virusCount = 0;
    
    public TextMeshProUGUI infectedCountText;
    public int infectedCount = 0;
    public TextMeshProUGUI curedCountText;
    public int curedCount = 0;
    public TextMeshProUGUI deadCountText;
    public int deadCount = 0;
    public TextMeshProUGUI birthCountText;
    public int birthCount = 0;
    
    void Start()
    {
        //Citizens = GameObject.FindGameObjectsWithTag("Citizen").ToList();
        UpdatePopulationText();
        UpdateVirusPercentText();
        UpdateCuredText();
        UpdateDeadText();
        UpdateInfectedText();
    }

    public void IncreaseInfected()
    {
        infectedCount++;
        virusCount++;
        UpdateInfectedText();
        UpdateVirusPercentText();
    }

    public void IncreaseCured()
    {
        curedCount++;
        virusCount--;
        UpdateCuredText();
        UpdateVirusPercentText();
    }

    public void IncreaseDead()
    {
        deadCount++;
        virusCount--;
        UpdateDeadText();
        UpdatePopulationText();
        UpdateVirusPercentText();
    }
    
    public void IncreaseBirth()
    {
        birthCount++;
        UpdateBirthText();
        UpdatePopulationText();
    }

    public void UpdatePopulationText()
    {
        populationText.text = "Population: " + Citizens.Count;
    }

    public void UpdateVirusPercentText()
    {
        Debug.Log(virusCount);
        virusPercentText.text = Citizens.Count > 0 ? "Virus: " + ((virusCount * 100) / Citizens.Count ) + "%" : "Virus: 100%";
    }
    
    public void UpdateInfectedText()
    {
        infectedCountText.text = "Infected: " + infectedCount;
    }

    public void UpdateCuredText()
    {
        curedCountText.text = "Cured: " + curedCount;
    }

    public void UpdateDeadText()
    {
        deadCountText.text = "Dead: " + deadCount;
    }
    
    public void UpdateBirthText()
    {
        birthCountText.text = "Birth: " + birthCount;
    }
}
