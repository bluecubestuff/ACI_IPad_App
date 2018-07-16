/* 
 * Author: Lim Rui An Ryan
 * Filename: ARCleanManager.cs
 * Description: A script that transits the scene accordingly depending on what ar marker was scanned.
 */
using UnityEngine;
using TMPro;

public class ARCleanManager : MonoBehaviour
{
    // Public Variables
    public TextMeshProUGUI ProgressHeaderText;
    public TextMeshProUGUI ProgressSubHeaderText;
    // Private Variables
    // Game Flags
    [SerializeField] GameObject Flag_Stove;
    [SerializeField] GameObject Flag_Wok;
    [SerializeField] GameObject Flag_Sink;
    [SerializeField] GameObject Flag_Preparation;
    [SerializeField] GameObject Flag_Trash;
    [SerializeField] GameObject Flag_Floor;
    [SerializeField] GameObject Flag_Chiller;
    [SerializeField] GameObject Flag_Laundry;

    [SerializeField] TransitionSystem LinkedTransit;
    [Header("Debug Parameter")]
    [SerializeField] bool DebugMode = false;
    [SerializeField] GameState DebugGameLocation = GameState.GS_Stove;
    [SerializeField] ARCleanDataStore.GameMode DebugGameMode = ARCleanDataStore.GameMode.GM_Undefined;
    [SerializeField] int DebugGamePhase = 0;
    enum GameState
    {
        GS_Default = 0,
        GS_Stove,
        GS_Wok,
        GS_Sink,
        GS_Prep,
        GS_Trash,
        GS_Floor,
        GS_Chiller,
        GS_Laundry,
    }

    // Use this for initialization
    void Start () {
        // Initiallization calls
    }
    
    // Update is called once per frame
    void Update () {
        // Update Scene Text
        UpdateSceneHeaders();
        UpdateSceneSubHeaders();
        // Check if anything was scanned in succession
        HandleSceneTransitions();
    }

    private void UpdateSceneHeaders()
    {
        switch (ARCleanDataStore.GetPlayerGameMode())
        {
            case ARCleanDataStore.GameMode.GM_Daily_StoveArea:
                ProgressHeaderText.text = "Stove Area Tasks";
                break;
            case ARCleanDataStore.GameMode.GM_Daily_WokPotsPans:
                ProgressHeaderText.text = "Wok, Pots & Pans Tasks";
                break;
            case ARCleanDataStore.GameMode.GM_Daily_WokArea:
                ProgressHeaderText.text = "Wok Area Tasks";
                break;
            case ARCleanDataStore.GameMode.GM_Daily_PreparationArea:
                ProgressHeaderText.text = "Preparation Area Tasks";
                break;
            case ARCleanDataStore.GameMode.GM_Daily_Sink:
                ProgressHeaderText.text = "Sink Area Tasks";
                break;
            case ARCleanDataStore.GameMode.GM_Daily_FloorDrains:
                ProgressHeaderText.text = "Floor & Drain Tasks";
                break;
            case ARCleanDataStore.GameMode.GM_Daily_Chiller:
                ProgressHeaderText.text = "Station Chiller Tasks";
                break;
        }
    }

    private void UpdateSceneSubHeaders()
    {
        // Set the location to go to based on the state the game
        switch (ARCleanDataStore.GetTargetGameModeLocation())
        {
            case ARCleanDataStore.GameLocation.GL_Stove:
                ProgressSubHeaderText.text = "Go to the Stove Area";
                break;
            case ARCleanDataStore.GameLocation.GL_Wok:
                ProgressSubHeaderText.text = "Go to the Wok Area";
                break;
            case ARCleanDataStore.GameLocation.GL_Sink:
                ProgressSubHeaderText.text = "Go to the Sink Area";
                break;
            case ARCleanDataStore.GameLocation.GL_Prep:
                ProgressSubHeaderText.text = "Go to the Preparation Area";
                break;
            case ARCleanDataStore.GameLocation.GL_Trash:
                ProgressSubHeaderText.text = "Go to the Trash Area";
                break;
            case ARCleanDataStore.GameLocation.GL_Floor:
                ProgressSubHeaderText.text = "Go to the Floor Area";
                break;
            case ARCleanDataStore.GameLocation.GL_Chiller:
                ProgressSubHeaderText.text = "Go to the Station Chiller Area";
                break;
            case ARCleanDataStore.GameLocation.GL_Laundry:
                ProgressSubHeaderText.text = "Go to the Laundry Room Area";
                break;
        }
    }

    private void HandleSceneTransitions()
    {
        GameState TempSceneState = GetCurrentFlag();
        if (DebugMode)
        {
            TempSceneState = DebugGameLocation;
            ARCleanDataStore.SetGamePhase(DebugGamePhase);
            ARCleanDataStore.SetPlayerGameMode(DebugGameMode);
        }

        if (TempSceneState != GameState.GS_Default)
        {
            switch (TempSceneState)
            {
                case GameState.GS_Stove:
                    ARCleanDataStore.SetPlayerLocation(ARCleanDataStore.GameLocation.GL_Stove);
                    break;
                case GameState.GS_Wok:
                    ARCleanDataStore.SetPlayerLocation(ARCleanDataStore.GameLocation.GL_Wok);
                    break;
                case GameState.GS_Sink:
                    ARCleanDataStore.SetPlayerLocation(ARCleanDataStore.GameLocation.GL_Sink);
                    break;
                case GameState.GS_Prep:
                    ARCleanDataStore.SetPlayerLocation(ARCleanDataStore.GameLocation.GL_Prep);
                    break;
                case GameState.GS_Trash:
                    ARCleanDataStore.SetPlayerLocation(ARCleanDataStore.GameLocation.GL_Trash);
                    break;
                case GameState.GS_Floor:
                    ARCleanDataStore.SetPlayerLocation(ARCleanDataStore.GameLocation.GL_Floor);
                    break;
                case GameState.GS_Chiller:
                    ARCleanDataStore.SetPlayerLocation(ARCleanDataStore.GameLocation.GL_Chiller);
                    break;
                case GameState.GS_Laundry:
                    ARCleanDataStore.SetPlayerLocation(ARCleanDataStore.GameLocation.GL_Laundry);
                    break;
            }
            LinkedTransit.IncrementScene();
        }
    }

    private GameState GetCurrentFlag(){
        // Obscenely hardcoded, nothing I can do about this
        if (Flag_Stove.activeSelf)
            return GameState.GS_Stove;
        else if (Flag_Wok.activeSelf)
            return GameState.GS_Wok;
        else if (Flag_Sink.activeSelf)
            return GameState.GS_Sink;
        else if (Flag_Preparation.activeSelf)
            return GameState.GS_Prep;
        else if (Flag_Trash.activeSelf)
            return GameState.GS_Trash;
        else if (Flag_Floor.activeSelf)
            return GameState.GS_Floor;
        else if (Flag_Chiller.activeSelf)
            return GameState.GS_Chiller;
        else if (Flag_Laundry.activeSelf)
            return GameState.GS_Laundry;
        else return GameState.GS_Default;
    }
    
    public void ReturnToScanMode()
    {
        Flag_Stove.SetActive(false);
        Flag_Wok.SetActive(false);
        Flag_Sink.SetActive(false);
        Flag_Preparation.SetActive(false);
        Flag_Trash.SetActive(false);
        Flag_Floor.SetActive(false);
        Flag_Chiller.SetActive(false);
        Flag_Laundry.SetActive(false);
    }
}
