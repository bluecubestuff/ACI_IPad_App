/* 
 * Author: Lim Rui An Ryan, Lee Kwan Liang
 * Filename: ARCleanWokPotsPansLogic.cs
 * Description: The wok pots and pans game mode's game logic codes that will be run by the ARGameLogicSystem.
 */
using UnityEngine;

public class ARCleanWokPotsPansLogic : ARCleanModeLogic
{
    /// Private Variables
    private float WaterLevelIncrement = 1.5f;
    private Vector3 WaterOrigin;
    private Vector3 TapDesiredRotation = new Vector3(0, -180, -15);

    public enum Progress
    {
        P_Transfer = 0,
        P_Wash,
        P_Dry,
        P_Take,
        P_Return,
        P_Oil,
    }

    public static ARCleanDataStore.GameLocation GetGameLocationForCurrentPhase(int Phase)
    {
        switch ((Progress)Phase)
        {
            case Progress.P_Transfer:
                return ARCleanDataStore.GameLocation.GL_Stove;
            case Progress.P_Wash:
                return ARCleanDataStore.GameLocation.GL_Sink;
            case Progress.P_Dry:
                return ARCleanDataStore.GameLocation.GL_Sink;
            case Progress.P_Take:
                return ARCleanDataStore.GameLocation.GL_Sink;
            case Progress.P_Return:
                return ARCleanDataStore.GameLocation.GL_Stove;
            case Progress.P_Oil:
                return ARCleanDataStore.GameLocation.GL_Stove;
        }
        return ARCleanDataStore.GameLocation.GL_Undefined;
    }

    public void Update()
    {
        ProgressHeaderText.text = "Woks, Pots & Pans Daily Tasks";
        switch ((Progress)ARCleanDataStore.GetGamePhase())
        {
            case Progress.P_Transfer:
                Phase_Transfer();
                break;
            case Progress.P_Wash:
                Phase_Wash();
                break;
            case Progress.P_Dry:
                Phase_Dry();
                break;
            case Progress.P_Oil:
                Phase_Oil();
                break;
            case Progress.P_Take:
                Phase_Take();
                break;
            case Progress.P_Return:
                Phase_Return();
                break;
        }
    }

    private void Phase_Transfer()
    {
        /// Logic for transfer step
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Stove)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 1: Take all the kitchen wares!";
                //Arrows
                if(ARCleanDataStore.CameraCurrentDirection == ARCleanCamera.Directions.D_Forward)
                {
                    ArrowIndication.Instance.WokPotPanArrows[0].SetActive(true);    //Pot   
                    ArrowIndication.Instance.WokPotPanArrows[1].SetActive(true);    //Pan
                }
                else if(ARCleanDataStore.CameraCurrentDirection == ARCleanCamera.Directions.D_Up)
                {
                    ArrowIndication.Instance.WokPotPanArrows[2].SetActive(true);    //Wok   
                    ArrowIndication.Instance.WokPotPanArrows[3].SetActive(true);    //Fork and spoon
                }
                if (ARCleanDataStore.Inventory.IsObjectInInventory("Spoon") && ARCleanDataStore.Inventory.IsObjectInInventory("Fork"))
                    ArrowIndication.Instance.WokPotPanArrows[3].SetActive(false);
                if (ARCleanDataStore.Inventory.IsObjectInInventory("Grill Pan"))
                    ArrowIndication.Instance.WokPotPanArrows[1].SetActive(false);
                if (ARCleanDataStore.Inventory.IsObjectInInventory("Stock Pot"))
                    ArrowIndication.Instance.WokPotPanArrows[0].SetActive(false);
                if (ARCleanDataStore.Inventory.IsObjectInInventory("Metal Wok"))
                    ArrowIndication.Instance.WokPotPanArrows[2].SetActive(false);    
            }
            else ProgressSubHeaderText.text = "";

            ARCleanDataStore.ObjectInteractibleFlag = true;
            if (ARCleanDataStore.Inventory.IsObjectInInventory("Spoon") &&
                ARCleanDataStore.Inventory.IsObjectInInventory("Fork") &&
                ARCleanDataStore.Inventory.IsObjectInInventory("Grill Pan") &&
                ARCleanDataStore.Inventory.IsObjectInInventory("Stock Pot") &&
                ARCleanDataStore.Inventory.IsObjectInInventory("Metal Wok"))
            {
                ARCleanDataStore.ObjectInteractibleFlag = false;
                ARCleanDataStore.IncrementGameProgress();
                for(int i = 0; i < ArrowIndication.Instance.WokPotPanArrows.Length; ++i)
                {
                    ArrowIndication.Instance.WokPotPanArrows[i].SetActive(false);
                }
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the stove area to complete this stage.";
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
                        ProgressSubHeaderText.text = "Stage 3: Place the kitchen wares in the sink!";
                        if (!ArrowIndication.Instance.inventory.GetApplianceIsOpen())
                            ArrowIndication.Instance.inventoryArrow.SetActive(true);
                        else if (ArrowIndication.Instance.inventory.GetApplianceIsOpen())
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

                    if (!ARCleanDataStore.Inventory.IsObjectInInventory("Spoon") &&
                        !ARCleanDataStore.Inventory.IsObjectInInventory("Fork") &&
                        !ARCleanDataStore.Inventory.IsObjectInInventory("Grill Pan") &&
                        !ARCleanDataStore.Inventory.IsObjectInInventory("Stock Pot") &&
                        !ARCleanDataStore.Inventory.IsObjectInInventory("Metal Wok"))
                        ARCleanDataStore.GameModeGameState++;
                    
                    break;
                case 1:
                    if (ARCleanDataStore.ShowHints)
                    {
                        ProgressSubHeaderText.text = "Stage 4: Turn on the sink's tap.";
                        ArrowIndication.Instance.sideBarArrow.SetActive(false);
                        ArrowIndication.Instance.activityArrow.SetActive(false);
                        ArrowIndication.Instance.sinkArrow.SetActive(true);
                        ArrowIndication.Instance.inventoryArrow.SetActive(false);
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
                        ProgressSubHeaderText.text = "Stage 5: Wait for the water level to rise.";
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
                        ProgressSubHeaderText.text = "Stage 6: Scrub the kitchen wares using [Genie]\n [Genie] can be found in the laundry room.";
                        if(!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_HeavySpongeGenie)
                            ArrowIndication.Instance.toolArrow.SetActive(true);
                        else if(ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_HeavySpongeGenie)
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
                        ProgressSubHeaderText.text = "Stage 7: Water draining.";
                        ArrowIndication.Instance.activityArrow.SetActive(false);
                    }
                    else ProgressSubHeaderText.text = "";
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
            if(ARCleanDataStore.ShowHints)
            ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
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
                ProgressSubHeaderText.text = "Stage 8: Dry the kitchen wares using [Dry Cloth]\n [Dry Cloth] can be found in the sink's cabinet.";
                if (!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_DryCloth)
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
            else ProgressSubHeaderText.text = "";

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
            ProgressSubHeaderText.text = "You need be at the sink area to complete this stage.";
        }
    }

    private void Phase_Take()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Sink)
        {
            if (ARCleanDataStore.ShowHints)
                ProgressSubHeaderText.text = "Stage 9: Take all the kitchen wares!";
            else ProgressSubHeaderText.text = "";
            ARCleanDataStore.ObjectInteractibleFlag = true;
            // Check if condition has been fulfilled. If so, increment the state
            if (ARCleanDataStore.Inventory.IsObjectInInventory("Spoon") &&
                ARCleanDataStore.Inventory.IsObjectInInventory("Fork") &&
                ARCleanDataStore.Inventory.IsObjectInInventory("Grill Pan") &&
                ARCleanDataStore.Inventory.IsObjectInInventory("Stock Pot") &&
                ARCleanDataStore.Inventory.IsObjectInInventory("Metal Wok"))
            {
                ARCleanDataStore.ObjectInteractibleFlag = false;
                // Check if condition has been fulfilled. Since this is the last state, increment the gamemode and reset the phase flag
                ARCleanDataStore.IncrementGameProgress();
                if (ARCleanDataStore.ShowHints)
                    ArrowIndication.Instance.activityArrow.SetActive(false);
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ProgressSubHeaderText.text = "You need be at the sink area to complete this stage.";
        }
    }

    private void Phase_Return()
    {

        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Stove)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 10: Place all the kitchen wares back.";
                if (!ArrowIndication.Instance.inventory.GetApplianceIsOpen())
                    ArrowIndication.Instance.inventoryArrow.SetActive(true);
                else
                {
                    ArrowIndication.Instance.inventoryArrow.SetActive(false);
                    ArrowIndication.Instance.sideBarArrow.SetActive(true);
                }
            }
            else ProgressSubHeaderText.text = "";
            ARCleanDataStore.ObjectInteractibleFlag = true;
            // Check if condition has been fulfilled. If so, increment the state
            if (!ARCleanDataStore.Inventory.IsObjectInInventory("Spoon") &&
                !ARCleanDataStore.Inventory.IsObjectInInventory("Fork") &&
                !ARCleanDataStore.Inventory.IsObjectInInventory("Grill Pan") &&
                !ARCleanDataStore.Inventory.IsObjectInInventory("Stock Pot") &&
                !ARCleanDataStore.Inventory.IsObjectInInventory("Metal Wok"))
            {
                ARCleanDataStore.ObjectInteractibleFlag = false;
                ARCleanDataStore.IncrementGameProgress();
                if (ARCleanDataStore.ShowHints)
                {
                    ArrowIndication.Instance.sideBarArrow.SetActive(false);
                    ArrowIndication.Instance.inventoryArrow.SetActive(false);
                }
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ProgressSubHeaderText.text = "You need be at the stove area to complete this stage.";
        }
    }
    private void Phase_Oil()
    {
        /// Logic for oil step
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Stove)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 11: Oil the wok using [Cooking Oil]\n [Cooking Oil] can be found on top of the wok area.";
                if (ARCleanDataStore.LinkedToolInventory[(int)ARCleanDataStore.PlayerTool.PT_CookingOil].LockedIcon.activeSelf)
                    ArrowIndication.Instance.scanArrow.SetActive(true);
                else if (!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_CookingOil)
                    ArrowIndication.Instance.toolArrow.SetActive(true);
                else if (ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_CookingOil)
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
                CleanTool.SetToolHitPosition(Input.mousePosition, false, "ARC_Wok");
                if (ARCleanDataStore.CleanToolActive)
                {
                    if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Up, "Wrong Direction", "You need to be looking from above!", "You can't use your tool here...", ARCleanDataStore.PenaltyCounter < 3) ||
                        ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_CookingOil, "Wrong Tool", "You need to be using Cooking Oil!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
                    {
                        ARCleanDataStore.CleanToolActive = false;
                        ARCleanDataStore.PenaltyCounter++;
                        return;
                    }
                    //if (CleanTool.CleaningCheck(0.3f))
                    if (ARCleanDataStore.GetPlayerTool() == ARCleanDataStore.PlayerTool.PT_CookingOil)
                    {
                        ProgressBar.value += CleaningIncrement * Time.deltaTime;
                        if (ProgressBar.value >= 100f)
                        {
                            ProgressBar.value = 0f;
                            ARCleanDataStore.CleanToolActive = false;
                            // Reset Values for next run
                            ARCleanDataStore.PlayerInputFlagReset = false;
                            // Check if condition has been fulfilled. Since this is the last state, increment the gamemode and reset the phase flag
                            ARCleanDataStore.SetupForNextGameMode();
                            ArrowIndication.Instance.StopAllArrows();
                        }
                    }
                }
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ProgressSubHeaderText.text = "You need be at the stove area to complete this stage.";
        }
    }

}
