using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StorageUIScript : MonoBehaviour {
    public Food food;
    public int index;
    public GameObject TouchManager;
    //TruckManager truckManager;
    public GameObject toReject;

    public GameObject selectionUI;
    public GameObject selectionUIHelpBtn;
    public GameObject truckInfo;
    public GameObject screenModelInfo;
    public GameObject mainUI;
    public GameObject wrongFoodPopup;

    public GameObject sceneBtnParent;
    public GameObject CircleRestaurant;

    void Start()
    {
        //truckManager = GameObject.FindGameObjectWithTag("TruckManager").GetComponent<TruckManager>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "AR_Main")
        {
            if (StocknPopularityManager.stockValue > 0)
            {
                CircleRestaurant.transform.localPosition = sceneBtnParent.transform.GetChild(0).localPosition;
            }
            else
                CircleRestaurant.transform.localPosition = sceneBtnParent.transform.GetChild(2).localPosition;
        }

        //food = truckManager.foodList[TouchManager.gameObject.GetComponent<JHTouchManager>().screenModelIndex];
    }

    public void Store()
    {
        //enable the acceptedTruckInfo UI
        TouchManager.gameObject.GetComponent<JHTouchManager>().acceptedTruckInfo.SetActive(true);

        //enable the storage type icon
        selectionUI.SetActive(true);

        //enble the help button
        selectionUIHelpBtn.SetActive(true);
        wrongFoodPopup.SetActive(false);
    }

    public void Accept()
    {
        OrderListManager.orderLimit--;
        //destroy the food from the orderlist
        TouchManager.gameObject.GetComponent<JHTouchManager>().DestroyFromOrderList();

        //if correct items
        if (TouchManager.gameObject.GetComponent<JHTouchManager>().CheckForCorrectItems())
        {
            TouchManager.gameObject.GetComponent<JHTouchManager>().pointsGiveToStockBar = 0.2f;
            Store();
        }
        else
        {
            TouchManager.gameObject.GetComponent<JHTouchManager>().pointsGiveToStockBar = 0.1f;

            //enable the wrong food pop up
            wrongFoodPopup.SetActive(true);
            //play that animation
            TouchManager.gameObject.GetComponent<JHTouchManager>().wrongFoodAnim.SetTrigger("Show");
        }
    }
    public void Reject()
    {
        //change the truck way point to reject path
        TouchManager.gameObject.GetComponent<JHTouchManager>().selectedTruck.GetComponentInChildren<MoveOnPath>().PathToFollow = toReject.GetComponent<EditPathScript>();
        //reset currect selection
        //TouchManager.gameObject.GetComponent<JHTouchManager>().selectedTruck = null;
    }

    public void Return()
    {
        //hide animation
        TouchManager.gameObject.GetComponent<JHTouchManager>().truckInfoAnim.SetTrigger("Hide");
        StartCoroutine(CloseTruckInfoAnim());
    }
    public IEnumerator CloseTruckInfoAnim()
    {
        //yield return new WaitForSeconds(TouchManager.gameObject.GetComponent<JHTouchManager>().truckInfoAnim.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(0.5f);
        //reset currect selection
        TouchManager.GetComponent<JHTouchManager>().selectedTruck = null;

        //disable truckInfo UI
        truckInfo.SetActive(false);
        screenModelInfo.SetActive(false);

        //enable main UI
        mainUI.SetActive(true);

    }

    public void PlacementTop()
    {
        if (TouchManager.gameObject.GetComponent<JHTouchManager>().acceptedTruckInfo.transform.GetChild(0).transform.GetChild(0).GetComponentInChildren<AcceptedFoodInfo>().food.foodPlacement.ToString().Equals("TOP"))
        {
            //Debug.Log("GOOD!!");
            TouchManager.gameObject.GetComponent<JHTouchManager>().reducePoints = false;
            TouchManager.gameObject.GetComponent<JHTouchManager>().OpenUserFeedback();
            TouchManager.gameObject.GetComponent<JHTouchManager>().UserFeedback.GetComponentInChildren<Text>().text = "Correct!";
            TouchManager.gameObject.GetComponent<JHTouchManager>().placementDone = true;

            //add to stock points
            StocknPopularityManager.stockValue += TouchManager.gameObject.GetComponent<JHTouchManager>().pointsGiveToStockBar;
        }
        else
        {
            //Debug.Log("BAD!!");
            TouchManager.gameObject.GetComponent<JHTouchManager>().reducePoints = true;
            TouchManager.gameObject.GetComponent<JHTouchManager>().OpenUserFeedback();
            TouchManager.gameObject.GetComponent<JHTouchManager>().UserFeedback.GetComponentInChildren<Text>().text = "Wrong placement!";
        }
    }
    public void PlacementBot()
    {
        if (TouchManager.gameObject.GetComponent<JHTouchManager>().acceptedTruckInfo.transform.GetChild(0).transform.GetChild(0).GetComponentInChildren<AcceptedFoodInfo>().food.foodPlacement.ToString().Equals("BOTTOM"))
        {
            //Debug.Log("GOOD!!");
            TouchManager.gameObject.GetComponent<JHTouchManager>().reducePoints = false;
            TouchManager.gameObject.GetComponent<JHTouchManager>().OpenUserFeedback();
            TouchManager.gameObject.GetComponent<JHTouchManager>().UserFeedback.GetComponentInChildren<Text>().text = "Correct!";
            TouchManager.gameObject.GetComponent<JHTouchManager>().placementDone = true;

            //add to stock points
            StocknPopularityManager.stockValue += TouchManager.gameObject.GetComponent<JHTouchManager>().pointsGiveToStockBar;
        }
        else
        {
            //Debug.Log("BAD!!");
            TouchManager.gameObject.GetComponent<JHTouchManager>().reducePoints = true;
            TouchManager.gameObject.GetComponent<JHTouchManager>().OpenUserFeedback();
            TouchManager.gameObject.GetComponent<JHTouchManager>().UserFeedback.GetComponentInChildren<Text>().text = "Wrong placement!";
        }
    }
}
