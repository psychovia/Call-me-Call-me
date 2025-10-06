using UnityEngine;

// ========== EnergySystem Script ==========
// manages functions that can reduce or add energy
// energy is a percentage with 100 being the maximum and 0 the minimum 
// 
// ---- USAGE ----
// EnergySystem.es.<functionCall> where functionCall is any of the public functions marked below (e.g. ReduceEnergy(n) or GetCurrentEnergy())

public class EnergySystem : MonoBehaviour
{

    public static EnergySystem es; 

    public float totalEnergy = 100f;  
    private float energy;
    private float energyDrainRate = 1f;

    // === Alternatives based on how we want to use the energy system moving forward ===
    // private float timeForDrain = 120f;                           // 2 minutes
    // private float altDrainRate = energy / timeForDrain;         // gives us the drain rate so that all energy will be drained after timeForDrain seconds


    private void Awake()
    {
        if (es == null)     // if no instance of the energy system exisits, create one
        {
            es = this;
            DontDestroyOnLoad(gameObject);  // ensures persistence across scenes
        }
        else
        {
            Destroy(gameObject);            // ensures only 1 instance exists
            return;
        }

        energy = totalEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        DrainEnergyOverTime();
    }

    private void DrainEnergyOverTime()
    {
        if (energy > 0)
        {
            // reduce energy by drain rate
            energy -= energyDrainRate * Time.deltaTime;
            energy = Mathf.Clamp(energy, 0f, totalEnergy);
        }
    }

    // ==============================================================
    //                        PUBLIC FUNCTIONS
    // ==============================================================

    // Decreases energy by a given number n
    // (e.g. energy is originally 80. DecreaseEnergy(15) => energy = 65)
    public void ReduceEnergy(float n)
    {
        if (n < 0)
        {
            Debug.LogWarning("ReduceEnergy called with negative value!");
            return;
        }
        energy -= n;
        energy = Mathf.Clamp(energy, 0f, totalEnergy);
    }

    // Increases energy by a given number n
    // (e.g. energy is originally 80. IncreaseEnergy(15) => energy = 95)
    public void IncreaseEnergy(float n)
    {
        if (n < 0)
        {
            Debug.LogWarning("IncreaseEnergy called with negative value!");
            return;
        }
        energy += n;
        energy = Mathf.Clamp(energy, 0f, totalEnergy);
    }

    // Sets a new drain rate
    public void SetDrainRate(float newDrainRate)
    {
        if (newDrainRate < 0)
        {
            Debug.LogWarning("newDrainRate called with negative value!");
            return;
        }
        energyDrainRate = newDrainRate;
    }

    // Returns energy value
    public float GetCurrentEnergy()
    {
        return energy;
    }

    // Sets energy back to its total value
    public void ResetEnergy()
    {
        energy = totalEnergy;
    }


}
