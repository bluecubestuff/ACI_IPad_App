using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MeatFabManager : MonoBehaviour {

    //Singleton? Maybe
    public static MeatFabManager Instance;

    [SerializeField]
    AudioClip choppingSound;
    [SerializeField]
    AudioClip applause;
    [SerializeField]
    AudioClip wrong;

    AudioManager SFX;
    public static bool fabricatedMeat = false;

    //Calls list of fabricated items from FabricationDatabase
    public FabricationDatabase database;

    //Type of meat - Chicken / Shellfish (Crab) / Fish
    public TYPE_OF_MEAT meatType;

    //Dropdown Menu for selecting steps
    public TMP_Dropdown MeatSelectionDropdownUI;
    ScrollRect scrollRect;
    //Parent of the sliceableObject - Used for resetting the sliceable object
    public Transform ParentOfSlicedObjects;

    //Sliceable Object Prefab to instantiate when resetting scene
    public GameObject SlicePrefab;

    //Result tab (Correct + Incorrect) | Result text (Correct + Incorrect) + Hint for Incorrect | Correct Result Image
    public GameObject correctResultTab, wrongResultTab;
    public TextMeshProUGUI correctResultText, wrongResultText, wrongResultHint;

    [SerializeField]
    Image wrongResultImage;
    public Image correctResultImage;

    //After fully fabricating a meat
    public GameObject finishedFabricationTab;
    public TextMeshProUGUI finishedFabricationText;
    public float locY;
    //Booleans to check for Start, End Success and Start, End Failures
    public bool startSuccess, endSuccess;
    public bool startFail, endFail;

    bool flipStartnEnd;
    [SerializeField]
    ParticleSystem ps;
    //Start Position, End Position and Range of Position
    public float startBaseValue_X, startBaseValue_Y, endBaseValue_X, endBaseValue_Y, rangeX, rangeY;

    [SerializeField]
    GameObject options;

    [SerializeField]
    GameObject orderList;

    //Enum of Meat Types
    public enum TYPE_OF_MEAT
    {
        DEFAULT,
        CHICKEN,
        FISH,
        SHELLFISH
    };

    //Steps for the fabrication
    public int selection = 0;
    //Bool for UIActive / Touchdown
    public bool UIActive, touchDown;

    //Reset line renderer
    public bool resetLinerenderer;
    int wrongCounter;
    //"Tutorial / Hint / Instructions" for first time users
    public GameObject instructionTab;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Use this for initialization
    void Start ()
    {
        scrollRect = MeatSelectionDropdownUI.transform.GetChild(0).GetComponent<ScrollRect>();
        SFX = GameObject.FindGameObjectWithTag("AudioParent").GetComponent<AudioManager>();

        wrongCounter = 0;
        MeatSelectionDropdownUI.onValueChanged.AddListener(delegate
        {
            ValueChange(MeatSelectionDropdownUI);
        });
        fabricatedMeat = true;

        instructionTab.SetActive(true);
        flipStartnEnd = false;
        rangeX = 1.8f;
        rangeY = 1.5f;
        selection = 0;
        UIActive = false;
        touchDown = false;
        startFail = false;
        endFail = false;
        
        startBaseValue_X = 0;
        endBaseValue_X = 0;
        startBaseValue_Y = 0;
        endBaseValue_Y = 0;

        startSuccess = false;
        endSuccess = false;
        resetLinerenderer = false;

        meatType = TYPE_OF_MEAT.DEFAULT;
    }

    // Update is called once per frame
    void Update ()
    {
        //Constantly resets bool unless cut fails / succeeds
        endFail = false;
        endSuccess = false;
        //Debug.Log("start success: " + startSuccess);
        //Debug.Log("start fail: " + startFail);
        //Debug.Log("end success: " + endSuccess);
        //Debug.Log("end fail: " + endFail);

        //UpdateSelection();
        if (wrongCounter < 3)
            wrongResultImage.sprite = Resources.Load<Sprite>("MeatFabrication/TryAgain");

        if (selection != MeatSelectionDropdownUI.value)
        {
            selection = MeatSelectionDropdownUI.value;
            UpdateSliceableBeforeCut();
        }
        wrongResultImage.SetNativeSize();

        //Check if UI is active - if UIActive, raycast for meat fabrication would be off
        if (correctResultTab.activeSelf || wrongResultTab.activeSelf || MeatSelectionDropdownUI.transform.childCount > 3 || finishedFabricationTab.activeSelf || options.activeSelf || orderList.activeSelf)
        {
            UIActive = true;
        }
        else
        {
            UIActive = false;
        }

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        //mouse down
        //if(meatType != TYPE_OF_MEAT.DEFAULT)
        if (Input.GetMouseButtonDown(0))
        {
            touchDown = true;
            //hits collider + not in UI
            if (hit.collider != null && !UIActive && meatType != TYPE_OF_MEAT.DEFAULT)
            {
                //Debug.Log("Target Position: " + hit.point.x + " " + hit.point.y);
                //if hit start x range
                if (hit.point.x < startBaseValue_X + rangeX && hit.point.x > startBaseValue_X - rangeX)
                {
                    //if hit start y range
                    if (hit.point.y < startBaseValue_Y + rangeY && hit.point.y > startBaseValue_Y - rangeY)
                    {
                        startSuccess = true;
                        startFail = false;
                    }
                    else
                    {
                        startFail = true;
                        startSuccess = false;
                    }
                }
                else if (hit.point.x < endBaseValue_X + rangeX && hit.point.x > endBaseValue_X - rangeX)
                {
                    //if hit start y range
                    if (hit.point.y < endBaseValue_Y + rangeY && hit.point.y > endBaseValue_Y - rangeY)
                    {
                        flipStartnEnd = true;
                        startSuccess = true;
                        startFail = false;
                    }
                    else
                    {
                        flipStartnEnd = false;
                        startFail = true;
                        startSuccess = false;
                    }
                }
                //if doesnt hit
                else
                {
                    startFail = true;
                    startSuccess = false;
                }
            }
        }
        if (Input.GetMouseButtonUp(0) && touchDown)
        {
            if (hit.collider != null && !UIActive)
            {
                //Debug.Log("Target Position: " + hit.point);
                if (!flipStartnEnd)
                {
                    if (hit.point.x < endBaseValue_X + rangeX && hit.point.x > endBaseValue_X - rangeX)
                    {
                        if (hit.point.y < endBaseValue_Y + rangeY && hit.point.y > endBaseValue_Y - rangeY)
                        {
                            endSuccess = true;
                            endFail = false;
                        }
                        else
                        {
                            endFail = true;
                            endSuccess = false;
                        }
                    }
                    else
                    {
                        endFail = true;
                        endSuccess = false;
                    }
                }
                else
                {
                    if (hit.point.x < startBaseValue_X + rangeX && hit.point.x > startBaseValue_X - rangeX)
                    {
                        if (hit.point.y < startBaseValue_Y + rangeY && hit.point.y > startBaseValue_Y - rangeY)
                        {
                            endSuccess = true;
                            endFail = false;
                        }
                        else
                        {
                            endFail = true;
                            endSuccess = false;
                            flipStartnEnd = false;
                        }
                    }
                    else
                    {
                        flipStartnEnd = false;
                        endFail = true;
                        endSuccess = false;
                    }
                }
            }
            touchDown = false;
            flipStartnEnd = false;
        }

        //if cut success
        if (startSuccess && endSuccess)
        {
            ParentOfSlicedObjects.GetComponentInChildren<SpriteRenderer>().enabled = false;

            ShowCorrectResults();
        }
        //if cut fail
        if((startFail && endFail) || (startSuccess && endFail) || (startFail && endSuccess)) 
        {
            ShowWrongResults();
        }
    }

    //For dropdown UI - on value change
    private void ValueChange(TMP_Dropdown g_dropdown)
    {
        //Debug.Log(g_dropdown.transform.GetChild(0).GetChild(0).GetChild(0).localPosition.y);
    }

    //When selecting meat type with button (Sets meat type + options[steps required])
    public void SetTypeOfMeat(string type)
    {
        MeatSelectionDropdownUI.options.Clear();
        wrongCounter = 0;
        locY = 0;
        EventSystem.current.SetSelectedGameObject(null);
        if(finishedFabricationTab.activeSelf)
        finishedFabricationTab.SetActive(false);

        switch (type)
        {
            case "chicken":
                meatType = TYPE_OF_MEAT.CHICKEN;
                for(int i = 0; i < database.ChickenParts.Count; i++)
                {
                    MeatSelectionDropdownUI.options.Add(new TMP_Dropdown.OptionData(database.ChickenParts[i].ChickenName));
                }
                break;
            case "fish":
                meatType = TYPE_OF_MEAT.FISH;
                for (int i = 0; i < database.FishParts.Count; i++)
                {
                    MeatSelectionDropdownUI.options.Add(new TMP_Dropdown.OptionData(database.FishParts[i].FishName));

                }
                break;
            case "shellfish":
                meatType = TYPE_OF_MEAT.SHELLFISH;
                for (int i = 0; i < database.ShellfishParts.Count; i++)
                {
                    MeatSelectionDropdownUI.options.Add(new TMP_Dropdown.OptionData(database.ShellfishParts[i].ShellfishName));
                }
                break;
        }
        MeatSelectionDropdownUI.value = 0;
        MeatSelectionDropdownUI.RefreshShownValue();
        UpdateSliceableBeforeCut();
    }

    //Check and update sprites accordingly before cut
    public void UpdateSliceableBeforeCut()
    {
        wrongCounter = 0;
        if ((MeatSelectionDropdownUI.transform.childCount > 3))
        {
            locY = MeatSelectionDropdownUI.transform.GetChild(3).transform.GetChild(0).GetChild(0).transform.localPosition.y;
            //Debug.Log(locY);
        }
        if (meatType == TYPE_OF_MEAT.CHICKEN)
        {
            ParentOfSlicedObjects.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>(database.ChickenParts[selection].defaultImage);

            if (database.ChickenParts[selection].imageRotate)
                ParentOfSlicedObjects.GetComponentInChildren<SpriteRenderer>().flipX = true;
            else
                ParentOfSlicedObjects.GetComponentInChildren<SpriteRenderer>().flipX = false;


            if (selection < database.ChickenParts.Count)
            {
                startBaseValue_X = database.ChickenParts[selection].startCutPointX;
                endBaseValue_X = database.ChickenParts[selection].endCutPointX;
                startBaseValue_Y = database.ChickenParts[selection].startCutPointY;
                endBaseValue_Y = database.ChickenParts[selection].endCutPointY;
            }
        }
        else if (meatType == TYPE_OF_MEAT.SHELLFISH)
        {
            if (database.ShellfishParts[selection].imageRotate)
                ParentOfSlicedObjects.GetComponentInChildren<SpriteRenderer>().flipX = true;
            else
                ParentOfSlicedObjects.GetComponentInChildren<SpriteRenderer>().flipX = false;

            ParentOfSlicedObjects.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("MeatFabrication/Shellfish/" + database.ShellfishParts[selection].defaultImage);

           
            if (selection < database.ShellfishParts.Count)
            {
                startBaseValue_X = database.ShellfishParts[selection].startCutPointX;
                endBaseValue_X = database.ShellfishParts[selection].endCutPointX;
                startBaseValue_Y = database.ShellfishParts[selection].startCutPointY;
                endBaseValue_Y = database.ShellfishParts[selection].endCutPointY;
            }

        }
        else if (meatType == TYPE_OF_MEAT.FISH)
        {
            switch (selection)
            {
                //Stomach
                case 0:
                    ParentOfSlicedObjects.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("MeatFabrication/Fish/1_Full Fish");
                    break;
                //Remove bone
                case 1:
                    ParentOfSlicedObjects.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("MeatFabrication/Fish/2_Side Cutted Fish");
                    break;
            }

            if(selection < database.FishParts.Count)
            {
                startBaseValue_X = database.FishParts[selection].startCutPointX;
                endBaseValue_X = database.FishParts[selection].endCutPointX;
                startBaseValue_Y = database.FishParts[selection].startCutPointY;
                endBaseValue_Y = database.FishParts[selection].endCutPointY;
            }
        }

        resetLinerenderer = false;
    }

    //Resets sliceable objects to next step or empty depending on what 
    public void ResetSliceableObjects()
    {
        //Debug.Log("ResetSliceableObjects");

        for(int i = ParentOfSlicedObjects.childCount; i > 0; i--)
        {
            Destroy(ParentOfSlicedObjects.GetChild(i-1).gameObject);
        }

        GameObject go = Instantiate(SlicePrefab);
        go.transform.SetParent(ParentOfSlicedObjects);
    }

    //After successful cut - Move on to next step + reset sprite / and show congratulatory text if finished
    public void ProceedToNextStep()
    {
        //Debug.Log("ProceedToNextStep");
        ResetSliceableObjects();
        ParentOfSlicedObjects.GetComponentInChildren<SpriteRenderer>().enabled = true;

        wrongCounter = 0;
        switch (meatType)
        {
            case TYPE_OF_MEAT.CHICKEN:
                //If there is still steps - proceed
                if (selection < database.ChickenParts.Count - 1)
                {
                    selection++;
                    MeatSelectionDropdownUI.value = selection;
                }
                //If there isn't any steps - proceed to congratulatory popup + reset gameobjects
                else
                {
                    SFX.PlaySFX(applause);

                    finishedFabricationTab.SetActive(true);
                    finishedFabricationText.text = "<color=red>Congratulations! </color>You have successfully fabricated a chicken";
                    resetLinerenderer = true;

                    startFail = false;
                    endFail = false;
                    startSuccess = false;
                    endSuccess = false;

                    meatType = TYPE_OF_MEAT.DEFAULT;
                    selection = 0;
                    MeatSelectionDropdownUI.ClearOptions();
                    MeatSelectionDropdownUI.RefreshShownValue();
                    MeatSelectionDropdownUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Choose Meat Type";
                }
                break;
            case TYPE_OF_MEAT.SHELLFISH:
                //If there is still steps - proceed
                if (selection < database.ShellfishParts.Count - 1)
                {
                    //Debug.Log("selection++");
                    selection++;
                    MeatSelectionDropdownUI.value = selection;
                }
                //If there isn't any steps - proceed to congratulatory popup + reset gameobjects
                else
                {
                    SFX.PlaySFX(applause);

                    finishedFabricationTab.SetActive(true);
                    finishedFabricationText.text = "<color=red>Congratulations! </color>You have successfully fabricated a crab";

                    resetLinerenderer = true;

                    startFail = false;
                    endFail = false;
                    startSuccess = false;
                    endSuccess = false;

                    meatType = TYPE_OF_MEAT.DEFAULT;
                    selection = 0;
                    MeatSelectionDropdownUI.ClearOptions();
                    MeatSelectionDropdownUI.RefreshShownValue();
                    MeatSelectionDropdownUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Choose Meat Type";
                }
                break;
            case TYPE_OF_MEAT.FISH:
                //If there is still steps - proceed
                if (selection < database.FishParts.Count - 1)
                {
                    selection++;
                    MeatSelectionDropdownUI.value = selection;
                }
                //If there isn't any steps - proceed to congratulatory popup + reset gameobjects
                else
                {
                    SFX.PlaySFX(applause);

                    finishedFabricationTab.SetActive(true);
                    finishedFabricationText.text = "<color=red>Congratulations! </color> You have successfully fabricated a fish";

                    resetLinerenderer = true;
                    startFail = false;
                    endFail = false;
                    startSuccess = false;
                    endSuccess = false;

                    meatType = TYPE_OF_MEAT.DEFAULT;
                    selection = 0;
                    MeatSelectionDropdownUI.ClearOptions();
                    MeatSelectionDropdownUI.RefreshShownValue();
                    MeatSelectionDropdownUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Choose Meat Type";
                }
                break;
        }
        
        //Debug.Log("proceed selection:" + selection);
        UpdateSliceableBeforeCut();
    }
  
    //Show correct results
    public void ShowCorrectResults()
    {

        correctResultTab.SetActive(true);
        correctResultImage.transform.localRotation = Quaternion.Euler(correctResultImage.transform.localRotation.x, 0, correctResultImage.transform.localRotation.z);
        SFX.PlaySFX(choppingSound);

        if (meatType == TYPE_OF_MEAT.CHICKEN)
        {
            correctResultImage.sprite = Resources.Load<Sprite>(database.ChickenParts[selection].correctImage);
            correctResultImage.SetNativeSize();
            correctResultText.text = database.ChickenParts[selection].correctText;

            if (database.ChickenParts[selection].imageRotate)
                correctResultImage.transform.localRotation = Quaternion.Euler(correctResultImage.transform.localRotation.x, 180, correctResultImage.transform.localRotation.z);

        }
        else if(meatType == TYPE_OF_MEAT.SHELLFISH)
        {
            correctResultImage.sprite = Resources.Load<Sprite>("MeatFabrication/Shellfish/"+database.ShellfishParts[selection].correctImage);
            correctResultImage.SetNativeSize();
            correctResultText.text = database.ShellfishParts[selection].correctText;
            if(database.ShellfishParts[selection].imageRotate)
                correctResultImage.transform.localRotation = Quaternion.Euler(correctResultImage.transform.localRotation.x, 180, correctResultImage.transform.localRotation.z);
        }
        else if (meatType == TYPE_OF_MEAT.FISH)
        {
            switch (selection)
            {
                //finish stomach cut
                case 0:
                    correctResultImage.sprite = Resources.Load<Sprite>("MeatFabrication/Fish/2_Side Cutted Fish");
                    correctResultImage.SetNativeSize();
                    correctResultText.text = "You have successfully fabricated the fish by cutting its stomach";
                    break;
                //finish debone 
                case 1:
                    correctResultImage.sprite = Resources.Load<Sprite>("MeatFabrication/Fish/3_Debone Fish");
                    correctResultImage.SetNativeSize();
                    correctResultText.text = "You have successfully fabricated the fish by removing its bone";
                    break;
            }
        }
    }

    //Show wrong results
    public void ShowWrongResults()
    {
        wrongResultTab.SetActive(true);
        wrongCounter++;
        wrongResultImage.transform.localRotation = Quaternion.Euler(correctResultImage.transform.localRotation.x, 0, correctResultImage.transform.localRotation.z);
        SFX.PlaySFX(wrong);

        if (meatType == TYPE_OF_MEAT.CHICKEN)
        {
            wrongResultText.text = "You have failed to fabricate the chicken";
            wrongResultHint.text = database.ChickenParts[selection].wrongText;
            if (wrongCounter >= 3)
            {
                wrongResultImage.sprite = Resources.Load<Sprite>(database.ChickenParts[selection].wrongImage);
                if (database.ChickenParts[selection].imageRotate)
                    wrongResultImage.transform.localRotation = Quaternion.Euler(correctResultImage.transform.localRotation.x, 180, correctResultImage.transform.localRotation.z);
            }
        }
           
        else if (meatType == TYPE_OF_MEAT.SHELLFISH)
        {
            wrongResultText.text = "You have failed to fabricate the shellfish";
            wrongResultHint.text = database.ShellfishParts[selection].wrongText;
            if (wrongCounter >= 3)
            {
                wrongResultImage.sprite = Resources.Load<Sprite>("MeatFabrication/ShellFish/" + database.ShellfishParts[selection].wrongImage);
                if (database.ShellfishParts[selection].imageRotate)
                    wrongResultImage.transform.localRotation = Quaternion.Euler(correctResultImage.transform.localRotation.x, 180, correctResultImage.transform.localRotation.z);
            }

        }
        else if (meatType == TYPE_OF_MEAT.FISH)
        {
            wrongResultText.text = "You have failed to fabricate the fish";
            switch (selection)
            {
                //fail stomach cut
                case 0:
                    wrongResultHint.text = "Hint : Start from the bottom of the fish\n[Cut from left to right]";
                    if(wrongCounter >= 3)
                        wrongResultImage.sprite = Resources.Load<Sprite>("MeatFabrication/Fish/1_Full Fish Hint");

                    break;
                //fail debone
                case 1:
                    wrongResultHint.text = "Hint : Start from bone behind the head\n[Cut from right to left]";
                    if (wrongCounter >= 3)
                        wrongResultImage.sprite = Resources.Load<Sprite>("MeatFabrication/Fish/2_Side Cutted Fish Hint");
                    break;
            }
        }
    }

    //Reset when unsuccessful cut
    public void ResetCutFail()
    {
        wrongResultTab.SetActive(false);
        startFail = false;
        endFail = false;
        startSuccess = false;
        endSuccess = false;
    }

}
