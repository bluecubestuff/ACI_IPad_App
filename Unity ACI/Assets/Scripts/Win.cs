using UnityEngine;
using System.Collections;

public class Win : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Replay()
    {
        MeatFabricationScript2.fabricatedMeat = false;
        StocknPopularityManager.popValue = 0;
        StocknPopularityManager.stockValue = 0;
        StocknPopularityManager.starRating = 0;
        //Tutorial.restaurantTutDone = false;
        //Tutorial.supplierTutDone = false;
        //Tutorial.storageTutDone = false;
        gameObject.SetActive(false);
        Time.timeScale = 1;
        LoadingScreenManager.LoadScene("Virt_Restuarant");
        //NewTutorials.currentStep = 0;
        //NewTutorials.tutDone = false;
    }
    public void Exit()
    {
        MeatFabricationScript2.fabricatedMeat = false;
        StocknPopularityManager.popValue = 0;
        StocknPopularityManager.stockValue = 0;
        StocknPopularityManager.starRating = 0;
        Tutorial.restaurantTutDone = false;
        Tutorial.supplierTutDone = false;
        Tutorial.storageTutDone = false;
        gameObject.SetActive(false);
        Time.timeScale = 1;
        LoadingScreenManager.LoadScene("MainMenu");
        NewTutorials.currentStep = 0;
        NewTutorials.tutDone = false;
    }
}
