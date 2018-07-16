using UnityEngine;
using UnityEngine.UI;

public class ToolInfo : MonoBehaviour {
   public bool unlocked;

    public Tools tool;
    public int index;
    public static string toolInUse;
    //Tool data from json file
    void Start () {
        unlocked = false;
        gameObject.GetComponent<Button>().interactable = unlocked;
        gameObject.GetComponent<Button>().image.sprite = Resources.Load<Sprite>("Tool/" + tool.imageSource);
        gameObject.GetComponentInChildren<Text>().text = "";
        tag = tool.name;
    }
    public void UseTool()
    {
        //Show model on screen when not in AR Camera mode
        //Destroys all other gameobject and load the tool that they pressed
        if (!CleanPoints.inARCamera)
        {
            GameObject[] gameObjects;
            gameObjects = GameObject.FindGameObjectsWithTag("Tools");

            for (int i = 0; i < gameObjects.Length; i++)
            {
                Destroy(gameObjects[i]);
            }
            toolInUse = tool.name;
            GameObject go = Instantiate(Resources.Load<GameObject>("Tool/" + tool.prefabLocation), new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
    // Update is called once per frame
    void Update()
    { 
    }
    public void UnlockThisTool()
    {
        gameObject.GetComponent<Button>().interactable = true;
    }
}
