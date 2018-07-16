using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RandomBG : MonoBehaviour
{

    public Sprite[] background;

    public string[] message;

    void Start()
    {
        Image bg = GetComponent(typeof(Image)) as Image;
        bg.sprite = background[Random.Range(0, background.Length)];

        Text hint = GetComponentInChildren(typeof(Text)) as Text;
        hint.text = message[Random.Range(0, message.Length)].ToString();
    }

    void Update()
    {
        if (Input.GetKeyUp("space"))
        {
            Image bg = GetComponent(typeof(Image)) as Image;
            bg.sprite = background[Random.Range(0, background.Length)];

            Text hint = GetComponentInChildren(typeof(Text)) as Text;
            hint.text = message[Random.Range(0, message.Length)];
        }
    }
}