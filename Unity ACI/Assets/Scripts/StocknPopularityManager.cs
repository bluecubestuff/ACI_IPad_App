using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StocknPopularityManager : MonoBehaviour {

    public static float stockValue;
    public static float popValue;
    public static float mainRatingValue;

    //once happiness bar is maxed, add 1 star, then reset happiness bar
    public static float starRating;
    public Image stockBar;
    public Image popularityBar;
    public Image mainRatingBar;

    // Use this for initialization
    void Start () {
        stockBar.fillAmount = 0.0f;
        popularityBar.fillAmount = 0.0f;
        mainRatingBar.fillAmount = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {

        if (stockValue >= 1) stockValue = 1;
        if (popValue >= 1)
        {
            starRating += 0.2f;
            popValue = 0;
        }
        if (starRating >= 1) starRating = 1;

        if (stockValue <= 0) stockValue = 0;
        if (popValue <= 0) popValue = 0;
        if (starRating <= 0) starRating = 0;


        popularityBar.fillAmount = popValue;
        mainRatingBar.fillAmount = starRating;
        stockBar.fillAmount = stockValue;

        #region DebugInputs
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.O))
        {
            ReduceStockPoints(0.1f);
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            AddStockPoints(0.1f);
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            ReducePopularityPoints(0.1f);
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            AddPopularityPoints(0.1f);
        }
        if (Input.GetKeyUp(KeyCode.N))
        {
            ReduceRatingsPoints(0.2f);
        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            AddRatingsPoints(0.2f);
            //Debug.Log(starRating);
        }
        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            ReinitiallizeRestaurant();
        }
#endif
        #endregion
    }

    public static void ReinitiallizeRestaurant()
    {
        starRating = popValue = mainRatingValue = stockValue = 0;
        if (TruckManager.foodList != null)
            TruckManager.foodList.Clear();
            //for (int i = 0; i < TruckManager.foodList.Count; ++i)
            //    TruckManager.foodList[i] = null;
        if (GameCache.Instance != null)
            foreach (Transform child in GameCache.Instance.transform)
            Destroy(child.gameObject);
        if (OrderListManager.orderInstance != null)
            foreach (Transform child in OrderListManager.orderInstance.transform)
                Destroy(child.gameObject);
        TutorialManager.page = TutorialManager.Page.Prompt;
    }

    public void AddStockPoints(float amt)
    {
        stockValue += amt;
    }

    public void ReduceStockPoints(float amt)
    {
        stockValue -= amt;
    }

    public void AddPopularityPoints(float amt)
    { 
        popValue += amt;
    }

    public void ReducePopularityPoints(float amt)
    {
        popValue -= amt;
    }
    public void AddRatingsPoints(float amt)
    {
        starRating += amt;
    }

    public void ReduceRatingsPoints(float amt)
    {
        starRating -= amt;
    }
}
