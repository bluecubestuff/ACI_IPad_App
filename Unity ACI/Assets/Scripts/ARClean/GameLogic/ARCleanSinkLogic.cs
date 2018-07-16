/* 
 * Author: Lim Rui An Ryan, Lee Kwan Liang
 * Filename: ARCleanSinkLogic.cs
 * Description: The sink game mode's game logic codes that will be run by the ARGameLogicSystem.
 */
using UnityEngine;

public class ARCleanSinkLogic : ARCleanModeLogic
{
    // Private Variables
    private float WaterLevelIncrement = 1.5f;
    private Vector3 WaterOrigin;
    private Vector3 TapDesiredRotation = new Vector3(0, -180, -15);

    public enum Progress
    {
        P_TakeTrash = 0,
        P_Discard,
        P_TakeContainer,
        P_Wash,
        P_Return,
        P_Scrub,
        P_Rinse,
        P_Dry,
    }

    public static ARCleanDataStore.GameLocation GetGameLocationForCurrentPhase(int Phase)
    {
        switch ((Progress)Phase)
        {
            case Progress.P_TakeTrash:
                return ARCleanDataStore.GameLocation.GL_Prep;
            case Progress.P_Discard:
                return ARCleanDataStore.GameLocation.GL_Trash;
            case Progress.P_TakeContainer:
                return ARCleanDataStore.GameLocation.GL_Prep;
            case Progress.P_Wash:
                return ARCleanDataStore.GameLocation.GL_Sink;
            case Progress.P_Return:
                return ARCleanDataStore.GameLocation.GL_Prep;
            case Progress.P_Scrub:
                return ARCleanDataStore.GameLocation.GL_Sink;
            case Progress.P_Rinse:
                return ARCleanDataStore.GameLocation.GL_Sink;
            case Progress.P_Dry:
                return ARCleanDataStore.GameLocation.GL_Sink;
        }
        return ARCleanDataStore.GameLocation.GL_Undefined;
    }

    public void Update()
    {
        ProgressHeaderText.text = "Sink Area Daily Tasks";
        switch ((Progress)ARCleanDataStore.GetGamePhase())
        {
            case Progress.P_TakeTrash:
                Phase_TakeTrash();
                break;
            case Progress.P_Discard:
                Phase_Discard();
                break;
            case Progress.P_TakeContainer:
                Phase_TakeContainer();
                break;
            case Progress.P_Wash:
                Phase_Wash();
                break;
            case Progress.P_Return:
                Phase_Return();
                break;
            case Progress.P_Scrub:
                Phase_Scrub();
                break;
            case Progress.P_Rinse:
                Phase_Rinse();
                break;
            case Progress.P_Dry:
                Phase_Dry();
                break;
        }
    }

    private void Phase_TakeTrash()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Prep)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 1: Take the trash.";
                if(ARCleanDataStore.CameraCurrentDirection == ARCleanCamera.Directions.D_Up)
                    ArrowIndication.Instance.TrashArrow.SetActive(true);
            }
            else ProgressSubHeaderText.text = "";

            ARCleanDataStore.RequiredObject = "Trash";

            if (ARCleanDataStore.CameraCurrentDirection != ARCleanCamera.Directions.D_Up)
            {
                ARCleanDataStore.ObjectInteractibleFlag = false;
                if (ARCleanDataStore.PlayerInputFlag && !ARCleanDataStore.CheckIfMouseIsOverUI())
                {
                    ARCleanDataStore.ModelAccess.SetupErrorInterface("Wrong Direction", "You need to be looking from above!", "You can't take object at this angle...", ARCleanDataStore.PenaltyCounter < 3);
                    ARCleanDataStore.PenaltyCounter++;
                }
            }
            else
                ARCleanDataStore.ObjectInteractibleFlag = true;

            if (ARCleanDataStore.Inventory.IsObjectInInventory("Trash"))
            {
                ArrowIndication.Instance.TrashArrow.SetActive(false);
                ARCleanDataStore.RequiredObject = "";
                ARCleanDataStore.IncrementGameProgress();
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the preparation area to complete this stage.";
        }
    }

    private void Phase_Discard()
    {
        /// Logic for discard step
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Trash)
        {
            if (ARCleanDataStore.ShowHints)
            {
                if (!ArrowIndication.Instance.inventory.GetApplianceIsOpen())
                    ArrowIndication.Instance.inventoryArrow.SetActive(true);
                else
                {
                    ArrowIndication.Instance.inventoryArrow.SetActive(false);
                    ArrowIndication.Instance.sideBarArrow.SetActive(true);
                    ArrowIndication.Instance.activityArrow.SetActive(true);
                }

                ProgressSubHeaderText.text = "Stage 2: Discard the trash.";
            }
            else ProgressSubHeaderText.text = "";

            if (ARCleanDataStore.PlayerInputFlag && ARCleanDataStore.Inventory.InputInInventory(Input.mousePosition, ARInventoryManager.InventoryState.IS_Appliance))
                if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Up, "Wrong Direction", "You need to be looking from above!", "You can't place object at this angle...", ARCleanDataStore.PenaltyCounter < 3) ||
                    ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CurrentSceneDoor.IsAnimationOpen(), true, "Object not Interacted", "You need to open the trash lid!", "The lid should be open...", ARCleanDataStore.PenaltyCounter < 3))
                {
                    ARCleanDataStore.ObjectInteractibleFlag = false;
                    ARCleanDataStore.PenaltyCounter++;
                    if(ARCleanDataStore.ShowHints)
                        ArrowIndication.Instance.activityArrow.SetActive(true);
                }
                else
                {

                    ARCleanDataStore.ObjectInteractibleFlag = true;
                }

            if (!ARCleanDataStore.Inventory.IsObjectInInventory("Trash"))
            {
                if (ARCleanDataStore.ShowHints)
                {
                    ArrowIndication.Instance.inventoryArrow.SetActive(false);
                    ArrowIndication.Instance.activityArrow.SetActive(false);
                    ArrowIndication.Instance.sideBarArrow.SetActive(false);
                }
                ARCleanDataStore.IncrementGameProgress();
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);            
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the trash area to complete this stage.";
        }
    }

    private void Phase_TakeContainer()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Prep)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 3: Take the metal bin!";
                if (ARCleanDataStore.CameraCurrentDirection == ARCleanCamera.Directions.D_Up)
                    ArrowIndication.Instance.TrashArrow.SetActive(true);
            }
            else ProgressSubHeaderText.text = "";
            
            if (ARCleanDataStore.CameraCurrentDirection != ARCleanCamera.Directions.D_Up)
            {
                ARCleanDataStore.ObjectInteractibleFlag = false;
                if (ARCleanDataStore.PlayerInputFlag && !ARCleanDataStore.CheckIfMouseIsOverUI())
                {
                    ARCleanDataStore.ModelAccess.SetupErrorInterface("Wrong Direction", "You need to be looking from above!", "You can't take object at this angle...", ARCleanDataStore.PenaltyCounter < 3);
                    ARCleanDataStore.PenaltyCounter++;
                }
            }
            else
                ARCleanDataStore.ObjectInteractibleFlag = true;

            if (ARCleanDataStore.Inventory.IsObjectInInventory("Metal Bin"))
            {
                ARCleanDataStore.IncrementGameProgress();
                if (ARCleanDataStore.ShowHints)
                    ArrowIndication.Instance.TrashArrow.SetActive(false);
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the preparation area to complete this stage.";
        }
    }

    private void Phase_Wash()
    {
        /// Logic for washing step
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Sink)
        {
            switch (ARCleanDataStore.GameModeGameState)
            {
                case 0:
                    if (ARCleanDataStore.ShowHints)
                    {
                        ProgressSubHeaderText.text = "Stage 4: Place the Metal Bin in the sink!";
                        if (!ArrowIndication.Instance.inventory.GetApplianceIsOpen())
                            ArrowIndication.Instance.inventoryArrow.SetActive(true);
                        else
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
                        {

                            ARCleanDataStore.ObjectInteractibleFlag = true;
                        }

                    if (!ARCleanDataStore.Inventory.IsObjectInInventory("Metal Bin"))
                    {
                        ARCleanDataStore.GameModeGameState++;
                        ArrowIndication.Instance.inventoryArrow.SetActive(false);
                        ArrowIndication.Instance.sideBarArrow.SetActive(false);
                        ArrowIndication.Instance.activityArrow.SetActive(false);
                    }
                    break;
                case 1:
                    if (ARCleanDataStore.ShowHints)
                    {
                        ProgressSubHeaderText.text = "Stage 5: Turn on the sink's tap.";
                        ArrowIndication.Instance.sinkArrow.SetActive(true);
                    }
                    else ProgressSubHeaderText.text = "";
                    ARCleanDataStore.ObjectInteractibleFlag = false;

                    if (ARCleanDataStore.PlayerInputFlag)
                    {
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
                case 2:
                    if (ARCleanDataStore.ShowHints)
                    {
                        ProgressSubHeaderText.text = "Stage 6: Wait for the water level to rise.";
                        ArrowIndication.Instance.sinkArrow.SetActive(false);
                    }
                    else ProgressSubHeaderText.text = "";
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
                case 3:
                    if (ARCleanDataStore.ShowHints)
                    {
                        ProgressSubHeaderText.text = "Stage 7: Scrub the Metal Bin using [Genie]\n [Genie] can be found in the laundry room.";
                        ArrowIndication.Instance.sinkArrow.SetActive(false);
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
                            ArrowIndication.Instance.activityArrow.SetActive(true);
                        }

                    }
                    else ProgressSubHeaderText.text = "";

                    ARCleanDataStore.ObjectInteractibleFlag = false;
                    if (ARCleanDataStore.PlayerInputFlag && !ARCleanDataStore.CheckIfMouseIsOverUI())
                    {
                        CleanTool.SetToolHitPosition(Input.mousePosition, true, "ARC_Interactables");
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
                                ARCleanDataStore.GameModeGameState++;
                            }
                        }
                    }
                    break;
                case 4:
                    if (ARCleanDataStore.ShowHints)
                    {
                        ProgressSubHeaderText.text = "Stage 8: Water draining.";
                        ArrowIndication.Instance.activityArrow.SetActive(false);
                    }
                    else ProgressSubHeaderText.text = "";
                    ProgressBar.value += 20f * Time.deltaTime;

                    if (ProgressBar.value < 100f)
                        ARCleanDataStore.ModelAccess.Sink_Water.transform.position = WaterOrigin + new Vector3(0, WaterLevelIncrement * (1 - ((ProgressBar.value) / 100f)));
                    else{
                        ARCleanDataStore.GameModeGameState++;
                    }
                    break;
                case 5:
                    if (ARCleanDataStore.ShowHints)
                    {
                        ProgressSubHeaderText.text = "Stage 9: Take the Metal Bin.";
                        ArrowIndication.Instance.activityArrow.SetActive(true);
                    }


                    else ProgressSubHeaderText.text = "";
                    ARCleanDataStore.ObjectInteractibleFlag = true;
                    // Check if condition has been fulfilled. If so, increment the state
                    if (ARCleanDataStore.Inventory.IsObjectInInventory("Metal Bin"))
                    {
                        // Reset the values
                        ProgressBar.value = 0f;
                        ARCleanDataStore.GameModeGameState = 0;
                        ARCleanDataStore.ObjectInteractibleFlag = false;
                        // Since this is the last state, increment the gamemode and reset the phase flag
                        ARCleanDataStore.IncrementGameProgress();
                        ArrowIndication.Instance.activityArrow.SetActive(false);
                    }
                    break;
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the sink area to complete this stage.";
        }
    }

    private void Phase_Return()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Prep)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 10: Return the metal bin to the preparation area.";
                if (!ArrowIndication.Instance.inventory.GetApplianceIsOpen())
                    ArrowIndication.Instance.inventoryArrow.SetActive(true);
                else
                {
                    ArrowIndication.Instance.inventoryArrow.SetActive(false);
                    ArrowIndication.Instance.sideBarArrow.SetActive(true);
                    ArrowIndication.Instance.TrashArrow.SetActive(true);
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

            if (!ARCleanDataStore.Inventory.IsObjectInInventory("Metal Bin"))
            {
                ARCleanDataStore.ObjectInteractibleFlag = false;
                ARCleanDataStore.IncrementGameProgress();
                ArrowIndication.Instance.inventoryArrow.SetActive(false);
                ArrowIndication.Instance.sideBarArrow.SetActive(false);
                ArrowIndication.Instance.TrashArrow.SetActive(false);
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the preparation area to complete this stage.";
        }
    }

    private void Phase_Scrub()
    {
        /// Logic for scrubbing step
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Sink)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 11: Scrub the sink's surface using [Genie]\n [Genie] can be found in the laundry room.";
                if(!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_HeavySpongeGenie)
                {
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
                CleanTool.SetToolHitPosition(Input.mousePosition, true, "", "ARC_Door");
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
                        ARCleanDataStore.IncrementGameProgress();
                    }
                }
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ProgressSubHeaderText.text = "You need be at the sink area to complete this stage.";
        }
    }

    private void Phase_Rinse()
    {
        /// Logic for rinse step
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Sink)
        {
            switch (ARCleanDataStore.GameModeGameState)
            {
                case 0:
                    if (ARCleanDataStore.ShowHints)
                    {
                        ProgressSubHeaderText.text = "Stage 12: Turn on the sink's tap.";
                        ArrowIndication.Instance.activityArrow.SetActive(false);
                        ArrowIndication.Instance.sinkArrow.SetActive(true);
                    }
                    else ProgressSubHeaderText.text = "";
                    if (ARCleanDataStore.PlayerInputFlag)
                    {
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
                            }
                        }
                    }
                    break;
                case 1:
                    if (ARCleanDataStore.ShowHints)
                    {
                        ProgressSubHeaderText.text = "Stage 13: Let the sink rinse in hot water.";
                        ArrowIndication.Instance.activityArrow.SetActive(false);
                    }
                    else ProgressSubHeaderText.text = "";
                    ProgressBar.value += 7.5f * Time.deltaTime;
                    if (ProgressBar.value < 45f)
                        ARCleanDataStore.ModelAccess.Sink_Water.transform.position = WaterOrigin + new Vector3(0, WaterLevelIncrement * ProgressBar.value / 45f);
                    else if (ProgressBar.value > 45f && ProgressBar.value <= 55f)
                    {
                        ARCleanDataStore.ModelAccess.Sink_Particles.GetComponent<ParticleSystem>().Stop();
                        Transform Temp = ARCleanDataStore.ModelAccess.Sink_TapHandle.transform;
                        Temp.eulerAngles = new Vector3(0, -180);
                        ARCleanDataStore.ModelAccess.Sink_TapHandle.transform.rotation = Quaternion.Lerp(ARCleanDataStore.ModelAccess.Sink_TapHandle.transform.rotation, Temp.rotation, Time.time * 5f);
                    }
                    else if (ProgressBar.value <= 99f)
                        ARCleanDataStore.ModelAccess.Sink_Water.transform.position = WaterOrigin + new Vector3(0, WaterLevelIncrement * (1 - ((ProgressBar.value - 55f) / 45f)));
                    else if (ProgressBar.value >= 99f)
                    {
                        ARCleanDataStore.GameModeGameState = 0;
                        ARCleanDataStore.IncrementGameProgress();
                        ProgressBar.value = 0f;
                    }
                    break;
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ProgressSubHeaderText.text = "You need be at the sink area to complete this stage.";
        }
    }

    private void Phase_Dry()
    {
        /// Logic for dry step
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Sink)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ArrowIndication.Instance.sinkArrow.SetActive(false);
                ProgressSubHeaderText.text = "Stage 14: Dry the sink's surface using [Dry Cloth]\n [Dry Cloth] can be found in the sink's cabinet.";
                if (!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_DryCloth)
                    ArrowIndication.Instance.toolArrow.SetActive(true);
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
                CleanTool.SetToolHitPosition(Input.mousePosition, true, "", "ARC_Door");
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
                    ProgressBar.value += CleaningIncrement *Time.deltaTime;
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
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ProgressSubHeaderText.text = "You need be at the sink area to complete this stage.";
        }
    }
}
