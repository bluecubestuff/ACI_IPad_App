using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Same as settings.cs (Under Script root folder) change scene functions
/// But doesnt require any object references from public variables
/// </summary>
public class ChangeScene : MonoBehaviour {

    private float gameSpeed = 2.0f;

    public GameObject promptCanvas;

    /// <summary>
    /// Used in ArCleanUp Scene (SideBarBackground->SceneChange) for going back to main menu
    /// </summary>
    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        StocknPopularityManager.popValue = 0;
        StocknPopularityManager.stockValue = 0;
        StocknPopularityManager.starRating = 0;
        Time.timeScale = gameSpeed;
        LoadingScreenManager.LoadScene("MainMenu");
    }

    public void ToggleGoToMainCanvas()
    {
        promptCanvas.SetActive(true);
    }

    public void Cancel()
    {
        promptCanvas.SetActive(false);
    }

}
