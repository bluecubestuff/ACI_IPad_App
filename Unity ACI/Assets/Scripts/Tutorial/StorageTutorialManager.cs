using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageTutorialManager : MonoBehaviour
{
    public GameObject mainGroup;

    public GameObject
        background,
        continueArea,
        menuButton;

    public GameObject
        instructions1,
        arrow1;

    public GameObject
        instructions2,
        instructions3,
        arrow2,
        arrow3;

    public GameObject dinerButton;

    public MenuScript linkedMenu;

    List<CanvasGroup> canvasGroups = new List<CanvasGroup>();
    List<GraphicRaycaster> raycasters = new List<GraphicRaycaster>();

    // Use this for initialization
    void Start ()
    {
        mainGroup.gameObject.SetActive(
            TutorialManager.page != TutorialManager.Page.Complete);

        if (TutorialManager.page == TutorialManager.Page.Suppliers_GoToStorage)
            StartCoroutine(TutorialFlow1());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    IEnumerator TutorialFlow1()
    {
        TutorialManager.page = TutorialManager.Page.Storage_CheckDelivery;
        AddCanvasGroup(menuButton);
        DisableInteractable();

        instructions1.SetActive(true);
        arrow1.SetActive(true);

        yield return new WaitWhile(() => TutorialManager.page == TutorialManager.Page.Storage_CheckDelivery);

        background.SetActive(true);
        continueArea.SetActive(true);
        instructions2.SetActive(true);

        yield return new WaitWhile(() => TutorialManager.page ==
            TutorialManager.Page.Storage_StoredFood);

        ResetCanvasGroups();
        continueArea.SetActive(false);
        instructions2.SetActive(false);
        instructions3.SetActive(true);
        arrow2.SetActive(true);

        AddRaycaster(menuButton);

        yield return new WaitUntil(() => linkedMenu.isOpen);

        ResetRaycasters();
        arrow2.SetActive(false);

        yield return new WaitUntil(() => linkedMenu.factor >= 1.0f);

        AddRaycaster(dinerButton);
        arrow3.SetActive(true);
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

    public void OnCheckedDelivery()
    {
        if (TutorialManager.page == TutorialManager.Page.Storage_CheckDelivery)
        {
            if (instructions1 == null || arrow1 == null)
                return;

            instructions1.SetActive(false);
            arrow1.SetActive(false);
        }
    }
    public void OnStoredFood()
    {
        if (TutorialManager.page == TutorialManager.Page.Storage_CheckDelivery)
            TutorialManager.page = TutorialManager.Page.Storage_StoredFood;
    }

    public void OnClickedArea()
    {
        switch (TutorialManager.page)
        {
            case TutorialManager.Page.Storage_StoredFood:
                TutorialManager.page = TutorialManager.Page.Storage_ReturnToDiner;
                break;
        }
    }
}
