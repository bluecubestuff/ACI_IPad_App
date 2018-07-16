/* 
 * Author: Lee Kwan Liang
 * Filename: ARObjectHandler.cs
 * Description: A script that is used to detect if the player touches an object within the scene and the logic that happens when it is dragged into an inventory tab.
 */
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class ARObjectHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    /// Public Variables
    [HideInInspector] public string ObjectName;
    [HideInInspector] public ARIconHandler Slot;
    public ARInventoryManager.InventoryState ObjectType;
    public ARCleanDataStore.PlayerTool ToolType = ARCleanDataStore.PlayerTool.PT_Undefined;
    public ARCleanDataStore.GameMode InteractibleGamemode;
    public Sprite IconImage;
    public SwipeTest Set;

    /// Protected Variables
    protected bool Selected = false;

    /// Private Variables
    [SerializeField] private Renderer ObjectRenderer;
    private Vector3 ScreenSpacePos;                     // Object position in screen space
    private Vector3 InputOffset;                        // Input offset from the position of object
    private Vector3 OriginalPos;                        // Stores the original spawn position of object
    private Quaternion OriginalRot;                     // Stores the original spawn rotation of object
    private bool ObjectInInventory = false;             // To check if last input is inside inventory rect, Prevent multiple setting of variable
    private bool AddedToInventory = false;
    private bool PointerDownInitialized = false;

    private void Awake()
    {
        OriginalPos = transform.position;
        OriginalRot = transform.rotation;
        ObjectName = gameObject.name;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (ARCleanDataStore.GetPlayerGameMode() != InteractibleGamemode && ObjectType == ARInventoryManager.InventoryState.IS_Appliance ||
            !ARCleanDataStore.ObjectInteractibleFlag && ObjectType == ARInventoryManager.InventoryState.IS_Appliance ||
            ARCleanDataStore.RequiredObject != "" && !gameObject.name.Contains(ARCleanDataStore.RequiredObject))
        {
            Debug.Log("ARObjectHandler | Not Interactible!");
            return;
        }

        StopAllCoroutines();
        ARCleanDataStore.Inventory.ToggleInventory((int)ObjectType, true);
        SetInputData(eventData.position, true);
        Selected = true;
        PointerDownInitialized = true;
        //Set.GetComponent<SwipeTest>().SetOBJInHand(PointerDownInitialized);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!Selected)
        {
            eventData.pointerDrag = null;
            return;
        }
        if (ARCleanDataStore.GetPlayerGameMode() != InteractibleGamemode && ObjectType == ARInventoryManager.InventoryState.IS_Appliance ||
            !ARCleanDataStore.ObjectInteractibleFlag && ObjectType == ARInventoryManager.InventoryState.IS_Appliance ||
            ARCleanDataStore.RequiredObject != "" && !gameObject.name.Contains(ARCleanDataStore.RequiredObject))
            return;
        else if (!PointerDownInitialized)
        {
            ARCleanDataStore.Inventory.ToggleInventory((int)ObjectType, true);
            SetInputData(eventData.position, true);
            PointerDownInitialized = true;
            Set.GetComponent<SwipeTest>().SetOBJInHand(PointerDownInitialized);
        }
        Vector3 curPos = new Vector3(
                   eventData.position.x - InputOffset.x,
                   eventData.position.y - InputOffset.y,
                   ScreenSpacePos.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
        transform.position = worldPos - Camera.main.transform.forward.normalized * 0.5f;
        
        bool inputInInventory = ARCleanDataStore.Inventory.InputInInventory(eventData.position, ObjectType);

        // Run this when there is changes
        if (ObjectInInventory != inputInInventory)
        {
            ObjectInInventory = inputInInventory;

            if (ObjectInInventory)
            {
                ObjectRenderer.enabled = false;
                foreach (Renderer InternalRenderer in GetComponentsInChildren<Renderer>())
                    InternalRenderer.enabled = false;

                if (ObjectType == ARInventoryManager.InventoryState.IS_Appliance)
                {
                    if (Slot == null)
                    {
                        Slot = ARCleanDataStore.Inventory.CreateSlot();
                        Slot.Object = gameObject;
                        Slot.SetIcon(IconImage);
                        Slot.IconType = ObjectType;
                    }
                    else
                        Slot.gameObject.SetActive(true);
                }
                else if (ObjectType == ARInventoryManager.InventoryState.IS_Tool)
                {
                    ARCleanDataStore.PlayerToolInventory[(int)ToolType] = true;
                    ARCleanDataStore.LinkedToolInventory[(int)ToolType].LockedIcon.SetActive(false);
                    ARCleanDataStore.LinkedToolInventory[(int)ToolType].SetButtonInteractability(true);
                    if (ARCleanDataStore.ModelAccess.ScrollerModule)
                        ARCleanDataStore.ModelAccess.ScrollerModule.LerpToPage((int)ToolType - 1);
                }
            }
            else
            {
                ObjectRenderer.enabled = true;
                foreach (Renderer InternalRenderer in GetComponentsInChildren<Renderer>())
                    InternalRenderer.enabled = true;
                if (ObjectType == ARInventoryManager.InventoryState.IS_Appliance)
                {
                    Slot.gameObject.SetActive(false);
                }
                else if (ObjectType == ARInventoryManager.InventoryState.IS_Tool)
                {
                    ARCleanDataStore.PlayerToolInventory[(int)ToolType] = false;
                    ARCleanDataStore.LinkedToolInventory[(int)ToolType].LockedIcon.SetActive(true);
                }
            }
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (ARCleanDataStore.GetPlayerGameMode() != InteractibleGamemode && ObjectType == ARInventoryManager.InventoryState.IS_Appliance ||
            !ARCleanDataStore.ObjectInteractibleFlag && ObjectType == ARInventoryManager.InventoryState.IS_Appliance)
            return;
        else if (ObjectType == ARInventoryManager.InventoryState.IS_Tool)
        {
            if (!ObjectInInventory)
            {
                gameObject.transform.position = OriginalPos;
                gameObject.SetActive(true);
                ToggleRenderer(true);
            }
            else gameObject.SetActive(false);
            ARCleanDataStore.Inventory.ToggleInventory((int)ObjectType, false);
            return;
        }

        ObjectInInventory = ARCleanDataStore.Inventory.InputInInventory(eventData.position, ObjectType);
        if (ObjectInInventory && !AddedToInventory)
        {
            AddedToInventory = true;
            ToggleRenderer(false);
            ARCleanDataStore.Inventory.ToggleInventory((int)ObjectType, false);
            ARCleanDataStore.InventoryList.Add(new KeyValuePair<string, Sprite>(ObjectName, IconImage));
            gameObject.SetActive(false);

            if (Slot == null)
            {
                Slot = ARCleanDataStore.Inventory.CreateSlot();
                Slot.Object = gameObject;
                Slot.SetIcon(IconImage);
            }
        }
        else if (!ObjectInInventory && Slot != null)
        {
            AddedToInventory = false;
            ToggleRenderer(true);
            Destroy(Slot.gameObject);
            StartCoroutine(LerpToPlacement());

            foreach (KeyValuePair<string, Sprite> it in ARCleanDataStore.InventoryList)
            {
                if (it.Key == gameObject.name)
                {
                    ARCleanDataStore.InventoryList.Remove(it);
                    
                    if (ARCleanDataStore.InventoryList.Count == 0)
                        ARCleanDataStore.Inventory.ToggleInventory((int)ObjectType, false);

                    break;
                }
            }
        }
        else
        {
            ARCleanDataStore.Inventory.ToggleInventory((int)ObjectType, false);
            StartCoroutine(LerpToPlacement());
        }

        Selected = false;
        PointerDownInitialized = false;
        Set.GetComponent<SwipeTest>().SetOBJInHand(PointerDownInitialized);
    }

    // To call when slot object leaves inventory
    public void RevealObjectFromInventory(Vector2 inputPos)
    {
        AddedToInventory = true;
        gameObject.SetActive(true);
        // Set object position to input position
        SetInputData(inputPos, false);
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(inputPos.x, inputPos.y, ScreenSpacePos.z));
        Selected = true;
        PointerDownInitialized = false;
        //Set.GetComponent<SwipeTest>().SetOBJInHand(PointerDownInitialized);
    }

    private void ToggleRenderer(bool on)
    {
        ObjectRenderer.enabled = on;
        foreach (Renderer InternalRenderer in GetComponentsInChildren<Renderer>())
            InternalRenderer.enabled = on;
    }

    // Set the input data for conversion from screen space to world space
    private void SetInputData(Vector2 inputPos, bool allowOffset)
    {
        // Set Screen space position
        ScreenSpacePos = Camera.main.WorldToScreenPoint(transform.position);
        // Direction vector of input to object position in screen space
        if (allowOffset)
            InputOffset = inputPos - new Vector2(ScreenSpacePos.x, ScreenSpacePos.y);
    }

    public IEnumerator LerpToPlacement()
    {
        while (transform.position != OriginalPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, OriginalPos, Screen.width * 0.01f * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, OriginalRot, Screen.width * 0.01f * Time.deltaTime);
            yield return null;
        }
    }
}
