using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Vuforia;

public class CameraSharp : MonoBehaviour
{
    bool focusModeSet;
    //Make vuforia auto focus the camera (If not it is too blur to scan)
    void Start()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        VuforiaARController.Instance.RegisterOnPauseCallback(OnPaused);
      focusModeSet  = CameraDevice.Instance.SetFocusMode(
      CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }

    private void OnVuforiaStarted()
    {
        CameraDevice.Instance.SetFocusMode(
            CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }

    private void OnPaused(bool paused)
    {
        if (!paused) //resumed
        {
            // Set again autofocus mode when app is resumed
            CameraDevice.Instance.SetFocusMode(
                CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }
    }

    void onEnable()
    {

        //CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO;
        if (!focusModeSet)
        {
            Debug.Log("Failed to set focus mode (unsupported mode).");
        }
    }
}

