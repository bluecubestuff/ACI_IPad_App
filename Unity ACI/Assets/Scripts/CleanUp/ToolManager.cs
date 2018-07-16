using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ToolManager : MonoBehaviour {
    public List<Tools> toolList = new List<Tools>();
    public GameObject prefab;
    public static Tools[] items;
    [SerializeField] GameObject parent;
    string jsonString;
    
    // Use this for initialization
    //Reading tools from the json file in to the database
    void Start () {
        string jsonFile = (Application.streamingAssetsPath + "/toolsdata.json");
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW reader = new WWW(jsonFile);
            while (!reader.isDone) { }

            jsonString = reader.text;
        }
        else
        {
            jsonString = File.ReadAllText(jsonFile);
        }
        
        items = JsonHelper.FromJson<Tools>(jsonString);
        //Making buttons for each of the tools
        for (int i = 0; i < items.Length; i++)
        {
            Tools newTool = items[i];
            toolList.Add(newTool);
            GameObject go = Instantiate(prefab, new Vector2(0, 0), Quaternion.identity);
            go.name = toolList[i].name;
            go.transform.SetParent(parent.transform);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.SetActive(true);
            go.GetComponent<ToolInfo>().tool = toolList[i];
        }
    }
    //Temporary to debug
    public void UnlockAllTools()
    {
        Button[] temp = parent.GetComponentsInChildren<Button>();
        foreach (Button child in temp)
        {
            child.interactable = true;
        }
    }
    //Temporary to debug
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp(KeyCode.V) && parent != null)
        {
            Button[] temp = parent.GetComponentsInChildren<Button>();
            foreach (Button child in temp)
            {
                child.interactable = true;
            }
        }
    }
}
