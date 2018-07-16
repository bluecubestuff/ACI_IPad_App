using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTool : MonoBehaviour {

    Camera main;
    int count = 0;
    AudioSource audio;
    [SerializeField]
    AudioClip audioClip;
    bool playingAudio;
    public static bool allowMove;
    public static bool touchedTool;
    //Moving tools around
    // Use this for initialization
    void Start () {
        audio = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioSource>();
        main = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playingAudio = false;
        touchedTool = false;
    }
    // Update is called once per frame
    public delegate void AudioCallback();
    public void PlaySoundWithCallback(AudioClip clip, AudioCallback callback)
    {
        if (clip != null)
        {
            audio.PlayOneShot(clip);
            StartCoroutine(DelayedCallback(clip.length, callback));
        }
    }
    //Wait for audio to finish
    private IEnumerator DelayedCallback(float time, AudioCallback callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }
    void AudioFinished()
    {
        playingAudio = false;
    }
    void Update()
    {
        if (Input.GetMouseButton(0) && allowMove)
            {
            Ray ray = main.ScreenPointToRay(Input.mousePosition);
            //Create Raycast
            RaycastHit hit;

            if ((Physics.Raycast(ray, out hit,Mathf.Infinity)))
            {

                //Continuously play audio when using tool
                if (!playingAudio)
                {
                    playingAudio = true;
                    PlaySoundWithCallback(audioClip, AudioFinished);
                }
                if (hit.collider.tag == "Tools")
                    touchedTool = true;
                
                //For scrubbing bubbles step xD
                if (hit.collider.tag == "Bubble" && ToolInfo.toolInUse == "Brush")
                {
                    if (hit.collider.gameObject.transform.localScale.x <= 8)
                        hit.collider.gameObject.transform.localScale += new Vector3(5, 5, 5) * Time.deltaTime;
                    ScrubbedBubbleChange a = hit.collider.gameObject.GetComponent<ScrubbedBubbleChange>();
                    if (!a.accomplished && hit.collider.gameObject.transform.localScale.x >= 8)
                    {
                        count++;
                        a.accomplished = true;
                    }
                }
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = (new Vector3(curPosition.x, curPosition.y, 0));
            }
        }
        if (count >= 3)
        {
            allowMove = false;
            CleanPoints cp = GameObject.FindGameObjectWithTag("CleanManager").GetComponent<CleanPoints>();
            cp.DoneWithSoap();
        }
    }
}
