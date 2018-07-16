using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JHTouchManager : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField]
    private GameObject dragCamera;

    [Header("3D world space model UI/Model")]
    [SerializeField]
    private GameObject screenModelInfo;
    [SerializeField]
    private GameObject selectionUI;
    public GameObject selectedTruck;
    public GameObject selectedTruckPopup;
    public GameObject OnChoosingPlacement;

    [Header("Way point")]
    [SerializeField]
    private GameObject toDry;
    [SerializeField]
    private GameObject toCold;
    [SerializeField]
    private GameObject toFreeze;

    [Header("Canvas UI")]
    [SerializeField]
    private GameObject truckInfo;
    [SerializeField]
    private GameObject orderList;
    public GameObject mainUI;
    [SerializeField]
    private GameObject orderListUI;
    public  GameObject UserFeedback;
    [SerializeField]
    private GameObject storageIconHelpBtn;
    [SerializeField]
    private Text TruckInfoName;
    [SerializeField]
    private Image TruckInfoType;
    public GameObject acceptedTruckInfo;
    public GameObject FoodPlacementUI;

    [Header("Manager")]
    [SerializeField]
    private StocknPopularityManager spm;
    [SerializeField]
    private GameObject truckManager;
    [SerializeField]
    private StorageUIScript storageUIscript;

    [Header("Animation")]
    public Animator truckInfoAnim;
    public Animator wrongFoodAnim;
    public Animator userFeedbackAnim;

    [Header("Debug")]
    public float pointsGiveToStockBar;
    public int chance = 2;
    public bool reducePoints = false;
    public bool placementDone = false;
   // Use this for initialization
   void Start()
    {
        orderList.GetComponent<OrderListManager>().GetOrderList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
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



            //Debug.Log("clicked");

            RaycastHit hit;
           

            // if raycast hit something and the UI is not enable and is not dragging the screen
            if ((Physics.Raycast(mousePosN, mousePosF - mousePosN, out hit)) &&
                !truckInfo.activeSelf &&
                !dragCamera.GetComponent<DragCamera>().IsDragging &&
                !orderListUI.activeSelf &&
                !UserFeedback.transform.parent.transform.GetChild(0).gameObject.activeSelf &&
                !storageUIscript.wrongFoodPopup.activeSelf)
            {
                switch (hit.collider.gameObject.tag)
                {
                    case ("DryStorage"):
                        {
                            //only works if the storage icon is enable
                            if (selectionUI.activeSelf)
                            {
                                //only works if player had selected the truck
                                if (selectedTruck != null)
                                {
                                    if (selectedTruck.GetComponent<Truck>().food.foodType.ToString().Equals("DRY"))
                                    {
                                        //disable the storage icon and enable the main UI
                                        selectionUI.SetActive(false);
                                        storageIconHelpBtn.SetActive(false);
                                        //mainUI.SetActive(true);

                                        //set the truck way point to dry storage
                                        selectedTruck.GetComponentInChildren<MoveOnPath>().PathToFollow = toDry.GetComponent<EditPathScript>();

                                        //Enable to truck info UI
                                        acceptedTruckInfo.SetActive(false);

                                        //reset the selected truck to nothing
                                        selectedTruck = null;

                                        //A boolean to check for correct/ wrong
                                        reducePoints = false;
                                        UserFeedback.GetComponentInChildren<Text>().text = "Correct!";
                                    }
                                    else
                                    {
                                        reducePoints = true;
                                        UserFeedback.GetComponentInChildren<Text>().text = "Incorrect, try again!";
                                    }
                                    OpenUserFeedback();
                                }
                            }
                        }
                        break;
                    case ("ColdStorage"):
                        {
                            //only works if the storage icon is enable
                            if (selectionUI.activeSelf)
                            {
                                //only works if player had selected the truck
                                if (selectedTruck != null)
                                {
                                    if (selectedTruck.GetComponent<Truck>().food.foodType.ToString().Equals("COLD"))
                                    {
                                        //disable the storage icon and enable the main UI
                                        selectionUI.SetActive(false);
                                        storageIconHelpBtn.SetActive(false);
                                        //mainUI.SetActive(true);

                                        //set the truck way point to cold storage
                                        selectedTruck.GetComponentInChildren<MoveOnPath>().PathToFollow = toCold.GetComponent<EditPathScript>();

                                        //Enable to truck info UI
                                        acceptedTruckInfo.SetActive(false);

                                        //reset the selected truck to nothing
                                        selectedTruck = null;

                                        //A boolean to check for correct/ wrong
                                        reducePoints = false;
                                        UserFeedback.GetComponentInChildren<Text>().text = "Correct!";
                                    }
                                    else
                                    {
                                        reducePoints = true;
                                        UserFeedback.GetComponentInChildren<Text>().text = "Incorrect, try again!";
                                    }
                                    OpenUserFeedback();
                                }
                            }
                        }
                        break;
                    case ("FreezeStorage"):
                        {
                            //only works if the storage icon is enable
                            if (selectionUI.activeSelf)
                            {
                                //only works if player had selected the truck
                                if (selectedTruck != null)
                                {
                                    if (selectedTruck.GetComponent<Truck>().food.foodType.ToString().Equals("FREEZE"))
                                    {
                                        //disable the storage icon and enable the main UI
                                        selectionUI.SetActive(false);
                                        storageIconHelpBtn.SetActive(false);
                                        //mainUI.SetActive(true);

                                        //set the truck way point to freeze storage
                                        selectedTruck.GetComponentInChildren<MoveOnPath>().PathToFollow = toFreeze.GetComponent<EditPathScript>();

                                        //Enable to truck info UI
                                        acceptedTruckInfo.SetActive(false);

                                        //reset the selected truck to nothing
                                        selectedTruck = null;

                                        //A boolean to check for correct/ wrong
                                        reducePoints = false;
                                        UserFeedback.GetComponentInChildren<Text>().text = "Correct!";
                                    }
                                    else
                                    {
                                        reducePoints = true;
                                        UserFeedback.GetComponentInChildren<Text>().text = "Incorrect, try again!";
                                    }
                                    OpenUserFeedback();
                                }
                            }
                        }
                        break;
                    case ("Truck"):
                        {
                            //only works if the truck contain food and the storage icon is disable
                            if (hit.collider.gameObject.GetComponent<Truck>().food.foodPrefab != null &&
                            hit.collider.gameObject.GetComponent<Truck>() != null &&
                            hit.collider.gameObject.GetComponentInChildren<MoveOnPath>().PathToFollow == null &&
                            !selectionUI.activeSelf)
                            {
                                //hide UI
                                mainUI.SetActive(false);

                                //which truck is being selected
                                selectedTruck = hit.collider.gameObject;

                                //pop up the choice for player
                                //selectedTruckPopup.SetActive(true);
                                //selectedTruckPopup.transform.position = selectedTruck.transform.position;

                                //enable the truck info UI and get the truck data
                                truckInfo.SetActive(true);

                                //enable animation
                                truckInfoAnim.SetTrigger("Show");

                                //display the screenModel to the truck data
                                screenModelInfo.SetActive(true);
                                screenModelInfo.transform.GetChild(0).GetComponent<MeshFilter>().mesh = selectedTruck.gameObject.GetComponent<Truck>().food.foodPrefab.GetComponent<MeshFilter>().sharedMesh;
                                screenModelInfo.transform.GetChild(0).GetComponent<MeshRenderer>().material = selectedTruck.gameObject.GetComponent<Truck>().food.foodPrefab.GetComponent<MeshRenderer>().sharedMaterial;

                                //change the truck info text to the truck data
                                TruckInfoName.text = selectedTruck.gameObject.GetComponent<Truck>().food.foodName.ToString();
                                TruckInfoType.sprite = selectedTruck.gameObject.GetComponent<Truck>().food.foodIconType;

                                //change the OnChoosingPlacement 3D UI to the truck data
                                OnChoosingPlacement.transform.GetChild(0).GetComponent<MeshFilter>().mesh = selectedTruck.gameObject.GetComponent<Truck>().food.foodPrefab.GetComponent<MeshFilter>().sharedMesh;
                                OnChoosingPlacement.transform.GetChild(0).GetComponent<MeshRenderer>().material = selectedTruck.gameObject.GetComponent<Truck>().food.foodPrefab.GetComponent<MeshRenderer>().sharedMaterial;

                                //change the acceptedTruckInfo 3D UI to the truck data
                                acceptedTruckInfo.transform.GetChild(0).transform.GetChild(0).GetComponentInChildren<AcceptedFoodInfo>().food = selectedTruck.gameObject.GetComponent<Truck>().food;
                                acceptedTruckInfo.transform.GetChild(1).GetComponentInChildren<Text>().text = selectedTruck.gameObject.GetComponent<Truck>().food.foodName.ToString();
                            }
                        }
                        break;
                    case ("ViewInfoButton"):
                        {
                            //hide the pop up UI
                            selectedTruckPopup.SetActive(false);

                            //enable the truck info UI and get the truck data
                            truckInfo.SetActive(true);
                            screenModelInfo.SetActive(true);
                            screenModelInfo.transform.GetChild(0).GetComponent<MeshFilter>().mesh = selectedTruck.gameObject.GetComponent<Truck>().food.foodPrefab.GetComponent<MeshFilter>().sharedMesh;
                            screenModelInfo.transform.GetChild(0).GetComponent<MeshRenderer>().material = selectedTruck.gameObject.GetComponent<Truck>().food.foodPrefab.GetComponent<MeshRenderer>().sharedMaterial;
                            TruckInfoName.text = selectedTruck.gameObject.GetComponent<Truck>().food.foodName.ToString();
                            TruckInfoType.sprite = selectedTruck.gameObject.GetComponent<Truck>().food.foodIconType;
                        }
                        break;
                    case ("StoreButton"):
                        {
                            //hide the pop up UI
                            selectedTruckPopup.SetActive(false);

                            //show the storage icon
                            selectionUI.SetActive(true);
                            storageIconHelpBtn.SetActive(true);
                        }
                        break;
                }
            }
        }
    }

    public bool CheckForCorrectItems()
    {
        bool status = false;
        //check the food in the truck is what the orderlist wants
        for (int i = 0; i < orderList.transform.childCount; i++)
        {
            //if correct food
            if (orderList.transform.GetChild(i).GetComponentInChildren<Order>().food == selectedTruck.GetComponent<Truck>().food)
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
    public void DestroyFromOrderList()
    {
        int num = 0;

        for (int i = 0; i < orderList.transform.childCount; i++)
        {
            //if correct food
            if (orderList.transform.GetChild(i).GetComponentInChildren<Order>().food == selectedTruck.GetComponent<Truck>().food)
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

    public void OpenUserFeedback()
    {
        UserFeedback.transform.localPosition = new Vector3(0, 0, 0);
        UserFeedback.transform.parent.transform.GetChild(0).gameObject.SetActive(true);
        userFeedbackAnim.SetTrigger("Show");

        //run a small check to see if the food is correct
        if (!reducePoints)
        {
            chance = 1;
            UserFeedback.GetComponent<Image>().sprite = Resources.Load<Sprite>("correct");
        }
        else
        {
            if (chance > 0)
            {
                UserFeedback.GetComponent<Image>().sprite = Resources.Load<Sprite>("wrong");
                pointsGiveToStockBar -= 0.025f;
                chance--;
            }
        }
    }
    public void CloseUserFeedback()
    {
        userFeedbackAnim.SetTrigger("Hide");
        StartCoroutine(ClosePopupAnim());
    }

    public IEnumerator ClosePopupAnim()
    {
        //yield return new WaitForSeconds(userFeedbackAnim.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(0.5f);
        UserFeedback.transform.localPosition = new Vector3(0, 1000, 0);
        UserFeedback.transform.parent.transform.GetChild(0).gameObject.SetActive(false);

        if (!reducePoints)
        {
            //boolean to check if the steps is done
            if (!placementDone)
            {
                FoodPlacementUI.SetActive(true);
                OnChoosingPlacement.SetActive(true);
            }
            else
            {
                FoodPlacementUI.SetActive(false);
                OnChoosingPlacement.SetActive(false);
                placementDone = false;
                mainUI.SetActive(true);

                //reset the chance
                chance = 2;
            }
        }
    }
}
