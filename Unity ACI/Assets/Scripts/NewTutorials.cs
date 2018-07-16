using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class NewTutorials : MonoBehaviour {

    [Header("UI Buttons")]
    public GameObject TUT_StorageButton;
    public GameObject TUT_ShopsButton;
    public GameObject TUT_DinerButton;
    public GameObject TUT_OrderListButton;
    public GameObject TUT_OptionsButton;
    public GameObject TUT_MeatFabButton;


    [Header("Others")]
    public GameObject TUT_OptionsMenu;
    public GameObject TUT_OrderList;
    public GameObject TUT_HUD;
    public GameObject TUT_blackBackground;
    public GameObject Arrow;
    public GameObject TUT_Instructions;
    public GameObject inShopInstructions;
    public GameObject TapAnywhereToCont;

    [Header("\"Holders\"")]
    public GameObject mainGame;
    public GameObject Tutorial;

    // Restaurant Scene use
    public GameObject skipTut;
    // Supplier Scene use
    public GameObject TutorialArrows;
    public GameObject inSupplierShopTutorial;

    // Storage Scene use
    [SerializeField]
    GameObject MainUIButtons;

    public static bool tutDone = false; // set to true during debug
    public static int currentStep = 0; // which part of the tutorial

    /*
     * Tutorial Steps/Screens active
     * Step 1: Welcome
     * Step 2: No Food
     * Step 3: Click on any shops to go in (supplier scene)
     * Step 4: Yellow arrows pointing at each of the shops
     * Step 5: Choose the food that looks the freshest
     * Step 6: Accept/Reject the food
     * Step 7: Confirm to buy food
     * Step 8: Storage, tap anywhere to continue
     * Step 9: Waiting for user to click on box
     * Step 10: Accept or reject explaination, tap anywhere to continue
     * Step 11: Completed Storing of food, waiting to go back to diner
     * Step 12: Win condition explaination
     * Step 13: Meat Fab button
     * Step 14: Tutorial done
     */
    // Use this for initialization
    void Start () {
        if (tutDone)
        {
            Tutorial.SetActive(false);
            mainGame.SetActive(true);
             
            if (SceneManager.GetActiveScene().name == "Virt_Suppliers")
            {
                TutorialArrows.SetActive(false);
                inSupplierShopTutorial.SetActive(false);
            }
        }
        if (currentStep == 0 && skipTut != null)
        {
            skipTut.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tutDone)
            return;

        var currentScene = SceneManager.GetActiveScene();

        if (SceneManager.GetActiveScene().name == "Virt_Restuarant")
            RestaurantSceneUpdate();
        else if (SceneManager.GetActiveScene().name == "Virt_Suppliers")
            SuppliersSceneUpdate();
        else if (SceneManager.GetActiveScene().name == "Virt_Storage")
            StorageSceneUpdate();
        
    }

    void RestaurantSceneUpdate()
    {
        switch (currentStep)
        {
            // Step 0: Ask user if they want to skip the tutorial
            case 0:
                break;
            // Step 1: Tap anywhere to continue, text changes to "need to buy food"
            case 1:
                if (Input.GetMouseButtonDown(0))
                {

                    TUT_HUD.SetActive(true);
                    TUT_Instructions.GetComponentInChildren<Text>().text = "First, to operate a restaurant, \nyou need food. You don't have\n food, let's buy some.";
                    currentStep += 1;
                }
                break;
            // Step 2: Tap anywhere to continue, text changes to "Click on Shops to head over to the suppliers."
            case 2:
                if (Input.GetMouseButtonDown(0))
                {
                    TapAnywhereToCont.SetActive(false);
                    TUT_HUD.SetActive(false);
                    TUT_ShopsButton.SetActive(true);
                    Arrow.SetActive(true);
                    TUT_Instructions.GetComponentInChildren<Text>().text = "Click on <color=red>Shops</color> to\n head over to the suppliers.";
                    currentStep += 1;
                    // Goes to supplier scene here
                }
                break;
            case 12:
                {
                    TUT_Instructions.SetActive(true);
                    TUT_Instructions.GetComponentInChildren<Text>().text = "To win the game, get your rating \nto 5 stars!";
                    TUT_HUD.SetActive(true);
                    TapAnywhereToCont.SetActive(true);
                    currentStep += 1;
                }
                break;
            case 13:
                if (Input.GetMouseButtonDown(0))
                {
                    TUT_Instructions.GetComponentInChildren<Text>().text = "That's all for the tutorial!";
                    TUT_HUD.SetActive(false);
                    currentStep += 1;
                }
                break;
            case 14:
                if (Input.GetMouseButtonDown(0))
                {
                    Tutorial.SetActive(false);
                    mainGame.SetActive(true);
                    tutDone = true;
                }
                break;
        }
    }
    void SuppliersSceneUpdate()
    {
        Button[] temp = MainUIButtons.GetComponentsInChildren<Button>();

        switch (currentStep)
        {
            //Step 3: Tap anywhere to continue, after this the tutorial window closes, allowing players to choose which shop they want to enter
            case 3:
                if (Input.GetMouseButtonDown(0))
                {
                    Tutorial.SetActive(false);
                    TutorialArrows.SetActive(true);

                    ++currentStep;
                }
                foreach (Button child in temp)
                {
                    child.interactable = false;
                }
                break;
            // Step 4: Entered Shop
            case 4:
                if (TouchManager.inShop)
                {
                    TutorialArrows.SetActive(false);
                    inSupplierShopTutorial.SetActive(true);
                    ++currentStep;

                }
                foreach (Button child in temp)
                {
                    child.interactable = false;
                }
                break;
            // Step 5: Choose which food they want

            case 5:
                if (Input.GetMouseButtonDown(0))
                {
                    inShopInstructions.transform.localPosition = new Vector3(357, 321, 0);
                    inShopInstructions.GetComponentInChildren<Text>().text = "<color=green>Accept</color> to confirm the\n order, and <color=red>Reject</color> to\n choose something else.";
                    ++currentStep;
                }
                foreach (Button child in temp)
                    child.interactable = false;
                break;
            // Step 6: Chosen food window is open, waiting on user to choose accept or reject
            // If user clicks reject (to choose another food, currentStep will -1, done on click of reject button 
            // If they click on the back to overview button, currentStep will -2, done on click of back to overview button
            case 6:
                foreach (Button child in temp)
                    child.interactable = false;
                break;

            // Step 7: After confirmation of buying food, waiting to go storage
            case 7:
                if (TUT_OrderList.transform.childCount >= 1)
                {
                    Tutorial.SetActive(true);
                    mainGame.SetActive(true);
                    TUT_blackBackground.SetActive(true);
                    TUT_Instructions.GetComponentInChildren<Text>().text = "Now you have to store your\n food. Click Storage.";
                    Arrow.SetActive(true);
                    TUT_StorageButton.SetActive(true);
                    TapAnywhereToCont.SetActive(false);
                    ++currentStep;
                }

                break;

        }
    }
    void StorageSceneUpdate()
    {
        switch (currentStep)
        {
            // Step 8: Tap anywhere to continue
            case 8:
                if (Input.GetMouseButtonDown(0))
                {
                    TapAnywhereToCont.SetActive(false);
                    TUT_Instructions.SetActive(false);
                    TUT_blackBackground.SetActive(false);
                    Arrow.SetActive(true);

                    Button[] temp = MainUIButtons.GetComponentsInChildren<Button>();
                    foreach (Button child in temp)
                    {
                        child.interactable = false;
                    }
                    currentStep += 1;

                    Vector3 mousePosFar = new Vector3(Input.mousePosition.x,
                                                    Input.mousePosition.y,
                                                    Camera.main.farClipPlane);
                    Vector3 mousePosNear = new Vector3(Input.mousePosition.x,
                                                       Input.mousePosition.y,
                                                       Camera.main.nearClipPlane);
                    Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
                    Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

                    Debug.DrawRay(mousePosN, mousePosF - mousePosN, Color.green);


                    //Create Raycast
                    RaycastHit hit;
                    if ((Physics.Raycast(mousePosN, mousePosF - mousePosN, out hit)))
                    {
                        if (hit.collider.gameObject.tag == "Truck" && hit.collider.gameObject.GetComponent<Truck>().food.foodName != "")
                        {
                            Arrow.SetActive(false);
                            TUT_blackBackground.SetActive(true);
                            TUT_Instructions.GetComponentInChildren<Text>().text = "Check if the delivery\nmatches the order list.";
                            TUT_Instructions.SetActive(true);
                            TapAnywhereToCont.SetActive(true);
                            currentStep += 1;

                        }
                    }
                }
                break;
            // Step 9 : Waiting on user to click on box
            case 9:
                if (Input.GetMouseButtonDown(0))
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


                    //Create Raycast
                    RaycastHit hit;
                    if ((Physics.Raycast(mousePosN, mousePosF - mousePosN, out hit)))
                    {
                        if (hit.collider.gameObject.tag == "Truck" && hit.collider.gameObject.GetComponent<Truck>().food.foodName != "")
                        {
                            Arrow.SetActive(false);
                            TUT_blackBackground.SetActive(true);
                            TUT_Instructions.GetComponentInChildren<Text>().text = "Check if the delivery\nmatches the order list.";
                            TUT_Instructions.SetActive(true);
                            TapAnywhereToCont.SetActive(true);
                            currentStep += 1;
                        }
                    }
                }
                break;
            // Step 10: User clicked on box
            case 10:
                if (Input.GetMouseButtonDown(0))
                {
                    TUT_blackBackground.SetActive(false);
                    TUT_Instructions.SetActive(false);
                    TapAnywhereToCont.SetActive(false);
                }
                break;
            // Step 11: Completed storing
            case 11:
                {
                    TUT_blackBackground.SetActive(true);
                    TUT_Instructions.GetComponentInChildren<Text>().text = "Now that you are done, go \nback to the <color=red>diner</color>.";
                    TUT_Instructions.SetActive(true);
                    TUT_DinerButton.SetActive(true);
                    Arrow.transform.localPosition = new Vector3(588, 230, 0);
                    Arrow.transform.rotation = Quaternion.Euler(0, 0, 90);
                    Arrow.SetActive(true);
                    currentStep += 1;
                }
                break;
        }
        }

    public void addCurrentStep(int amt)
    {
        if (!tutDone)
            currentStep += amt;
    }

    public void backToOverViewReduceStep()
    {
        if (!tutDone)
            currentStep = 4;
        TouchManager.inShop = false;
    }

    public void skipTutorial(bool yesOrno)
    {
        tutDone = yesOrno;
    }
}
