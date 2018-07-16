using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestaurantTutorialManager : MonoBehaviour
{
    public GameObject mainGroup;

    public GameObject
        background,
        continueArea;

    public GameObject tutorialPrompt;

    public GameObject instructions1;

    public GameObject
        instructions2,
        linkedHUD;

    public GameObject
        instructions3,
        menuButton,
        suppliersButton,
        arrow1,
        arrow2;

    public GameObject
        instructions4,
        instructions5,
        instructions6,
        instructions7;

    public MenuScript linkedMenu;

    List<GraphicRaycaster> raycasters = new List<GraphicRaycaster>();


    // Use this for initialization
	void Start ()
    {
        mainGroup.gameObject.SetActive(
            TutorialManager.page != TutorialManager.Page.Complete);

        switch (TutorialManager.page)
        {
            case TutorialManager.Page.Prompt:
                OpenTutorialPrompt();
                break;

            case TutorialManager.Page.Storage_ReturnToDiner:
                StartCoroutine(TutorialFlow2());
                break;
        }
	}
	
    void OpenTutorialPrompt()
    {
        background.SetActive(true);
        tutorialPrompt.SetActive(true);
    }
    void EndTutorial()
    {
        TutorialManager.page = TutorialManager.Page.Complete;
        mainGroup.SetActive(false);

        NewTutorials.tutDone = true;
    }

    void AddRaycaster(GameObject o)
    {
        var r = o.AddComponent<GraphicRaycaster>();
        var c = r.GetComponent<Canvas>();
        c.overrideSorting = true;
        c.sortingOrder = 1;

        raycasters.Add(r);
    }
    void ResetRaycasters()
    {
        foreach (var r in raycasters)
        {
            var c = r.GetComponent<Canvas>();
            Destroy(r);
            Destroy(c);
        }

        raycasters.Clear();
    }

    IEnumerator TutorialFlow1()
    {
        TutorialManager.page = TutorialManager.Page.Restaurant_Intro1;
        continueArea.SetActive(true);
        instructions1.SetActive(true);

        yield return
            new WaitUntil(() => TutorialManager.page != TutorialManager.Page.Restaurant_Intro1);

        instructions1.SetActive(false);
        instructions2.SetActive(true);
        AddRaycaster(linkedHUD);

        yield return
            new WaitUntil(() => TutorialManager.page != TutorialManager.Page.Restaurant_Intro2);

        ResetRaycasters();
        instructions2.SetActive(false);
        instructions3.SetActive(true);
        continueArea.SetActive(false);
        arrow1.SetActive(true);
        AddRaycaster(menuButton);

        yield return
            new WaitUntil(() => linkedMenu.isOpen);

        arrow1.SetActive(false);
        ResetRaycasters();
        
        yield return
            new WaitUntil(() => linkedMenu.factor >= 1.0f);

        arrow2.SetActive(true);
        AddRaycaster(suppliersButton);
    }

    IEnumerator TutorialFlow2()
    {
        TutorialManager.page = TutorialManager.Page.Restaurant_Info1;

        background.SetActive(true);
        continueArea.SetActive(true);
        AddRaycaster(linkedHUD);
        instructions4.SetActive(true);

        yield return new WaitWhile(() => TutorialManager.page ==
            TutorialManager.Page.Restaurant_Info1);

        instructions4.SetActive(false);
        instructions5.SetActive(true);

        yield return new WaitWhile(() => TutorialManager.page ==
            TutorialManager.Page.Restaurant_Info2);

        instructions5.SetActive(false);
        instructions6.SetActive(true);

        yield return new WaitWhile(() => TutorialManager.page ==
            TutorialManager.Page.Restaurant_Info3);

        ResetRaycasters();
        instructions6.SetActive(false);
        instructions7.SetActive(true);

        yield return new WaitWhile(() => TutorialManager.page ==
            TutorialManager.Page.Restaurant_End);

        EndTutorial();
    }

    public void OnPromptSelect(bool decision)
    {
        tutorialPrompt.SetActive(false);

        if (decision)
        {
            StartCoroutine(TutorialFlow1());
            Time.timeScale = 1f;
        }
        else
            EndTutorial();
    }
    public void OnClickedArea()
    {
        switch (TutorialManager.page)
        {
            case TutorialManager.Page.Restaurant_Intro1:
                TutorialManager.page = TutorialManager.Page.Restaurant_Intro2;
                break;
            case TutorialManager.Page.Restaurant_Intro2:
                TutorialManager.page = TutorialManager.Page.Restaurant_ToSuppliers;
                break;

            case TutorialManager.Page.Restaurant_Info1:
                TutorialManager.page = TutorialManager.Page.Restaurant_Info2;
                break;
            case TutorialManager.Page.Restaurant_Info2:
                TutorialManager.page = TutorialManager.Page.Restaurant_Info3;
                break;
            case TutorialManager.Page.Restaurant_Info3:
                TutorialManager.page = TutorialManager.Page.Restaurant_End;
                break;
            case TutorialManager.Page.Restaurant_End:
                TutorialManager.page = TutorialManager.Page.Complete;
                break;
        }
    }
}
