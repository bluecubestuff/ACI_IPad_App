/* 
 * Author: Lim Rui An Ryan, Lee Kwan Liang
 * Filename: ARGameLogicSystem.cs
 * Description: A script that is used to determine where the player is touching and object and whether the touch counts as an increment to the progress.
 */
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ARGameLogicSystem : MonoBehaviour
{
    // Public Variables
    public TextMeshProUGUI ProgressHeaderText;
    public TextMeshProUGUI ProgressSubHeaderText;
    public Slider ProgressBar;
    public GameObject TransitionInterface;
    public TextMeshProUGUI TransitionHeader, TransitionSubHeader;
    public Button TransitionScan, TransitionMenu;
    public float InterfaceMovementSpeed = 5f;
    public static float PlayTime = 0f;

    // Private Variables
    [SerializeField] private GameObject Hints_Page;
    private Vector3 HintOrigin;
    private Vector3 HintTarget = new Vector3(Screen.width * -1f, 0, 0);
    private bool DisplayHints;
    private ARCleanStoveLogic StoveGameLogic = new ARCleanStoveLogic();
    private ARCleanWokAreaLogic WokAreaGameLogic = new ARCleanWokAreaLogic();
    private ARCleanWokPotsPansLogic WokPotPansGameLogic = new ARCleanWokPotsPansLogic();
    private ARCleanPrepAreaLogic PrepAreaGameLogic = new ARCleanPrepAreaLogic();
    private ARCleanFloorDrainsLogic FloorDrainsGameLogic = new ARCleanFloorDrainsLogic();
    private ARCleanSinkLogic SinkGameLogic = new ARCleanSinkLogic();
    private ARCleanChillerLogic ChillerGameLogic = new ARCleanChillerLogic();
    private bool InitiallizeTransitUI = false;

    // Use this for initialization
    void Start()
    {
        HintOrigin = Hints_Page.GetComponent<RectTransform>().position;
        InitiallizeScene();
        // Here I assume the player already set stuff
        StoveGameLogic.ProgressBar = WokAreaGameLogic.ProgressBar = WokPotPansGameLogic.ProgressBar = PrepAreaGameLogic.ProgressBar = FloorDrainsGameLogic.ProgressBar = SinkGameLogic.ProgressBar = ChillerGameLogic.ProgressBar = ProgressBar;
        StoveGameLogic.ProgressHeaderText = WokAreaGameLogic.ProgressHeaderText = WokPotPansGameLogic.ProgressHeaderText = PrepAreaGameLogic.ProgressHeaderText = FloorDrainsGameLogic.ProgressHeaderText = SinkGameLogic.ProgressHeaderText = ChillerGameLogic.ProgressHeaderText = ProgressHeaderText;
        StoveGameLogic.ProgressSubHeaderText = WokAreaGameLogic.ProgressSubHeaderText = WokPotPansGameLogic.ProgressSubHeaderText = PrepAreaGameLogic.ProgressSubHeaderText = FloorDrainsGameLogic.ProgressSubHeaderText = SinkGameLogic.ProgressSubHeaderText = ChillerGameLogic.ProgressSubHeaderText = ProgressSubHeaderText;
        // Here I assume the player already set stuff, do some logic related stuff
    }

    public void InitiallizeScene()
    {
        Hints_Page.GetComponent<RectTransform>().position = HintTarget;
        DisplayHints = false;
        ARCleanDataStore.ModelAccess.Model_Room.SetActive(ARCleanDataStore.GetPlayerLocation() != ARCleanDataStore.GameLocation.GL_Laundry);
        ARCleanDataStore.ModelAccess.HideMainProps();
        switch (ARCleanDataStore.GetPlayerLocation())
        {
            case ARCleanDataStore.GameLocation.GL_Stove:
                ARCleanDataStore.ModelAccess.Model_Stove.SetActive(true);
                break;
            case ARCleanDataStore.GameLocation.GL_Wok:
                ARCleanDataStore.ModelAccess.Model_Wok.SetActive(true);
                break;
            case ARCleanDataStore.GameLocation.GL_Prep:
                ARCleanDataStore.ModelAccess.Model_Prep.SetActive(true);
                break;
            case ARCleanDataStore.GameLocation.GL_Trash:
                ARCleanDataStore.ModelAccess.Model_Trash.SetActive(true);
                break;
            case ARCleanDataStore.GameLocation.GL_Floor:
                ARCleanDataStore.ModelAccess.Model_Floor.SetActive(true);
                break;
            case ARCleanDataStore.GameLocation.GL_Sink:
                ARCleanDataStore.ModelAccess.Model_Sink.SetActive(true);
                break;
            case ARCleanDataStore.GameLocation.GL_Chiller:
                ARCleanDataStore.ModelAccess.Model_Chiller.SetActive(true);
                break;
            case ARCleanDataStore.GameLocation.GL_Laundry:
                ARCleanDataStore.ModelAccess.Model_Laundry.SetActive(true);
                break;
        }
        TransitionInterface.GetComponent<RectTransform>().position = new Vector3(Screen.width * 0.5f, Screen.height * 2f, 0);
        ARCleanDataStore.HideRotationalArrows = false;
    }

    public void DisplayHintsInterface()
    {
        DisplayHints = !DisplayHints;
    }

    public bool GetDisplayHint()
    {
        return DisplayHints;
    }

    public void SetHintSetting(bool Enable)
    {
        ARCleanDataStore.ShowHints = Enable;
        DisplayHints = false;
    }

    void MoveDownInterface(GameObject Page, bool MoveDown, Vector3 Origin, Vector3 Target)
    {
        Vector3 NewTarget = Vector3.zero;
        if (MoveDown)
            NewTarget = Origin;
        else NewTarget = Target;
        Vector3 Direction = Page.GetComponent<RectTransform>().position - NewTarget;
        if (Direction.sqrMagnitude > 1)
            Page.GetComponent<RectTransform>().position -= Direction * Time.deltaTime * InterfaceMovementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        MoveDownInterface(Hints_Page, DisplayHints, HintOrigin, HintTarget);
        if (ARCleanDataStore.GameModeTransit)
        {
            // Display the change of game mode UI
            TransitionUIUpdate();
            ARCleanDataStore.GameModeTransitDelimiter = false;
        }
        else if (!ErrorUIUpdate(ARCleanDataStore.ErrorScreenActive))
        {
            ARCleanDataStore.DetectPlayerInput();
            GameLogicUpdate();
            ToolLogicUpdate();
            PlayTime += Time.deltaTime;
            //ToolSelectionUIUpdate();
        }
    }

    private void ToolSelectionUIUpdate()
    {
        foreach (ToolUIQuickAccess Tool in ARCleanDataStore.LinkedToolInventory)
            if (Tool != null)
                Tool.SelectedIcon.SetActive(ARCleanDataStore.GetPlayerTool() == Tool.ToolType);
    }

    private void TransitionUIUpdate()
    {
        if (!InitiallizeTransitUI)
        {
            // Set the text of the UI
            switch (ARCleanDataStore.GetPlayerGameMode())
            {
                case ARCleanDataStore.GameMode.GM_Daily_StoveArea:
                    TransitionHeader.text = "Stove Area Cleaned Up!";
                    break;
                case ARCleanDataStore.GameMode.GM_Daily_WokPotsPans:
                    TransitionHeader.text = "Wok, Pots & Pans Cleaned Up!";
                    break;
                case ARCleanDataStore.GameMode.GM_Daily_WokArea:
                    TransitionHeader.text = "Wok Area Cleaned Up!";
                    break;
                case ARCleanDataStore.GameMode.GM_Daily_PreparationArea:
                    TransitionHeader.text = "Preparation Area Cleaned Up!";
                    break;
                case ARCleanDataStore.GameMode.GM_Daily_FloorDrains:
                    TransitionHeader.text = "Floor and Drains Cleaned Up!";
                    break;
                case ARCleanDataStore.GameMode.GM_Daily_Sink:
                    TransitionHeader.text = "Sink Cleaned Up!";
                    break;
                case ARCleanDataStore.GameMode.GM_Daily_Chiller:
                    TransitionHeader.text = "Station Chiller Cleaned Up!";
                    break;
            }
            if (ARCleanDataStore.GetPlayerGameMode() != ARCleanDataStore.GameMode.GM_Max - 1)
                TransitionSubHeader.text = "You have successfully completed the current game mode! \n\nPress the button below to return to Scanning Mode!";
            else
            {
                int Minutes = (int)(PlayTime / 60f);
                int Seconds = (int)(PlayTime - Minutes * 60f);
                TransitionSubHeader.text = "You have successfully completed all the game modes!\n\n" + "You made a total of " + ARCleanDataStore.PenaltyCounter.ToString() + " Penalties and completed the cleaning game mode in ";
                if (Minutes > 0)
                    TransitionSubHeader.text += Minutes + " Minute(s) and ";
                TransitionSubHeader.text += Seconds + " Second(s)!";
                TransitionScan.gameObject.SetActive(false);
                TransitionMenu.gameObject.SetActive(true);
            }
            InitiallizeTransitUI = true;
        }
        Vector3 Direction = TransitionInterface.GetComponent<RectTransform>().position - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        if (Direction.sqrMagnitude > 1)
            TransitionInterface.GetComponent<RectTransform>().position -= Direction * Time.deltaTime * InterfaceMovementSpeed;
    }

    private bool ErrorUIUpdate(bool MoveDown)
    {
        Vector3 TargetPosition = Vector3.zero;
        if (MoveDown)
            TargetPosition = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        else TargetPosition = new Vector3(Screen.width * 0.5f, Screen.height * 2f, 0);
        Vector3 Direction = ARCleanDataStore.ModelAccess.ErrorInterface.GetComponent<RectTransform>().position - TargetPosition;
        if (Direction.sqrMagnitude > 1)
            ARCleanDataStore.ModelAccess.ErrorInterface.GetComponent<RectTransform>().position -= Direction * Time.deltaTime * InterfaceMovementSpeed;
        return MoveDown;
    }

    private void GameLogicUpdate()
    {
        switch (ARCleanDataStore.GetPlayerGameMode())
        {
            case ARCleanDataStore.GameMode.GM_Daily_StoveArea:
                StoveGameLogic.Update();
                break;
            case ARCleanDataStore.GameMode.GM_Daily_WokArea:
                WokAreaGameLogic.Update();
                break;
            case ARCleanDataStore.GameMode.GM_Daily_WokPotsPans:
                WokPotPansGameLogic.Update();
                break;
            case ARCleanDataStore.GameMode.GM_Daily_PreparationArea:
                PrepAreaGameLogic.Update();
                break;
            case ARCleanDataStore.GameMode.GM_Daily_FloorDrains:
                FloorDrainsGameLogic.Update();
                break;
            case ARCleanDataStore.GameMode.GM_Daily_Sink:
                SinkGameLogic.Update();
                break;
            case ARCleanDataStore.GameMode.GM_Daily_Chiller:
                ChillerGameLogic.Update();
                break;
        }
    }

    private void ToolLogicUpdate()
    {
        ARCleanDataStore.ModelAccess.SetCurrentTool();
        if (ARCleanDataStore.ModelAccess.CurrentCleaningTool != null && !ARCleanDataStore.CheckIfMouseIsOverUI())
        {
            ParticleSystem CurrentParticleSystem = ARCleanDataStore.ModelAccess.CurrentCleaningTool.GetComponent<ParticleSystem>();
            if (ARCleanDataStore.CleanToolActive)
            {
                if (CurrentParticleSystem != null)
                    CurrentParticleSystem.Play();
                foreach (Transform Child in ARCleanDataStore.ModelAccess.CurrentCleaningTool.transform)
                    Child.gameObject.SetActive(true);
            }
            else
            {
                if (CurrentParticleSystem != null)
                    CurrentParticleSystem.Stop();
                foreach (Transform Child in ARCleanDataStore.ModelAccess.CurrentCleaningTool.transform)
                    Child.gameObject.SetActive(false);
            }
        }
        ARCleanDataStore.CleanToolActive = false;
    }
}