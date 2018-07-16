/* 
 * Author: Lim Rui An Ryan, Lee Kwan Liang
 * Filename: ARCleanPrepAreaLogic.cs
 * Description: The preparation game mode's game logic codes that will be run by the ARGameLogicSystem.
 */
using UnityEngine;
using System.Collections.Generic;

public class ARCleanPrepAreaLogic : ARCleanModeLogic
{
    public enum Progress
    {
        P_Scrub = 0,
        P_Wash,
        P_Dry,
        P_Disinfect,
    }

    public static ARCleanDataStore.GameLocation GetGameLocationForCurrentPhase(int Phase)
    {
        switch ((Progress)Phase)
        {
            case Progress.P_Scrub:
                return ARCleanDataStore.GameLocation.GL_Prep;
            case Progress.P_Wash:
                return ARCleanDataStore.GameLocation.GL_Prep;
            case Progress.P_Dry:
                return ARCleanDataStore.GameLocation.GL_Prep;
            case Progress.P_Disinfect:
                return ARCleanDataStore.GameLocation.GL_Prep;
        }
        return ARCleanDataStore.GameLocation.GL_Undefined;
    }

    public void Update()
    {
        ProgressHeaderText.text = "Preparation Area Daily Tasks";
        switch ((Progress)ARCleanDataStore.GetGamePhase())
        {
            case Progress.P_Scrub:
                Phase_Scrub();
                break;
            case Progress.P_Wash:
                Phase_Wash();
                break;
            case Progress.P_Dry:
                Phase_Dry();
                break;
            case Progress.P_Disinfect:
                Phase_Disinfect();
                break;
        }
    }

    private void Phase_Scrub()
    {
        /// Logic for scrubbing step
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Prep)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 1: Scrub the work table using [Genie]\n [Genie] can be found in the laundry room.";
                if (!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_HeavySpongeGenie)
                    ArrowIndication.Instance.toolArrow.SetActive(true);
                else if (ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_HeavySpongeGenie)
                {
                    ArrowIndication.Instance.toolArrow.SetActive(false);
                    ArrowIndication.Instance.sideBarArrow.SetActive(true);
                }
                else
                {
                    ArrowIndication.Instance.sideBarArrow.SetActive(false);
                    if (ARCleanDataStore.CameraCurrentDirection == ARCleanCamera.Directions.D_Up)
                    ArrowIndication.Instance.activityArrow.SetActive(true);
                }
            }
            else ProgressSubHeaderText.text = "";

            ARCleanDataStore.ObjectInteractibleFlag = false;
            if (ARCleanDataStore.PlayerInputFlag && !ARCleanDataStore.CheckIfMouseIsOverUI())
            {
                CleanTool.SetToolHitPosition(Input.mousePosition, true, "");
                if (ARCleanDataStore.CleanToolActive)
                    if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Up, "Wrong Direction", "You need to be looking from above!", "You can't use your tool here...", ARCleanDataStore.PenaltyCounter < 3) ||
                        ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_HeavySpongeGenie, "Wrong Tool", "You need to be using Genie!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
                    {
                        ARCleanDataStore.CleanToolActive = false;
                        ARCleanDataStore.PenaltyCounter++;
                        return;
                    }
                if (CleanTool.CleaningCheck(0.3f))
                {
                    ProgressBar.value += CleaningIncrement * Time.deltaTime;
                    if (ProgressBar.value >= 100f)
                    {
                        ProgressBar.value = 0f;
                        ARCleanDataStore.CleanToolActive = false;
                        // Reset Values for next run
                        ARCleanDataStore.PlayerInputFlagReset = false;
                        ArrowIndication.Instance.sideBarArrow.SetActive(false);
                        ARCleanDataStore.IncrementGameProgress();
                    }
                }
            }
        }
        else
        {
            ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the preparation area to complete this stage.";
        }
    }

    private void Phase_Wash()
    {
        /// Logic for washing step
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Prep)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 2: Wash and rinse the work table using [Sponge]\n [Sponge] can be found in on top of the sink.";
                if(!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Sponge)
                {
                    ArrowIndication.Instance.activityArrow.SetActive(false);
                    ArrowIndication.Instance.toolArrow.SetActive(true);
                }
                else if (ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Sponge)
                {
                    ArrowIndication.Instance.toolArrow.SetActive(false);
                    ArrowIndication.Instance.sideBarArrow.SetActive(true);
                }
                else
                {
                    ArrowIndication.Instance.sideBarArrow.SetActive(false);
                    ArrowIndication.Instance.activityArrow.SetActive(true);
                }
            }
            else ProgressSubHeaderText.text = "";

            ARCleanDataStore.ObjectInteractibleFlag = false;
            Timer+= Time.deltaTime;
            if (ARCleanDataStore.PlayerInputFlag)
            {
                Vector3 CleanToolPosition = CleanTool.SetToolHitPosition(Input.mousePosition, true, "");
                if (ARCleanDataStore.CleanToolActive)
                    if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Up, "Wrong Direction", "You need to be looking from above!", "You can't use your tool here...", ARCleanDataStore.PenaltyCounter < 3) ||
                        ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_Sponge, "Wrong Tool", "You need to be using Sponge!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
                    {
                        ARCleanDataStore.CleanToolActive = false;
                        ARCleanDataStore.PenaltyCounter++;
                        return;
                    }
                    else if (Timer > WaterSpawnTime)
                    {
                        Timer = 0f;
                        if (WaterContainer == null)
                            WaterContainer = new List<GameObject>();
                        ARCleanDataStore.ModelAccess.gameObject.GetComponent<ARCleanObjectSpawner>().SpawnWater(WaterContainer, new Vector3(CleanToolPosition.x, CleanToolPosition.y, CleanToolPosition.z));
                    }
                if (CleanTool.CleaningCheck(0.3f))
                {
                    ProgressBar.value += CleaningIncrement * Time.deltaTime;
                    if (ProgressBar.value >= 100f)
                    {
                        ProgressBar.value = 0f;
                        ARCleanDataStore.CleanToolActive = false;
                        ARCleanDataStore.IncrementGameProgress();
                        // Reset Values for next run
                        ARCleanDataStore.PlayerInputFlagReset = false;
                        NumberOfWater = WaterContainer.Count;
                        CleanupPercentageGains = 100f / (float)NumberOfWater;
                        ARCleanDataStore.ModelAccess.GetComponent<ARCleanObjectSpawner>().ActivateInternalColliders(WaterContainer);
                        Timer = 0f;
                        ProgressBar.value = 0;
                        ArrowIndication.Instance.sideBarArrow.SetActive(false);
                    }
                }
            }
        }
        else
        {
            ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the preparation area to complete this stage.";
        }
    }

    private void Phase_Dry()
    {
        /// Logic for dry step
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Prep)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 3: Dry up the work table using [Dry Cloth]\n [Dry Cloth] can be found in the sink's cabinet.";
                if(!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_DryCloth)
                {
                    ArrowIndication.Instance.activityArrow.SetActive(false);
                    ArrowIndication.Instance.toolArrow.SetActive(true);
                }
                else if (ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_DryCloth)
                {
                    ArrowIndication.Instance.toolArrow.SetActive(false);
                    ArrowIndication.Instance.sideBarArrow.SetActive(true);
                }
                else
                {
                    ArrowIndication.Instance.sideBarArrow.SetActive(false);
                    ArrowIndication.Instance.activityArrow.SetActive(true);
                }
            }
            else ProgressSubHeaderText.text = "";

            ARCleanDataStore.ObjectInteractibleFlag = false;
            if (WaterContainer == null)
            {
                WaterContainer = new List<GameObject>();
                NumberOfWater = ARCleanDataStore.ModelAccess.gameObject.GetComponent<ARCleanObjectSpawner>().GenerateWater(WaterContainer, ARCleanDataStore.ModelAccess.SpawningArea_Stove.transform.localScale, 30, "ARC_SpawnSurface");
                ARCleanDataStore.ModelAccess.GetComponent<ARCleanObjectSpawner>().ActivateInternalColliders(WaterContainer);
                CleanupPercentageGains = 100f / (float)NumberOfWater;
            }
            if (ARCleanDataStore.PlayerInputFlag)
            {
                Vector3 CleanToolPosition = CleanTool.SetToolHitPosition(Input.mousePosition, true, "");
                // Error Handling
                if (ARCleanDataStore.CleanToolActive)
                    if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Up, "Wrong Direction", "You need to be looking from above!", "You can't use your tool here...", ARCleanDataStore.PenaltyCounter < 3) ||
                    ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_DryCloth, "Wrong Tool", "You need to be using the Dry Cloth!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
                    {
                        ARCleanDataStore.CleanToolActive = false;
                        ARCleanDataStore.PenaltyCounter++;
                        return;
                    }
                if (CleanTool.CleaningCheck(0.3f) && WaterContainerCollisionDetection(CleanToolPosition, "ARC_Water"))
                {
                    foreach (GameObject Obj in WaterContainer)
                        GameObject.Destroy(Obj);
                    WaterContainer.Clear();
                    ARCleanDataStore.CleanToolActive = false;
                    ARCleanDataStore.IncrementGameProgress();
                    // Reset Values for next run
                    ARCleanDataStore.PlayerInputFlagReset = false;
                    Timer = 0f;
                    ProgressBar.value = 0;
                    ArrowIndication.Instance.sideBarArrow.SetActive(false);
                }
            }
        }
        else
        {
            ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the preparation area to complete this stage.";
        }
    }

    private void Phase_Disinfect()
    {
        /// Logic for disinfect step
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Prep)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 4: Disinfect the work table using [Alcosan]\n [Alcosan] can be found in the laundry room.";
                if (!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Alcosan)
                {
                    ArrowIndication.Instance.activityArrow.SetActive(false);
                    ArrowIndication.Instance.toolArrow.SetActive(true);
                }
                else if (ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Alcosan)
                {
                    ArrowIndication.Instance.toolArrow.SetActive(false);
                    ArrowIndication.Instance.sideBarArrow.SetActive(true);
                }
                else
                {
                    ArrowIndication.Instance.sideBarArrow.SetActive(false);
                    ArrowIndication.Instance.activityArrow.SetActive(true);
                }


                       
            }
            else ProgressSubHeaderText.text = "";

            ARCleanDataStore.ObjectInteractibleFlag = false;
            if (ARCleanDataStore.PlayerInputFlag && !ARCleanDataStore.CheckIfMouseIsOverUI())
            {
                CleanTool.SetToolHitPosition(Input.mousePosition, true, "");
                if (ARCleanDataStore.CleanToolActive)
                    if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Up, "Wrong Direction", "You need to be looking from above!", "You can't use your tool here...", ARCleanDataStore.PenaltyCounter < 3) ||
                        ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_Alcosan, "Wrong Tool", "You need to be using Alcosan!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
                    {
                        ARCleanDataStore.CleanToolActive = false;
                        ARCleanDataStore.PenaltyCounter++;
                        return;
                    }
                if (CleanTool.CleaningCheck(0.3f))
                {
                    ProgressBar.value += CleaningIncrement * Time.deltaTime;
                    if (ProgressBar.value >= 100f)
                    {
                        ProgressBar.value = 0f;
                        ARCleanDataStore.CleanToolActive = false;
                        // Reset Values for next run
                        ARCleanDataStore.PlayerInputFlagReset = false;
                        ARCleanDataStore.SetupForNextGameMode();
                        ArrowIndication.Instance.StopAllArrows();
                    }
                }
            }
        }
        else
        {
            ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the preparation area to complete this stage.";
        }
    }
}
