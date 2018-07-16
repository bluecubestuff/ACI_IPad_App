/* 
 * Author: Lim Rui An Ryan
 * Filename: ToolUIGenerator.cs
 * Description: A script that is used generate the tool interfaces tool buttons depending on what tools the player already has.
 */
using UnityEngine;
using UnityEngine.UI;

public class ToolUIGenerator : MonoBehaviour{

    // Public Variables
    public GameObject InstantiatedUIPrefab;

    // Private Variables
    private RectTransform PageContainer;

    // Use this for initialization
    void Start()
    {
        ARCleanDataStore.LinkedToolInventory.Clear();
        for (int i = (int)ARCleanDataStore.PlayerTool.PT_Undefined; i < (int)ARCleanDataStore.PlayerTool.PT_MAX; ++i)
            ARCleanDataStore.LinkedToolInventory.Add(null);
        InitiallizeScrollerUI();
    }
    
    // Internal
    void InitiallizeScrollerUI()
    {
        PageContainer = ARCleanDataStore.ModelAccess.ScrollerModule.GetComponent<ScrollRect>().content;
        // I will need to initiallize the elements relative to the data that I have
        GenerateUIElements();
        // I've added respective UI elements for each tool, now I can set the scroller ui
        ARCleanDataStore.ModelAccess.ScrollerModule.GetComponent<UIScroller>().StartingPage = 0;
        ARCleanDataStore.ModelAccess.ScrollerModule.GetComponent<UIScroller>().Initiallize();
    }

    public bool GetToolDataByKey(ARCleanDataStore.PlayerTool ID)
    {
        return ARCleanDataStore.PlayerToolInventory[(int)ID];
    }

    void GenerateUIElements()
    {
        for (int i = 1; i < (int)ARCleanDataStore.PlayerTool.PT_MAX; ++i)
            GenerateToolUIElement((ARCleanDataStore.PlayerTool)i, i, GetToolDataByKey((ARCleanDataStore.PlayerTool)i));
    }

    void GenerateToolUIElement(ARCleanDataStore.PlayerTool ID, int Index, bool ToolStatus)
    {
        GameObject Sign = Instantiate(InstantiatedUIPrefab);
        Sign.GetComponent<RectTransform>().SetParent(PageContainer);
        Sign.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        Sign.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        ToolUIQuickAccess ToolAccess = Sign.GetComponent<ToolUIQuickAccess>();

        // I will need to determine whether this object has or has not been taken by the player
        if (ToolStatus)
        {
            ToolAccess.LockedIcon.SetActive(false);
            ToolAccess.SetButtonInteractability(true);
        }
        ToolAccess.ToolType = ID;

        ToolAccess.SetIconBasedOnType();

        // Link the quick access
        ARCleanDataStore.LinkedToolInventory[(int)ID] = ToolAccess;
    }
}
