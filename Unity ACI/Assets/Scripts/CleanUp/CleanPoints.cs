using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CleanPoints : MonoBehaviour {

    [SerializeField]
    TMP_Dropdown cleanPointDropDown;
    [SerializeField]
    GameObject showNextStep;
    [SerializeField]
    GameObject blackScreen;
    [SerializeField]
    GameObject boundNumer;
    public static int ToClean;
    [SerializeField]
    GameObject[] stepsToClean;
    int warningSignCount;
    Camera main;
    GameObject[] bubbles;
    [SerializeField]
    GameObject buttonToRinse;
    [SerializeField]
    GameObject victoryPopUp;
    [SerializeField]
    AudioClip applause;
    AudioSource audio;
    bool isPlaying;
    [SerializeField]
    GameObject victoryParticle;

    [SerializeField]
    Sprite wetSignImage;
    [SerializeField]
    Sprite towelImage;

    public static bool inARCamera;
    //This is just a concept idea came up before client replied, just wanted to show them one type of gameplay that they could possibly implement
    //Main script for all the cleaning steps inside clean up
	void Start ()
    {
        inARCamera = false;
        isPlaying = false;
        Time.timeScale = 1;

        cleanPointDropDown.value = 0;
        cleanPointDropDown.options.Clear();

        //Serialized from inspector
        for(int i = 0; i < stepsToClean.Length; i++)
        cleanPointDropDown.options.Add(new TMP_Dropdown.OptionData(stepsToClean[i].name));

        audio = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioSource>();
        main = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        cleanPointDropDown.onValueChanged.AddListener(delegate
        {
            ValueChange(cleanPointDropDown);
        });
        cleanPointDropDown.RefreshShownValue();
        //Count the number of empty collision boxes in the step itself (Not good way of doing this i guess)
        ToClean = stepsToClean[cleanPointDropDown.value].transform.GetChild(0).transform.childCount;
    }
    public void GetInARCamera(bool ar)
    {
        inARCamera = ar;
    }
    public void AllowToolMovement()
    {
        MoveTool.allowMove = true;
    }
    public void AddStep()
    {
        //Goes to next step
        cleanPointDropDown.value++;
        MoveTool.allowMove = true;
        isPlaying = false;
    }

    void ValueChange(TMP_Dropdown cleanPointDropDown)
    {
        UpdateCleanStep(cleanPointDropDown.value);
        switch (cleanPointDropDown.value)
        {
            //Sweep floor
            case 0:
                ToClean = stepsToClean[cleanPointDropDown.value].transform.GetChild(0).transform.childCount;
                main.GetComponent<LineRenderer>().startColor = new Color(1F, 0.9F, 0.7F);
                main.GetComponent<LineRenderer>().endColor = new Color(1F, 0.7F, 0.3F);
                break;
            //Warning sign
            case 1:
                break;
            //wet floor
            case 2:
                ToClean = stepsToClean[cleanPointDropDown.value].transform.GetChild(0).transform.childCount;
                main.GetComponent<LineRenderer>().startColor = Color.blue;
                main.GetComponent<LineRenderer>().endColor = Color.white;
                break;
            //Scrub floor
            case 3:
                bubbles = GameObject.FindGameObjectsWithTag("Bubble");
                break;
            //Rinse floor
            case 4:
                ToClean = stepsToClean[cleanPointDropDown.value].transform.GetChild(0).transform.childCount;
                main.GetComponent<LineRenderer>().startColor = Color.blue;
                main.GetComponent<LineRenderer>().endColor = new Color(0F, 0F, 1F);
                Debug.Log(ToClean);
                break;
            //Mop floor
            case 5:
                ToClean = stepsToClean[cleanPointDropDown.value].transform.GetChild(0).transform.childCount;
                main.GetComponent<LineRenderer>().startColor = Color.blue;
                main.GetComponent<LineRenderer>().endColor = Color.gray;
                Debug.Log(ToClean);
                break;
            case 6:
                warningSignCount = 0;
                break;
            case 7:
                warningSignCount = 0;
                break;
        }
    }
    //Controlled from buttons outside (The step for removing wet sign)
    public void RemoveWarningSign()
    {
        warningSignCount++;
    }
    //Controlled from buttons outside (The step for setting up wet sign)
    public void SetWarningSign(Button but)
    {
        if (ToolInfo.toolInUse == "WetSign")
        {
            warningSignCount++;
            but.interactable = false;
            but.image.sprite = wetSignImage;
        }
    }
    //Controlled from buttons outside (The step for drying)
    public void SetTowel(Button but)
    {
        if (ToolInfo.toolInUse == "Towel")
        {
            warningSignCount++;
            but.interactable = false;
            but.image.sprite = towelImage;
        }
    }
    //Controlled from Move Tool (Using raycast from tool to scrub the bubbles)
    public void DoneWithSoap()
    {
        if (!isPlaying)
        {
            audio.PlayOneShot(applause);
            isPlaying = true;
        }
        blackScreen.SetActive(true);
        showNextStep.SetActive(true);
        ToolInfo.toolInUse = "";
        showNextStep.transform.GetChild(0).GetComponent<Text>().text = "\nThe solution has turned foamy!\n Next is to rinse the floor";
    }

    // Update is called once per frame
    void Update() { 
        switch (cleanPointDropDown.value)
        {
            //Sweep floor
            case 0:
                if (ToolInfo.toolInUse == "Broom")
                    DrawLine.selfUpdate = true;
                else
                    DrawLine.selfUpdate = false;
                if (ToClean <= 0)
                {
                    blackScreen.SetActive(true);
                    showNextStep.SetActive(true);
                    ToolInfo.toolInUse = "";
                    if (!isPlaying)
                    {
                        audio.PlayOneShot(applause);
                        isPlaying = true;
                    }
                    showNextStep.transform.GetChild(0).GetComponent<Text>().text = "\nYou have swept the floor\n Next is to display the warning signs";
                    MoveTool.allowMove = false;
                }
                break;
            //Warning sign
            case 1:
                if(DrawLine.selfUpdate != false)
                DrawLine.selfUpdate = false;
                if (warningSignCount >= 2)
                {
                    blackScreen.SetActive(true);
                    showNextStep.SetActive(true);
                    ToolInfo.toolInUse = "";
                    if (!isPlaying)
                    {
                        audio.PlayOneShot(applause);
                        isPlaying = true;
                    }
                    showNextStep.transform.GetChild(0).GetComponent<Text>().text = "\nYou have put down the warning signs\n Next is to wet the floor";
                    MoveTool.allowMove = false;
                }
                break;
            //Wet floor
            case 2:
                if (ToolInfo.toolInUse == "SoapWater")
                    DrawLine.selfUpdate = true;
                else
                    DrawLine.selfUpdate = false;
                if (ToClean <= 0)
                {
                    blackScreen.SetActive(true);
                    showNextStep.SetActive(true);
                    ToolInfo.toolInUse = "";
                    MoveTool.allowMove = false;
                    if (!isPlaying)
                    {
                        audio.PlayOneShot(applause);
                        isPlaying = true;
                    }
                    showNextStep.transform.GetChild(0).GetComponent<Text>().text = "\nYou have wet the floor\n Next is to scrub the floor";
                }
                break;
            //Scrub floor
            case 3:
                DrawLine.selfUpdate = false;
                //MoveTool.allowMove = false;
                break;
            //Rinse floor
            case 4:
                if (ToolInfo.toolInUse == "WaterHose")
                    DrawLine.selfUpdate = true;
                else
                    DrawLine.selfUpdate = false;
                if (ToClean <= 0)
                {
                    blackScreen.SetActive(true);
                    showNextStep.SetActive(true);
                    ToolInfo.toolInUse = "";
                    MoveTool.allowMove = false;
                    if (!isPlaying)
                    {
                        audio.PlayOneShot(applause);
                        isPlaying = true;
                    }
                    showNextStep.transform.GetChild(0).GetComponent<Text>().text = "\nYou have rinsed the floor\n Next is to mop the floor";
                }
                break;
            //Mop floor
            case 5:
                if (ToolInfo.toolInUse == "Mop")
                    DrawLine.selfUpdate = true;
                else
                    DrawLine.selfUpdate = false;
                if(ToClean<= 15 && ToClean >0)
                {
                    buttonToRinse.gameObject.SetActive(true);
                }
                if (ToClean <= 0)
                {
                    blackScreen.SetActive(true);
                    showNextStep.SetActive(true);
                    ToolInfo.toolInUse = "";
                    MoveTool.allowMove = false;
                    if (!isPlaying)
                    {
                        audio.PlayOneShot(applause);
                        isPlaying = true;
                    }
                    showNextStep.transform.GetChild(0).GetComponent<Text>().text = "You have mopped the floor\nand changed the rinse water\n Next is to dry the floor";
                }
                break;
            //Put down towels
            case 6:
                if (DrawLine.selfUpdate != false)
                    DrawLine.selfUpdate = false;
                if (warningSignCount >= 2)
                {
                    blackScreen.SetActive(true);
                    showNextStep.SetActive(true);
                    ToolInfo.toolInUse = "";
                    MoveTool.allowMove = false;
                    if (!isPlaying)
                    {
                        audio.PlayOneShot(applause);
                        isPlaying = true;
                    }
                    showNextStep.transform.GetChild(0).GetComponent<Text>().text = "\nYou have put down the towels\n Next is to remove the warning signs";
                }
                break;
            //Remove warning signs
            case 7:
                if(DrawLine.selfUpdate!=false)
                DrawLine.selfUpdate = false;
                if (warningSignCount >= 2)
                {
                    blackScreen.SetActive(true);
                    victoryPopUp.SetActive(true);
                    victoryParticle.SetActive(true);
                    ToolInfo.toolInUse = "";
                    MoveTool.allowMove = false;
                    if (!isPlaying)
                    {
                        audio.PlayOneShot(applause);
                        isPlaying = true;
                    }
                    //showNextStep.transform.GetChild(0).GetComponent<Text>().text = "\nYou have removed the warning signs\n and completed the clean up!";
                }
                break;
        }
    }
    public void ChangeRinseWater()
    {
        ToClean--;
    }
    //Resets the gesture drawn line
    //Destroy all tools model on the screen when switching steps
    //Set the certain step to active
    public void UpdateCleanStep(int value)
    {
        DrawLine drawLine = main.GetComponent<DrawLine>();
        drawLine.Reset();
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("Tools");

        ToolInfo.toolInUse = "";
        for (int i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
        foreach (GameObject a in stepsToClean)
        {
            a.SetActive(false);
        }
        if (stepsToClean.Length >= value)
        {
            stepsToClean[cleanPointDropDown.value].gameObject.SetActive(true);
        }
    }
}
