/* 
 * Author: Lim Rui An Ryan, Lee Kwan Liang
 * Filename: ARCleanDataStore.cs
 * Description: An object that contains data regarding the cleaning game mode which is carried across scenes
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ARCleanDataStore : MonoBehaviour {
    
    // Public Variables
    public enum GameLocation
    {
        GL_Undefined = 0,
        GL_Tutorial,
        GL_Stove,
        GL_Wok,
        GL_Sink,
        GL_Prep,
        GL_Trash,
        GL_Floor,
        GL_Chiller,
        GL_Laundry,
    }

    public enum GameMode
    {
        GM_Undefined = 0,
        GM_Daily_StoveArea,
        GM_Daily_WokPotsPans,
        GM_Daily_WokArea,
        GM_Daily_PreparationArea,
        GM_Daily_Sink,
        GM_Daily_FloorDrains,
        GM_Daily_Chiller,
        GM_Max,
        GM_Tutorial,
    }

    public enum PlayerTool
    {
        PT_Undefined = 0,
        PT_Sponge,
        PT_Genie,
        PT_Kellen,
        PT_DryCloth,
        PT_CookingOil,
        PT_Alcosan,
        PT_Broom,
        PT_Glove,
        PT_Water,
        PT_Alkaclean,
        PT_Wiper,
        PT_HardBrush,
        PT_MAX,

        // combinations to be place below max
        PT_HeavySpongeGenie,
        PT_HeavySpongeKellen,
        PT_ClothAlcosan,
    }

    public static List<KeyValuePair<string, Sprite>> InventoryList;
    public static ARInventoryManager Inventory;
    public static ARCleanCamera.Directions CameraCurrentDirection;
    public static ARModelAccessibility ModelAccess;
    public static ARCleanDoorAnimation CurrentSceneDoor;
    public static string RequiredObject;
    public static bool CleanToolActive;
    public static bool HideRotationalArrows;
    public static List<bool> PlayerToolInventory;
    public static List<ToolUIQuickAccess> LinkedToolInventory;
    public static bool GameModeTransit;
    public static bool GameModeTransitDelimiter;
    public static bool ErrorScreenActive;
    public static bool ShowHints;
    public static bool ForceCurrentDoorClose;
    public static int PenaltyCounter;
    public static int GameModeGameState;
    public static string SoundSystemName = "ARCSoundSystem";
    public static Pair<PlayerTool, PlayerTool> playerTools;

    // Private Variables
    [SerializeField] private bool DebugMode = false;
    [SerializeField] private bool EnableHints = true;
    [SerializeField] private static GameLocation CurrentGameLocation;
    [SerializeField] private static GameMode CurrentGameMode;
    [SerializeField] private static int CurrentGamePhase;
    [SerializeField] private static PlayerTool CurrentPlayerTool;

    public static bool ObjectInteractibleFlag;
    public static bool PlayerInputFlag;
    public static bool PlayerInputFlagReset;
    static bool Initiallized = false;

    // Use this for initialization
    void Start ()
    {
		if (!Initiallized)
        {
            Initiallize();
        }
    }

    public void Initiallize()
    {
        if (DebugMode)
            ShowHints = EnableHints;
        Initiallized = true;
        CurrentGameLocation = GameLocation.GL_Laundry;
        CurrentGameMode = GameMode.GM_Daily_StoveArea;
        CurrentPlayerTool = PlayerTool.PT_Undefined;
        CurrentGamePhase = 0;
        PlayerToolInventory = new List<bool>();
        for (int i = (int)PlayerTool.PT_Undefined; i < (int)PlayerTool.PT_MAX; ++i)
            if (!DebugMode)
                PlayerToolInventory.Add(false);
            else
                PlayerToolInventory.Add(true);
        InventoryList = new List<KeyValuePair<string, Sprite>>();
        LinkedToolInventory = new List<ToolUIQuickAccess>();
        PenaltyCounter = 0;
        GameModeGameState = 0;
        ObjectInteractibleFlag = true;
        PlayerInputFlag = false;
        PlayerInputFlagReset = true;
        GameModeTransit = false;
        GameModeTransitDelimiter = false;
        ErrorScreenActive = false;
        ShowHints = true;
        ForceCurrentDoorClose = false;
        HideRotationalArrows = false;
        RequiredObject = "";
        playerTools = new Pair<PlayerTool, PlayerTool>(PlayerTool.PT_Undefined, PlayerTool.PT_Undefined);
    }

    // Getters
    public static GameLocation GetPlayerLocation()
    {
        return CurrentGameLocation;
    }
    public static GameMode GetPlayerGameMode()
    {
        return CurrentGameMode;
    }
    public static int GetGamePhase()
    {
        return CurrentGamePhase;
    }
    public static PlayerTool GetPlayerTool()
    {
        return CurrentPlayerTool;
    }

    public static GameLocation GetTargetGameModeLocation()
    {
        switch (CurrentGameMode)
        {
            case GameMode.GM_Daily_StoveArea:
                return ARCleanStoveLogic.GetGameLocationForCurrentPhase(CurrentGamePhase);
            case GameMode.GM_Daily_WokPotsPans:
                return ARCleanWokPotsPansLogic.GetGameLocationForCurrentPhase(CurrentGamePhase);
            case GameMode.GM_Daily_WokArea:
                return ARCleanWokAreaLogic.GetGameLocationForCurrentPhase(CurrentGamePhase);
            case GameMode.GM_Daily_PreparationArea:
                return ARCleanPrepAreaLogic.GetGameLocationForCurrentPhase(CurrentGamePhase);
            case GameMode.GM_Daily_Sink:
                return ARCleanSinkLogic.GetGameLocationForCurrentPhase(CurrentGamePhase);
            case GameMode.GM_Daily_FloorDrains:
                return ARCleanFloorDrainsLogic.GetGameLocationForCurrentPhase(CurrentGamePhase);
            case GameMode.GM_Daily_Chiller:
                return ARCleanChillerLogic.GetGameLocationForCurrentPhase(CurrentGamePhase);
        }
        return GameLocation.GL_Undefined;
    }

    public static void FindAndDestroySoundSystem()
    {
        GameObject MusicPlayer = GameObject.Find(SoundSystemName);
        if (MusicPlayer != null)
            Destroy(MusicPlayer);
    }

    // Setters
    public static void SetPlayerLocation(GameLocation DesiredLocation)
    {
        CurrentGameLocation = DesiredLocation;
    }
    public static void SetPlayerGameMode(GameMode DesiredMode)
    {
        CurrentGameMode = DesiredMode;
    }
    public static void SetGamePhase(int DesiredPhase)
    {
        CurrentGamePhase = DesiredPhase;
    }
    public static void SetPlayerTool(PlayerTool DesiredTool)
    {
        CurrentPlayerTool = DesiredTool;
    }
    public void SetPlayerTool(int DesiredTool)
    {
        CurrentPlayerTool = (PlayerTool)DesiredTool;
    }

    public static void IncrementGameProgress()
    {
        SetGamePhase(GetGamePhase() + 1);
    }

    public static void SetupForNextGameMode()
    {
        GameModeTransit = true;
    }
    
    public void SetErrorInterface(bool ErrorActive)
    {
        ErrorScreenActive = ErrorActive;
    }

    public void IncrementGameModeReturnToScan()
    {
        GameModeTransit = false;
        if (!ARCleanDataStore.GameModeTransitDelimiter)
            SetPlayerGameMode(GetPlayerGameMode() + 1);
        ARCleanDataStore.GameModeTransitDelimiter = true;
        SetGamePhase(0);
        // Return to scan mode
        HideRotationalArrows = false;
        GetComponent<TransitionSystem>().DecrementScene();
    }

    public void ResetGameInternalPhase()
    {
        GameModeGameState = 0;
        playerTools.First = PlayerTool.PT_Undefined;
        playerTools.Second = PlayerTool.PT_Undefined;
    }

    public static bool DetectPlayerInput()
    {
        if (Input.GetMouseButton(0))
        {
            PlayerInputFlag = true;
        }
        else
        {
            // Reset it if the player lets go
            PlayerInputFlagReset = true;
            PlayerInputFlag = false;
        }
        // If not reset, do not register input
        if (!PlayerInputFlagReset)
            PlayerInputFlag = false;
        return PlayerInputFlag;
    }

    public static bool CheckIfMouseIsOverUI()
    {
        PointerEventData PED = new PointerEventData(null);
        PED.position = Input.mousePosition;
        List<RaycastResult> HitList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(PED, HitList);
        if (HitList.Count > 0 && HitList[0].gameObject.GetComponent<RectTransform>() != null)
            return true;
        else return false;
    }

    public void ReinitallizeEntireGame()
    {
        // Reinit game variables
        Initiallize();
        FindAndDestroySoundSystem();
        StocknPopularityManager.ReinitiallizeRestaurant();
    }
}