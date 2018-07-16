/* 
 * Author: Lim Rui An Ryan, Lee Kwan Liang
 * Filename: ARCleanStoveLogic.cs
 * Description: The stove game mode's game logic codes that will be run by the ARGameLogicSystem.
 */
using UnityEngine;
using System.Collections.Generic;

public class ARCleanStoveLogic : ARCleanModeLogic
{
    // Private Variables
    private float WaterLevelIncrement = 1.5f;
    private Vector3 WaterOrigin;
    private Vector3 TapDesiredRotation = new Vector3(0, -180, -15);
    private int AssembleOrder = 0;

    public enum Progress
    {
        P_Cooldown = 0,
        P_Get,
        P_Wash,
        P_Scrub,
        P_Dry,
        P_Reassemble,
    }

    public static ARCleanDataStore.GameLocation GetGameLocationForCurrentPhase(int Phase)
    {
        switch ((Progress)Phase)
        {
            case Progress.P_Cooldown:
                return ARCleanDataStore.GameLocation.GL_Stove;
            case Progress.P_Get:
                return ARCleanDataStore.GameLocation.GL_Stove;
            case Progress.P_Wash:
                return ARCleanDataStore.GameLocation.GL_Sink;
            case Progress.P_Scrub:
                return ARCleanDataStore.GameLocation.GL_Stove;
            case Progress.P_Dry:
                return ARCleanDataStore.GameLocation.GL_Stove;
            case Progress.P_Reassemble:
                return ARCleanDataStore.GameLocation.GL_Stove;
        }
        return ARCleanDataStore.GameLocation.GL_Undefined;
    }

    public void Update()
    {
        ProgressHeaderText.text = "Stove Area Daily Tasks";
        switch ((Progress)ARCleanDataStore.GetGamePhase())
        {
            case Progress.P_Cooldown:
                Phase_Cooldown();
                break;
            case Progress.P_Get:
                Phase_Get();
                break;
            case Progress.P_Wash:
                Phase_Wash();
                break;
            case Progress.P_Scrub:
                Phase_Scrub();
                break;
            case Progress.P_Dry:
                Phase_Dry();
                break;
            case Progress.P_Reassemble:
                Phase_Reassemble();
                break;
        }
    }

    private void Phase_Cooldown()
    {
        /// Logic for cooldown step
        // Start with stove being "hot"
        // Wait a certain time
        // Check if the player is in wrong scene before performing logic
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Stove)
        {
            const float TimeToWait = 10f;
            if (Timer < TimeToWait)
            {
                ParticleSystem SmokeSystem = ARCleanDataStore.ModelAccess.Stove_Particles.GetComponent<ParticleSystem>();
                if (!SmokeSystem.isPlaying)
                    SmokeSystem.Play();
                ProgressBar.value = Timer / TimeToWait * 100f;
                SmokeSystem.maxParticles = (int)(100f * (1f - Timer / TimeToWait));
                Timer += Time.deltaTime;
                ProgressBar.value = Timer / TimeToWait * 100f;
                if (ARCleanDataStore.ShowHints)
                    ProgressSubHeaderText.text = "Stage 1: Let the stove cool down!";
                else ProgressSubHeaderText.text = "";
                ARCleanDataStore.ObjectInteractibleFlag = false;
                if (Timer >= TimeToWait)
                {
                    SmokeSystem.Stop();
                    ARCleanDataStore.IncrementGameProgress();
                    // Reset Values for next run
                    ProgressBar.value = 0;
                    Timer = 0f;
                }
            }
        }
        else
        {
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the stove area to complete this stage.";
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
        }
    }

    private void Phase_Get()
    {
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Stove)
        {
            ARCleanDataStore.ObjectInteractibleFlag = true;
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 2: Take the stove tops and burners!";
                ArrowIndication.Instance.activityArrow.SetActive(true);
            }
            else ProgressSubHeaderText.text = "";
            // Check if condition has been fulfilled. If so, increment the state
            if (ARCleanDataStore.Inventory.IsObjectInInventory("Stove Top 1") &&
                ARCleanDataStore.Inventory.IsObjectInInventory("Stove Top 2") &&
                ARCleanDataStore.Inventory.IsObjectInInventory("Stove Burner Cap 1") &&
                ARCleanDataStore.Inventory.IsObjectInInventory("Stove Burner Cap 2"))
            {
                if (ARCleanDataStore.ShowHints)
                    ArrowIndication.Instance.activityArrow.SetActive(false);

                ARCleanDataStore.ObjectInteractibleFlag = false;
                ARCleanDataStore.IncrementGameProgress();
            }
        }
        else
        {
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the stove area to complete this stage.";
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
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
                        ProgressSubHeaderText.text = "Stage 3: Place the stove tops and burners in the sink!";
                        if (!ArrowIndication.Instance.inventory.GetApplianceIsOpen())
                        {
                            ArrowIndication.Instance.inventoryArrow.SetActive(true);
                            ArrowIndication.Instance.sideBarArrow.SetActive(false);
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
                        if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Up, "Wrong Direction", "You need to be looking from above!", "You can't place object at this angle...", ARCleanDataStore.PenaltyCounter < 3))
                        {
                            ARCleanDataStore.ObjectInteractibleFlag = false;
                            ARCleanDataStore.PenaltyCounter++;
                        }
                        else
                            ARCleanDataStore.ObjectInteractibleFlag = true;

                    if (!ARCleanDataStore.Inventory.IsObjectInInventory("Stove Top 1") &&
                        !ARCleanDataStore.Inventory.IsObjectInInventory("Stove Top 2") &&
                        !ARCleanDataStore.Inventory.IsObjectInInventory("Stove Burner Cap 1") &&
                        !ARCleanDataStore.Inventory.IsObjectInInventory("Stove Burner Cap 2"))
                    {
                        if (ARCleanDataStore.ShowHints)
                        {
                            ArrowIndication.Instance.activityArrow.SetActive(false);
                            ArrowIndication.Instance.sideBarArrow.SetActive(false);
                            ArrowIndication.Instance.inventoryArrow.SetActive(false);
                            ArrowIndication.Instance.sinkArrow.SetActive(true);
                        }

                        // After everything is washed
                        Timer = 0f;
                        ARCleanDataStore.GameModeGameState++;
                    }
                    break;
                case 1:
                    ProgressBar.value = 50;
                    if (ARCleanDataStore.ShowHints)
                        ProgressSubHeaderText.text = "Stage 4: Turn on the sink's tap.";
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
                                ArrowIndication.Instance.sinkArrow.SetActive(false);
                            }
                        }
                    }
                    break;
                case 2:
                    if (ARCleanDataStore.ShowHints)
                        ProgressSubHeaderText.text = "Stage 5: Wash the stove tops and burners with hot water.";
                    else ProgressSubHeaderText.text = "";
                    if (ProgressBar.value < 75f)
                    {
                        ProgressBar.value += 3f * Time.deltaTime;
                        ARCleanDataStore.ModelAccess.Sink_Water.transform.position = WaterOrigin + new Vector3(0, WaterLevelIncrement * ((ProgressBar.value - 50f) / 25f));
                    }
                    else if (ProgressBar.value <= 85f)
                    {
                        // TODO: Add scrubbing logic here
                        ProgressBar.value += 3f * Time.deltaTime;

                        ARCleanDataStore.ModelAccess.Sink_Particles.GetComponent<ParticleSystem>().Stop();
                        Transform Temp = ARCleanDataStore.ModelAccess.Sink_TapHandle.transform;
                        Temp.eulerAngles = new Vector3(0, -180);
                        ARCleanDataStore.ModelAccess.Sink_TapHandle.transform.rotation = Quaternion.Lerp(ARCleanDataStore.ModelAccess.Sink_TapHandle.transform.rotation, Temp.rotation, Time.time * 5f);
                    }
                    else if (ProgressBar.value <= 100f)
                    {
                        ProgressBar.value += 3f * Time.deltaTime;
                        ARCleanDataStore.ModelAccess.Sink_Water.transform.position = WaterOrigin + new Vector3(0, WaterLevelIncrement * (1 - ((ProgressBar.value - 85f) / 15f)));
                        if (ProgressBar.value >= 100f)
                        {
                            ARCleanDataStore.GameModeGameState++;
                            ProgressBar.value = 0f;
                        }
                    }
                    break;
                case 3:
                    ARCleanDataStore.ObjectInteractibleFlag = true;
                    if (ARCleanDataStore.ShowHints)
                    {
                        ProgressSubHeaderText.text = "Stage 6: Take the stove tops and burners!";
                        ArrowIndication.Instance.activityArrow.SetActive(true);
                    }
                    else ProgressSubHeaderText.text = "";
                    // Check if condition has been fulfilled. If so, increment the state
                    if (ARCleanDataStore.Inventory.IsObjectInInventory("Stove Top 1") &&
                        ARCleanDataStore.Inventory.IsObjectInInventory("Stove Top 2") &&
                        ARCleanDataStore.Inventory.IsObjectInInventory("Stove Burner Cap 1") &&
                        ARCleanDataStore.Inventory.IsObjectInInventory("Stove Burner Cap 2"))
                    {
                        if (ARCleanDataStore.ShowHints)
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
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the sink area to complete this stage.";
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
        }
    }

    private void Phase_Scrub()
    {
        /// Logic for scrubbing step
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Stove)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 7: Scrub the stove range using [Kleen]\n [Kleen] can be found in the laundry room.";
                if (ARCleanDataStore.LinkedToolInventory[(int)ARCleanDataStore.PlayerTool.PT_HeavySpongeKellen].LockedIcon.activeSelf)
                    ArrowIndication.Instance.scanArrow.SetActive(true);
                else
                {
                    ArrowIndication.Instance.scanArrow.SetActive(false);
                    if (ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_HeavySpongeKellen && !ArrowIndication.Instance.inventory.GetToolIsOpen())
                        ArrowIndication.Instance.toolArrow.SetActive(true);
                    else if (ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_HeavySpongeKellen && ArrowIndication.Instance.inventory.GetToolIsOpen())
                    {
                        ArrowIndication.Instance.toolArrow.SetActive(false);
                        ArrowIndication.Instance.sideBarArrow.SetActive(true);
                    }
                    else
                    {
                        ArrowIndication.Instance.activityArrow.SetActive(true);
                        ArrowIndication.Instance.sideBarArrow.SetActive(false);
                    }
                }
            }
            else ProgressSubHeaderText.text = "";
            if (!DirtSpawned)
            {
                DirtContainer = new List<GameObject>();
                NumberOfDirt = ARCleanDataStore.ModelAccess.gameObject.GetComponent<ARCleanObjectSpawner>().GenerateDirt(DirtContainer, ARCleanDataStore.ModelAccess.SpawningArea_Stove.transform.localScale, 30);
                DirtSpawned = true;
                ProgressBar.value = 0;
                CleanupPercentageGains = 100f / (float)NumberOfDirt;
                WaterContainer = new List<GameObject>();
                ARCleanDataStore.ModelAccess.GetComponent<ARCleanObjectSpawner>().ActivateInternalColliders(DirtContainer);
            }
            Timer += Time.deltaTime;
            if (ARCleanDataStore.PlayerInputFlag)
            {
                Vector3 CleanToolPosition = CleanTool.SetToolHitPosition(Input.mousePosition, true, "", "ARC_Door");
                // Error Handling
                if (ARCleanDataStore.CleanToolActive)
                    if (ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.CameraCurrentDirection, ARCleanCamera.Directions.D_Up, "Wrong Direction", "You need to be looking from above!", "You can't use your tool here...", ARCleanDataStore.PenaltyCounter < 3) ||
                        ARCleanDataStore.ModelAccess.SetupErrorWithNotComparator(ARCleanDataStore.GetPlayerTool(), ARCleanDataStore.PlayerTool.PT_HeavySpongeKellen, "Wrong Tool", "You need to be using Kleen!", "Try another tool...", ARCleanDataStore.PenaltyCounter < 3))
                    {
                        ARCleanDataStore.CleanToolActive = false;
                        ARCleanDataStore.PenaltyCounter++;
                        return;
                    }
                    else if (Timer > WaterSpawnTime)
                    {
                        Timer = 0f;
                        ARCleanDataStore.ModelAccess.gameObject.GetComponent<ARCleanObjectSpawner>().SpawnWater(WaterContainer, new Vector3(CleanToolPosition.x, CleanToolPosition.y, CleanToolPosition.z));
                    }
                if (CleanTool.CleaningCheck(0.3f) && DirtContainerCollisionDetection(CleanToolPosition, "ARC_Stain"))
                {
                    foreach (GameObject Obj in DirtContainer)
                        GameObject.Destroy(Obj);
                    DirtContainer.Clear();
                    ARCleanDataStore.CleanToolActive = false;
                    ARCleanDataStore.IncrementGameProgress();
                    // Reset Values for next run
                    ARCleanDataStore.ModelAccess.GetComponent<ARCleanObjectSpawner>().ActivateInternalColliders(WaterContainer);
                    ARCleanDataStore.PlayerInputFlagReset = false;
                    NumberOfWater = WaterContainer.Count;
                    CleanupPercentageGains = 100f / (float)NumberOfWater;
                    Timer = 0f;
                    ProgressBar.value = 0;
                }
            }
        }
        else
        {
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the stove area to complete this stage.";
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
        }
    }

    private void Phase_Dry()
    {
        /// Logic for dry step
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Stove)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 8: Dry the stove range using [Dry Cloth]\n [Dry Cloth] can be found in the sink's cabinet.";
                if (ARCleanDataStore.LinkedToolInventory[(int)ARCleanDataStore.PlayerTool.PT_DryCloth].LockedIcon.activeSelf)
                {
                    ArrowIndication.Instance.scanArrow.SetActive(true);
                    ArrowIndication.Instance.inventoryArrow.SetActive(false);
                    ArrowIndication.Instance.activityArrow.SetActive(false);

                }
                else
                {
                    ArrowIndication.Instance.scanArrow.SetActive(false);
                    if (ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_DryCloth && !ArrowIndication.Instance.inventory.GetToolIsOpen())
                    {
                        ArrowIndication.Instance.sideBarArrow.SetActive(false);
                        ArrowIndication.Instance.toolArrow.SetActive(true);
                        ArrowIndication.Instance.activityArrow.SetActive(false);
                    }
                    else if (ARCleanDataStore.GetPlayerTool() != ARCleanDataStore.PlayerTool.PT_DryCloth && ArrowIndication.Instance.inventory.GetToolIsOpen())
                    {
                        ArrowIndication.Instance.toolArrow.SetActive(false);
                        ArrowIndication.Instance.sideBarArrow.SetActive(true);
                    }
                    else
                    {
                        ArrowIndication.Instance.activityArrow.SetActive(true);
                        ArrowIndication.Instance.sideBarArrow.SetActive(false);
                    }
                }
            }
            else ProgressSubHeaderText.text = "";
            ARCleanDataStore.ObjectInteractibleFlag = false;
            if (WaterContainer == null)
            {
                WaterContainer = new List<GameObject>();
                NumberOfWater = ARCleanDataStore.ModelAccess.gameObject.GetComponent<ARCleanObjectSpawner>().GenerateWater(WaterContainer, ARCleanDataStore.ModelAccess.SpawningArea_Stove.transform.localScale, 30, "");
                ARCleanDataStore.ModelAccess.GetComponent<ARCleanObjectSpawner>().ActivateInternalColliders(WaterContainer);
                CleanupPercentageGains = 100f / (float)NumberOfWater;
            }
            if (ARCleanDataStore.PlayerInputFlag)
            {
                Vector3 CleanToolPosition = CleanTool.SetToolHitPosition(Input.mousePosition, true, "", "ARC_Door");
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
                }
            }
        }
        else
        {
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the stove area to complete this stage.";
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
        }
    }

    private void Phase_Reassemble()
    {
        /// Logic for reassemble step
        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Stove)
        {
            if (ARCleanDataStore.ShowHints)
            {
                ProgressSubHeaderText.text = "Stage 9: Reassemble the stove!";
                if(!ArrowIndication.Instance.inventory.GetApplianceIsOpen())
                    ArrowIndication.Instance.inventoryArrow.SetActive(true);               
                else
                {
                    ArrowIndication.Instance.sideBarArrow.SetActive(true);
                    ArrowIndication.Instance.inventoryArrow.SetActive(false);
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

            if (AssembleOrder == 0 && ARCleanDataStore.Inventory.I_ApplianceTransform.childCount > 0)
            {
                foreach (Transform it in ARCleanDataStore.Inventory.I_ApplianceTransform)
                {
                    if (it.GetComponent<ARIconHandler>().Object == null)
                        return;

                    if (it.GetComponent<ARIconHandler>().Object.name.Contains("Stove Burner Cap"))
                        it.GetComponent<ARIconHandler>().Interactible = true;
                    else
                        it.GetComponent<ARIconHandler>().Interactible = false;
                }

                if (!ARCleanDataStore.Inventory.IsObjectInInventory("Stove Burner Cap 1") &&
                    !ARCleanDataStore.Inventory.IsObjectInInventory("Stove Burner Cap 2"))
                {
                    foreach (Transform it in ARCleanDataStore.Inventory.I_ApplianceTransform)
                        it.GetComponent<ARIconHandler>().Interactible = true;
                    AssembleOrder++;
                }
            }
            else if (AssembleOrder == 1)
            {
                if (ARCleanDataStore.Inventory.IsObjectInInventory("Stove Burner Cap 1") ||
                    ARCleanDataStore.Inventory.IsObjectInInventory("Stove Burner Cap 2"))
                    AssembleOrder--;

                if (!ARCleanDataStore.Inventory.IsObjectInInventory("Stove Top 1") &&
                    !ARCleanDataStore.Inventory.IsObjectInInventory("Stove Top 2"))
                {
                    // After everything is placed down
                    // Since this is the last state, increment the gamemode and reset the phase flag
                    ARCleanDataStore.SetupForNextGameMode();
                    ProgressBar.value = 0;
                    Timer = 0f;
                    AssembleOrder = 0;
                    ARCleanDataStore.GameModeGameState = 0;
                    ArrowIndication.Instance.StopAllArrows();
                }
            }
        }
        else
        {
            ARCleanDataStore.ObjectInteractibleFlag = false;
            ProgressSubHeaderText.text = "You need be at the stove area to complete this stage.";
            if (ARCleanDataStore.ShowHints)
                ArrowIndication.Instance.scanArrow.SetActive(true);
        }
    }
}