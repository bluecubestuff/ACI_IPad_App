using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TouchManager : MonoBehaviour
{

    /*TouchManager/GameManager for Supplier Scene
     
         Scene is divided into 3 parts : Main Scene, Supplier Info, Selection Scene
         - Main Scene : Initial Scene
         - Supplier Info : Displays supplier info
         - Selection Scene : The gameplay that players chooses their food
         
         This is the main script that controls SUPPLIER SCENE*/


    [Header("Animation")]
    public Animator SupplierAnim;          //Animation for Supplier Info popup
    public Animator SelectionTopAnim;      //During Selection, Top bar Drop animation
    public Animator SelectionBtmAnim;      //During Selection, Btm bar rise animation


    [Header("Canvas Objects")]
    //Canvas as GameObjects to enable easy active off/on in codes
    public GameObject UI_MainCanvas;
    public GameObject UI_SelectionCanvas;
    public GameObject UI_SettingsCanvas;
    public GameObject UI_FullStockNotif;
    public GameObject ObjectListParent;         //Parent to Create instance of each "order" under as child
    public GameObject ObjectListBackPanel;
    public GameObject UI_SelectConfirmation;    //Seperated Confirmation UI with Purchase UI so Borders of 
    public GameObject UI_PurchaseUI;                //Selection Scene not needed to remove
    //Popup when food bought in Main scene
    public GameObject popupnotifObject;
    public GameObject selectionBG; // A semi-black background for when selecting food item
    
    [Header("Canvas 3D Objects")]
    //3D on UI is done by putting 3D model then a plane UI as Camera Child
    public GameObject UI_SelectionModels;
    public GameObject UI_SelectionPanel;
    public GameObject AllSuppliers_Parent;
    public GameObject AR_StarQuality_Parent;



    [Header("InGame Objects")]
    public GameObject SelectionSceneForAR;      //AR Images and UI on AR Scene
    public GameObject AllEnvironmentForAR;      //All environment for UI
    public GameObject SelectionCircleForAR;
    public GameObject ARConfirmation;
    public GameObject ARPurchased;

    public GameObject Grime;

    [Header("Spawn Locations")]
    //Spawn location for trucks for different Suppliers
    public GameObject Spawn_A;
    public GameObject Spawn_B;
    public GameObject Spawn_C;
    public GameObject Spawn_D;

    [Header("DataTemps")]
    public GameObject selectedFood;        //Data holder for last food selected
    public Text FoodOftheDay;               //Text to show general food type during selection
    public GameObject FinalPurchased_ReturnPosition;    //Coded Animation For Food selection in Virtual Scene
    public GameObject FinalPurchased_Target;


    [Header("Miscellaneous")]
    public GameObject cameraCheck;
    bool mainCanvas_open;
    bool SelConfm_open;
    private bool isDragging;    //Check for Drag so suppliers or other elements cant be selected while drag
    public GameObject inShopInstructions; // Tutorial usage
    bool preventTouch; // Remove in the future, just adding cuz last minute, used to prevent touch when the images are moving
    //bool for selection model animation
    bool moveforward;
    bool moveback;

    public StocknPopularityManager stocknPopInstance;   //Bar UI Instance
    public Tutorial tutorial;                   //Tutorial Instance
    //number that decides food type to supplier type
    private int decidefoodnumber;

    public GameObject cameraManager;
    public Text selectedFoodName;
    public Image selectedFoodQuality;

    public static bool inShop;

    //Create Raycast
    RaycastHit hit;
    [SerializeField]
    GameObject blackBackground;

    // Use this for initialization
    void Start()
    {
        mainCanvas_open = true;

        //Loads the OrderList from GameCache In Editor
        ObjectListParent.GetComponent<OrderListManager>().GetOrderList();

        /*99 = any food ok (Random)
            0 = Vegetables Only (Tomato, Mushroom)
            1 = Canned/Packed Products (Canned Food)
            2 = Meat Products (Steak, Chicken)
            3 = Dairy (Cheese)*/
        decidefoodnumber = 99;

    }

    // Update is called once per frame
    void Update()
    {
        #region Animation for model display when selected, must be before "preventTouch"

        //SelectFoodAnim
        RunForwardSelectionAnimation();

        #endregion
        if (preventTouch)
            return;
        //Check on touch
        if (Input.GetMouseButtonUp(0))
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
            
            if (!IsPointerOverUIObject())
                if ((Physics.Raycast(ray, out hit, Mathf.Infinity))
                    
                && !UI_SettingsCanvas.activeSelf
                && !cameraCheck.GetComponent<DragCamera>().IsDragging
                && !ObjectListBackPanel.activeSelf
                /*&& tutorialObject.activeSelf == false*/)
                {


                    //Check Raycast hit with Gameobject's tag
                    switch (hit.collider.gameObject.tag)
                    {
                        #region RaycastHitTags

                        case ("example"):
                            {
                                Destroy(hit.transform.gameObject);
                            }
                            break;

                        case ("Supplier"):
                            {
                                //To OPEN supplier screen
                                //Check if SelectionModels is off AND if Tutorial is done
                                if (ObjectListParent.transform.childCount >= 5)
                                {
                                    Debug.Log("Full");
                                    UI_FullStockNotif.SetActive(true);
                                }

                                if (!UI_SelectionModels.activeSelf && ObjectListParent.transform.childCount < 5)
                                {
                                    CloseMainUI();

                                    popupnotifObject.SetActive(false);
                                    //NEW
                                    if (hit.collider.gameObject.name == "Cheese_Shop") //Dairy/Cheese food supplier
                                    {
                                        Debug.Log("CHEESESHOP RAYCASTED");
                                        decidefoodnumber = 3;
                                        cameraManager.GetComponent<CameraMovement>().EnterShop("CheeseShop");
                                        inShop = true;
                                    }
                                    if (hit.collider.gameObject.name == "Canned_Shop")
                                    {
                                        Debug.Log("CANNEDSHOP RAYCASTED");
                                        decidefoodnumber = 1;
                                        cameraManager.GetComponent<CameraMovement>().EnterShop("CannedShop");
                                        inShop = true;
                                    }
                                    if (hit.collider.gameObject.name == "Veggie_Shop")
                                    {
                                        Debug.Log("VEGGIESHOP RAYCASTED");
                                        decidefoodnumber = 0;
                                        cameraManager.GetComponent<CameraMovement>().EnterShop("VeggieShop");
                                        inShop = true;
                                    }
                                    if (hit.collider.gameObject.name == "Meat_Shop")
                                    {
                                        Debug.Log("MEATSHOP RAYCASTED");
                                        decidefoodnumber = 2;
                                        cameraManager.GetComponent<CameraMovement>().EnterShop("MeatShop");
                                        inShop = true;
                                    }
                                    //Setting temp model to model of clicked supplier
                                    //UI_SupplierModel.transform.GetChild(0).gameObject.GetComponent<MeshFilter>().mesh = hit.collider.gameObject.GetComponent<MeshFilter>().mesh;
                                    //UI_SupplierModel.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = hit.collider.gameObject.GetComponent<MeshRenderer>().material;

                                    OpenSupplierUI();
                                }
                            }
                            break;


                        //During Selection Scene (Virt), FoodSelection tag per Food item during selection
                        case ("FoodSelection"):
                            {
                                //On selected raycast hit food

                                //Check if first Confirmation(yes/no) is active
                                if (UI_SelectConfirmation.activeSelf == false)
                                {
                                    //check if already purchased screen is active

                                    //set current selected stock to the current hit by raycast. StockInfo hold the data . current is a data holder
                                    StockManager.StockInstance.CurrentStock = hit.collider.GetComponent<StockInfo>();

                                    //selectedfood is same as current. holds data of current food but used in this script (TouchManager)
                                    selectedFood = hit.collider.gameObject;
                                    //Enable/show and move animated model
                                    if (selectedFood.name == "Placement")
                                        MoveSelectedFoodAnimation();
                                    preventTouch = true;
                                }

                            }
                            break;

                        //During Selection Scene(AR), ImageSelectionAR tag per Food item during selection
                        case ("ImageSelectionAR"):
                            {
                                SelectionCircleForAR.transform.position = hit.collider.gameObject.transform.position;
                                SelectionCircleForAR.SetActive(true);
                                ARConfirmation.SetActive(true);
                                selectedFood = hit.collider.gameObject;
                            }
                            break;
                        case ("ARWrongBtn"):
                            {
                                SelectionCircleForAR.SetActive(false);
                                ARConfirmation.SetActive(false);
                            }
                            break;
                        case ("ARRightBtn"):
                            {
                                Debug.Log(selectedFood.GetComponent<StockInfo>().food.foodID);
                                //Where is orderlist manager?
                                //OrderListManager.orderInstance.ShowOrder(selectedFood.GetComponent<StockInfo>().food.foodName);
                                ARConfirmation.SetActive(false);
                                SupplierSceneManager.SupplierInstance.RandomSupplierRating();
                                OpenPurchaseUI();
                                //off selection header;
                            }
                            break;

                        //Enable Purchased Image
                        case ("ARPurchased"):
                            {
                                ClosePurchaseAR();

                                BuyStock();
                            }
                            break;

                        case ("Grime"):
                            {
                                hit.collider.gameObject.SetActive(false);
                            }

                            break;

                            #endregion
                    }
                }
        }
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    #region OpenCloseUIStuff

    public void OpenMainUI()
    {
        UI_MainCanvas.SetActive(true);

    }
    public void CloseMainUI()
    {
        UI_MainCanvas.SetActive(false);
    }
    public void OpenSupplierUI()
    {
        BasicCheckBeforeBuying();
        //UI_SupplierCanvas.SetActive(true);
        //UI_SupplierModel.SetActive(true);   //3D supplier on UI Screen
        //SupplierAnim.SetTrigger("Open");
    }

    public void OpenSelectionUI()
    {
        UI_SelectionCanvas.SetActive(true);
        UI_SelectionModels.SetActive(true);     //4 Food Models that players will see to choose quality from
    }
    public void CloseSelectionUI()
    {
        UI_SelectionCanvas.SetActive(false);
        UI_SelectionModels.SetActive(false);
    }

    //Close ONLY "yes/no" buttons DURING SELECTION
    public void CloseSelectionConfirmation()
    {
        UI_SelectConfirmation.SetActive(false);
        selectionBG.SetActive(false);
        if (!NewTutorials.tutDone)
        {
            inShopInstructions.GetComponentInChildren<Text>().text = "Choose the one that seems\nthe <color=red>freshest</color>.";
            NewTutorials.currentStep -= 1;
        }
    }

    //"You have Bought xxx!" UI.
    public void OpenPurchaseUI()
    {
        if (SceneManager.GetActiveScene().name != "AR_Main")
        {
            if (!tutorial.tutDone)
                tutorial.NextBtn();

            UI_PurchaseUI.SetActive(true);

            //Ratings on Purchase Scene to show quality of food bought
            StockManager.StockInstance.SetRatingFill();
        }
        else
        {
            //markaa
            //TODO : Add Quality of food during AR Mode
            /* Current Quality of food in virtual mode is done with "Image Fill Amount".
                In 3D space of AR mode, Image has no "Fill Amount".*/


            int tempQuality = (int)(selectedFood.GetComponent<StockInfo>().food.foodRarity);


            for (int i = 0; i < tempQuality; i++)
            {
                AR_StarQuality_Parent.transform.GetChild(i + 1).gameObject.SetActive(true);
            }

            ARPurchased.SetActive(true);
        }
    }

    public void ClosePurchase()
    {
        if (!tutorial.tutDone)
            tutorial.NextBtn();

        UI_PurchaseUI.SetActive(false);
    }

    public void ClosePurchaseAR()
    {
        ARPurchased.SetActive(false);
        AllEnvironmentForAR.SetActive(true);
        SelectionSceneForAR.SetActive(false);
        SelectionCircleForAR.SetActive(false);
        UI_MainCanvas.SetActive(true);

        for (int i = 1; i < AR_StarQuality_Parent.transform.childCount; i++)
        {
            AR_StarQuality_Parent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }


    #endregion
    //Check if Purchase amount has reached orderlist Limit
    public void BasicCheckBeforeBuying()
    {
        //if (!tutorial.tutDone)
        //    tutorial.NextBtn();

        //Max 5 limit of Purchases
        if (ObjectListParent.transform.childCount >= 5)
        {
            //if reached max, Show MaxLimit Notification
            LimitedPurchase();
        }
        else
        {
            SuccessfulPurchase();
        }
    }
    public void SuccessfulPurchase()
    {
        //Open Selection Scene
        if (SceneManager.GetActiveScene().name != "AR_Main")
        {
            OpenSelectionUI();
            UI_SelectionPanel.SetActive(true);
        }
        else
        {
            SelectionSceneForAR.SetActive(true);
            AllEnvironmentForAR.SetActive(false);
        }

        //Set Quality of foods during Selection Scene (4 Choices)
        StockManager.StockInstance.SetStockRatings();
        //Generate Type of Food to be Chosen. ("decidefoodnumber" is fixed based on factory/Supplier type during raycast collision)
        StockManager.StockInstance.RandomizeFoodType(decidefoodnumber);
    }
    public void LimitedPurchase()
    {
        UI_FullStockNotif.SetActive(true);
    }

    //public void ReturnWithoutBuying()
    //{
    //    CheckWhichSupplierToSendTruck = 0;
    //}

    public void BuyStock()
    {
        //Set Feedback to tell player to go Storage Scene

        //PopUpBarNotifAnim.ispopup = true;

        var tutorialManager = FindObjectOfType<SuppliersTutorialManager>();
        if (tutorialManager)
            tutorialManager.OnFoodBought();

        if (TutorialManager.page == TutorialManager.Page.Complete)
            popupnotifObject.SetActive(true);

        //Update Overall Quality of food. (Player interface at top left)
        UpdateNewStockRatings();
    }


    //Update Player's Overall Food Quality
    /*This element of the game determines how much popularity points 
        is given by the customers in the later <Restaurant> Stage.
        
         It is determined by the average of "Player's Current Quality" 
          and the quality of food that the player last bought.
         */
    public void UpdateNewStockRatings()
    {
        float currTemp = StocknPopularityManager.mainRatingValue * 5;
        float currFoodRarity = (float)selectedFood.GetComponent<StockInfo>().food.foodRarity;
        float final = (currTemp + currFoodRarity) * 0.5f;

        //Round off the quality fill at "half" the star.
        if (final % 1 > 0.5)
            StocknPopularityManager.mainRatingValue = ((int)final + 1) * 0.2f;
        else if (final % 1 < 0.5)
            StocknPopularityManager.mainRatingValue = (int)final * 0.2f;
        else
            StocknPopularityManager.mainRatingValue = final * 0.2f;

    }

    //After Purchase close
    public void PurchaseReturnMarco(bool finalCheck)
    {
        CloseSelectionConfirmation();

        CloseSelectionUI();

        OpenMainUI();
            inShop = false;

        if (finalCheck)
        {
            BuyStock();
        }
        ReturnSelectedFoodAnimation();
    }


    //Animation to control food movement during selection
    #region AnimationDuringSelection

    /* Coded Animation of 3D Selection models movement to Center of Screen.
     
         Actual order of execution is numbered by "#".
         To see where the code is running, Find: SelectFoodAnim */

    //#2
    public void RunForwardSelectionAnimation()
    {

        //On Update, if animated model is moveforward is true
        if (moveforward)
        {
            //Check distance btwn animated model and display target
            float dist = Vector3.Distance(FinalPurchased_Target.transform.position, selectedFood.transform.position);
            if (hit.collider.gameObject.name == "Placement")
            {
                if (dist > 0.1)
                {
                    selectedFood.transform.position += (FinalPurchased_Target.transform.position - selectedFood.transform.position).normalized * 6f * Time.deltaTime;
                    //if (((selectedFood.GetComponent<StockInfo>().food.foodID >= 10 && selectedFood.GetComponent<StockInfo>().food.foodID <= 14)) || (selectedFood.GetComponent<StockInfo>().food.foodID >= 20 && selectedFood.GetComponent<StockInfo>().food.foodID <= 24))
                    //    selectedFood.transform.Rotate(Vector3.left * 2.1f);
                    //else
                    //    selectedFood.transform.Rotate(Vector3.forward * 2.1f);
                }
                else
                {
                    //when reached target, set position and stop bool to stop movement
                    selectedFood.transform.position = FinalPurchased_Target.transform.position;
                    moveforward = false;
                    UI_SelectConfirmation.SetActive(true);
                    selectedFoodName.text = selectedFood.GetComponent<StockInfo>().food.foodName;
                    selectionBG.SetActive(true);
                    if (selectedFoodName.text.Contains("Chilli") || selectedFoodName.text.Contains("Carrot") || selectedFoodName.text.Contains("Tomato"))
                        selectionBG.transform.position = new Vector3(selectedFood.transform.position.x, 1, selectedFood.transform.position.z);
                    else if (selectedFoodName.text.Contains("Steak"))
                        selectionBG.transform.position = new Vector3(selectedFood.transform.position.x, -0.7f, 11.7f);
                    else if (selectedFoodName.text.Contains("Fish") || (selectedFoodName.text.Contains("Crab")))
                        selectionBG.transform.position = new Vector3(selectedFood.transform.position.x, 1.15f, 7.0f);
                    else if (selectedFoodName.text.Contains("Canned"))
                        selectionBG.transform.position = new Vector3(selectedFood.transform.position.x, 0.9f, 7f);


                    selectionBG.transform.rotation = Camera.main.transform.rotation;
                    //selectedFoodQuality.fillAmount = (float)StockManager.StockInstance.CurrentStock.transform.GetComponent<StockInfo>().food.foodRarity * 0.2f;
                }
            }
        }
    }

    //#4
    //public void RunReturnSelectionAnimation()
    //{
    //    //StartCoroutine(selectedFood.GetComponent<StockInfo>().ReturnToPosition());
    //    //if (selectedFood != null)
    //    //{
    //    //    if (moveback)
    //    //    {
    //    //        float dist = Vector3.Distance(FinalPurchased_ReturnPosition.transform.position, selectedFood.transform.position);
                
    //    //        if (hit.collider.gameObject.name == "Placement")
    //    //        {
    //    //            if (dist > 0.1)
    //    //            {
    //    //                selectedFood.transform.position += (FinalPurchased_ReturnPosition.transform.position - selectedFood.transform.position).normalized * 6f * Time.deltaTime;
    //    //                //if (((selectedFood.GetComponent<StockInfo>().food.foodID >= 10 && selectedFood.GetComponent<StockInfo>().food.foodID <= 14)) || (selectedFood.GetComponent<StockInfo>().food.foodID >= 20 && selectedFood.GetComponent<StockInfo>().food.foodID <= 24) )
    //    //                //    selectedFood.transform.Rotate(Vector3.right * 2.1f);
    //    //                //else
    //    //                //    selectedFood.transform.Rotate(Vector3.back * 2.1f);
    //    //            }
    //    //            else
    //    //            {
    //    //                selectedFood.transform.position = FinalPurchased_ReturnPosition.transform.position;
    //    //                moveback = false;
    //    //                preventTouch = false;
    //    //                //Set all models to show
    //    //                for (int i = 0; i < StockManager.StockInstance.SelectionModel.transform.childCount; i++)
    //    //                {
    //    //                    UI_SelectionModels.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = true;
    //    //                    UI_SelectionModels.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().enabled = true;
    //    //                }
    //    //            }
    //    //        }
    //    //    }
    //    //}
    //}

    //#1
    public void MoveSelectedFoodAnimation()
    {
        //Setting the animated model position to selected food position
        FinalPurchased_ReturnPosition.transform.position = selectedFood.transform.position;
        //Disable not selected models
        for (int i = 0; i < StockManager.StockInstance.SelectionModel.transform.childCount; i++)
        {
            if (UI_SelectionModels.transform.GetChild(i).name != selectedFood.name)
            {
                UI_SelectionModels.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = false;
                UI_SelectionModels.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().enabled = false;

            }
            else
            {
                UI_SelectionModels.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = true;
                UI_SelectionModels.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }

        UI_SelectionPanel.SetActive(false);

        moveforward = true;
    }

    //#3
    public void ReturnSelectedFoodAnimation()
    {
        if (selectedFood == null)
            return;

        StartCoroutine(selectedFood.GetComponent<StockInfo>().ReturnToPosition(() =>
        {
            UI_SelectionPanel.SetActive(true);
            preventTouch = false;
        }));
        //moveback = true;
    }

    #endregion


}
