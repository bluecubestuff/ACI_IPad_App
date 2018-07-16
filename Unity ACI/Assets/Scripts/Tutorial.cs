using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public GameObject[] tutorialList;
    public static int tutorialNumber = 0;
    public bool tutDone = false;
    bool enableTutorial = false;

    public static bool supplierTutDone = false;
    public static bool storageTutDone = false;
    public static bool restaurantTutDone = false;

    public bool isThisSupplierScene;
    public bool isThisStorageScene;
    public bool isThisRestaurantScene;

    public GameObject allButtons;

    public Button btnNext;
    public Button btnBack;
    public Button bthClose;

    public GameObject pageParent;
    public GameObject pagePrefab;

    public GameObject MainUI;

    bool hint = false;

    void Start()
    {
        tutorialNumber = 0;
        allButtons.SetActive(true);

        for (int i = 1; i <= tutorialList.Length; i++)
        {
            GameObject page = Instantiate(pagePrefab);
            page.transform.SetParent(pageParent.gameObject.transform);
            page.name = "Page " + i;
        }

    }

    void Update () {

        //To check if the tutorial is done at least 1 time
        if (isThisSupplierScene && supplierTutDone)
            tutDone = true;
        if (isThisStorageScene && storageTutDone)
            tutDone = true;
        if (isThisRestaurantScene && restaurantTutDone)
            tutDone = true;

        //first page and last page button disable
        if (tutorialNumber == 0)
            btnBack.interactable = false;
        else
            btnBack.interactable = true;

        if (tutorialNumber == tutorialList.Length -1)
            btnNext.interactable = false;
        else
            btnNext.interactable = true;

        //make sure the user read all the tutorial at least 1 time
        if(!tutDone)
        {
            enableTutorial = true;

            if (tutorialNumber < tutorialList.Length-1)
                bthClose.interactable = false;
            else
                bthClose.interactable = true;
        }


        if (enableTutorial)
        {
            //disable the main ui
            MainUI.SetActive(false);

            //tutorial UI will display depends on tutorialNumber
            for (int i = 0; i < tutorialList.Length; i++)
            {
                if (tutorialList[i] != null)
                {
                    if (i == tutorialNumber)
                    {
                        pageParent.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 175, 0, 100);
                        tutorialList[i].SetActive(true);
                    }
                    else
                    {
                        pageParent.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 100);
                        tutorialList[i].SetActive(false);
                    }
                }
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void NextBtn()
    {
        tutorialNumber++;
    }
    public void BackBtn()
    {
        tutorialNumber--;
    }

    public void CloseSupplierTutorial()
    {
        gameObject.SetActive(false);
        tutorialNumber = 0;
        supplierTutDone = true;
        if (!hint)
            MainUI.SetActive(true);
        hint = false;
    }
    public void CloseStorageTutorial()
    {
        gameObject.SetActive(false);
        tutorialNumber = 0;
        storageTutDone = true;
        if (!hint)
            MainUI.SetActive(true);
        hint = false;
    }
    public void CloseRestaurantTutorial()
    {
        gameObject.SetActive(false);
        tutorialNumber = 0;
        restaurantTutDone = true;
        if(!hint)
            MainUI.SetActive(true);
        hint = false;
    }

    public void OpenTutorial()
    {
        //start from 0
        gameObject.SetActive(true);
        tutorialNumber = 0;
        enableTutorial = true;
    }
    public void OpenTutorialFrom(int number)
    {
        //start from the number you choose
        gameObject.SetActive(true);
        tutorialNumber = number;
        enableTutorial = true;
        hint = true;
    }
}
