using UnityEngine;
using System.Collections;

public class GameplayManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SoundManager.Instance.Start();
        SoundManager.Instance.SetBGM(GetComponent<AudioSource>());
        SoundManager.Instance.PlayBGM();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
