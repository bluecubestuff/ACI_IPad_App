/* 
 * Author: Lim Rui An Ryan, Lee Kwan Liang
 * Filename: ARCleanTool.cs
 * Description: A script that is used to determine where the player is touching and object and whether the touch counts as an increment to the progress.
 */
using UnityEngine;

public class ARCleanTool
{
    /// Private Variables
    private Vector3 LastHitPoint;

    /// Public Variables
    public Vector3 PreviousPosition = Vector3.zero;

    private RaycastHit HitFromCamera(Vector3 InputPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(InputPosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return hit;
    }

    public Vector3 SetToolHitPosition(Vector3 InputPosition, bool RotateTool, string tag, string blockedTag = "")
    {
        ARCleanDataStore.CleanToolActive = false;
        RaycastHit RCHit = HitFromCamera(InputPosition);
        if (ARCleanDataStore.CheckIfMouseIsOverUI() || ARCleanDataStore.ModelAccess.CurrentCleaningTool == null || RCHit.collider == null || (!tag.Equals("") && RCHit.collider.tag != tag) || (!blockedTag.Equals("") && RCHit.collider.tag == blockedTag))
            return Vector3.zero;
        LastHitPoint = RCHit.point;
        ARCleanDataStore.ModelAccess.CurrentCleaningTool.transform.position = LastHitPoint;
        if (RotateTool){
            ARCleanDataStore.ModelAccess.CurrentCleaningTool.transform.eulerAngles = new Vector3( 
                Mathf.Atan2(-RCHit.normal.y, -RCHit.normal.x) * 180f / Mathf.PI + 90f, 
                Mathf.Atan2(-RCHit.normal.z, RCHit.normal.y) * 180f / Mathf.PI - 90, 
                0);
        }
        ARCleanDataStore.Inventory.ToggleInventory(0);
        ARCleanDataStore.CleanToolActive = true;
        return RCHit.point;
    }

    public bool CleaningCheck(float DetectionRadius)
    {
        if (PreviousPosition == LastHitPoint)
            return false;
        if (PreviousPosition != Vector3.zero)
        {
            float distance = (LastHitPoint - PreviousPosition).magnitude;
            if (distance > DetectionRadius * DetectionRadius)
            {
                PreviousPosition = Vector3.zero;
                return true;
            }
        }
        else PreviousPosition = LastHitPoint;
        return false;
    }
}
