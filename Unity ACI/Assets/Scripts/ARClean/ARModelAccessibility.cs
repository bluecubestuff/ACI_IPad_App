/* 
 * Author: Lim Rui An Ryan, Lee Kwan Liang
 * Filename: ARModelAccessibility.cs
 * Description: A script that is used hold references to objects used in game logic codes
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ARModelAccessibility : MonoBehaviour
{
    [Header("Debug Parameters")]
    public bool ForceTransitToLocation = false;
    public ARCleanDataStore.GameLocation TargetLocation;
    public ARGameLogicSystem LinkedLogic;

    [Header("Error Interface")]
    public GameObject ErrorInterface;
    public TextMeshProUGUI ErrorHeader, ErrorSubHeader;

    [Header("Rotation Buttons")]
    public GameObject Button_Up;
    public GameObject Button_Down;
    public GameObject Button_Left;
    public GameObject Button_Right;

    [Header("Game Models")]
    public GameObject Model_Stove;
    public GameObject Model_Wok;
    public GameObject Model_Sink;
    public GameObject Model_Prep;
    public GameObject Model_Trash;
    public GameObject Model_Floor;
    public GameObject Model_Chiller;
    public GameObject Model_Laundry;
    public GameObject Model_Room;

    [Header("Dirt Spawning Area")]
    public GameObject SpawningArea_Stove;
    public GameObject SpawningArea_Wok;
    public GameObject SpawningArea_Prep;
    public GameObject SpawningArea_Sink;

    [Header("Tool UI")]
    public UIScroller ScrollerModule;

    ///  Stove to Sink
    [Header("Stove Gamemode")]
    [SerializeField]
    private List<GameObject> StoveGM_StoveProps;
    [SerializeField]
    private List<GameObject> StoveGM_SinkProps;

    [Header("WokPotsPans Gamemode")]
    [SerializeField]
    private List<GameObject> WokPotsPansGM_StoveProps;
    [SerializeField]
    private List<GameObject> WokPotsPansGM_SinkProps;

    [Header("Sink Gamemode")]
    [SerializeField]
    private GameObject SinkGM_SinkContainer;
    [SerializeField]
    private GameObject SinkGM_PrepContainer;
    [SerializeField]
    private GameObject SinkGM_PrepTrash;
    [SerializeField]
    private List<GameObject> SinkGM_TrashProps;

    [Header("Floor Gamemode")]
    [SerializeField]
    private List<GameObject> FloorGM_SinkProps;
    [SerializeField]
    private List<GameObject> FloorGM_FloorProps;
    [SerializeField]
    private GameObject FloorGM_FloorLMetal;

    [Header("Chiller Gamemode")]
    [SerializeField]
    private List<GameObject> ChillerGM_SinkProps;
    [SerializeField]
    private List<GameObject> ChillerGM_ChillerProps;

    [Header("Cleaning Tools")]
    public GameObject CurrentCleaningTool;
    public GameObject Tool_Sponge;
    public GameObject Tool_Genie;
    public GameObject Tool_Kellen;
    public GameObject Tool_Cloth;
    public GameObject Tool_Oil;
    public GameObject Tool_Alcosan;
    public GameObject Tool_Broom;
    public GameObject Tool_Glove;
    public GameObject Tool_Water;
    public GameObject Tool_Alkaclean;
    public GameObject Tool_Wiper;
    public GameObject Tool_HardBrush;
    public Image CurrentToolImage;
    public Sprite DefaultToolIcon;

    [Header("Tool Props")]
    public GameObject Prop_Sponge;
    public GameObject Prop_Kellen;
    public GameObject Prop_Genie;
    public GameObject Prop_Cloth;
    public GameObject Prop_Oil;
    public GameObject Prop_Alcosan;
    public GameObject Prop_Broom;
    public GameObject Prop_Glove;
    public GameObject Prop_Water;
    public GameObject Prop_Alkaclean;
    public GameObject Prop_Wiper;
    public GameObject Prop_HardBrush;

    [Header("Props for Animation")]
    public GameObject Sink_Water;
    public GameObject Sink_TapHandle;
    public GameObject Sink_Particles;
    public GameObject Wok_Particles;
    public GameObject Stove_Particles;

    private void Start()
    {
        InitiallizeScene();
        HideTakenProps();
    }

    private void Update()
    {
        if (ForceTransitToLocation)
        {
            ForceTransitToLocation = false;
            ARCleanDataStore.SetPlayerLocation(TargetLocation);
            ARCleanDataStore.Inventory.GenerateUI();
            LinkedLogic.InitiallizeScene();
        }
        if (DefaultToolIcon != null && ARCleanDataStore.GetPlayerTool() == 0)
            ARCleanDataStore.ModelAccess.CurrentToolImage.sprite = DefaultToolIcon;
    }

    public void SetUpGameObjectList(List<GameObject> ObjectList, bool Active)
    {
        if (ObjectList != null && ObjectList.Count > 0)
            foreach (GameObject GO in ObjectList)
                GO.SetActive(Active);
    }

    public bool SetupErrorWithNotComparator<T>(T Comparator, T Target, string Header, string SubHeader, string AlternateHeader, bool UseAltHeader) where T : struct
    {
        if (!Equals(Comparator, Target))
        {
            SetupErrorInterface(Header, SubHeader, AlternateHeader, UseAltHeader);
            return true;
        }
        else return false;
    }

    public bool SetupErrorWithEqualComparator<T>(T Comparator, T Target, string Header, string SubHeader, string AlternateHeader, bool UseAltHeader) where T : struct
    {
        if (Equals(Comparator, Target))
        {
            SetupErrorInterface(Header, SubHeader, AlternateHeader, UseAltHeader);
            return true;
        }
        else return false;
    }

    public void SetupErrorInterface(string Header, string SubHeader, string AlternateHeader, bool UseAltHeader)
    {
        ARCleanDataStore.ErrorScreenActive = true;
        ErrorHeader.text = Header;
        if (UseAltHeader)
            ErrorSubHeader.text = AlternateHeader;
        else ErrorSubHeader.text = SubHeader;
    }

    public void HideMainProps()
    {
        // Hide the main models
        Model_Stove.SetActive(false);
        Model_Wok.SetActive(false);
        Model_Prep.SetActive(false);
        Model_Trash.SetActive(false);
        Model_Floor.SetActive(false);
        Model_Sink.SetActive(false);
        Model_Chiller.SetActive(false);
        Model_Laundry.SetActive(false);
    }

    private void InitiallizeScene()
    {
        // Hide all props by default
        SetUpGameObjectList(StoveGM_StoveProps, false);
        SetUpGameObjectList(StoveGM_SinkProps, false);
        SetUpGameObjectList(WokPotsPansGM_StoveProps, false);
        SetUpGameObjectList(WokPotsPansGM_SinkProps, false);
        SetUpGameObjectList(SinkGM_TrashProps, false);
        SetUpGameObjectList(FloorGM_SinkProps, false);
        SetUpGameObjectList(FloorGM_FloorProps, false);
        SetUpGameObjectList(ChillerGM_SinkProps, false);
        SetUpGameObjectList(ChillerGM_ChillerProps, false);
        SinkGM_PrepContainer.SetActive(false);
        SinkGM_PrepTrash.SetActive(false);
        SinkGM_SinkContainer.SetActive(false);
        FloorGM_FloorLMetal.SetActive(false);

        // Show gamemode specific props
        DisplayPropsByGamemode();
    }

    void DisplayPropsByGamemode()
    {
        ARCleanDataStore.ModelAccess = this;
        switch (ARCleanDataStore.GetPlayerGameMode())
        {
            case ARCleanDataStore.GameMode.GM_Daily_StoveArea:
                if (ARCleanDataStore.GetGamePhase() > 1 && ARCleanDataStore.GetGamePhase() < 5)
                    SetUpGameObjectList(StoveGM_SinkProps, true);
                else SetUpGameObjectList(StoveGM_StoveProps, true);
                break;
            case ARCleanDataStore.GameMode.GM_Daily_WokArea:
                break;
            case ARCleanDataStore.GameMode.GM_Daily_WokPotsPans:
                if (ARCleanDataStore.GetGamePhase() > 0 && ARCleanDataStore.GetGamePhase() < 4)
                    SetUpGameObjectList(WokPotsPansGM_SinkProps, true);
                else SetUpGameObjectList(WokPotsPansGM_StoveProps, true);
                break;
            case ARCleanDataStore.GameMode.GM_Daily_PreparationArea:
                break;
            case ARCleanDataStore.GameMode.GM_Daily_FloorDrains:
                // Need hide l shape metal separately
                if (ARCleanDataStore.GetGamePhase() < 5 || ARCleanDataStore.GetGamePhase() > 8)
                    SetUpGameObjectList(FloorGM_FloorProps, true);
                if (ARCleanDataStore.GetGamePhase() < 8 || ARCleanDataStore.GetGamePhase() > 9)
                    FloorGM_FloorLMetal.SetActive(true);
                if (ARCleanDataStore.GetGamePhase() == 8)
                    SetUpGameObjectList(FloorGM_SinkProps, true);
                break;
            case ARCleanDataStore.GameMode.GM_Daily_Chiller:
                if (ARCleanDataStore.GetGamePhase() > 0 && ARCleanDataStore.GetGamePhase() < 4)
                    SetUpGameObjectList(ChillerGM_SinkProps, true);
                else SetUpGameObjectList(ChillerGM_ChillerProps, true);
                break;
            case ARCleanDataStore.GameMode.GM_Daily_Sink:
                if (ARCleanDataStore.GetGamePhase() > 1)
                    SetUpGameObjectList(SinkGM_TrashProps, true);
                else SinkGM_PrepTrash.SetActive(true);
                if (ARCleanDataStore.GetGamePhase() == 3)
                    SinkGM_SinkContainer.SetActive(true);
                else SinkGM_PrepContainer.SetActive(true);
                break;
        }
    }

    public void SetCurrentTool()
    {
        // Deactivate the previous tool
        if (ARCleanDataStore.ModelAccess.CurrentCleaningTool != null)
        {
            ParticleSystem CurrentParticleSystem = ARCleanDataStore.ModelAccess.CurrentCleaningTool.GetComponent<ParticleSystem>();
            if (CurrentParticleSystem != null)
                CurrentParticleSystem.Stop();
            foreach (Transform Child in ARCleanDataStore.ModelAccess.CurrentCleaningTool.transform)
                Child.gameObject.SetActive(false);
        }
        // Set the new tool
        switch (ARCleanDataStore.GetPlayerTool())
        {
            case ARCleanDataStore.PlayerTool.PT_Sponge:
                ARCleanDataStore.ModelAccess.CurrentCleaningTool = ARCleanDataStore.ModelAccess.Tool_Sponge;
                break;
            case ARCleanDataStore.PlayerTool.PT_HeavySpongeGenie:
                ARCleanDataStore.ModelAccess.CurrentCleaningTool = ARCleanDataStore.ModelAccess.Tool_Genie;
                break;
            case ARCleanDataStore.PlayerTool.PT_HeavySpongeKellen:
                ARCleanDataStore.ModelAccess.CurrentCleaningTool = ARCleanDataStore.ModelAccess.Tool_Kellen;
                break;
            case ARCleanDataStore.PlayerTool.PT_DryCloth:
                ARCleanDataStore.ModelAccess.CurrentCleaningTool = ARCleanDataStore.ModelAccess.Tool_Cloth;
                break;
            case ARCleanDataStore.PlayerTool.PT_CookingOil:
                ARCleanDataStore.ModelAccess.CurrentCleaningTool = ARCleanDataStore.ModelAccess.Tool_Oil;
                break;
            case ARCleanDataStore.PlayerTool.PT_Alcosan:
                ARCleanDataStore.ModelAccess.CurrentCleaningTool = ARCleanDataStore.ModelAccess.Tool_Alcosan;
                break;
            case ARCleanDataStore.PlayerTool.PT_Broom:
                ARCleanDataStore.ModelAccess.CurrentCleaningTool = ARCleanDataStore.ModelAccess.Tool_Broom;
                break;
            case ARCleanDataStore.PlayerTool.PT_Glove:
                ARCleanDataStore.ModelAccess.CurrentCleaningTool = ARCleanDataStore.ModelAccess.Tool_Glove;
                break;
            case ARCleanDataStore.PlayerTool.PT_Water:
                ARCleanDataStore.ModelAccess.CurrentCleaningTool = ARCleanDataStore.ModelAccess.Tool_Water;
                break;
            case ARCleanDataStore.PlayerTool.PT_Alkaclean:
                ARCleanDataStore.ModelAccess.CurrentCleaningTool = ARCleanDataStore.ModelAccess.Tool_Alkaclean;
                break;
            case ARCleanDataStore.PlayerTool.PT_Wiper:
                ARCleanDataStore.ModelAccess.CurrentCleaningTool = ARCleanDataStore.ModelAccess.Tool_Wiper;
                break;
            case ARCleanDataStore.PlayerTool.PT_HardBrush:
                ARCleanDataStore.ModelAccess.CurrentCleaningTool = ARCleanDataStore.ModelAccess.Tool_HardBrush;
                break;
        }
    }

    public Transform LocateObject(Transform obj, string name)
    {
        foreach (Transform it in obj)
        {
            Transform temp = it.Find(name);
            if (temp != null)
                return temp;
        }
        return null;
    }

    public void ShowRotationalArrows(bool Show)
    {
        Button_Up.SetActive(Show);
        Button_Down.SetActive(Show);
        Button_Left.SetActive(Show);
        Button_Right.SetActive(Show);
    }

    private void HideTakenProps()
    {
        HideProp(ARCleanDataStore.PlayerTool.PT_Sponge, Prop_Sponge);
        HideProp(ARCleanDataStore.PlayerTool.PT_HeavySpongeKellen, Prop_Kellen);
        HideProp(ARCleanDataStore.PlayerTool.PT_HeavySpongeGenie, Prop_Genie);
        HideProp(ARCleanDataStore.PlayerTool.PT_DryCloth, Prop_Cloth);
        HideProp(ARCleanDataStore.PlayerTool.PT_CookingOil, Prop_Oil);
        HideProp(ARCleanDataStore.PlayerTool.PT_Alcosan, Prop_Alcosan);
        HideProp(ARCleanDataStore.PlayerTool.PT_Broom, Prop_Broom);
        HideProp(ARCleanDataStore.PlayerTool.PT_Glove, Prop_Glove);
        HideProp(ARCleanDataStore.PlayerTool.PT_Water, Prop_Water);
        HideProp(ARCleanDataStore.PlayerTool.PT_Alkaclean, Prop_Alkaclean);
        HideProp(ARCleanDataStore.PlayerTool.PT_Wiper, Prop_Wiper);
        HideProp(ARCleanDataStore.PlayerTool.PT_HardBrush, Prop_HardBrush);
    }

    private void HideProp(ARCleanDataStore.PlayerTool ToolType, GameObject Prop)
    {
        if (ARCleanDataStore.PlayerToolInventory[(int)ToolType])
            Prop.SetActive(false);
    }
}