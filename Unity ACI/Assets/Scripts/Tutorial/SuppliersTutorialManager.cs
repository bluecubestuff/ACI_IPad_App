using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuppliersTutorialManager : MonoBehaviour
{
    public GameObject mainGroup;

    public GameObject
        background,
        continueArea,
        menuButton;

    public GameObject 
        instructions1,
        arrowGroup1;

    public GameObject
        instructions2,
        instructions3;

    public GameObject
        instructions4,
        instructions5,
        arrow1,
        arrow2;

    public GameObject storageButton;

    public MenuScript linkedMenu;

    List<CanvasGroup> canvasGroups = new List<CanvasGroup>();
    List<GraphicRaycaster> raycasters = new List<GraphicRaycaster>();


	// Use this for initialization
	void Start ()
    {
        mainGroup.gameObject.SetActive(
            TutorialManager.page != TutorialManager.Page.Complete);

        switch (TutorialManager.page)
        {
            case TutorialManager.Page.Restaurant_ToSuppliers:
                StartCoroutine(TutorialFlow1());
                break;
        }
    }

    IEnumerator TutorialFlow1()
    {
        TutorialManager.page = TutorialManager.Page.Suppliers_EnterShop;

        instructions1.SetActive(true);
        arrowGroup1.SetActive(true);
        AddCanvasGroup(menuButton);
        DisableInteractable();

        yield return new WaitUntil(() => TouchManager.inShop);

        TutorialManager.page = TutorialManager.Page.Suppliers_Instructions1;

        arrowGroup1.SetActive(false);
        instructions1.SetActive(false);
        background.SetActive(true);
        instructions2.SetActive(true);
        continueArea.SetActive(true);

        yield return new WaitWhile(() => TutorialManager.page ==
            TutorialManager.Page.Suppliers_Instructions1);

        instructions2.SetActive(false);
        instructions3.SetActive(true);

        yield return new WaitWhile(() => TutorialManager.page ==
            TutorialManager.Page.Suppliers_Instructions2);

        instructions3.SetActive(false);
        background.SetActive(false);
        continueArea.SetActive(false);

        yield return new WaitWhile(() => TutorialManager.page ==
            TutorialManager.Page.Suppliers_BuyFood);

        background.SetActive(true);
        continueArea.SetActive(true);
        instructions4.SetActive(true);

        yield return new WaitWhile(() => TutorialManager.page ==
            TutorialManager.Page.Suppliers_InfoStorage);

        continueArea.SetActive(false);
        instructions4.SetActive(false);
        instructions5.SetActive(true);

        ResetCanvasGroups();
        AddRaycaster(menuButton);
        arrow1.SetActive(true);

        yield return new WaitUntil(() => linkedMenu.isOpen);

        ResetRaycasters();
        arrow1.SetActive(false);

        yield return new WaitUntil(() => linkedMenu.factor >= 1.0f);

        AddRaycaster(storageButton);
        arrow2.SetActive(true);
    }

    void AddCanvasGroup(GameObject o)
    {
        var g = o.AddComponent<CanvasGroup>();

        canvasGroups.Add(g);
    }
    void ResetCanvasGroups()
    {
        foreach (var g in canvasGroups)
        {
            g.interactable = true;
            Destroy(g);
        }

        canvasGroups.Clear();
    }
    void DisableInteractable()
    {
        foreach (var g in canvasGroups)
            g.interactable = false;
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

    public void OnClickedArea()
    {
        switch (TutorialManager.page)
        {
            case TutorialManager.Page.Suppliers_Instructions1:
                TutorialManager.page = TutorialManager.Page.Suppliers_Instructions2;
                break;

            case TutorialManager.Page.Suppliers_Instructions2:
                TutorialManager.page = TutorialManager.Page.Suppliers_BuyFood;
                break;

            case TutorialManager.Page.Suppliers_InfoStorage:
                TutorialManager.page = TutorialManager.Page.Suppliers_GoToStorage;
                break;
        }
    }
    public void OnFoodBought()
    {
        if (TutorialManager.page == TutorialManager.Page.Suppliers_BuyFood)
            TutorialManager.page = TutorialManager.Page.Suppliers_InfoStorage;
    }
}
