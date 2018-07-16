/* 
 * Author: Lim Rui An Ryan, Lee Kwan Liang
 * Filename: ARCleanFloorDrainsLogic.cs
 * Description: The floor game mode's game logic codes that will be run by the ARGameLogicSystem.
 */
using UnityEngine;

public class ARCleanFloorDrainsLogic : ARCleanModeLogic
{
    // Private Variables
    private int AssembleOrder = 0;

    public enum Progress
    {
        P_Sweep = 0,
        P_WetFloor,
        P_ScrubFloor,
        P_RinseFloor,
        P_RemoveGrill,
        P_ScrubDownDrain,
        P_WashDownDrain,
        P_TakeFilter,
        P_ScrubFilter,
        P_ReassembleFloor,
        P_DryFloor,
    }

    public static ARCleanDataStore.GameLocation GetGameLocationForCurrentPhase(int Phase)
    {
        switch ((Progress)Phase)
        {
            case Progress.P_TakeFilter:
            case Progress.P_ScrubFilter:
                return ARCleanDataStore.GameLocation.GL_Sink;
            default:
                return ARCleanDataStore.GameLocation.GL_Floor;
        }
    }

    public void Update()
    {
        ProgressHeaderText.text = "Floor Area Daily Tasks";

        switch ((Progress)ARCleanDataStore.GetGamePhase())
        {
            case Progress.P_Sweep:
                Phase_Sweep();
                break;
            case Progress.P_WetFloor:
                Phase_WetFloor();
                break;
            case Progress.P_ScrubFloor:
                Phase_ScrubFloor();
                break;
            case Progress.P_RinseFloor:
                Phase_RinseFloor();
                break;
            case Progress.P_RemoveGrill:
                Phase_RemoveGrill();
                break;
            case Progress.P_ScrubDownDrain:
                Phase_ScrubDownDrain();
                break;
            case Progress.P_WashDownDrain:
                Phase_WashDownDrain();
                break;
            case Progress.P_TakeFilter:
                Phase_TakeFilter();
                break;
            case Progress.P_ScrubFilter:
                Phase_ScrubFilter();
                break;
            case Progress.P_ReassembleFloor:
                Phase_ReassembleFloor();
                break;
            case Progress.P_DryFloor:
                Phase_DryFloor();
                break;
        }
    }

    private void Phase_Sweep()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Floor)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 1: Sweep the floor using [Broom]\n [Broom] can be found in the laundry room.";
                if(!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Broom)
                    ArrowIndication.Instance.toolArrow.SetActive(true);
                else if (ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Broom)
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
            if (ARCleanDataStore.PlayerInputFlag)
            {
                CleanTool.SetToolHitPosition(Input.mousePosition, true, "ARC_Interactables");
                if (ARCleanDataStore.CleanToolActive)
                    if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_Broom, "Wrong Tool", "You need to be using the Broom!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
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
            ProgressSubHeaderText.text = "You need be at the floor area to complete this part of the stage.";
        }
    }

    private void Phase_WetFloor()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Floor)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 2: Wet the floor using [Alkaclean]\n [Alkaclean] can be found in the laundry room.";
                if(!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Alkaclean)
                {
                    ArrowIndication.Instance.activityArrow.SetActive(false);
                    ArrowIndication.Instance.toolArrow.SetActive(true);
                }
                else if (ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Alkaclean)
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
            if (ARCleanDataStore.PlayerInputFlag)
            {
                CleanTool.SetToolHitPosition(Input.mousePosition, true, "ARC_Interactables");
                if (ARCleanDataStore.CleanToolActive)
                    if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_Alkaclean, "Wrong Tool", "You need to be using Alkaclean!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
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
            ProgressSubHeaderText.text = "You need be at the floor area to complete this part of the stage.";
        }
    }

    private void Phase_ScrubFloor()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Floor)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 3: Scrub the floor using [Hard Brush]\n [Hard Brush] can be found in the laundry room.";
                if(!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_HardBrush)
                {
                    ArrowIndication.Instance.activityArrow.SetActive(false);
                    ArrowIndication.Instance.toolArrow.SetActive(true);
                }
                else if (ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_HardBrush)
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

            if (ARCleanDataStore.PlayerInputFlag)
            {
                CleanTool.SetToolHitPosition(Input.mousePosition, true, "ARC_Interactables");
                if (ARCleanDataStore.CleanToolActive)
                    if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_HardBrush, "Wrong Tool", "You need to be using the Hard Brush!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
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
            ProgressSubHeaderText.text = "You need be at the floor area to complete this part of the stage.";
        }
    }

    private void Phase_RinseFloor()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Floor)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 4: Rinse the floor using the [Pail of Water]\n [Pail of Water] can be found in the laundry room.";
                if(!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Water)
                {
                    ArrowIndication.Instance.activityArrow.SetActive(false);
                    ArrowIndication.Instance.toolArrow.SetActive(true);
                }
                else if (ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Water)
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

            if (ARCleanDataStore.PlayerInputFlag)
            {
                CleanTool.SetToolHitPosition(Input.mousePosition, false, "ARC_Interactables");
                if (ARCleanDataStore.CleanToolActive)
                    if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_Water, "Wrong Tool", "You need to be using the Water!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
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
            ProgressSubHeaderText.text = "You need be at the floor area to complete this part of the stage.";
        }
    }

    private void Phase_RemoveGrill()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Floor)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 5: Remove the drain grills using [Protective Metal Gloves]\n [Protective Metal Gloves] can be found in the sink cabinet.";
                if(!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Glove)
                {
                    ArrowIndication.Instance.activityArrow.SetActive(false);
                    ArrowIndication.Instance.toolArrow.SetActive(true);
                }
                else if (ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Glove)
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

            //* METAL GLOVE

            if (ARCleanDataStore.GetPlayerTool() == ARCleanDataStore.PlayerTool.PT_Glove) ARCleanDataStore.ObjectInteractibleFlag = true;
            else ARCleanDataStore.ObjectInteractibleFlag = false;

            if (ARCleanDataStore.PlayerInputFlag && !ARCleanDataStore.CheckIfMouseIsOverUI())
                if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_Glove, "Wrong Tool", "You need to be using the Metal Glove!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
                {
                    ARCleanDataStore.PenaltyCounter++;
                    return;
                }

            if (ARCleanDataStore.Inventory.IsObjectInInventory("Metal Grill 1") &&
                ARCleanDataStore.Inventory.IsObjectInInventory("Metal Grill 2") &&
                ARCleanDataStore.Inventory.IsObjectInInventory("Metal Grill 3"))
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
            ProgressSubHeaderText.text = "You need be at the floor area to complete this part of the stage.";
        }
    }

    private void Phase_ScrubDownDrain()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Floor)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 6: Scrub down the drains using [Hard Brush]\n [Hard Brush] can be found in the laundry room.";
                if(!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_HardBrush)
                {
                    ArrowIndication.Instance.activityArrow.SetActive(false);
                    ArrowIndication.Instance.toolArrow.SetActive(true);
                }
                else if (ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_HardBrush)
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
                if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Up, "Wrong Direction", "You need to be looking from above!", "You can't use your tool here...", ARCleanDataStore.PenaltyCounter < 3))
                    return;

                CleanTool.SetToolHitPosition(Input.mousePosition, true, "ARC_Drain");
                if (ARCleanDataStore.CleanToolActive)
                    if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_HardBrush, "Wrong Tool", "You need to be using the Hard Brush!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
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
            ProgressSubHeaderText.text = "You need be at the floor area to complete this part of the stage.";
        }
    }

    private void Phase_WashDownDrain()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Floor)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 7: Wash down detergent into the drains using [AlkaClean]\n [AlkaClean] can be found in the laundry room.";
                if(!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Alkaclean)
                {
                    ArrowIndication.Instance.activityArrow.SetActive(false);
                    ArrowIndication.Instance.toolArrow.SetActive(true);
                }
                else if (ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Alkaclean)
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
            if (ARCleanDataStore.PlayerInputFlag)
            {
                CleanTool.SetToolHitPosition(Input.mousePosition, true, "ARC_Drain");
                if (ARCleanDataStore.CleanToolActive)
                    if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Up, "Wrong Direction", "You need to be looking from above!", "You can't use your tool here...", ARCleanDataStore.PenaltyCounter < 3) || 
                        ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_Alkaclean, "Wrong Tool", "You need to be using Alkaclean!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
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
            ProgressSubHeaderText.text = "You need be at the floor area to complete this part of the stage.";
        }
    }

    private void Phase_TakeFilter()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Floor)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 8: Take the L-Shape Metal Drain Filter using the [Protective Metal Gloves] \n [Protective Metal Gloves] can be found in the sink cabinet.";
                if(!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Glove)
                {
                    ArrowIndication.Instance.activityArrow.SetActive(false);
                    ArrowIndication.Instance.toolArrow.SetActive(true);
                }
                else if (ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Glove)
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

            //* METAL GLOVE

            if (ARCleanDataStore.GetPlayerTool() == ARCleanDataStore.PlayerTool.PT_Glove) ARCleanDataStore.ObjectInteractibleFlag = true;
            else ARCleanDataStore.ObjectInteractibleFlag = false;

            if (ARCleanDataStore.PlayerInputFlag && !ARCleanDataStore.CheckIfMouseIsOverUI())
                if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Up, "Wrong Direction", "You need to be looking from above!", "You can't take object at this angle...", ARCleanDataStore.PenaltyCounter < 3) ||
                    ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_Glove, "Wrong Tool", "You need to be using the Metal Glove!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
                {
                    ARCleanDataStore.PenaltyCounter++;
                    return;
                }

            if (ARCleanDataStore.Inventory.IsObjectInInventory("L Shape Metal Drainer"))
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
            ProgressSubHeaderText.text = "You need be at the floor area to complete this part of the stage.";
        }
    }

    private void Phase_ScrubFilter()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Sink)
        {
            switch (ARCleanDataStore.GameModeGameState)
            {
                case 0:
                    if (ARCleanDataStore.ShowHints)
                    {
                        ProgressSubHeaderText.text = "Stage 9: Place the L-Shape Metal Drain Filterusing the [Protective Metal Gloves] \n [Protective Metal Gloves] can be found in the sink cabinet.";
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

                    //* METAL GLOVE

                    if (ARCleanDataStore.GetPlayerTool() == ARCleanDataStore.PlayerTool.PT_Glove && 
                        ARCleanDataStore.CameraCurrentDirection == ARCleanCamera.Directions.D_Up)
                        ARCleanDataStore.ObjectInteractibleFlag = true;
                    else ARCleanDataStore.ObjectInteractibleFlag = false;

                    if (ARCleanDataStore.PlayerInputFlag && !ARCleanDataStore.CheckIfMouseIsOverUI())
                        if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Up, "Wrong Direction", "You need to be looking from above!", "You can't place object at this angle...", ARCleanDataStore.PenaltyCounter < 3) ||
                            ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_Glove, "Wrong Tool", "You need to be using the Metal Glove!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
                        {
                            ARCleanDataStore.PenaltyCounter++;
                            return;
                        }

                    if (!ARCleanDataStore.Inventory.IsObjectInInventory("L Shape Metal Drainer"))
                    {
                        ARCleanDataStore.ObjectInteractibleFlag = false;
                        ARCleanDataStore.GameModeGameState++;
                    }
                    break;
                case 1:
                    if (ARCleanDataStore.ShowHints)
                    {
                        ProgressSubHeaderText.text = "Stage 10: Scrub the L-Shape Metal Drain Filter using [Genie]\n [Genie] can be found in the laundry room.";
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
                        if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Up, "Wrong Direction", "You need to be looking from above!", "You can't use your tool here...", ARCleanDataStore.PenaltyCounter < 3))
                            return;

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
                                ProgressBar.value = 0f;
                                ARCleanDataStore.CleanToolActive = false;
                                // Reset Values for next run
                                ARCleanDataStore.PlayerInputFlagReset = false;
                                ARCleanDataStore.GameModeGameState++;
                            }
                        }
                    }
                    break;
                case 2:
                    if (ARCleanDataStore.ShowHints)
                    {
                        ProgressSubHeaderText.text = "Stage 11: Take the L-Shape Metal Drain Filter using the [Protective Metal Gloves]\n [Protective Metal Gloves] can be found in the sink cabinet.";
                        if(!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Glove)
                        {
                            ArrowIndication.Instance.activityArrow.SetActive(false);
                            ArrowIndication.Instance.toolArrow.SetActive(true);
                        }
                        else if (ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Glove)
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

                    //* METAL GLOVE

                    if (ARCleanDataStore.GetPlayerTool() == ARCleanDataStore.PlayerTool.PT_Glove) ARCleanDataStore.ObjectInteractibleFlag = true;
                    else ARCleanDataStore.ObjectInteractibleFlag = false;

                    if (ARCleanDataStore.PlayerInputFlag && !ARCleanDataStore.CheckIfMouseIsOverUI())
                        if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Up, "Wrong Direction", "You need to be looking from above!", "You can't place object at this angle...", ARCleanDataStore.PenaltyCounter < 3) ||
                            ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_Glove, "Wrong Tool", "You need to be using the Metal Glove!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
                        {
                            ARCleanDataStore.PenaltyCounter++;
                            return;
                        }

                    if (ARCleanDataStore.Inventory.IsObjectInInventory("L Shape Metal Drainer"))
                    {
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

    private void Phase_ReassembleFloor()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Floor)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 12: Reassemble the drain using the [Protective Metal Gloves]\n [Protective Metal Gloves] can be found in the sink cabinet.";
                if(!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Glove)
                {
                    ArrowIndication.Instance.toolArrow.SetActive(true);
                    ArrowIndication.Instance.activityArrow.SetActive(false);
                }
                else if (ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Glove)
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
            //* METAL GLOVE

            if (ARCleanDataStore.GetPlayerTool() == ARCleanDataStore.PlayerTool.PT_Glove &&
                ARCleanDataStore.CameraCurrentDirection == ARCleanCamera.Directions.D_Up)
                ARCleanDataStore.ObjectInteractibleFlag = true;
            else ARCleanDataStore.ObjectInteractibleFlag = false;

            if (ARCleanDataStore.PlayerInputFlag && !ARCleanDataStore.CheckIfMouseIsOverUI())
                if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Up, "Wrong Direction", "You need to be looking from above!", "You can't place object at this angle...", ARCleanDataStore.PenaltyCounter < 3) ||
                    ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_Glove, "Wrong Tool", "You need to be using the Metal Glove!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
                {
                    ARCleanDataStore.PenaltyCounter++;
                    return;
                }

            if (AssembleOrder == 0 && ARCleanDataStore.Inventory.I_ApplianceTransform.childCount > 0)
            {
                foreach (Transform it in ARCleanDataStore.Inventory.I_ApplianceTransform)
                {
                    if (it.GetComponent<ARIconHandler>().Object == null)
                        return;

                    Debug.Log(it.GetComponent<ARIconHandler>().Object.name);
                    if (it.GetComponent<ARIconHandler>().Object.name.Contains("L Shape"))
                        it.GetComponent<ARIconHandler>().Interactible = true;
                    else
                        it.GetComponent<ARIconHandler>().Interactible = false;
                }

                if (!ARCleanDataStore.Inventory.IsObjectInInventory("L Shape Metal Drainer"))
                {
                    foreach (Transform it in ARCleanDataStore.Inventory.I_ApplianceTransform)
                        it.GetComponent<ARIconHandler>().Interactible = true;
                    AssembleOrder++;
                }
            }
            else if (AssembleOrder == 1)
            {
                if (ARCleanDataStore.Inventory.IsObjectInInventory("L Shape Metal Drainer"))
                    AssembleOrder--;

                if (!ARCleanDataStore.Inventory.IsObjectInInventory("Metal Grill 1") &&
                    !ARCleanDataStore.Inventory.IsObjectInInventory("Metal Grill 2") &&
                    !ARCleanDataStore.Inventory.IsObjectInInventory("Metal Grill 3"))
                {
                    AssembleOrder = 0;
                    ARCleanDataStore.ObjectInteractibleFlag = false;
                    ARCleanDataStore.IncrementGameProgress();
                }
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the floor area to complete this part of the stage.";
        }
    }

    private void Phase_DryFloor()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Floor)
        {
            ARCleanDataStore.ObjectInteractibleFlag = false;
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 13: Dry the floor using the [Wiper]\n [Wiper] can be found in the laundry room.";
                if(!ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Wiper)
                {
                    ArrowIndication.Instance.activityArrow.SetActive(false);
                    ArrowIndication.Instance.toolArrow.SetActive(true);
                }
                else if (ArrowIndication.Instance.inventory.GetToolIsOpen() && ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_Wiper)
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

            if (ARCleanDataStore.PlayerInputFlag)
            {
                CleanTool.SetToolHitPosition(Input.mousePosition, true, "ARC_Interactables");
                if (ARCleanDataStore.CleanToolActive)
                    if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_Wiper, "Wrong Tool", "You need to be using the Wiper!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
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
                    }
                }
            }
        }
        else
        {
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the floor area to complete this part of the stage.";
        }
    }
}
