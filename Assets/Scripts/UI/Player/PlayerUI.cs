using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    // Events


    // References
    [SerializeField] private TextMeshProUGUI cycleText, timeText, energyText;
    [SerializeField] private Image timeBar, energyBar;
    
    // Variables

    // On Enable
    private void OnEnable()
    {
        EnergySystem.es.OnEnergyChanged += ES_OnEnergyChanged;
        CycleManager.Instance.OnTimeChanged += CM_OnTimeChanged;
    }

    // On Disable
    private void OnDisable()
    {
        EnergySystem.es.OnEnergyChanged -= ES_OnEnergyChanged;
        CycleManager.Instance.OnTimeChanged -= CM_OnTimeChanged;
    }

    // CM- On Time Changed
    private void CM_OnTimeChanged(object sender, System.EventArgs e)
    {
        float currentTime = CycleManager.Instance.GetCurrentTime();
        float maxTime = CycleManager.Instance.GetMaxTime();
        int currentCycle = CycleManager.Instance.GetCurrentCycle();

        timeText.text = "Current Time: " + currentTime.ToString();
        if (currentTime == 0)
            timeBar.fillAmount = 0;
        else 
            timeBar.fillAmount = currentTime / maxTime;

        cycleText.text = "Current Cycle: " + currentCycle.ToString();
    }

    // ES- On Energy Changed
    private void ES_OnEnergyChanged(object sender, System.EventArgs e)
    {
        float currentEnergy = EnergySystem.es.GetCurrentEnergy();
        float maxEnergy = EnergySystem.es.totalEnergy;

        energyText.text = "Current Energy: " + currentEnergy.ToString();
        if (currentEnergy == 0)
            energyBar.fillAmount = 0;
        else
            energyBar.fillAmount = currentEnergy / maxEnergy;
    }
}
