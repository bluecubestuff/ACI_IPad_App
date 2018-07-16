using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewStorageScene : MonoBehaviour {

    float pointsToBeAddedToStock;
    public Button orderListClose;
    [Header("Camera")]
    public Camera mainCam;


    [Header("Order Details")]
    public GameObject blackBackground;
    public GameObject deliveryDetails;
    public Text foodName;
    public Text foodStorageType;
    private GameObject selectedBox;

    [Header("Storing Related")]
    public GameObject selectStorage;
    public Text acceptedWrongOrderFeedback;
    public Button placementTop;
    public Button placementBottom;
    public GameObject StoragePlacementFeedback;
    public Button ExitStorage;
    [SerializeField]
    Text text;
    [Header("Managers")]
    [SerializeField]
    private GameObject orderList;
    [SerializeField]
    private GameObject truckManager;
    [SerializeField]
    private GameObject stockAndPopularityManager;
    [SerializeField]
    GameObject settings;
    private Vector3 originalBoxLocation;
    [SerializeField]
    GameObject Tut_BlackBackground;

    [SerializeField]
    GameObject MainUI;
    // Use this for initialization
    void Start() {
        orderList.GetComponent<OrderListManager>().GetOrderList();
    }

    // Update is called once per frame
    void Update() {
        if(orderList.transform.parent.GetComponent<Animator>().gameObject.activeSelf && orderList.transform.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Empty"))
         {
            orderList.GetComponent<OrderListManager>().orderlistParent.transform.localPosition = new Vector3(42, 0, 0);
            orderListClose.gameObject.SetActive(true);
            orderList.transform.parent.GetComponent<Animator>().speed = 1;
        }
        if (Input.GetMouseButton(0) && !settings.activeSelf && !orderList.transform.parent.gameObject.activeSelf)
        {
            Vector3 mousePosFar = new Vector3(Input.mousePosition.x,
                                               Input.mousePosition.y,
                                               Camera.main.farClipPlane);
            Vector3 mousePosNear = new Vector3(Input.mousePosition.x,
                                               Input.mousePosition.y,
                                               Camera.main.nearClipPlane);
            Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
            Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

            Debug.DrawRay(mousePosN, mousePosF - mousePosN, Color.green);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Create Raycast
            RaycastHit hit;


            if ((Physics.Raycast(ray, out hit)))
            {
                if (!selectStorage.activeSelf && selectedBox == null)
                {
                    text.text = hit.collider.gameObject.name;
                    if (hit.collider.gameObject.tag == "Truck")
                    {
                        if (hit.collider.transform.GetChild(0).gameObject.activeSelf &&
                            hit.collider.gameObject.GetComponent<Truck>().food.foodName != "" &&
                                   hit.collider.gameObject.GetComponent<Truck>() != null &&
                                   hit.collider.gameObject.GetComponent<Truck>().food != null)
                        {
                            Debug.Log(hit.collider.gameObject.name);
                            selectedBox = hit.collider.gameObject;
                            foodName.text = selectedBox.gameObject.GetComponent<Truck>().food.foodName;
                            switch (selectedBox.gameObject.GetComponent<Truck>().food.foodType)
                            { 
                                // Store in Dry
                                case ((Food.FoodType)0):
                                    foodStorageType.text = "Dry";
                                    break;
                                // Store in Cold
                                case ((Food.FoodType)1):
                                    foodStorageType.text = "Cold";
                                    break;
                                // Store in Freeze
                                case ((Food.FoodType)2):
                                    foodStorageType.text = "Freeze";
                                    break;
                            }

                            blackBackground.SetActive(true);
                            deliveryDetails.SetActive(true);
                            orderList.transform.parent.gameObject.SetActive(true);
                            Debug.Log("FOOD NAME" + foodName.text);
                            text.text += " FOOD " + foodName.text;
                            orderList.GetComponent<OrderListManager>().orderlistParent.transform.localPosition = new Vector3(500, 0, 0);
                            orderList.transform.parent.GetComponent<Animator>().Play("OptionsDown");
                            orderListClose.gameObject.SetActive(false);
                            originalBoxLocation = selectedBox.transform.position;

                            {
                                var tutorialManager = FindObjectOfType<StorageTutorialManager>();
                                if (tutorialManager != null)
                                    tutorialManager.OnCheckedDelivery();
                            }
                        }
                    }
                }
                else if (selectStorage.activeSelf || acceptedWrongOrderFeedback.gameObject.activeSelf)
                {
                    if (hit.collider.tag == ("dryDoor"))
                    {
                        checkForCorrectDoor(0); // Dry food
                    }
                    else if (hit.collider.tag == ("wetDoor"))
                    {
                        checkForCorrectDoor(1);// Cold food
                    }
                    else if (hit.collider.tag == ("freezeDoor"))
                    {
                        checkForCorrectDoor(2); // Frozen food
                    }
                }
            }
        }
    }

    void showOrderDetails()
    {
        blackBackground.SetActive(true);
        deliveryDetails.SetActive(true);
        
    }

    public void acceptOrder()
    {
        //OrderListManager.orderLimit--;
        ////destroy the food from the orderlist
        //removeFromOrderList();

        ////if correct items
        //if (checkForCorrectItems())
        //{
        //    pointsToBeAddedToStock = (0.1f * (float)selectedBox.GetComponent<Truck>().food.foodRarity) * 0.5f;
        //    selectStorage.SetActive(true);
        //}
        //else
        //{
        //    pointsToBeAddedToStock = 0.1f;
        //    acceptedWrongOrderFeedback.gameObject.SetActive(true);
        //}
        selectStorage.SetActive(true);
        blackBackground.SetActive(false);
        deliveryDetails.SetActive(false);

        orderList.transform.parent.GetComponent<Animator>().Play("Close");
    }

    public void rejectOrder()
    {
        selectedBox.GetComponent<Truck>().readyToRespawn = true;
        selectedBox.transform.position = originalBoxLocation;
        selectedBox = null;
        blackBackground.SetActive(false);
        deliveryDetails.SetActive(false);
        orderList.transform.parent.GetComponent<Animator>().speed = 2;
        orderList.transform.parent.GetComponent<Animator>().Play("Close");
        originalBoxLocation.Set(0, 0, 0);
        selectStorage.SetActive(false);
    }

    public void closeOrder()
    {
        // closes the delivery details UI, allowing you to select another box to view its contents
        blackBackground.SetActive(false);
        deliveryDetails.SetActive(false);
        selectedBox.transform.position = originalBoxLocation;
        selectedBox = null;
        orderList.transform.parent.GetComponent<Animator>().speed = 2;

        orderList.transform.parent.GetComponent<Animator>().Play("Close");
        originalBoxLocation.Set(0, 0, 0);
        selectStorage.SetActive(false);
    }

    public void removeFromOrderList()
    {
        int num = 0;

        for (int i = 0; i < orderList.transform.childCount; i++)
        {
            //if correct food
            if (orderList.transform.GetChild(i).GetComponentInChildren<Order>().food == selectedBox.GetComponent<Truck>().food)
            {
                num = i;
                break;
            }
            //if wrong food
            else
            {
                //run a check between the orderlist and truck manager to find the odd one out(the wrong food)
                for (int j = 0; j < orderList.transform.childCount; j++)
                    for (int k = 0; k < truckManager.transform.childCount; k++)
                        if (orderList.transform.GetChild(j).GetComponentInChildren<Order>().food != truckManager.transform.GetChild(k).GetComponent<Truck>().food)
                            num = i;
            }
        }

        //delete them from orderlist
        Destroy(orderList.transform.GetChild(num).gameObject);

    }

    public bool checkForCorrectItems()
    {
        bool status = false;
        //check the food in the truck is what the orderlist wants
        for (int i = 0; i < orderList.transform.childCount; i++)
        {
            //if correct food
            if (orderList.transform.GetChild(i).GetComponentInChildren<Order>().food == selectedBox.GetComponent<Truck>().food)
            {
                status = true;
                break;
            }
            else
                status = false;
        }

        //return if its the correct food
        return status;
    }

    void checkForCorrectDoor(int selectedFoodType)
    {
        if (selectedBox.gameObject.GetComponent<Truck>().food.foodType == (Food.FoodType)selectedFoodType)
        {
            OrderListManager.orderLimit--;
            //destroy the food from the orderlist
            removeFromOrderList();
            pointsToBeAddedToStock = (0.1f * (float)selectedBox.GetComponent<Truck>().food.foodRarity) * 0.5f;

            selectStorage.SetActive(false);
            if (acceptedWrongOrderFeedback.gameObject.activeSelf)
                acceptedWrongOrderFeedback.gameObject.SetActive(false);
            mainCam.transform.position = new Vector3(-15.2f, 1.85f, -3.65f);
            mainCam.transform.rotation = Quaternion.Euler(15, 0, 0);
            MainUI.SetActive(false);
            placementTop.gameObject.SetActive(true);
            placementBottom.gameObject.SetActive(true);
            Destroy(selectedBox.GetComponent<DragObjOnly>());
            selectedBox.transform.position = originalBoxLocation;
        }
        else
        {
            if (selectStorage.activeSelf)
                selectStorage.transform.GetComponentInChildren<Text>().text = "Not there. Try Again";
            else if (acceptedWrongOrderFeedback.IsActive())
                acceptedWrongOrderFeedback.GetComponent<Text>().text = "Not there. Try Again";
        }
    }

    public void PlacementTop()
    {
        if (selectedBox.GetComponent<Truck>().food.foodPlacement.ToString().Equals("TOP"))
        {
            placementTop.gameObject.SetActive(false);
            placementBottom.gameObject.SetActive(false);
            selectStorage.SetActive(false);
            selectStorage.transform.GetComponentInChildren<Text>().text = "Where should it be stored?";
            acceptedWrongOrderFeedback.gameObject.SetActive(false);
            TruckManager.foodList[selectedBox.GetComponent<Truck>().index] = new Food();
            selectedBox.GetComponent<Truck>().foodObject.gameObject.GetComponentInParent<Truck>().food = new Food();
            selectedBox.GetComponent<Truck>().foodObject.SetActive(false);
            selectedBox = null;

            ExitStorage.gameObject.SetActive(true);
            blackBackground.SetActive(true);
            StoragePlacementFeedback.SetActive(false);

            //add to stock points
            print(pointsToBeAddedToStock);

            StocknPopularityManager.stockValue += pointsToBeAddedToStock;
        }
        else
        {
            StoragePlacementFeedback.SetActive(true);
        }
    }
    public void PlacementBot()
    {
        if (selectedBox.GetComponent<Truck>().food.foodPlacement.ToString().Equals("BOTTOM"))
        {
            placementTop.gameObject.SetActive(false);
            placementBottom.gameObject.SetActive(false);
            selectStorage.SetActive(false);
            selectStorage.transform.GetComponentInChildren<Text>().text = "Where should it be stored?";
            acceptedWrongOrderFeedback.gameObject.SetActive(false);
            TruckManager.foodList[selectedBox.GetComponent<Truck>().index] = new Food();
            selectedBox.GetComponent<Truck>().foodObject.gameObject.GetComponentInParent<Truck>().food = new Food();
            selectedBox.GetComponent<Truck>().foodObject.SetActive(false);
            selectedBox = null;
            ExitStorage.gameObject.SetActive(true);
            StoragePlacementFeedback.SetActive(false);

            //add to stock points
            print(pointsToBeAddedToStock);

            StocknPopularityManager.stockValue += pointsToBeAddedToStock;
        }
        else
        { 
            StoragePlacementFeedback.SetActive(true);
        }
    }

    public void ExitStorageFunc()
    {
        mainCam.transform.position = new Vector3(-0.5f, 3.05f, -6.22f);
        mainCam.transform.rotation = Quaternion.Euler(10, 0, 0);
        ExitStorage.gameObject.SetActive(false);
        deliveryDetails.SetActive(false);
        selectStorage.SetActive(false);
        MainUI.SetActive(true);
    }
}
