/* 
 * Author: Lee Kwan Liang
 * Filename: ARInventoryManager.cs
 * Description: The inventory manager of the ARClean scene that displays the tool, object inventories or none at all depending on an internal state.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARInventoryManager : MonoBehaviour
{
    public enum InventoryState
    {
        IS_None = 0,
        IS_Appliance,
        IS_Tool,
    }

    /// Public Variables
    public Transform I_ApplianceTransform;
    public Transform I_ToolTransform;

    // Private Variables
    [SerializeField] private GameObject IconPrefab;
    private bool ApplianceIsOpen = false;
    private bool ToolIsOpen = false;

    private void Start()
    {
        ARCleanDataStore.Inventory = this;
        GenerateUI();
    }

    public ARIconHandler CreateSlot()
    {
        return Instantiate(IconPrefab, I_ApplianceTransform, false).GetComponent<ARIconHandler>();
    }

    // Handle regenration of UI/Object when scene is changed
    public void GenerateUI()
    {
        // Reset stored item in inventory //* FOR DEBUGGING
        for (int i = 0; i < I_ApplianceTransform.childCount; ++i)
            Destroy(I_ApplianceTransform.GetChild(i).gameObject);

        Transform tempParent = null;
        switch (ARCleanDataStore.GetPlayerLocation())
        {
            case ARCleanDataStore.GameLocation.GL_Stove:
                tempParent = ARCleanDataStore.ModelAccess.Model_Stove.transform;
                break;
            case ARCleanDataStore.GameLocation.GL_Wok:
                tempParent = ARCleanDataStore.ModelAccess.Model_Wok.transform;
                break;
            case ARCleanDataStore.GameLocation.GL_Sink:
                tempParent = ARCleanDataStore.ModelAccess.Model_Sink.transform;
                break;
            case ARCleanDataStore.GameLocation.GL_Trash:
                tempParent = ARCleanDataStore.ModelAccess.Model_Trash.transform;
                break;
            case ARCleanDataStore.GameLocation.GL_Prep:
                tempParent = ARCleanDataStore.ModelAccess.Model_Prep.transform;
                break;
            case ARCleanDataStore.GameLocation.GL_Floor:
                tempParent = ARCleanDataStore.ModelAccess.Model_Floor.transform;
                break;
            case ARCleanDataStore.GameLocation.GL_Chiller:
                tempParent = ARCleanDataStore.ModelAccess.Model_Chiller.transform;
                break;
            case ARCleanDataStore.GameLocation.GL_Laundry:
                tempParent = ARCleanDataStore.ModelAccess.Model_Laundry.transform;
                break;
        }

        foreach (KeyValuePair<string, Sprite> it in ARCleanDataStore.InventoryList)
        {
            ARObjectHandler tempObject = null;
            // Just create the slot
            ARIconHandler tempSlot = CreateSlot();
            // Set the slot image
            tempSlot.SetIcon(it.Value);
            Transform tempTransform = ARCleanDataStore.ModelAccess.LocateObject(tempParent, it.Key);
            // If cannot find
            if (tempTransform == null)
            {
                tempSlot.Interactible = false;
                continue;
            }

            tempObject = tempTransform.GetComponent<ARObjectHandler>();
            tempObject.Slot = tempSlot;
            tempObject.Slot.Object = tempObject.gameObject;
            tempObject.gameObject.SetActive(false);
        }
    }

    public bool IsObjectInInventory(string ObjectName)
    {
        foreach (KeyValuePair<string, Sprite> it in ARCleanDataStore.InventoryList)
        {
            if (it.Key == ObjectName)
                return true;
        }
        return false;
    }

    public void ToggleInventory(int index)
    {
        StopAllCoroutines();

        if (index == 1)
        {
            StartCoroutine(CloseInventory(I_ToolTransform));
            ToolIsOpen = false;

            if (ApplianceIsOpen)
            {
                StartCoroutine(CloseInventory(I_ApplianceTransform));
                ARCleanDataStore.HideRotationalArrows = false;
                ApplianceIsOpen = false;
            }
            else
            {
                StartCoroutine(OpenInventory(I_ApplianceTransform));
                ARCleanDataStore.HideRotationalArrows = true;
                ApplianceIsOpen = true;
            }

        }
        else if (index == 2)
        {
            StartCoroutine(CloseInventory(I_ApplianceTransform));
            ApplianceIsOpen = false;

            if (ToolIsOpen)
            {
                StartCoroutine(CloseInventory(I_ToolTransform));
                ARCleanDataStore.HideRotationalArrows = false;
                ToolIsOpen = false;
            }
            else
            {
                StartCoroutine(OpenInventory(I_ToolTransform));
                ARCleanDataStore.HideRotationalArrows = true;
                ToolIsOpen = true;
            }
        }
        else
        {
            StartCoroutine(CloseInventory(I_ApplianceTransform, (bool isDone) => { ApplianceIsOpen = false; }));
            StartCoroutine(CloseInventory(I_ToolTransform, (bool isDone) => { ToolIsOpen = false; }));
        }
    }

    public void ToggleInventory(int index, bool turnOn)
    {
        StopAllCoroutines();

        if (index == 1)
        {
            StartCoroutine(CloseInventory(I_ToolTransform, (bool isDone) => { ToolIsOpen = false; }));
            if (turnOn)
            {
                StartCoroutine(OpenInventory(I_ApplianceTransform));
                ARCleanDataStore.HideRotationalArrows = true;
                ApplianceIsOpen = true;
            }
            else
            {
                StartCoroutine(CloseInventory(I_ApplianceTransform));
                ARCleanDataStore.HideRotationalArrows = false;
                ApplianceIsOpen = false;
            }
        }
        else if (index == 2)
        {
            StartCoroutine(CloseInventory(I_ApplianceTransform, (bool isDone) => { ApplianceIsOpen = false; }));
            if (turnOn)
            {
                StartCoroutine(OpenInventory(I_ToolTransform));
                ARCleanDataStore.HideRotationalArrows = true;
                ToolIsOpen = true;
            }
            else
            {
                StartCoroutine(CloseInventory(I_ToolTransform));
                ARCleanDataStore.HideRotationalArrows = false;
                ToolIsOpen = false;
            }
        }
    }

    private IEnumerator OpenInventory(Transform transformType, System.Action<bool> callback = null)
    {
        RectTransform rectTrans = transformType.parent.GetComponent<RectTransform>();

        float factor = 0f;
        while (factor < 1f)
        {
            factor += 0.05f;
            rectTrans.anchoredPosition = new Vector2(Mathf.Lerp(rectTrans.anchoredPosition.x, 0f, factor), rectTrans.anchoredPosition.y);
            yield return null;
        }

        if (callback != null)
            callback(true);
    }

    private IEnumerator CloseInventory(Transform transformType, System.Action<bool> callback = null)
    {
        RectTransform rectTrans = transformType.parent.GetComponent<RectTransform>();

        float factor = 0f;
        while (factor < 1f)
        {
            factor += 0.05f;
            rectTrans.anchoredPosition = new Vector2(Mathf.Lerp(rectTrans.anchoredPosition.x, -rectTrans.sizeDelta.x, factor), rectTrans.anchoredPosition.y);
            yield return null;
        }
        
        if (callback != null)
            callback(true);
    }

    public bool InputInInventory(Vector2 inputPos, InventoryState ObjectType)
    {
        if (ObjectType == InventoryState.IS_Appliance)
            return RectTransformUtility.RectangleContainsScreenPoint(ARCleanDataStore.Inventory.I_ApplianceTransform.GetComponent<RectTransform>(), inputPos);
        else if (ObjectType == InventoryState.IS_Tool)
            return RectTransformUtility.RectangleContainsScreenPoint(ARCleanDataStore.Inventory.I_ToolTransform.GetComponent<RectTransform>(), inputPos);

        return false;
    }

    public bool GetToolIsOpen()
    {
        return ToolIsOpen;
    }

    public bool GetApplianceIsOpen()
    {
        return ApplianceIsOpen;
    }
}
