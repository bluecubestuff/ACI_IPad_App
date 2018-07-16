using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SupplierSceneManager : MonoBehaviour
{
    public static SupplierSceneManager SupplierInstance;

    /*DESCRIPTION
     * Supplier Scene Manager is a Singleton
     *  manages all Supplier Randomization */


    /*image used to change filled amount 
     * Displays when supplier is selected
     * Shows the range of ratings player can expect */
    public Image minRatingImg;
    public Image maxRatingImg;


    //Current Selected Supplier is saved
    //public SupplierInfo CurrentSupplier = null;
    //public Tutorial tutorial;
    public GameObject tutorialParticles;
    public GameObject tutorialHighlight;
    public GameObject buyButtonforTutorial;

    public GameObject OtherSupplierA;
    public GameObject OtherSupplierB;
    public GameObject OtherSupplierC;

    void Awake()
    {
        if (SupplierInstance == null)
        {
            SupplierInstance = this;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*if(tutorial.tutorialList[1].activeSelf)
        {
            tutorialHighlight.SetActive(true);
            tutorialParticles.SetActive(true);
        }
        else
        {
            tutorialHighlight.SetActive(false);
            tutorialParticles.SetActive(false);
        }

        if (tutorial.tutorialList[4].activeSelf || tutorial.tutDone)
        {
            buyButtonforTutorial.GetComponent<Button>().interactable = true;
        }
        else
            buyButtonforTutorial.GetComponent<Button>().interactable = false;

        if(tutorial.tutDone)
        {
            OtherSupplierA.SetActive(true);
            OtherSupplierB.SetActive(true);
            OtherSupplierC.SetActive(true);
        }
        else
        {
            OtherSupplierA.SetActive(false);
            OtherSupplierB.SetActive(false);
            OtherSupplierC.SetActive(false);
        }*/
    }

    public void ChangeUI()
    {
        //setting the image fill based on percentage of actual ratings
        //minRatingImg.fillAmount = (Mathf.Round(CurrentSupplier.minRating)) * 0.2f;
        //maxRatingImg.fillAmount = (Mathf.Round(CurrentSupplier.maxRating)) * 0.2f;

    }


    public void RandomSupplierRating()
    {
        /*Calls when Supplier Ratings need to be random again*/

        //int tempmin;
        //CurrentSupplier.minRating = tempmin = Random.Range(1, 3);
        //CurrentSupplier.maxRating = Random.Range(tempmin+1, 6);

        /* Debug.Log("temp:" + tempmin);
        CurrentSupplier.minRating = Random.Range(1.0f, 4.0f);
        CurrentSupplier.maxRating = Random.Range(3.0f, 6.0f);
        Mathf.Round(CurrentSupplier.minRating,) */


        /*Minus the current rating with the remainder x.
         * To get/round off x of the actual ratings */
        //CurrentSupplier.minRating -= (CurrentSupplier.minRating % 1);
        //CurrentSupplier.maxRating -= (CurrentSupplier.maxRating % 1);
    }
}
