using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour {

    public Camera innerCamera;
    public GameObject tempOverview;

    //----------------------------------------------------------------------------
    //Main Camera / First Camera in each shop
    public Camera MainCam, VegMainCam, MeatMainCam, CheeseMainCam, CannedMainCam, DefaultCamera;

    //Veg Sub Camera in each shop [UNUSED]
    public Camera VegLeftCam/*, VegRightCam*/;

    //Meat Sub Camera in each shop
    public Camera MeatLeftCam, MeatRightCam;

    //Cheese Sub Camera in each shop [UNUSED]
    //public Camera CheeseLeftCam, CheeseRightCam;

    //Canned Sub Camera in each shop [UNUSED]
    //public Camera CannedLeftCam, CannedRightCam;

    //Bool for which shop the main camera is at
    public enum SHOP_TYPE
    {
        S_MAIN,
        S_VEGGIE,
        S_MEAT,
        S_CHEESE,
        S_CANNED
    };

    SHOP_TYPE typeOfShop;

    public GameObject /*LeftArrow, RightArrow,*/ OverviewButton, MenuUIButtons;

    //----------------------------------------------------------------------------
    //Fade Transition
    public Texture2D fadeOutTexture;    // the texture that will overlay the screen. This can be a black image or a loading graphic
    public float fadeSpeed = 0.8f;      // the fading speed

    private int drawDepth = -1000;      // the texture's order in the draw hierarchy: a low number means it renders on top
    private float alpha = 1.0f;         // the texture's alpha value between 0 and 1
    private int fadeDir = -1;           // the direction to fade: in = -1 or out = 1
    //----------------------------------------------------------------------------

    //Camera Transition (LERP)
    IEnumerator LerpToPosition(float lerpSpeed, Transform newPosition)
    {
        float t = 0.0f;
        Transform startingPos = MainCam.transform;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / lerpSpeed);

            MainCam.transform.position = Vector3.Lerp(startingPos.position, newPosition.position, t);
            MainCam.transform.rotation = Quaternion.Lerp(startingPos.rotation, newPosition.rotation, t);
            yield return 0;
        }
    }

    //Camera Transition (INSTANT MOVE)
    public IEnumerator MoveToPosition(Transform newPosition)
    {
        MainCam.transform.position = newPosition.position;
        MainCam.transform.rotation = newPosition.rotation;
        yield return 0;
    }

    private void MoveSelectionModels(string ShopType)
    {
        Transform SupplierSelected = null;

        switch (ShopType)
        {
            case "Veggie":
                SupplierSelected = StockManager.StockInstance.VeggieSelectionParent.transform;
                break;
            case "Meat":
                SupplierSelected = StockManager.StockInstance.MeatSelectionParent.transform;
                break;
            case "Cheese":
                SupplierSelected = StockManager.StockInstance.CheeseSelectionParent.transform;
                break;
            case "Canned":
                SupplierSelected = StockManager.StockInstance.CannedSelectionParent.transform;
                break;
        }

        for (int i = 0; i < SupplierSelected.childCount - 1; i++)
        {
            StockManager.StockInstance.SelectionModel.transform.GetChild(i).transform.position = SupplierSelected.GetChild(i).transform.position;
            StockManager.StockInstance.SelectionModel.transform.GetChild(i).transform.localRotation = SupplierSelected.GetChild(i).transform.localRotation;
            StockManager.StockInstance.SelectionModel.transform.GetChild(i).GetComponent<StockInfo>().InitialPosition = SupplierSelected.GetChild(i).transform.position;
            StockManager.StockInstance.FinalSelectionModel.transform.position = SupplierSelected.GetChild(StockManager.StockInstance.SelectionModel.transform.childCount).transform.position;
            StockManager.StockInstance.FinalSelectionModel.transform.localRotation = SupplierSelected.GetChild(StockManager.StockInstance.SelectionModel.transform.childCount).transform.localRotation;
        }
    }

    // Use this for initialization
    void Start () {
        typeOfShop = SHOP_TYPE.S_MAIN;
        OverviewButton.SetActive(false);
        //LeftArrow.SetActive(false);
        //RightArrow.SetActive(false);
        MenuUIButtons.SetActive(true);

        //tempOverview.SetActive(false);
    }

    void OnGUI()
    {
        // fade out/in the alpha value using a direction, a speed and Time.deltaTime to convert the operation to seconds
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        // force (clamp) the number to be between 0 and 1 because GUI.color uses Alpha values between 0 and 1
        alpha = Mathf.Clamp01(alpha);

        // set color of our GUI (in this case our texture). All color values remain the same & the Alpha is set to the alpha variable
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;                                                              // make the black texture render on top (drawn last)
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);       // draw the texture to fit the entire screen area
    }

    // sets fadeDir to the direction parameter making the scene fade in if -1 and out if 1
    public float BeginFade(int direction)
    {
        Debug.Log("BEGAN FADE");
        fadeDir = direction;
        return (fadeSpeed);
    }

    public SHOP_TYPE GetShopType()
    {
        return typeOfShop;
    }

    //From Main Default Camera, enter the different shops based on name entered in inspector
    public void EnterShop(string shopName)
    {
        Debug.Log("ENTERED");

        //MenuUIButtons.SetActive(false);

        //Fade Call ///////
        alpha = 1;       //
        BeginFade(-1);   //
        ///////////////////

        switch(shopName)
        {
            case "VeggieShop":
                StartCoroutine(MoveToPosition(VegMainCam.transform));
                typeOfShop = SHOP_TYPE.S_VEGGIE;
                MoveSelectionModels("Veggie");
                break;
            case "MeatShop":
               StartCoroutine(MoveToPosition(MeatMainCam.transform));
                typeOfShop = SHOP_TYPE.S_MEAT;
                MoveSelectionModels("Meat");
                break;
            case "CheeseShop":
                StartCoroutine(MoveToPosition(CheeseMainCam.transform));
                typeOfShop = SHOP_TYPE.S_CHEESE;
                MoveSelectionModels("Cheese");
                break;
            case "CannedShop":
                StartCoroutine(MoveToPosition(CannedMainCam.transform));
                typeOfShop = SHOP_TYPE.S_CANNED;
                MoveSelectionModels("Canned");
                break;
            default:
                typeOfShop = SHOP_TYPE.S_MAIN;
                break;
        }

        OverviewButton.SetActive(true);
        MenuUIButtons.SetActive(false);
    }

    //In Shop, On click on left arrow, transition left
    public void MoveLeft()
    {
        switch (typeOfShop)
        {
            case SHOP_TYPE.S_VEGGIE:
                if(MainCam.transform.position == VegMainCam.transform.position)
                {
                    StartCoroutine(LerpToPosition(5.0f, VegLeftCam.transform));
                }
                break;
            case SHOP_TYPE.S_MEAT:
                if (MainCam.transform.position == MeatRightCam.transform.position)
                {
                    Debug.Log("MOVING TO MAIN POS");
                    StartCoroutine(LerpToPosition(0.5f, MeatMainCam.transform));
                   // LeftArrow.GetComponent<Button>().interactable = true;
                    //RightArrow  .GetComponent<Button>().interactable = true;
                }
                if (MainCam.transform.position == MeatMainCam.transform.position)
                {
                    Debug.Log("MOVING TO LEFT POS");
                   // StartCoroutine(LerpToPosition(0.5f, MeatLeftCam.transform));
                   // LeftArrow.GetComponent<Button>().interactable = false;
                }
                break;
            case SHOP_TYPE.S_CHEESE:
                //StartCoroutine(LerpToPosition(5.0f, CheeseLeftCam.transform));
                break;
            case SHOP_TYPE.S_CANNED:
                //StartCoroutine(LerpToPosition(5.0f, CannedLeftCam.transform));
                break;
        }
    }

    //In Shop, On click on right arrow, transition right
    public void MoveRight()
    {
        switch (typeOfShop)
        {
            case SHOP_TYPE.S_VEGGIE:
                if(MainCam.transform.position == VegLeftCam.transform.position)
                {
                    StartCoroutine(LerpToPosition(5.0f, VegMainCam.transform));
                }
                break;
            case SHOP_TYPE.S_MEAT:
                if (MainCam.transform.position == MeatLeftCam.transform.position)
                {
                    Debug.Log("MOVING TO MAIN POS");
                    StartCoroutine(LerpToPosition(0.5f, MeatMainCam.transform));
                   // RightArrow.GetComponent<Button>().interactable = true;
                    //LeftArrow.GetComponent<Button>().interactable = true;
                }
                if (MainCam.transform.position == MeatMainCam.transform.position)
                {
                    Debug.Log("MOVING TO RIGHT POS");
                    //StartCoroutine(LerpToPosition(0.5f, MeatRightCam.transform));
                    //RightArrow.GetComponent<Button>().interactable = false;
                }
                break;
            case SHOP_TYPE.S_CHEESE:
                //StartCoroutine(LerpToPosition(5.0f, CheeseRightCam.transform));
                break;
            case SHOP_TYPE.S_CANNED:
                //StartCoroutine(LerpToPosition(5.0f, CannedRightCam.transform));
                break;
        }
    }

    public void ToOverview()
    {
        //Fade Call ///////
        alpha = 1;       //
        BeginFade(-1);   //
        ///////////////////

        StartCoroutine(MoveToPosition(DefaultCamera.transform));
        OverviewButton.SetActive(false);
        // LeftArrow.SetActive(false);
        // RightArrow.SetActive(false);
        MenuUIButtons.SetActive(true);
    }
    
    public void toStorageRoom()
    {
        //Fade Call ///////
        alpha = 1;       //
        BeginFade(-1);   //
        ///////////////////

        StartCoroutine(MoveToPosition(innerCamera.transform));
        //tempOverview.SetActive(true);
    }

    public void toStorageOverview()
    {
        //Fade Call ///////
        alpha = 1;       //
        BeginFade(-1);   //
        ///////////////////

        StartCoroutine(MoveToPosition(DefaultCamera.transform));
        //tempOverview.SetActive(false);
    }
}
