using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DayNightClock : MonoBehaviour {

    [Tooltip("Duration in minutes, timing is not exact as using fixedDeltaTime")]
    [SerializeField] private float duration = 0.0f;
    [SerializeField] private GameObject ClockHand;
    [SerializeField] private GameObject Clock;
    [SerializeField] private GameObject MainLight;
    [SerializeField] private GameObject WIN;
    [SerializeField] private GameObject blackBackground;
    private float clockRotation;
    private static float time = 0.0f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        if (SceneManager.GetActiveScene().name == "Virt_Restuarant")
        {
            if (!this.gameObject.activeSelf)
                this.gameObject.SetActive(true);
            if (time / duration > -60)
            {
                time -= Time.fixedDeltaTime;
                clockRotation = time * 360.0f / 60.0f / duration;
                ClockHand.transform.rotation = Quaternion.Euler(0, 0, clockRotation);
                float lightRotation = -clockRotation / 2;
                MainLight.transform.rotation = Quaternion.Euler(lightRotation, 0, lightRotation);
            }
            else
            {
                WIN.SetActive(true);
                blackBackground.SetActive(true);
                Time.timeScale = 0; // Pauses the game      
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
