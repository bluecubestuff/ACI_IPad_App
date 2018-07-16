using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Settings : MonoBehaviour {

    public GameObject Panel, BGMSlider, SFXSlider, ResumeButton, B2Menu;
    public Slider BGMSliderVal, SFXSliderVal;
    private bool Fastforwarded;
    public GameObject UI_Buttons;
    public GameObject Options;
    public GameObject blackBackground;
    public GameObject orderList;
    bool playMenuRotation;
    [SerializeField] GameObject arrow;
    public Button setGamespeedA; // Game speed 1x
    public Button setGamespeedB; // Game speed 1.5x
    public Button setGamespeedC; // Game speed 2x
    bool UI_IsShown;
    GameObject CurrentLevelPanel;
    float gameSpeed=2;
    bool optionsAnimation = false;
    bool orderListAnimation = false;
    bool LoadingInitiated = false;

    // Use this for initialization
    void Start () {
        Application.targetFrameRate = 60;
        playMenuRotation = false;
        Fastforwarded = false;
        DOTween.Init(false, false, LogBehaviour.Default);
        UI_IsShown = true;
        
        if (SceneManager.GetActiveScene().name == "Virt_Restuarant")
            Time.timeScale = gameSpeed;

        if (gameSpeed == 2f)
        {
            SetTransparency255(setGamespeedA.GetComponent<Image>()); 
            SetTransparency0(setGamespeedB.GetComponent<Image>());
            SetTransparency0(setGamespeedC.GetComponent<Image>());
        }
        else if (gameSpeed == 3f)
        {
            SetTransparency0(setGamespeedA.GetComponent<Image>());
            SetTransparency255(setGamespeedB.GetComponent<Image>());
            SetTransparency0(setGamespeedC.GetComponent<Image>());
        }
        else if (gameSpeed == 4f)
        {
            SetTransparency0(setGamespeedA.GetComponent<Image>());
            SetTransparency0(setGamespeedB.GetComponent<Image>());
            SetTransparency255(setGamespeedC.GetComponent<Image>());
        }
    }

    public void SettingsButt()
    {
        Time.timeScale = 0;
        Panel.gameObject.SetActive(true);
        BGMSlider.gameObject.SetActive(true);
        SFXSlider.gameObject.SetActive(true);
        ResumeButton.gameObject.SetActive(true);
        B2Menu.gameObject.SetActive(true);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Panel.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = gameSpeed;
        Panel.gameObject.SetActive(false);
        BGMSlider.gameObject.SetActive(false);
        SFXSlider.gameObject.SetActive(false);
        ResumeButton.gameObject.SetActive(false);
        B2Menu.gameObject.SetActive(false);
    }
    public void Back2Menu()
    {
        Time.timeScale = gameSpeed;
        Panel.gameObject.SetActive(false);
        BGMSlider.gameObject.SetActive(false);
        SFXSlider.gameObject.SetActive(false);
        ResumeButton.gameObject.SetActive(false);
        B2Menu.gameObject.SetActive(false);
    }

    public void Fastforward()
    {
        Fastforwarded = !Fastforwarded;
    }

    public float GetBGMValue()
    {
        return BGMSliderVal.value;
    }
    public void SetBGMValue(float BGMVal)
    {
        BGMSliderVal.value = BGMVal;
    }
    
    public void GetSFXValue(float SFXVal)
    {
        SFXVal = SFXSliderVal.value;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void CurrentLevelNotif()
    {
        CurrentLevelPanel.SetActive(true);
    }
    IEnumerator DelayedLoad(string ToLoad)
    {
        //Wait until clip finish playing
        yield return new WaitForSeconds(0.5f);

        //Load scene here
        if (SceneManager.GetActiveScene().name == "something")
        {
            CurrentLevelNotif();
        }
        else
        {
            Time.timeScale = 1;
            if (ToLoad == "Supplier")
            {
                LoadingScreenManager.LoadScene("Virt_Suppliers");
            }
            if (ToLoad == "Storage")
            {
                LoadingScreenManager.LoadScene("Virt_Storage");
            }
            if (ToLoad == "Restaurant")
            {
                if (!MeatFabricationScript2.fabricatedMeat && StocknPopularityManager.stockValue > 0 && SceneManager.GetActiveScene().name == "Virt_Storage")
                {
                    LoadingScreenManager.LoadScene("Virt_MeatFabrication");
                }
                else
                    LoadingScreenManager.LoadScene("Virt_Restuarant");
            }
        }
    }
    public void GoCleanUpScene()
    {
		Time.timeScale = 1;
		StocknPopularityManager.popValue = 0;
		StocknPopularityManager.stockValue = 0;
		StocknPopularityManager.starRating = 0;
        if (SceneManager.GetActiveScene().name == "something")
        {
            CurrentLevelNotif();
        }
        else
        {
            LoadingScreenManager.LoadScene("ARScanInstructions");
        }
    }
    public void GoSupplierScene()
    {
        if (!LoadingInitiated)
        {
            StartCoroutine(DelayedLoad("Supplier"));
            LoadingInitiated = true;
        }
    }

    public void PlayMenuRotation()
    {
        playMenuRotation = !playMenuRotation;
        if(playMenuRotation)
            arrow.GetComponent<Animator>().SetTrigger("state1");
        else
            arrow.GetComponent<Animator>().SetTrigger("state2");

    }
    private void LateUpdate()
    {

    }
    public void GoStorageScene()
    {
        if (!LoadingInitiated)
        {
            StartCoroutine(DelayedLoad("Storage"));
            LoadingInitiated = true;
        }
    }

    public void GoRestaurantScene()
    {
        if (!LoadingInitiated)
        {
            StartCoroutine(DelayedLoad("Restaurant"));
            LoadingInitiated = true;
        }
    }
    public void GoMainMenu()
    {
        if (SceneManager.GetActiveScene().name == "something")
        {
            CurrentLevelNotif();
        }
        else
        {
            //Time.timeScale = 1;
            //StocknPopularityManager.popValue = 0;
            //StocknPopularityManager.stockValue = 0;
            //StocknPopularityManager.starRating = 0;
            Time.timeScale = gameSpeed;
            LoadingScreenManager.LoadScene("MainMenu");

        }
    }

    public void TestScene()
    {
        Time.timeScale = gameSpeed;
        LoadingScreenManager.LoadScene("TestScene");
    }

    public void MeatFab()
    {
        Time.timeScale = gameSpeed;
        LoadingScreenManager.LoadScene("Virt_MeatFabrication");
    }

    public void StorageTest()
    {
        Time.timeScale = gameSpeed;
        LoadingScreenManager.LoadScene("StorageTest");
    }

    public void SetAugmentedMode()
    {
        if (SceneManager.GetActiveScene().name == "AR_Main")
        {
            Time.timeScale = gameSpeed;
            SceneManager.LoadScene("Virt_Restaurant");
        }
        else
        {
            Time.timeScale = gameSpeed;
            LoadingScreenManager.LoadScene("AR_Main");
        }
    }
    public void MoveUI(float show)
    {
        if (!UI_IsShown)
        {
           // UI_Buttons.transform.DOMoveY(show, 0.4f);
            UI_IsShown = true;
            UI_Buttons.transform.DOLocalMoveY(show, 0.4f);
        }
        else
        {
            UI_Buttons.transform.DOLocalMoveY(1500, 0.4f);
            UI_IsShown = false;
        }
    }

    public void ShowOptions()
    {
        EventSystem.current.SetSelectedGameObject(null);

        Options.SetActive(true);
        if (blackBackground != null)
            blackBackground.SetActive(true);
        optionsAnimation = true;
    }

    public void ShowOrderList()
    {
        EventSystem.current.SetSelectedGameObject(null);

        orderList.SetActive(true);
        if(blackBackground != null)
        blackBackground.SetActive(true);
        orderListAnimation = true;
    }
     public void HideOrderList()
    {
        orderList.GetComponent<Animator>().SetInteger("end", 1);
        orderListAnimation = false;
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Virt_Restuarant" && Input.GetKeyUp(KeyCode.C))
        {
            Debug.Log("aaa");
            GoCleanUpScene();
        }
        if (Options.GetComponent<Animator>().isInitialized)
        {
            if (optionsAnimation && Options.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("New State"))
            {
                Time.timeScale = 0;
            }
            if (!optionsAnimation && Options.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Empty"))
            {
                Time.timeScale = gameSpeed;
                optionsAnimation = false;
                if(blackBackground != null)
                blackBackground.SetActive(false);
                Options.GetComponent<Animator>().SetInteger("end", 10);
                Options.SetActive(false);
            }
        }
        if (orderList != null)
        {
            if (orderList.GetComponent<Animator>().isInitialized)
            {
                if (orderListAnimation && orderList.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("New State"))
                {
                    Time.timeScale = 0;
                }
                if (!orderListAnimation && orderList.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Empty"))
                {
                    Time.timeScale = gameSpeed;
                    orderListAnimation = false;
                    blackBackground.SetActive(false);
                    orderList.GetComponent<Animator>().SetInteger("end", 10);
                    orderList.SetActive(false);
                }
            }
        }
    }
    public void CloseOptions()
    {
        Options.GetComponent<Animator>().SetInteger("end", 1);
        optionsAnimation = false;
    }

    public void ChangeGameSpeed(float newGameSpeed)
    {
        EventSystem.current.SetSelectedGameObject(null);

        gameSpeed = newGameSpeed;

        if (newGameSpeed == 2f)
        {
            SetTransparency255(setGamespeedA.GetComponent<Image>());
            SetTransparency0(setGamespeedB.GetComponent<Image>());
            SetTransparency0(setGamespeedC.GetComponent<Image>());
        }
        else if (newGameSpeed == 3f)
        {
            SetTransparency0(setGamespeedA.GetComponent<Image>());
            SetTransparency255(setGamespeedB.GetComponent<Image>());
            SetTransparency0(setGamespeedC.GetComponent<Image>());
        }
        else if (newGameSpeed == 4f)
        {
            SetTransparency0(setGamespeedA.GetComponent<Image>());
            SetTransparency0(setGamespeedB.GetComponent<Image>());
            SetTransparency255(setGamespeedC.GetComponent<Image>());
        }
    }

    void SetTransparency255(Image p_image)
    {
        if (p_image != null)
        {
            Color __alpha = p_image.color;
            __alpha.a = 255;
            p_image.color = __alpha;
        }
    }

    void SetTransparency0(Image p_image)
    {
        if (p_image != null)
        {
            Color __alpha = p_image.color;
            __alpha.a = 0;
            p_image.color = __alpha;
        }
    }
}
