using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    [SerializeField]
    private Image health_Stats, ammo_Stats;


    
    public void Display_HealthStats(float healthValue)
    {
        healthValue /= 100f;

        health_Stats.fillAmount = healthValue;
    }

    public void Display_AmmoStats(float ammoValue)
    {
        ammoValue /= 100f;

        ammo_Stats.fillAmount = ammoValue;
    }
}
