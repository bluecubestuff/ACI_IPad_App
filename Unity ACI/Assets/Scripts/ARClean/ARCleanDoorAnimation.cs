/* 
 * Author: Lee Kwan Liang
 * Filename: ARCleanDoorAnimation.cs
 * Description: A script that is used in triggering the door's open/close animation
 */
using UnityEngine;
using UnityEngine.EventSystems;

public class ARCleanDoorAnimation : MonoBehaviour, IPointerClickHandler
{
    // Public Variable
    public Animator DoorAnimatorController;

    // Private Variable
    private bool IsOpen = false;
    private bool Triggered = false;
    private bool PreviousChange = false;

    private void OnEnable()
    {
        ARCleanDataStore.CurrentSceneDoor = this;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Triggered)
        {
            Triggered = true;
            DoorAnimatorController.SetTrigger("Door Trigger");
        }
    }

    public bool IsAnimationOpen()
    {
        if (DoorAnimatorController.GetCurrentAnimatorStateInfo(0).IsName("Door Opened"))
            return true;
        return false;
    }

    private void Update()
    {
        if (DoorAnimatorController.GetCurrentAnimatorStateInfo(0).IsName("Door Opened"))
            IsOpen = true;
        else if (DoorAnimatorController.GetCurrentAnimatorStateInfo(0).IsName("Door Closed"))
            IsOpen = false;

        if (ARCleanDataStore.ForceCurrentDoorClose ||
            ARCleanDataStore.CameraCurrentDirection == ARCleanCamera.Directions.D_Up)
            DoorAnimatorController.SetBool("Close Door", true);
        else
            DoorAnimatorController.SetBool("Close Door", false);
        
        if (PreviousChange != IsOpen)
        {
            PreviousChange = IsOpen;
            Triggered = false;
        }
    }
}
