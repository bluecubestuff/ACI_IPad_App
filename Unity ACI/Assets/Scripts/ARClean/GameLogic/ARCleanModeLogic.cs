/* 
 * Author: Lim Rui An Ryan, Lee Kwan Liang
 * Filename: ARCleanModeLogic.cs
 * Description: The generic game mode logic that will be inherited by other game modes.
 */
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ARCleanModeLogic
{
    // Public Variables
    public TextMeshProUGUI ProgressHeaderText;
    public TextMeshProUGUI ProgressSubHeaderText;
    public Slider ProgressBar;

    // Protected Variables
    protected float Timer = 0f;
    protected const float IntermissionTime = 1f;
    //protected bool TriggerWaitTimer = false;
    protected ARCleanTool CleanTool = new ARCleanTool();
    protected float CleaningIncrement = 40f;
    //protected static int InternalGameState = 0;

    protected List<GameObject> DirtContainer, WaterContainer;
    protected bool DirtSpawned = false;
    protected int NumberOfDirt = 0;
    protected int NumberOfWater = 0;
    protected float DirtCleanRate = 10f;
    protected float WaterCleanRate = 25f;
    protected float WaterSpawnTime = 0.5f;

    protected float CleanupPercentageGains = 0f;

    protected bool DirtContainerCollisionDetection(Vector3 CleanToolPosition, string Tag)
    {
        for (int i = 0; i < DirtContainer.Count; ++i)
        {
            GameObject Child = DirtContainer[i];
            if (Child.activeSelf && Child.tag == Tag)
                if ((Child.transform.position - CleanToolPosition).sqrMagnitude <= Child.transform.localScale.x * 0.5f)
                {
                    Child.GetComponent<ARCleanDirt>().ObjectHealth -= (int)DirtCleanRate;
                    Child.GetComponent<Renderer>().material.color = new Color(Child.GetComponent<Renderer>().material.color.r, Child.GetComponent<Renderer>().material.color.g, Child.GetComponent<Renderer>().material.color.b, Child.GetComponent<ARCleanDirt>().StartingAlpha * (float)Child.GetComponent<ARCleanDirt>().ObjectHealth / 100f + 0.4f);
                    if (Child.GetComponent<ARCleanDirt>().ObjectHealth <= 0)
                    {
                        NumberOfDirt--;
                        ProgressBar.value += CleanupPercentageGains;
                        Child.SetActive(false);
                        if (NumberOfDirt <= 0)
                            return true;
                    }
                }
        }
        return false;
    }
    protected bool WaterContainerCollisionDetection(Vector3 CleanToolPosition, string Tag)
    {
        for (int i = 0; i < WaterContainer.Count; ++i)
        {
            GameObject Child = WaterContainer[i];
            if (Child.activeSelf && Child.tag == Tag)
                if ((Child.transform.position - CleanToolPosition).sqrMagnitude <= Child.transform.localScale.x * 0.5f)
                {
                    Child.GetComponent<ARCleanDirt>().ObjectHealth -= (int)WaterCleanRate;
                    Child.GetComponent<Renderer>().material.color = new Color(Child.GetComponent<Renderer>().material.color.r, Child.GetComponent<Renderer>().material.color.g, Child.GetComponent<Renderer>().material.color.b, Child.GetComponent<ARCleanDirt>().StartingAlpha * (float)Child.GetComponent<ARCleanDirt>().ObjectHealth / 100f + 0.4f);
                    if (Child.GetComponent<ARCleanDirt>().ObjectHealth <= 0)
                    {
                        NumberOfWater--;
                        ProgressBar.value += CleanupPercentageGains;
                        Child.SetActive(false);
                        if (NumberOfWater <= 0)
                            return true;
                    }
                }
        }
        return false;
    }
}
