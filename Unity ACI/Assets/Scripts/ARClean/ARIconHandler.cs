/* 
 * Author:Lee Kwan Liang
 * Filename: ARIconHandler.cs
 * Description: A script that handles the input of the player when it is on an object's icon, such that the player is able to drag said object out from the User Interface into the scene.
 */
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ARIconHandler : MonoBehaviour/*, IPointerDownHandler*/, IDragHandler, IPointerUpHandler
{
    /// Public Variable
    [HideInInspector] public GameObject Object;
    [HideInInspector] public bool Interactible = true;
    public ARInventoryManager.InventoryState IconType;

    public Sprite UnlockedImage;
    public Sprite LockedImage;
    
    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    if (!ARCleanDataStore.ObjectInteractibleFlag || !Interactible ||
    //        ARCleanDataStore.RequiredObject != "" && !Object.name.Contains(ARCleanDataStore.RequiredObject))
    //    {
    //        Debug.Log("ARIconHandler | Not Interactible!");
    //        return;
    //    }
    //}

    public void OnDrag(PointerEventData eventData)
    {
        if (!ARCleanDataStore.ObjectInteractibleFlag || !Interactible ||
            ARCleanDataStore.RequiredObject != "" && !Object.name.Contains(ARCleanDataStore.RequiredObject))
            return;

        transform.position = eventData.position;

        if (!ARCleanDataStore.Inventory.InputInInventory(eventData.position, ARInventoryManager.InventoryState.IS_Appliance))
        {
            Object.GetComponent<ARObjectHandler>().RevealObjectFromInventory(eventData.position);
            eventData.pointerDrag = Object;
            eventData.pointerPress = Object;
            gameObject.SetActive(false);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!ARCleanDataStore.ObjectInteractibleFlag || !Interactible ||
            ARCleanDataStore.RequiredObject != "" && !Object.name.Contains(ARCleanDataStore.RequiredObject))
            return;

        ResetIcon();
    }

    public void SetIcon(Sprite image)
    {
        GetComponent<Image>().sprite = image;
    }

    private void ResetIcon()
    {
        transform.localPosition = Vector3.zero;
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
    }
}
