using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for showing arrow indications for the current steps in AR Clean up
/// </summary>
public class ArrowIndication : MonoBehaviour {

    public static ArrowIndication Instance { get; private set; }

    //Arrow for inventory/tools bar
    public GameObject sideBarArrow;

    //Arrow for indicating the need to scan and change scene
    public GameObject scanArrow;

    //Arrow for indicating the need to open up tool bar
    public GameObject toolArrow;

    //Arrow for indicating the need to open up inventory bar
    public GameObject inventoryArrow;

    //Arrow pointed at the center of the screen to show use of tool / placement (or taking) of inventory item
    public GameObject activityArrow;

    //Arrow to indicate use of sink tap 
    public GameObject sinkArrow;

    public ARInventoryManager inventory;

    public GameObject TrashArrow;

    //Array of arrows for the tools (according to the if statement)
    public GameObject[] toolArrows;

    public GameObject[] WokPotPanArrows;

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        //Check if tools are taken, if not show the arrow on them
        #region Lots of if-else statements
        //Laundry area stuffs
        if (ARCleanDataStore.GameLocation.GL_Laundry == ARCleanDataStore.GetPlayerLocation())   
        {
            if (ARCleanDataStore.LinkedToolInventory[(int)ARCleanDataStore.PlayerTool.PT_Alcosan].LockedIcon.activeSelf || ARCleanDataStore.LinkedToolInventory[(int)ARCleanDataStore.PlayerTool.PT_HeavySpongeGenie].LockedIcon.activeSelf)
                toolArrows[0].SetActive(true);
            else
                toolArrows[0].SetActive(false);

            if (ARCleanDataStore.LinkedToolInventory[(int)ARCleanDataStore.PlayerTool.PT_Alkaclean].LockedIcon.activeSelf || ARCleanDataStore.LinkedToolInventory[(int)ARCleanDataStore.PlayerTool.PT_HardBrush].LockedIcon.activeSelf || ARCleanDataStore.LinkedToolInventory[(int)ARCleanDataStore.PlayerTool.PT_HeavySpongeKellen].LockedIcon.activeSelf)
                toolArrows[1].SetActive(true);
            else
                toolArrows[1].SetActive(false);

            if (ARCleanDataStore.LinkedToolInventory[(int)ARCleanDataStore.PlayerTool.PT_Broom].LockedIcon.activeSelf)
                toolArrows[2].SetActive(true);
            else
                toolArrows[2].SetActive(false);

            if (ARCleanDataStore.LinkedToolInventory[(int)ARCleanDataStore.PlayerTool.PT_Water].LockedIcon.activeSelf)
                toolArrows[6].SetActive(true);
            else
                toolArrows[6].SetActive(false);

            if (ARCleanDataStore.LinkedToolInventory[(int)ARCleanDataStore.PlayerTool.PT_Wiper].LockedIcon.activeSelf)
                toolArrows[7].SetActive(true);
            else
                toolArrows[7].SetActive(false);
        }

        if (ARCleanDataStore.GetPlayerLocation() == ARCleanDataStore.GameLocation.GL_Wok)
        {

            if (ARCleanDataStore.CameraCurrentDirection == ARCleanCamera.Directions.D_Up)
            {
                if (ARCleanDataStore.LinkedToolInventory[(int)ARCleanDataStore.PlayerTool.PT_CookingOil].LockedIcon.activeSelf)
                    toolArrows[3].SetActive(true);
                else
                    toolArrows[3].SetActive(false);
            }
        }

        //Sink areas tools (sponge, dry cloth + glove)
        if (ARCleanDataStore.GameLocation.GL_Sink == ARCleanDataStore.GetPlayerLocation() && ARCleanDataStore.CameraCurrentDirection == ARCleanCamera.Directions.D_Up)
        {
            if (ARCleanDataStore.LinkedToolInventory[(int)ARCleanDataStore.PlayerTool.PT_Sponge].LockedIcon.activeSelf)
                toolArrows[5].SetActive(true);
            else
                toolArrows[5].SetActive(false);
        }
        else if (ARCleanDataStore.GameLocation.GL_Sink == ARCleanDataStore.GetPlayerLocation() && ARCleanDataStore.CameraCurrentDirection == ARCleanCamera.Directions.D_Forward)
        {
            if (ARCleanDataStore.LinkedToolInventory[(int)ARCleanDataStore.PlayerTool.PT_DryCloth].LockedIcon.activeSelf || ARCleanDataStore.LinkedToolInventory[(int)ARCleanDataStore.PlayerTool.PT_Glove].LockedIcon.activeSelf)
                toolArrows[4].SetActive(true);
            else
                toolArrows[4].SetActive(false);
        }

        #endregion
    }

    //Set all arrows to false
    public void StopAllArrows()
    {
        TrashArrow.SetActive(false);
        sideBarArrow.SetActive(false);
        scanArrow.SetActive(false);
        toolArrow.SetActive(false);
        inventoryArrow.SetActive(false);
        activityArrow.SetActive(false);
        sinkArrow.SetActive(false);
        for(int i = 0; i < toolArrows.Length;++i)
        {
            toolArrows[i].SetActive(false);
        }
        for(int j = 0; j < WokPotPanArrows.Length; ++j)
        {
            WokPotPanArrows[j].SetActive(false);
        }
    }
}
