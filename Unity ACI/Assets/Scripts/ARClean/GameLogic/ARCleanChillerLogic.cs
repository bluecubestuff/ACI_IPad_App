/* 
 * Author: Lim Rui An Ryan, Lee Kwan Liang
 * Filename: ARCleanChillerLogic.cs
 * Description: The chiller game mode's game logic codes that will be run by the ARGameLogicSystem.
 */
using UnityEngine;

public class ARCleanChillerLogic : ARCleanModeLogic
{
    // Private Variables
    private Vector3 WaterOrigin;
    private Vector3 TapDesiredRotation = new Vector3(0, -180, -15);
    private float WaterLevelIncrement = 1.5f;

    public enum Progress
    {
        P_GetRacks = 0,
        P_ScrubRacks,
        P_WashRacks,
        P_DryRacks,
        P_WipeInterior,
        P_WashDoors,
        P_WipeDoors,
        P_ReassembleRacks,
    }

    public static ARCleanDataStore.GameLocation GetGameLocationForCurrentPhase(int Phase)
    {
        switch ((Progress)Phase)
        {
            case Progress.P_GetRacks:
                return ARCleanDataStore.GameLocation.GL_Chiller;
            case Progress.P_ScrubRacks:
                return ARCleanDataStore.GameLocation.GL_Sink;
            case Progress.P_WashRacks:
                return ARCleanDataStore.GameLocation.GL_Sink;
            case Progress.P_DryRacks:
                return ARCleanDataStore.GameLocation.GL_Sink;
            case Progress.P_WipeInterior:
                return ARCleanDataStore.GameLocation.GL_Chiller;
            case Progress.P_WashDoors:
                return ARCleanDataStore.GameLocation.GL_Chiller;
            case Progress.P_WipeDoors:
                return ARCleanDataStore.GameLocation.GL_Chiller;
            case Progress.P_ReassembleRacks:
                return ARCleanDataStore.GameLocation.GL_Chiller;
        }
        return ARCleanDataStore.GameLocation.GL_Undefined;
    }
    
    public void Update()
    {
        ProgressHeaderText.text = "Station Chiller Area Daily Tasks";
        switch ((Progress)ARCleanDataStore.GetGamePhase())
        {
            case Progress.P_GetRacks:
                Phase_GetRacks();
                break;
            case Progress.P_ScrubRacks:
                Phase_ScrubRacks();
                break;
            case Progress.P_WashRacks:
                Phase_WashRacks();
                break;
            case Progress.P_DryRacks:
                Phase_DryRacks();
                break;
            case Progress.P_WipeInterior:
                Phase_WipeInterior();
                break;
            case Progress.P_WashDoors:
                Phase_WashDoors();
                break;
            case Progress.P_WipeDoors:
                Phase_WipeDoors();
                break;
            case Progress.P_ReassembleRacks:
                Phase_ReassembleRacks();
                break;
        }
    }

    private void Phase_GetRacks()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Chiller)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 1: Take the chiller's racks.";
                ArrowIndication.Instance.activityArrow.SetActive(true);
            }
            else ProgressSubHeaderText.text = "";

            ARCleanDataStore.ObjectInteractibleFlag = true;
            if (ARCleanDataStore.Inventory.IsObjectInInventory("Chiller Rack 1") &&
                ARCleanDataStore.Inventory.IsObjectInInventory("Chiller Rack 2"))
            {
                ARCleanDataStore.ObjectInteractibleFlag = false;
                ARCleanDataStore.IncrementGameProgress();
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the station chiller area to complete this part of the stage.";
        }
    }

    private void Phase_ScrubRacks()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Sink)
        {
            switch (ARCleanDataStore.GameModeGameState)
            {
                case 0:
                    if (ARCleanDataStore.ShowHints)
                    {
                        ProgressSubHeaderText.text = "Stage 2: Place the chiller racks in the sink.";
                        if(!ArrowIndication.Instance.inventory.GetApplianceIsOpen())
                        {
                            ArrowIndication.Instance.inventoryArrow.SetActive(true);
                        }
                        else if(ArrowIndication.Instance.inventory.GetApplianceIsOpen())
                        {
                            ArrowIndication.Instance.inventoryArrow.SetActive(false);
                            ArrowIndication.Instance.sideBarArrow.SetActive(true);
                            ArrowIndication.Instance.activityArrow.SetActive(true);
                        }
                    }
                    else ProgressSubHeaderText.text = "";

                    if (ARCleanDataStore.PlayerInputFlag && ARCleanDataStore.Inventory.InputInInventory(Input.mousePosition, ARInventoryManager.InventoryState.IS_Appliance))
                        if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Up, "Wrong Direction", "You need to be looking from above!", "You can't place object at this angle...", ARCleanDataStore.PenaltyCounter < 3))
                        {
                            ARCleanDataStore.ObjectInteractibleFlag = false;
                            ARCleanDataStore.PenaltyCounter++;
                        }
                        else
                            ARCleanDataStore.ObjectInteractibleFlag = true;

                    if (!ARCleanDataStore.Inventory.IsObjectInInventory("Chiller Rack 1") &&
                        !ARCleanDataStore.Inventory.IsObjectInInventory("Chiller Rack 2"))
                    {
                        ARCleanDataStore.GameModeGameState++;
                        ArrowIndication.Instance.inventoryArrow.SetActive(false);
                        ArrowIndication.Instance.sideBarArrow.SetActive(false);
                    }

                    break;
                case 1:
                    if (ARCleanDataStore.ShowHints)
                    {
                        ProgressSubHeaderText.text = "Stage 3: Scrub the racks in the sink using [Genie]\n [Genie] can be found in the laundry room.";
                        if (!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_HeavySpongeGenie)
                        {
                            ArrowIndication.Instance.activityArrow.SetActive(false);
                            ArrowIndication.Instance.toolArrow.SetActive(true);
                        }
                        else if (ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_HeavySpongeGenie)
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
                        CleanTool.SetToolHitPosition(Input.mousePosition, true, "ARC_Interactables");
                        if (ARCleanDataStore.CleanToolActive)
                            if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_HeavySpongeGenie, "Wrong Tool", "You need to be using Genie!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
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
                                ARCleanDataStore.GameModeGameState = 0;
                                ProgressBar.value = 0f;
                                ARCleanDataStore.CleanToolActive = false;
                                // Reset Values for next run
                                ARCleanDataStore.PlayerInputFlagReset = false;
                                ARCleanDataStore.IncrementGameProgress();
                            }
                        }
                    }
                    break;
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the sink area to complete this part of the stage.";
        }
    }

    private void Phase_WashRacks()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Sink)
        {
            switch (ARCleanDataStore.GameModeGameState)
            {
                case 0:
                    if (ARCleanDataStore.ShowHints)
                    {
                        ProgressSubHeaderText.text = "Stage 4: Turn on the sink's tap.";
                        ArrowIndication.Instance.sinkArrow.SetActive(true);
                        ArrowIndication.Instance.activityArrow.SetActive(false);
                    }
                    else ProgressSubHeaderText.text = "";

                    ARCleanDataStore.ObjectInteractibleFlag = false;
                    if (ARCleanDataStore.PlayerInputFlag && !ARCleanDataStore.CheckIfMouseIsOverUI())
                    {
                        if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Up, "Wrong Direction", "You need to be looking from above!", "You can't place object at this angle...", ARCleanDataStore.PenaltyCounter < 3))
                            return;
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        Physics.Raycast(ray, out hit);
                        if (hit.transform != null)
                        {
                            if (hit.transform.gameObject == ARCleanDataStore.ModelAccess.Sink_TapHandle)
                            {
                                Transform Temp = ARCleanDataStore.ModelAccess.Sink_TapHandle.transform;
                                Temp.eulerAngles = TapDesiredRotation;
                                ARCleanDataStore.ModelAccess.Sink_TapHandle.transform.rotation = Quaternion.Lerp(ARCleanDataStore.ModelAccess.Sink_TapHandle.transform.rotation, Temp.rotation, Time.time * 5f);
                                ARCleanDataStore.GameModeGameState++;
                                WaterOrigin = ARCleanDataStore.ModelAccess.Sink_Water.transform.position;
                                ARCleanDataStore.ModelAccess.Sink_Particles.GetComponent<ParticleSystem>().Play();
                                ProgressBar.value = 50;
                                
                            }
                        }
                    }
                    break;
                case 1:
                    if (ARCleanDataStore.ShowHints)
                    {
                        ProgressSubHeaderText.text = "Stage 5: Wait for the water level to rise.";
                        ArrowIndication.Instance.sinkArrow.SetActive(false);
                    }
                    else ProgressSubHeaderText.text = "";

                    ARCleanDataStore.ObjectInteractibleFlag = false;
                    ProgressBar.value += 6f * Time.deltaTime;
                    if (ProgressBar.value < 100f)
                        ARCleanDataStore.ModelAccess.Sink_Water.transform.position = WaterOrigin + new Vector3(0, WaterLevelIncrement * ((ProgressBar.value - 50f) / 50f));
                    else
                    {
                        ARCleanDataStore.ModelAccess.Sink_Particles.GetComponent<ParticleSystem>().Stop();
                        Transform Temp = ARCleanDataStore.ModelAccess.Sink_TapHandle.transform;
                        Temp.eulerAngles = new Vector3(0, -180);
                        ARCleanDataStore.ModelAccess.Sink_TapHandle.transform.rotation = Quaternion.Lerp(ARCleanDataStore.ModelAccess.Sink_TapHandle.transform.rotation, Temp.rotation, Time.time * 5f);
                        ProgressBar.value = 0f;
                        ARCleanDataStore.GameModeGameState++;
                    }
                    break;
                case 2:
                    if (ARCleanDataStore.ShowHints)
                        ProgressSubHeaderText.text = "Stage 6: Water draining.";
                    else ProgressSubHeaderText.text = "";

                    ARCleanDataStore.ObjectInteractibleFlag = false;
                    ProgressBar.value += 20f * Time.deltaTime;
                    if (ProgressBar.value < 100f)
                        ARCleanDataStore.ModelAccess.Sink_Water.transform.position = WaterOrigin + new Vector3(0, WaterLevelIncrement * (1 - ((ProgressBar.value) / 100f)));
                    else
                    {
                        ProgressBar.value = 0f;
                        ARCleanDataStore.GameModeGameState = 0;
                        ARCleanDataStore.IncrementGameProgress();
                    }
                    break;
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the sink area to complete this part of the stage.";
        }
    }

    private void Phase_DryRacks()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Sink)
        {
            switch (ARCleanDataStore.GameModeGameState)
            {
                case 0:
                    if (ARCleanDataStore.ShowHints)
                    {
                        ProgressSubHeaderText.text = "Stage 7: Dry the racks in the sink using the [Dry Cloth]\n [Dry Cloth] can be found in the sink cabinet.";
                        if(!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_DryCloth)
                            ArrowIndication.Instance.toolArrow.SetActive(true);
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
                    else
                        ProgressSubHeaderText.text = "";

                    ARCleanDataStore.ObjectInteractibleFlag = false;
                    if (ARCleanDataStore.PlayerInputFlag && !ARCleanDataStore.CheckIfMouseIsOverUI())
                    {
                        CleanTool.SetToolHitPosition(Input.mousePosition, true, "ARC_Interactables");
                        if (ARCleanDataStore.CleanToolActive)
                            if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Up, "Wrong Direction", "You need to be looking from above!", "You can't use your tool here...", ARCleanDataStore.PenaltyCounter < 3) ||
                                ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_DryCloth, "Wrong Tool", "You need to be using Dry Cloth!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
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
                                ARCleanDataStore.GameModeGameState++;
                            }
                        }
                    }
                    break;
                case 1:
                    if (ARCleanDataStore.ShowHints)
                        ProgressSubHeaderText.text = "Stage 8: Take the chiller's racks.";
                    else ProgressSubHeaderText.text = "";

                    ARCleanDataStore.ObjectInteractibleFlag = true;
                    if (ARCleanDataStore.Inventory.IsObjectInInventory("Chiller Rack 1") &&
                        ARCleanDataStore.Inventory.IsObjectInInventory("Chiller Rack 2"))
                    {
                        ArrowIndication.Instance.activityArrow.SetActive(false);
                        ARCleanDataStore.ObjectInteractibleFlag = false;
                        ARCleanDataStore.GameModeGameState = 0;
                        ARCleanDataStore.IncrementGameProgress();
                    }
                    break;
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the sink area to complete this part of the stage.";
        }
    }

    private void Phase_WipeInterior()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Chiller)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 9: Wipe interior of chiller using the [Dry Cloth]\n [Dry Cloth] can be found in the sink's cabinet room.";
                if(!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_DryCloth)
                {
                    ArrowIndication.Instance.activityArrow.SetActive(false);
                    ArrowIndication.Instance.toolArrow.SetActive(true);
                }
                else if(ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_DryCloth)
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
                CleanTool.SetToolHitPosition(Input.mousePosition, true, "ARC_Interactables");
                if (ARCleanDataStore.CleanToolActive)
                    //* CLOTH
                    if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Forward, "Wrong Direction", "You need to be looking from the front!", "You can't use your tool here...", ARCleanDataStore.PenaltyCounter < 3) ||
                        ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_DryCloth, "Wrong Tool", "You need to be using Dry Cloth!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
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
                        ARCleanDataStore.IncrementGameProgress();
                    }
                }
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the station chiller area to complete this part of the stage.";
        }
    }

    private void Phase_WashDoors()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Chiller)
        {
            //* CLOSE DOOR
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 10: Wash the chiller doors using [Genie]\n [Genie] can be found in the laundry room.";
                if (!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_HeavySpongeGenie)
                {
                    ArrowIndication.Instance.activityArrow.SetActive(false);
                    ArrowIndication.Instance.toolArrow.SetActive(true);
                }
                else if (ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_HeavySpongeGenie)
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

            ARCleanDataStore.ForceCurrentDoorClose = true;
            ARCleanDataStore.ObjectInteractibleFlag = false;
            if (ARCleanDataStore.PlayerInputFlag && !ARCleanDataStore.CheckIfMouseIsOverUI())
            {
                CleanTool.SetToolHitPosition(Input.mousePosition, true, "ARC_Door");
                if (ARCleanDataStore.CleanToolActive)
                    if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Forward, "Wrong Direction", "You need to be looking from the front!", "You can't use your tool here...", ARCleanDataStore.PenaltyCounter < 3) ||
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
                        ARCleanDataStore.IncrementGameProgress();
                    }
                }
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the station chiller area to complete this part of the stage.";
        }
    }

    private void Phase_WipeDoors()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Chiller)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 11: Dry the chiller doors using the [Dry Cloth]\n [Dry Cloth] can be found in the sink's cabinet.";
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

            ARCleanDataStore.ForceCurrentDoorClose = true;
            ARCleanDataStore.ObjectInteractibleFlag = false;
            if (ARCleanDataStore.PlayerInputFlag && !ARCleanDataStore.CheckIfMouseIsOverUI())
            {
                CleanTool.SetToolHitPosition(Input.mousePosition, true, "ARC_Door");
                if (ARCleanDataStore.CleanToolActive)
                    if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Forward, "Wrong Direction", "You need to be looking from the front!", "You can't use your tool here...", ARCleanDataStore.PenaltyCounter < 3) ||
                        ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_DryCloth, "Wrong Tool", "You need to be using Dry Cloth!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
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
                        ARCleanDataStore.ForceCurrentDoorClose = false;
                        ARCleanDataStore.CleanToolActive = false;
                        // Reset Values for next run
                        ARCleanDataStore.PlayerInputFlagReset = false;
                        ARCleanDataStore.IncrementGameProgress();
                    }
                }
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the station chiller area to complete this part of the stage.";
        }
    }

    private void Phase_ReassembleRacks()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Chiller)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 12: Reassemble the racks.";
                if(!ArrowIndication.Instance.inventory.GetApplianceIsOpen())
                {
                    ArrowIndication.Instance.activityArrow.SetActive(false);
                    ArrowIndication.Instance.inventoryArrow.SetActive(true);
                }
                else
                {
                    ArrowIndication.Instance.inventoryArrow.SetActive(false);
                    ArrowIndication.Instance.sideBarArrow.SetActive(true);
                    ArrowIndication.Instance.activityArrow.SetActive(true);
                }
            }
            else ProgressSubHeaderText.text = "";

            if (ARCleanDataStore.PlayerInputFlag && ARCleanDataStore.Inventory.InputInInventory(Input.mousePosition, ARInventoryManager.InventoryState.IS_Appliance))
                if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Forward, "Wrong Direction", "You need to be looking from the front!", "You can't place object at this angle...", ARCleanDataStore.PenaltyCounter < 3) ||
                    ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CurrentSceneDoor.IsAnimationOpen(), true, "Object not Interacted", "You need to open the chiller door!", "The door should be open...", ARCleanDataStore.PenaltyCounter < 3))
                {
                    ARCleanDataStore.ObjectInteractibleFlag = false;
                    ARCleanDataStore.PenaltyCounter++;
                }
                else
                    ARCleanDataStore.ObjectInteractibleFlag = true;

            if (!ARCleanDataStore.Inventory.IsObjectInInventory("Chiller Rack 1") &&
                !ARCleanDataStore.Inventory.IsObjectInInventory("Chiller Rack 2"))
            {
                ARCleanDataStore.SetupForNextGameMode();
                ArrowIndication.Instance.StopAllArrows();
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the station chiller area to complete this part of the stage.";
        }
    }
}
